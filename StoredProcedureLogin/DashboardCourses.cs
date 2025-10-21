using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace StoredProcedureLogin
{
    public partial class DashboardCourses : Form
    {

        private BindingSource CoursesBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;

        string userName = string.Empty;    

        bool isAvailableCoursesClicked = true;
        bool isEnrolledCoursesClicked = false;
        bool isCellClicked = false;

        int StudentID = 0;
        int CourseID = 0;

        string[] EnrollArray;
        string[] DropArray;

        private readonly Logger _logger;
        private string fullName;
        private int roleID;

        public DashboardCourses(string userame, int studentID, string FullName, int RoleID)
        {
            InitializeComponent();
            userName = userame;
            btnDrop.Enabled = false;

            fullName = FullName;
            roleID = RoleID;

            // connect => Logger.cs
            _logger = new Logger();

            StudentID = studentID;
            Console.WriteLine($"Courses StudentID => {StudentID}");
            
        }
        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

        private void GetData(string selectCommand)
        {
            try
            {

                // Create data adapter
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Fill dataset and bind to BindingSource
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                CoursesBindingSource.DataSource = table;

                // Bind DataGridView to BindingSource
                dgvCourses.DataSource = CoursesBindingSource;

                // Set Fill mode for all columns except one
                foreach (DataGridViewColumn column in dgvCourses.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Set the last column to fill remaining space
                int lastColumnIndex = dgvCourses.Columns.Count - 1;
                dgvCourses.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvCourses.AutoResizeColumns();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }

        private void LoadCourses(string selectCommand)
        {
            try
            {

                // Create connection and command objects
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create command object
                    SqlCommand command = new SqlCommand(selectCommand, connection);

                    // Add parameter safely
                    SqlParameter param = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
                    param.Value = userName;
                    command.Parameters.Add(param);

                    // Create data adapter with the prepared command
                    dataAdapter = new SqlDataAdapter(command);

                    // Fill dataset and bind to BindingSource
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    CoursesBindingSource.DataSource = table;

                    // Bind DataGridView to BindingSource
                    dgvCourses.DataSource = CoursesBindingSource;

                    // Set Fill mode for all columns except one
                    foreach (DataGridViewColumn column in dgvCourses.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    // Set the last column to fill remaining space
                    int lastColumnIndex = dgvCourses.Columns.Count - 1;
                    dgvCourses.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvCourses.AutoResizeColumns();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coures => Error loading data: " + ex.Message);
            }
        }



        private void DashboardCourses_Load(object sender, EventArgs e)
        {
            GetData(@"SELECT * FROM dbo.DisplayAvailableCourses()");
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
                foreach (DataGridViewRow row in dgvCourses.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.Selected = false;
                    }
                }

                bool foundMatch = false;

                for (int rowIndex = 0; rowIndex < dgvCourses.Rows.Count; rowIndex++)
                {
                    if (dgvCourses.Rows[rowIndex].IsNewRow)
                        continue;

                    // 2 to check the first column only, which is CourseCode 
                    string currentValue = dgvCourses.Rows[rowIndex].Cells[2].Value?.ToString()?.ToLower();

                    if (!string.IsNullOrEmpty(currentValue) &&
                        currentValue == searchText)
                    {
                        dgvCourses.Rows[rowIndex].Selected = true;
                        dgvCourses.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                        dgvCourses.FirstDisplayedScrollingRowIndex = rowIndex;
                        foundMatch = true;
                    }
                }

                MessageBox.Show(foundMatch ?
                    "Information found based on CoursedCode!" :
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

        private void btnAvailableCourses_Click(object sender, EventArgs e)
        {
            GetData(@"SELECT * FROM dbo.DisplayAvailableCourses()");
            btnEnroll.Enabled = true; btnDrop.Enabled = false;
            isEnrolledCoursesClicked = false; isAvailableCoursesClicked = true;
            isCellClicked = false;

        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnEnroll_Click => Clicked indeed!");

            if (isCellClicked == true)
            {
                isCellClicked = false;
            }

            else
            {
                try
                {
                    string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if student is already enrolled in the course
                        using (SqlCommand checkCommand = new SqlCommand(@"
                            SELECT CASE 
                                WHEN EXISTS (
                                    SELECT 1 
                                    FROM Enrollment 
                                    WHERE StudentID = @StudentID 
                                    AND CourseID = @CourseID
                                ) THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END", connection))
                        {
                            checkCommand.Parameters.AddWithValue("@StudentID", EnrollArray[0]);
                            checkCommand.Parameters.AddWithValue("@CourseID", EnrollArray[1]);

                            bool isAlreadyEnrolled = (bool)checkCommand.ExecuteScalar();

                            if (isAlreadyEnrolled)
                            {
                                MessageBox.Show("You are already enrolled in this course.",
                                    "Enrollment Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                return;
                            }

                        }
                            // Execute stored procedure to add subject
                        using (SqlCommand command = new SqlCommand("dbo.EnrollSubject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@StudentID", EnrollArray[0]);
                            command.Parameters.AddWithValue("@CourseID", EnrollArray[1]);
                            command.Parameters.AddWithValue("@SemesterID", EnrollArray[2]);
                            command.Parameters.AddWithValue("@Grade", EnrollArray[3]);

                            // Execute the stored procedure
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Failed to enroll in course.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("Course enrolled successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                _logger.Log(fullName, $"{fullName} enrolls himself to CourseID: {EnrollArray[1]}", roleID);
                            }

                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error adding course: " + ex.Message);
                }
            }
        }

        private void btnEnrolledCourses_Click(object sender, EventArgs e)
        {
            LoadCourses("EXEC [dbo].[SP_LoadStudentCoursesV2] @Username");
            btnEnroll.Enabled = false; btnDrop.Enabled = true;
            isEnrolledCoursesClicked = true; isAvailableCoursesClicked = false;
            isCellClicked = false;
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvCourses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course to drop.", "Selection Required",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the ProfileID from the selected row
            int enrollmentID = Convert.ToInt32(dgvCourses.SelectedRows[0].Cells["EnrollmentID"].Value);

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to drop this course?", "Confirm Drop",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DeleteEnrollment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                    // Execute the stored procedure
                    int returnValue = command.ExecuteNonQuery();
                    if (returnValue > 0)
                    {
                        MessageBox.Show("Failed to drop course.", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Course has been drop successfully!", "Sumakses!",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _logger.Log(fullName, $"{fullName} de-enrolls himself to CourseID: {EnrollArray[1]}", roleID);


                        // Refresh the grid
                        LoadCourses("EXEC [dbo].[SP_LoadStudentCoursesV2] @Username");
                    }
                }
            }
        }

        private void dgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCourses.Rows[e.RowIndex];

                if (isAvailableCoursesClicked)
                {

                    // Initialize EnrollArray with 4 elements first to wprk
                    EnrollArray = new string[4];

                    EnrollArray[0] = Convert.ToString(StudentID);
                    EnrollArray[1] = row.Cells["CourseID"].Value.ToString(); // CourseID 
                    EnrollArray[2] = "1"; // Let's put it to one, I don't give a damn 
                    EnrollArray[3] = "1.0"; // Default on 1.0 

                    Console.WriteLine($"StudentID => {EnrollArray[0]}");
                    Console.WriteLine($"CourseID => {EnrollArray[1]}");
                    Console.WriteLine($"SemesterID => {EnrollArray[2]}");
                    Console.WriteLine($"Grade => {EnrollArray[3]}");

                }

            }
        }
    }
}
