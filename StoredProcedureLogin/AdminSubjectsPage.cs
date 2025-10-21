using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace StoredProcedureLogin
{
    public partial class AdminSubjectsPage : Form
    {

        private BindingSource StudentBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;

        string errorMessage = string.Empty;
        int errorCounter = 0;

        bool isCellClicked = false;

        int InstructorID = 0;   
        int DepartmentID = 0;

        private readonly Logger _logger;
        private string fullName;
        private int roleID;

        // Grr... for complicatd InstructorID retrieval and its association per teacher 
        private Dictionary<string, int> instructorDictionary = new Dictionary<string, int>();
        private Dictionary<string, int> departmentDictionary = new Dictionary<string, int>();

        public AdminSubjectsPage(string FullName, int RoleID)
        {
            InitializeComponent();

            btnUpdate.Enabled = false; btnDelete.Enabled = false;

            fullName = FullName;
            roleID = RoleID;

            // connect => Logger.cs
            _logger = new Logger();

            // Load ComboBox items dynamically from database
            LoadComboBoxDataDeparments();
            LoadComboBoxDataTeachers();
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
                dgvSubjects.DataSource = StudentBindingSource;

                // Set Fill mode for all columns except one
                foreach (DataGridViewColumn column in dgvSubjects.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Set the last column to fill remaining space
                int lastColumnIndex = dgvSubjects.Columns.Count - 1;
                dgvSubjects.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvSubjects.AutoResizeColumns();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }

        private void LoadComboBoxDataTeachers()
        {

            string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
            string query = @"
                SELECT DISTINCT 
                    p.FirstName AS InstructorName,
                    i.InstructorID
                FROM Profiles p 
                INNER JOIN Instructors i ON p.ProfileID = i.ProfileID";

            instructorDictionary.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string instructorName = reader["InstructorName"].ToString();
                                int instructorId = Convert.ToInt32(reader["InstructorID"]);

                                cbxTeachers.Items.Add(instructorName);
                                instructorDictionary[instructorName] = instructorId;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error loading instructors: {ex.Message}");
            }
        }

        // Return those IDs from Instructors and Departments dynamically from the database 
        private int GetSelectedInstructorID()
        {
            if (cbxTeachers.SelectedIndex >= 0 && instructorDictionary.ContainsKey(cbxTeachers.Text))
            {
                return instructorDictionary[cbxTeachers.Text];
            }
            return 0;
        }

        private int GetSelectedDepartmentID()
        {
            if (cbxDepartment.SelectedIndex >= 0 && departmentDictionary.ContainsKey(cbxDepartment.Text))
            {
                return departmentDictionary[cbxDepartment.Text];
            }
            return 0;
        }

        private void LoadComboBoxDataDeparments()
        {
            string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
            //string query = "SELECT DISTINCT DepartmentName FROM Departments";
            string query = @"
                SELECT DISTINCT 
                    d.DepartmentName AS DepartmentName,
                    d.DepartmentID
                FROM Departments d";

            departmentDictionary.Clear();


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                string departmentName = reader["departmentName"].ToString();
                                int departmentID = Convert.ToInt32(reader["departmentID"]);

                                cbxDepartment.Items.Add(departmentName);
                                departmentDictionary[departmentName] = departmentID;
                                //instructorDictionary[departmentName] = departmentID;

                                //cbxDepartment.Items.Add(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }


        private void AdminSubjectsPage_Load(object sender, EventArgs e)
        {
            // load subjects here 
            GetData(@"
                SELECT * FROM dbo.DisplayAllSubjects()
            ");

        }

        private void btnSearch_Click(object sender, EventArgs e)
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
                foreach (DataGridViewRow row in dgvSubjects.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.Selected = false;
                    }
                }

                bool foundMatch = false;

                for (int rowIndex = 0; rowIndex < dgvSubjects.Rows.Count; rowIndex++)
                {
                    if (dgvSubjects.Rows[rowIndex].IsNewRow)
                        continue;

                    // 0 to check the first column only, which is CourseID 
                    string currentValue = dgvSubjects.Rows[rowIndex].Cells[0].Value?.ToString()?.ToLower();

                    if (!string.IsNullOrEmpty(currentValue) &&
                        currentValue == searchText)
                    {
                        dgvSubjects.Rows[rowIndex].Selected = true;
                        dgvSubjects.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                        dgvSubjects.FirstDisplayedScrollingRowIndex = rowIndex;
                        dgvSubjects_CellClick(dgvSubjects, new DataGridViewCellEventArgs(0, rowIndex));
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnAdd_Click => Clicked indeed!");

            if (isCellClicked == true)
            {
                //clear the fields first before adding a new teacher
                ClearField();
                isCellClicked = false;
            }

            else
            {
                // Check if inputs are correct for adding subject 
                VerifyInputs();

                try
                {
                    

                    string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if username already exists
                        SqlCommand cmdCheckCourseName = new SqlCommand(@"
                            SELECT CASE WHEN EXISTS (
                                SELECT 1 FROM Users WHERE Username = @CourseName
                            ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END", connection);

                        cmdCheckCourseName.Parameters.AddWithValue("@CourseName", txtSubject.Text);

                        bool courseNameExists = (bool)cmdCheckCourseName.ExecuteScalar();

                        if (courseNameExists)
                        {
                            MessageBox.Show("The course already exists. Please choose a different course name.",
                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Execute stored procedure to add subject
                        using (SqlCommand command = new SqlCommand("dbo.SP_AddSubject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@CourseName", txtSubject.Text);
                            command.Parameters.AddWithValue("@CourseCode", txtCourseCode.Text);
                            command.Parameters.AddWithValue("@Credits", Convert.ToDouble(txtCredits.Text));
                            command.Parameters.AddWithValue("@Description", txtDetails.Text);

                            // Add InstructorID parameter
                            int instructorId = GetSelectedInstructorID();
                            command.Parameters.AddWithValue("@InstructorID", instructorId);

                            int departmentId = GetSelectedDepartmentID();
                            command.Parameters.AddWithValue("@DepartmentID", departmentId);


                            // Execute the command and get the new CourseID hmpph
                            int newCourseID = (int)command.ExecuteScalar();
                            Console.WriteLine($"Course added successfully with ID: {newCourseID}");

                            _logger.Log(fullName, $"Adds new subhect: {txtSubject.Text} ", roleID);

                            // Refresh the grid to show the new course
                            GetData(@"
                                SELECT * FROM dbo.DisplayAllSubjects()
                            ");

                            ClearField();
                            MessageBox.Show("Course added successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error adding course: " + ex.Message);
                }

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the update is valid 
                VerifyInputs();

                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.SP_UpdateSubject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Get the selected CourseID from the DataGridView
                        if (dgvSubjects.CurrentRow != null)
                        {
                            int selectedCourseID = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["CourseID"].Value);
                            command.Parameters.AddWithValue("@CourseID", selectedCourseID);
                            command.Parameters.AddWithValue("@CourseName", txtSubject.Text);
                            command.Parameters.AddWithValue("@CourseCode", txtCourseCode.Text);
                            command.Parameters.AddWithValue("@Credits", Convert.ToDouble(txtCredits.Text));
                            command.Parameters.AddWithValue("@Description", txtDetails.Text);
                            // Add InstructorID parameter
                            int instructorId = GetSelectedInstructorID();
                            command.Parameters.AddWithValue("@InstructorID", instructorId);
                            int departmentId = GetSelectedDepartmentID();
                            command.Parameters.AddWithValue("@DepartmentID", departmentId);
                            // Execute the update command
                            command.ExecuteNonQuery();
                            Console.WriteLine($"Course with ID: {selectedCourseID} updated successfully.");
                            // Refresh the grid to show the updated course

                            _logger.Log(fullName, $"Updates the subhect: {txtSubject.Text} ", roleID);

                            GetData(@"
                                SELECT * FROM dbo.DisplayAllSubjects()
                            ");
                            ClearField();
                            MessageBox.Show("Course updated successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No course selected for update.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try {

                // Check if a row is selected
                if (dgvSubjects.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a teacher to delete.", "Selection Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the ProfileID from the selected row
                int courseID = Convert.ToInt32(dgvSubjects.SelectedRows[0].Cells["CourseID"].Value);

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this course?", "Confirm Deletion",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                // Execute the stored procedure
                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteSubject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CourseID", courseID);

                        // Execute the stored procedure
                        int returnValue = command.ExecuteNonQuery();
                        if (returnValue > 0)
                        {
                            MessageBox.Show("Failed to delete course.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Course has been deleted successfully!", "Sumakses!",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                _logger.Log(fullName, $"Deletes the subhect: {txtSubject.Text} ", roleID);

                // Refresh the grid
                GetData(@"SELECT * FROM dbo.DisplayAllSubjects()");

                ClearField();
                btnUpdate.Enabled = false; btnDelete.Enabled = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            isCellClicked = true;

            btnUpdate.Enabled = true; btnDelete.Enabled = true;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSubjects.Rows[e.RowIndex];
                txtSubject.Text = row.Cells["CourseName"].Value.ToString();
                txtCourseCode.Text = row.Cells["CourseCode"].Value.ToString();
                txtCredits.Text = row.Cells["Credits"].Value.ToString();


                txtDetails.Text = row.Cells["Description"].Value.ToString();

                // Add InstructorID parameter
                int instructorId = GetSelectedInstructorID();

                // Get the instructor name from the grid
                string instructorName = row.Cells["FirstName"].Value.ToString();

                // Find the corresponding index in the ComboBox
                int selectedIndex = -1;
                for (int i = 0; i < cbxTeachers.Items.Count; i++)
                {
                    if (instructorDictionary.ContainsKey(cbxTeachers.Items[i].ToString()))
                    {
                        if (cbxTeachers.Items[i].ToString() == instructorName)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                }

                // Only set SelectedIndex if we found a match
                if (selectedIndex != -1)
                {
                    cbxTeachers.SelectedIndex = selectedIndex;
                }
                else
                {
                    // Handle case where instructor is not found
                    MessageBox.Show($"Warning: Instructor '{instructorName}' not found in the available instructors list.");
                }

                // Get the department name from the grid
                string departmentName = row.Cells["DepartmentName"].Value.ToString();

                // Find the corresponding index in the ComboBox
                //int selectedIndex = -1;
                for (int i = 0; i < cbxDepartment.Items.Count; i++)
                {
                    if (departmentDictionary.ContainsKey(cbxDepartment.Items[i].ToString()))
                    {
                        if (cbxDepartment.Items[i].ToString() == departmentName)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                }

                // Only set SelectedIndex if we found a match
                if (selectedIndex != -1)
                {
                    cbxDepartment.SelectedIndex = selectedIndex;
                }
                else
                {
                    // Handle case where department is not found
                    MessageBox.Show($"Warning: Department '{departmentName}' not found in the available departments list.");
                }
            }
        }

        private void ClearField()
        {
            txtSubject.Clear();
            txtDetails.Clear();
            txtCourseCode.Clear();
            txtCredits.Clear();
            cbxTeachers.SelectedIndex = -1;
            cbxDepartment.SelectedIndex = -1;
        }   

        private void VerifyInputs()
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                errorMessage += "- Subject is required. \n";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(txtCredits.Text))
            {
                errorMessage += "- Credits is required. \n";
                errorCounter++;
            }

            if (!decimal.TryParse(txtCredits.Text, out decimal result))
            {
                errorMessage += "- Credits can\'t be converted to decimal. \n";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(cbxTeachers.Text) || cbxTeachers.SelectedIndex == -1)
            {
                errorMessage += "- Teacher is required. \n";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(cbxDepartment.Text) || cbxDepartment.SelectedIndex == -1)
            {
                errorMessage += "- Department is required. \n";
                errorCounter++;
            }

            if (string.IsNullOrWhiteSpace(txtDetails.Text))
            {
                errorMessage += "- Subject Details is required. \n";
                errorCounter++;
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
