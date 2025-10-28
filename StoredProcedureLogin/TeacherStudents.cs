using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace StoredProcedureLogin
{
    public partial class TeacherStudents : Form
    {

        private BindingSource CoursesBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;

        private Dictionary<string, int> subjectsDictionary = new Dictionary<string, int>();

        string userName;

        public TeacherStudents(string username)
        {
            InitializeComponent();

            userName = username;
        }

        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

        private void LoadStudents(string selectCommand)
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
                    dgvStudents.DataSource = CoursesBindingSource;

                    // Set Fill mode for all columns except one
                    foreach (DataGridViewColumn column in dgvStudents.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    // Set the last column to fill remaining space
                    int lastColumnIndex = dgvStudents.Columns.Count - 1;
                    dgvStudents.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvStudents.AutoResizeColumns();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coures => Error loading data: " + ex.Message);
            }
        }

        private void LoadCourses()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("EXEC [dbo].[SP_ComboBoxLoadTeacherCourses] @Username", connection);
                    command.Parameters.AddWithValue("@Username", userName);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        cbxSubject.Items.Clear();
                        subjectsDictionary.Clear();

                        while (reader.Read())
                        {
                            int courseId = reader.GetInt32(0);
                            string courseCode = reader.GetString(1);

                            // Store just the CourseCode
                            cbxSubject.Items.Add(courseCode);
                            subjectsDictionary[courseCode] = courseId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}");
            }
        }

        private void LoadCoursesPerStudent(string selectCommand)
        {
            try
            {

                // Create connection and command objects
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create command object
                    SqlCommand command = new SqlCommand(selectCommand, connection);

                    // Add parameter safely
                    SqlParameter param = new SqlParameter("@Username", SqlDbType.NVarChar, 70);
                    SqlParameter param2 = new SqlParameter("@CourseCode", SqlDbType.NVarChar, 70);

                    // Assign values to parameters, damn 
                    param.Value = userName;
                    param2.Value = cbxSubject.SelectedItem.ToString();
                    command.Parameters.Add(param);
                    command.Parameters.Add(param2);

                    // Create data adapter with the prepared command
                    dataAdapter = new SqlDataAdapter(command);

                    // Fill dataset and bind to BindingSource
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    CoursesBindingSource.DataSource = table;

                    // Bind DataGridView to BindingSource
                    dgvStudents.DataSource = CoursesBindingSource;

                    // Set Fill mode for all columns except one
                    foreach (DataGridViewColumn column in dgvStudents.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    // Set the last column to fill remaining space
                    int lastColumnIndex = dgvStudents.Columns.Count - 1;
                    dgvStudents.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvStudents.AutoResizeColumns();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coures => Error loading data: " + ex.Message);
            }
        }

        private void TeacherStudents_Load(object sender, EventArgs e)
        {
            Console.WriteLine($"TeacherStudents_Load Username => {userName}");
            LoadStudents("EXEC [dbo].[SP_LoadTeacherStudents] @Username");
            LoadCourses();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadStudents("EXEC [dbo].[SP_LoadTeacherStudents] @Username");
            cbxSubject.SelectedIndex = -1; // Back to default
        }


        private void cbxSubject_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cbxSubject.SelectedIndex >= 0)
                {
                    string selectedCourseCode = cbxSubject.SelectedItem.ToString();
                    LoadCoursesPerStudent(@"EXEC [dbo].[SP_LoadTeacherStudentsByCourse] @Username, @CourseCode;");
                    //DisplayByCourse(userName, selectedCourseCode);
                }
                else
                {
                    LoadStudents("EXEC [dbo].[SP_LoadTeacherStudents] @Username");
                    
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering students: {ex.Message}");
            }
        }
    }
}