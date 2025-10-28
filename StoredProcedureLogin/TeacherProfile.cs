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
    public partial class TeacherProfile : Form
    {
        string userName;

        private BindingSource StudentBindingSource = new BindingSource();
        private BindingSource CoursesBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;
        public TeacherProfile(string username)
        {
            InitializeComponent();
            userName = username;
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

        private void TeacherProfile_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Click => TeacherProfile_Load");
            Console.WriteLine($"Usename: {userName}");

            GetData("EXEC [dbo].[SP_LoadTeacherProfile] @Username");

            txtTeacherID.Text = dgvDataReference.Rows[0].Cells["InstructorID"].Value.ToString();
            txtFirstName.Text = dgvDataReference.Rows[0].Cells["FirstName"].Value.ToString();
            txtLastName.Text = dgvDataReference.Rows[0].Cells["LastName"].Value.ToString();
            txtAge.Text = dgvDataReference.Rows[0].Cells["Age"].Value.ToString();

        }
    }
}
