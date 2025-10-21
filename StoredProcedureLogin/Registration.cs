using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoredProcedureLogin
{
    public partial class Registration : Form
    {
        private OpenFileDialog ofdPicture;
        string FilePath = string.Empty;
        public Registration()
        {
            InitializeComponent();

            ofdPicture = new OpenFileDialog();
        }

        string connectingString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

        private async Task<string> GetNextStudentID()
        {
            using (SqlConnection connection = new SqlConnection(connectingString))
            {
                await connection.OpenAsync();

                const string query = @"
            SELECT COALESCE(MAX(CAST(REPLACE(UserID, 'ST-', '') AS INT)), 0) + 1 
            FROM Users 
            WHERE UserID LIKE 'ST-%'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var result = await command.ExecuteScalarAsync();
                    return $"ST-{result:00000}";
                }
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            int errorCounter = 0;
            string errorMessage = string.Empty;
            string detailedError = string.Empty;

            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    errorMessage += "- Username is required. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    errorMessage += "- Password is required. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    errorMessage += "- First name is required. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    errorMessage += "- Last name is required. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtAge.Text))
                {
                    errorMessage += "- Age is required. \n";
                    errorCounter++;
                }

                if (!int.TryParse(txtAge.Text.Trim(), out _))
                {
                    errorMessage += "- Age must be a valid whole number. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(cbxGender.Text))
                {
                    errorMessage += "- Gender is required. \n";
                    errorCounter++;
                }

                if (cbxGender.Text == "Transformer")
                {
                    errorMessage += "- There could only be 2 genders! Others are a delusion. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    errorMessage += "- Phone number is required. \n";
                    errorCounter++;
                }

                if (!decimal.TryParse(txtPhone.Text.Trim(), out _))
                {
                    errorMessage += "- Number field must contain a valid decimal number. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    errorMessage += "- Address is required. \n";
                    errorCounter++;
                }

                string emailRegex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    if (!Regex.IsMatch(txtEmail.Text.Trim(), emailRegex, RegexOptions.IgnoreCase))
                    {
                        errorMessage += "- Invalid email format. Please use .com, .net, .org, or .gov domains. \n";
                        errorCounter++;
                    }
                }

                if (string.IsNullOrWhiteSpace(txtRecoveryCode.Text))
                {
                    errorMessage += "- Recovery Code is required. \n";
                    errorCounter++;
                }

                
                else
                {
                    if (!int.TryParse(txtRecoveryCode.Text.Trim(), out _))
                    {
                        errorMessage += "- Recovery Code must be a valid integer.\n";
                        errorCounter++;
                    }
                    else if (txtRecoveryCode.Text.Trim().Length < 4)
                    {
                        errorMessage += "- Recovery Code must be at least 4 digits.\n";
                        errorCounter++;
                    }
                }


                // if error, list error to guide user what to fix
                if (errorCounter >= 1)
                {
                    MessageBox.Show(errorMessage, "Validation Error");
                    errorCounter = 0;
                    return;
                }

                string studentID = await GetNextStudentID();

                // Create connection object
                using (SqlConnection connection = new SqlConnection(connectingString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@Role", "Student");
                        //command.Parameters.AddWithValue("@ID", studentID);
                        command.Parameters.AddWithValue("@Username", txtUsername.Text);
                        command.Parameters.AddWithValue("@PasswordHash", txtPassword.Text); // Note: In production, hash the password first
                        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        command.Parameters.AddWithValue("@Age", Convert.ToInt32(txtAge.Text));
                        command.Parameters.AddWithValue("@Gender", cbxGender.Text);
                        command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@Address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@PictureDirectory", FilePath);
                        command.Parameters.AddWithValue("@RecoveryCode", Convert.ToInt32(txtRecoveryCode.Text));

                        // Add the output parameter for error messages
                        SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1);
                        errorMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(errorMessageParam);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected < 0)
                        {
                            MessageBox.Show("Registration successful!", "Success");
                            Hide();
                            new LoginForm().Show();
                            ClearForm();
                        }
                        else
                        {
                            // Get detailed error information
                            string procedureError = command.Parameters["@ErrorMessage"].Value?.ToString() ?? "Unknown error";
                            MessageBox.Show($"Registration failed: {procedureError}", "Error");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();

            txtUsername.Focus();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Hide();
        }

        private void btnSelectPicture_Click(object sender, EventArgs e)
        {
            // configure the OpenFileDialog
            ofdPicture.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
            ofdPicture.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ofdPicture.Title = "Select Picture";

            // Show the dialog and check if user clicked OK
            if (ofdPicture.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofdPicture.FileName;
                // Now you can use the filePath variable as needed
                MessageBox.Show($"Selected picture: {filePath}");

                pbxProfile.Image = Image.FromFile(filePath);    
                FilePath = filePath;    
            }
        }


    }
}
