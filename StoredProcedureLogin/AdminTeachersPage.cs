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


namespace StoredProcedureLogin
{
    public partial class AdminTeachersPage : Form
    {

        private BindingSource StudentBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;
        private OpenFileDialog ofdPicture;
        string FilePath = "";

        string errorMessage = String.Empty;
        int errorCounter = 0;

        private readonly Logger _logger;
        private string fullName;
        private int roleID;

        // check if cell is clicked 
        bool isCellClicked = false;
        public AdminTeachersPage(string FullName, int RoleID)
        {
            InitializeComponent();

            // disable by default unless the admin clicks the datagridview 
            btnUpdateTeacher.Enabled = false;   
            btnDeleteTeacher.Enabled = false;
            //btnSelectPicture.Enabled = false;

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
                dgvTeachers.DataSource = StudentBindingSource;

                // Adjust column widths
                //dgvStudents.AutoResizeColumns(
                //    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

                // Set Fill mode for all columns except one
                foreach (DataGridViewColumn column in dgvTeachers.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Set the last column to fill remaining space
                int lastColumnIndex = dgvTeachers.Columns.Count - 1;
                dgvTeachers.Columns[lastColumnIndex].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;

                dgvTeachers.AutoResizeColumns();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }

        private void AdminTeachersPage_Load(object sender, EventArgs e)
        {
            GetData(@"
                SELECT * FROM dbo.DisplayAllTeachers()
            ");
        }

        private void btnAddTeacher_Click(object sender, EventArgs e)
        {

            if (isCellClicked == true)
            {
                //clear the fields first before adding a new teacher
                ClearField();
                isCellClicked = false;
                txtPassword.Enabled = true; // enable password field when adding a new teacher
            }

            else
            {
                try
                {
                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                        string.IsNullOrWhiteSpace(txtLastName.Text) ||
                        string.IsNullOrWhiteSpace(txtUsername.Text) ||
                        string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        MessageBox.Show("Please fill in all required fields.", "Validation Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //string filePath = ofdPicture.FileName;
                    //pbxTeacherProfile.Image = Image.FromFile(filePath);

                    // Validate username uniqueness
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

                        // Execute the stored procedure
                        using (SqlCommand command = new SqlCommand("dbo.SP_AddTeacher", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                            command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            command.Parameters.AddWithValue("@Department", cbxDepartments.SelectedItem.ToString());
                            //command.Parameters.AddWithValue("@Department", cbxDepartments.SelectedIndex.ToString());
                            command.Parameters.AddWithValue("@UserName", txtUsername.Text);
                            command.Parameters.AddWithValue("@Password", txtPassword.Text);
                            command.Parameters.AddWithValue("@PictureDirectory", FilePath);
                            //command.Parameters.AddWithValue("@PictureDirectory", pbxTeacherProfile.ImageLocation ?? "");

                            // Add the new profile fields, why did I forget this earlier? Hahaha
                            command.Parameters.AddWithValue("@Age", txtAge.Text);
                            command.Parameters.AddWithValue("@Gender", cbxGender.SelectedIndex.ToString());
                            command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@Address", txtAddress.Text);
                            command.Parameters.AddWithValue("@Email", txtEmail.Text);
                            command.Parameters.AddWithValue("@RecoveryCode", Convert.ToInt32(txtRecoveryCode.Text));

                            command.ExecuteNonQuery();
                        }

                        _logger.Log(fullName, $"Adds new teacher: {txtFirstName.Text} {txtLastName.Text}", roleID);

                        // Refresh the dgvTeachers table
                        GetData(@"
                        SELECT * FROM dbo.DisplayAllTeachers()
                    ");

                        // Clear form fields
                        ClearField();

                        MessageBox.Show("Teacher added successfully!", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {


                    MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

                
        }

        private void btnUpdateTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //string filePath = ofdPicture.FileName;
                //pbxTeacherProfile.Image = Image.FromFile(filePath);

                // Validate username uniqueness
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
                    using (SqlCommand command = new SqlCommand("dbo.SP_UpdateTeacher", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        command.Parameters.AddWithValue("@Department", cbxDepartments.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@UserName", txtUsername.Text);
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);
                        command.Parameters.AddWithValue("@PictureDirectory", FilePath); 
                        //command.Parameters.AddWithValue("@PictureDirectory", pbxTeacherProfile.ImageLocation ?? "");

                        command.Parameters.AddWithValue("@Age", txtAge.Text);
                        command.Parameters.AddWithValue("@Gender", cbxGender.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@Address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@RecoveryCode", Convert.ToInt32(txtRecoveryCode.Text));

                        command.ExecuteNonQuery();
                    }

                    _logger.Log(fullName, $"Updates the teacher: {txtFirstName.Text} {txtLastName.Text}", roleID);


                    // Refresh the dgvTeachers table
                    GetData(@"
                        SELECT * FROM dbo.DisplayAllTeachers()
                    ");

                    // Clear form fields
                    ClearField();
                    btnUpdateTeacher.Enabled = false; btnDeleteTeacher.Enabled = false;

                    MessageBox.Show("Teacher information updated successfully!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a row is selected
                if (dgvTeachers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a teacher to delete.", "Selection Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the ProfileID from the selected row
                int profileID = Convert.ToInt32(dgvTeachers.SelectedRows[0].Cells["ProfileID"].Value);
                int instructorID = Convert.ToInt32(dgvTeachers.SelectedRows[0].Cells["InstructorID"].Value);

                Console.WriteLine($"btnDeleteTeacher_Click => InstructorID: {instructorID}, profileID: {profileID}");

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this teacher?", "Confirm Deletion",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                // Execute the stored procedure
                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteTeacher", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProfileID", profileID);
                        command.Parameters.AddWithValue("@InstructorID", instructorID);

                        // Execute the stored procedure
                        int returnValue = command.ExecuteNonQuery();
                        if (returnValue > 0)
                        {
                            MessageBox.Show("Failed to delete teacher.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            

                            MessageBox.Show("Teacher deleted successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                _logger.Log(fullName, $"Deletes the teacher: {txtFirstName.Text} {txtLastName.Text}", roleID);


                // Refresh the grid
                GetData(@"SELECT * FROM dbo.DisplayAllTeachers()");
                ClearField();
                btnUpdateTeacher.Enabled = false; btnDeleteTeacher.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void btnSelectPicture_Click(object sender, EventArgs e)
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

                pbxTeacherProfile.Image = Image.FromFile(filePath);
                pbxTeacherProfile.ImageLocation = filePath;
                FilePath = filePath; // connect string to FilePath to link get directory to the stored procedure
                Console.WriteLine("File Path: " + FilePath);
            }
        }

        private void dgvTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            isCellClicked = true;
            txtPassword.Enabled = false; // disable password field when updating a teacher
            // enable the buttons when a row is selected
            btnUpdateTeacher.Enabled = true; btnDeleteTeacher.Enabled = true; btnSelectPicture.Enabled = true;

            // Check if a valid row was clicked
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTeachers.Rows[e.RowIndex];

                // Assuming the DataGridView is bound to a DataTable or similar data source
                txtTeacherID.Text = row.Cells["InstructorID"].Value.ToString();
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtHireDate.Text = row.Cells["HireDate"].Value.ToString();

                // insert the 

                txtUsername.Text = row.Cells["UserName"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();

                if (row.Cells["Gender"].Value.ToString() == "Male") {
                    cbxGender.SelectedIndex = 0;
                }
                else
                {
                    cbxGender.SelectedIndex = 1;
                }

                txtAge.Text = row.Cells["Age"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtRecoveryCode.Text = row.Cells["RecoveryCode"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString(); 
                
                if (row.Cells["Department"].Value.ToString() == "CCS")
                {
                    cbxDepartments.SelectedIndex = 0;
                }

                else
                {
                    cbxDepartments.SelectedIndex = -1;
                }

                    //Load and display the profile picture if the path is available
                    string profilePicturePath = row.Cells["PictureDirectory"].Value.ToString();
                if (!string.IsNullOrEmpty(profilePicturePath) && System.IO.File.Exists(profilePicturePath))
                {
                    pbxTeacherProfile.Image = Image.FromFile(profilePicturePath);
                }
                else
                {
                    pbxTeacherProfile.Image = null; // or set to a default image
                }
            }
        }

        private void ClearField() {
            // Clear form fields
            txtTeacherID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtHireDate.Clear();
            txtAge.Clear();
            cbxGender.SelectedIndex = -1;
            cbxDepartments.SelectedIndex = -1;
            txtPhone.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtRecoveryCode.Clear();
            pbxTeacherProfile.Image = null;

        }

        private void VerifyInputs()
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

            if (string.IsNullOrWhiteSpace(cbxGender.Text) || cbxGender.SelectedIndex == -1)
            {
                errorMessage += "- Gender is required. \n";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(cbxDepartments.Text) || cbxDepartments.SelectedIndex == -1)
            {
                errorMessage += "- Department is required. \n";
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
        }

    }
}
