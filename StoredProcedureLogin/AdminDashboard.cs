using StoredProcedureLogin.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoredProcedureLogin
{
    public partial class AdminDashboard : Form
    {
        private string username;
        private string _profilePictureString;
        private string fullName;
        private int roleID;

        private readonly Logger _logger;
        

        public AdminDashboard(string username, string profilePictureString, string FullName, int RoleID)
        {
            InitializeComponent();
            this.username = username;
            lblName.Text = username;

            fullName = FullName;
            roleID = RoleID;
            _profilePictureString = profilePictureString;

            // connect => Logger.cs
            _logger = new Logger();
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

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            LoadForm(new AdminDashboardPanelPage());
            pbxProfilePicture.Image = System.Drawing.Image.FromFile(_profilePictureString);
            Console.WriteLine("ADMIN Profile Picture Path: " + _profilePictureString);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminDashboardPanelPage());
            
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminStudentPage(fullName, roleID));
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminTeachersPage(fullName, roleID));
        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminSubjectsPage(fullName, roleID));
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminReportsPage());
        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminLogsPage());
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
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
