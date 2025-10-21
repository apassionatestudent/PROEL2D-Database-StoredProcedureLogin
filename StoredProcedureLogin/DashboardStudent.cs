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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace StoredProcedureLogin
{
    public partial class DashboardStudent : Form
    {

        private BindingSource bindingSource;
        private DataSet dataSet;

        private BindingSource StudentBindingSource = new BindingSource();
        private BindingSource CoursesBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;


        string userName;
        int studentID = 0;
        public DashboardStudent(string username)
        {
            InitializeComponent();
            userName = username;

            // disable them as they shouldn't be edited 
            txtStudentID.Enabled = false; txtAge.Enabled = false;
        }

        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

        private void GetData(string selectCommand)
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
                    StudentBindingSource.DataSource = table;

                    // Bind DataGridView to BindingSource
                    dgvDataReference.DataSource = StudentBindingSource;

                    // Set Fill mode for all columns except one
                    foreach (DataGridViewColumn column in dgvDataReference.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    // Set the last column to fill remaining space
                    int lastColumnIndex = dgvDataReference.Columns.Count - 1;
                    dgvDataReference.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvDataReference.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void LoadCourses (string selectCommand)
        {
            try
            {

                // Create connection and command objects
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create command object
                    SqlCommand command = new SqlCommand(selectCommand, connection);

                    // Add parameter safely
                    SqlParameter param = new SqlParameter("@StudentID", SqlDbType.NVarChar, 50);
                    param.Value = studentID;
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



        private void DashboardStudent_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Click => DashboardStudent_Load");
            Console.WriteLine($"Usename: {userName}");

            //GetData(@"SELECT * FROM dbo.LoadStudentProfile(@Username)");
            GetData("EXEC [dbo].[SP_LoadStudentProfile] @Username");

            txtStudentID.Text = dgvDataReference.Rows[0].Cells["StudentID"].Value.ToString();
            txtFirstName.Text = dgvDataReference.Rows[0].Cells["FirstName"].Value.ToString();
            txtLastName.Text = dgvDataReference.Rows[0].Cells["LastName"].Value.ToString();
            txtAge.Text = dgvDataReference.Rows[0].Cells["Age"].Value.ToString();

            studentID = (int)Convert.ToInt64(txtStudentID.Text);

            LoadCourses("EXEC [dbo].[SP_LoadStudentCourses] @StudentID");
           
        }
    }
}
