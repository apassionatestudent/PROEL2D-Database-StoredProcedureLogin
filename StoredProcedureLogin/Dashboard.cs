using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace StoredProcedureLogin
{
    public partial class Dashboard : Form
    {

        private string _profilePictureString;

        private BindingSource CoursesBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;

        private readonly Logger _logger;
        private string fullName;
        private int roleID;

        string userName;
        int studentID = 0; // I need to fill this it for adding courses later
        public Dashboard(string username, string profilePictureString, string FullName, int RoleID)
        {
            InitializeComponent();
            lblUsername.Text = username;
            userName = username;

            fullName = FullName;
            roleID = RoleID;

            // connect => Logger.cs
            _logger = new Logger();


            _profilePictureString = profilePictureString;
            // sample only to test file directory 
            //pbxProfilePic.Image = System.Drawing.Image.FromFile("./Resources/ProfilePictures/asdas.jpg");
        }

        // container for other panels 
        public void LoadForm(object Form)
        {
            if (this.pnlMain.Controls.Count > 0)
            {
                this.pnlMain.Controls.RemoveAt(0);
            }

            Form f = Form as Form;
            f.TopLevel = false;

            f.Dock = DockStyle.Fill;
            this.pnlMain.Controls.Add(f);
            this.pnlMain.Tag = f;
            f.Show();
        }

        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

        private void LoadStudentID()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create command object
                    SqlCommand command = new SqlCommand("SP_GetStudentID", connection);
                    command.CommandType = CommandType.StoredProcedure; // Specify command type

                    // Add parameter safely
                    SqlParameter param = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
                    param.Value = userName;
                    command.Parameters.Add(param);

                    // Execute the stored procedure and read the result
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Store the StudentID from the third column
                        studentID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        Console.WriteLine("Loaded StudentID: " + studentID);    
                    }

                    reader.Close();
                }


            }


            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }


        private void Dashboard_Load(object sender, EventArgs e)
        {

            Console.WriteLine("Click => Dashboard_Load");
            Console.WriteLine("Profile Picture Path: " + _profilePictureString);
            LoadForm(new DashboardStudent(userName));
            // sample only to test file directory 
            pbxProfilePic.Image = System.Drawing.Image.FromFile(_profilePictureString);
            LoadStudentID();
            

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click => btnProfile_Click");
            LoadForm(new DashboardStudent(userName));

        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click => btnCourses_Click");
            LoadForm(new DashboardCourses(userName, studentID, fullName, roleID));

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click => btnLogOut_Click");
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Hide();
                new LoginForm().Show();

                _logger.Log(fullName, "Logs out", roleID);
            }
        }


    }
}
