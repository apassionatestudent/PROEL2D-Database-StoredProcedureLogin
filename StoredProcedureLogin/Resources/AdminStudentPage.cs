using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StoredProcedureLogin.Resources
{
    public partial class AdminStudentPage : Form
    {

        private BindingSource StudentBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;
        private OpenFileDialog ofdPicture;

        string errorMessage = String.Empty; 
        int errorCounter = 0;

        private readonly Logger _logger;
        private string fullName;
        private int roleID;

        //clear field
        bool isCellClicked = false;

        bool isErrorFound = false;

        string FilePath = ""; // for pic directory
        public AdminStudentPage(string FullName, int RoleID)
        {
            InitializeComponent();
            
            // disabled them by default unless there's a student selected at a time 
            btnUpdate.Enabled = false; btnDelete.Enabled = false; 
            //btnChangePicture.Enabled = false; 
            btnShowDetails.Enabled = false;

            fullName = FullName;
            roleID = RoleID;

            // connect => Logger.cs
            _logger = new Logger();


            ofdPicture = new OpenFileDialog();
        }

        private void GetData(string selectCommand)
        {
            try
            {
                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                // Create data adapter
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Fill dataset and bind to BindingSource
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                StudentBindingSource.DataSource = table;

                // Bind DataGridView to BindingSource
                dgvStudents.DataSource = StudentBindingSource;

                // Adjust column widths
                //dgvStudents.AutoResizeColumns(
                //    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

                // Set Fill mode for all columns except one
                foreach (DataGridViewColumn column in dgvStudents.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Set the last column to fill remaining space
                int lastColumnIndex = dgvStudents.Columns.Count - 1;
                dgvStudents.Columns[lastColumnIndex].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;

                dgvStudents.AutoResizeColumns();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }

        private void AdminStudentPage_Load(object sender, EventArgs e)
        {
            GetData(@"
                SELECT * FROM dbo.DisplayAllStudentData()
            ");
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            if (isCellClicked == true)
            {
                //clear the fields first before adding a new teacher
                ClearField();
                isCellClicked = false;
                txtPassword.Enabled = true; // enable password field when adding a new student 
            }

            else
            {
                // Check if the inputs are valid first
                VerifyInputs();
                ConfirmInputs();

                try {
                    string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if username already exists
                        SqlCommand cmdCheckUsername = new SqlCommand(@"
                            SELECT CASE WHEN EXISTS (
                                SELECT 1 FROM Users WHERE Username = @Username
                            ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END", connection);

                        cmdCheckUsername.Parameters.AddWithValue("@Username", txtUsername.Text);

                        bool usernameExists = (bool)cmdCheckUsername.ExecuteScalar();

                        if (usernameExists)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.",
                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (isErrorFound == false)
                        {
                            // Execute the stored procedure
                            using (SqlCommand command = new SqlCommand("dbo.SP_AddStudent", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                                command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                                //command.Parameters.AddWithValue("@Department", cbxDepartments.SelectedItem.ToString());
                                //command.Parameters.AddWithValue("@Department", cbxDepartments.SelectedIndex.ToString());
                                command.Parameters.AddWithValue("@UserName", txtUsername.Text);
                                command.Parameters.AddWithValue("@Password", txtPassword.Text);
                                command.Parameters.AddWithValue("@PictureDirectory", FilePath);
                                command.Parameters.AddWithValue("@Status", cbStatus.SelectedItem.ToString());
                                //command.Parameters.AddWithValue("@PictureDirectory", pbxTeacherProfile.ImageLocation ?? "");

                                // Add the new profile fields, why did I forget this earlier? Hahaha
                                command.Parameters.AddWithValue("@Age", txtAge.Text);
                                command.Parameters.AddWithValue("@Gender", cbGender.SelectedIndex.ToString());
                                command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                                command.Parameters.AddWithValue("@Email", txtEmailAddress.Text);
                                command.Parameters.AddWithValue("@RecoveryCode", Convert.ToInt32(txtRecoveryCode.Text));

                                //command.ExecuteNonQuery();

                                // Execute the stored procedure
                                int returnValue = command.ExecuteNonQuery();
                                if (returnValue > 0)
                                {
                                    MessageBox.Show("Failed to add student!", "Failed!",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Student has been added successfully!", "Sumakses!",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    _logger.Log(fullName, $"Adds new student: {txtFirstName.Text} {txtLastName.Text}", roleID);

                                    // Refresh the dgvTeachers table
                                    GetData(@"
                                    SELECT * FROM dbo.DisplayAllStudentData()
                                ");

                                    // Clear form fields
                                    ClearField();
                                }
                            }
                        }
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error adding student: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                VerifyInputs();
                ConfirmInputs();

                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if username exists (for update validation)
                    SqlCommand cmdCheckUsername = new SqlCommand(@"
                        SELECT CASE WHEN EXISTS (
                            SELECT 1 FROM Users WHERE Username = @Username
                        ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END", connection);

                    cmdCheckUsername.Parameters.AddWithValue("@Username", txtUsername.Text);

                    bool usernameExists = (bool)cmdCheckUsername.ExecuteScalar();

                    if (!usernameExists)
                    {
                        MessageBox.Show("Teacher record not found. Please select an existing teacher to update.",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Execute the stored procedure
                    using (SqlCommand command = new SqlCommand("dbo.SP_UpdateStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        command.Parameters.AddWithValue("@UserName", txtUsername.Text);
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);
                        command.Parameters.AddWithValue("@PictureDirectory", FilePath);
                        command.Parameters.AddWithValue("@Status", cbStatus.SelectedItem.ToString());

                        // Add the new profile fields, why did I forget this earlier? Hahaha
                        command.Parameters.AddWithValue("@Age", txtAge.Text);
                        command.Parameters.AddWithValue("@Gender", cbGender.SelectedIndex.ToString());
                        command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@Address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Email", txtEmailAddress.Text);
                        command.Parameters.AddWithValue("@RecoveryCode", Convert.ToInt32(txtRecoveryCode.Text));

                        command.ExecuteNonQuery();

                        int returnValue = command.ExecuteNonQuery();
                        if (returnValue > 0)
                        {
                            MessageBox.Show("Failed to update student!", "Failed!",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                            MessageBox.Show("Student information updated successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                            _logger.Log(fullName, $"Updates the student: {txtFirstName.Text} {txtLastName.Text}", roleID);

                            // Refresh the dgvTeachers table
                            GetData(@"
                                SELECT * FROM dbo.DisplayAllStudentData()
                            ");

                            // Clear form fields
                            ClearField();
                            btnUpdate.Enabled = false; btnDelete.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a row is selected
                if (dgvStudents.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a teacher to delete.", "Selection Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the ProfileID from the selected row
                int profileID = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["ProfileID"].Value);

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Deletion",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                // Execute the stored procedure
                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProfileID", profileID);

                        // Execute the stored procedure
                        int returnValue = command.ExecuteNonQuery();
                        if (returnValue > 0)
                        {
                            MessageBox.Show("Failed to delete student.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {


                            MessageBox.Show("Student has been deleted successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                            _logger.Log(fullName, $"Deletes the student: {txtFirstName.Text} {txtLastName.Text}", roleID);

                            // Refresh the grid
                            GetData(@"SELECT * FROM dbo.DisplayAllStudentData()");
                            ClearField();
                            btnUpdate.Enabled = false; btnDelete.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchStudent_Click(object sender, EventArgs e)
        {
            // search would be based on txtSearch.Text value
            try
            {
                string searchText = txtSearch.Text.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    MessageBox.Show("Please enter search ID.",
                                  "Search Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Exclamation);
                    txtSearch.Focus();
                    return;
                }

                // Reset all highlights
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.Selected = false;
                    }
                }

                bool foundMatch = false;

                for (int rowIndex = 0; rowIndex < dgvStudents.Rows.Count; rowIndex++)
                {
                    if (dgvStudents.Rows[rowIndex].IsNewRow)
                        continue;

                    // 0 to check the first column only, which is StudentID 
                    string currentValue = dgvStudents.Rows[rowIndex].Cells[0].Value?.ToString()?.ToLower();

                    if (!string.IsNullOrEmpty(currentValue) &&
                        currentValue == searchText)
                    {
                        dgvStudents.Rows[rowIndex].Selected = true;
                        dgvStudents.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                        dgvStudents.FirstDisplayedScrollingRowIndex = rowIndex;
                        dgvStudents_CellClick(dgvStudents, new DataGridViewCellEventArgs(0, rowIndex));
                        foundMatch = true;
                    }
                }

                MessageBox.Show(foundMatch ?
                    "Information found!" :
                    "No matching records found.",
                    "Search Result",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void btnChangePicture_Click(object sender, EventArgs e)
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

                pbxPicture.Image = Image.FromFile(filePath);
                pbxPicture.ImageLocation = filePath;
                FilePath = filePath; // set the file path to the global variable
            }
        }

        // show message box here 
        private void btnShowDetails_Click(object sender, EventArgs e)
        {

        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            isCellClicked = true;
            // enable the buttons when a student is selected
            btnUpdate.Enabled = true; btnDelete.Enabled = true;  btnChangePicture.Enabled = true; btnShowDetails.Enabled = true; txtPassword.Enabled = false;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                // Assuming the DataGridView has columns named "StudentID", "FirstName", "LastName
                //txtStudentID.Text = row.Cells["StudentID"].Value.ToString();
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                txtEmailAddress.Text = row.Cells["Email"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtRecoveryCode.Text = row.Cells["RecoveryCode"].Value.ToString();

                if (row.Cells["Gender"].Value.ToString() == "Male")
                {
                    cbGender.SelectedIndex = 0;
                }

                else
                {
                    cbGender.SelectedIndex = 1;
                }

                if (row.Cells["Status"].Value.ToString() == "Active")
                {
                    cbStatus.SelectedIndex = 0;
                }

                else
                {
                    cbStatus.SelectedIndex = 1;
                }

                    string profilePicturePath = row.Cells["PictureDirectory"].Value.ToString();
                if (!string.IsNullOrEmpty(profilePicturePath) && System.IO.File.Exists(profilePicturePath))
                {
                    pbxPicture.Image = Image.FromFile(profilePicturePath);
                }
                else
                {
                    pbxPicture.Image = null; // or set to a default image
                }
            }
        }

        private void ClearField()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtAge.Clear();
            txtEmailAddress.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            pbxPicture.Image = null;
            cbGender.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;
            txtRecoveryCode.Clear();
            // disable the buttons when fields are cleared
            btnUpdate.Enabled = false; btnDelete.Enabled = false; btnShowDetails.Enabled = false;
        }


        private void VerifyInputs() {
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

            if (string.IsNullOrWhiteSpace(txtAge.Text) || !int.TryParse(txtAge.Text.Trim(), out _) || Convert.ToInt64(txtAge.Text) < 18)
            {
                errorMessage += "- Age is required, or must be a whole number, or at least 18 years old";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(cbGender.Text) || cbGender.SelectedIndex == -1)
            {
                errorMessage += "- Gender is required. \n";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(cbStatus.Text) || cbStatus.SelectedIndex == -1)
            {
                errorMessage += "- Status is required. \n";
                errorCounter++;
            }

            if (cbGender.Text == "Transformer")
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
            if (!string.IsNullOrWhiteSpace(txtEmailAddress.Text))
            {
                if (!Regex.IsMatch(txtEmailAddress.Text.Trim(), emailRegex, RegexOptions.IgnoreCase))
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
                isErrorFound = true;
                errorCounter = 0;
                return;
            }
        }

        private void ConfirmInputs() {

            if (isErrorFound == false)
            {
                MessageBox.Show($"First Name: {txtFirstName.Text} \n" +
                            $"Last Name: {txtLastName.Text}. \n" +
                            $"Username: {txtUsername.Text} \n" +
                            $"Password: {txtPassword.Text} \n" +
                            $"Age {txtAge.Text}  \n" +
                            $"Email: {txtEmailAddress.Text} \n" +
                            $"Address: {txtAddress.Text} \n " +
                            $"Status: {cbStatus.SelectedItem.ToString()} \n" +
                            $"Gender: {cbGender.SelectedItem.ToString()} \n" +
                            $"Phone: {txtPhone.Text} \n " +
                            $"Recovery Code: {txtRecoveryCode.Text} \n", "Input Confirmation",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


    }
}
