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
    public partial class TeacherDashboard : Form
    {
        private string _profilePictureString;

        private string fullName;
        private int roleID;

        private readonly Logger _logger;

        public TeacherDashboard(string username, string profilePictureString, string FullName, int RoleID)
        {
            InitializeComponent();
            lblUsername.Text = username;
            _profilePictureString = profilePictureString;
            fullName = FullName;
            roleID = RoleID;

            // connect => Logger.cs
            _logger = new Logger();
        }
        private void TeacherDashboard_Load(object sender, EventArgs e)
        {
            pbxPictureProfile.Image = System.Drawing.Image.FromFile(_profilePictureString);
            Console.WriteLine("TEACHER Profile Picture Path: " + _profilePictureString);

            LoadForm(new TeacherProfile(lblUsername.Text));
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

        private void btnProfile_Click(object sender, EventArgs e)
        {
            LoadForm(new TeacherProfile(lblUsername.Text));
        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            LoadForm(new TeacherSubjects(lblUsername.Text));
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            LoadForm(new TeacherStudents(lblUsername.Text));
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click => btnLogOut_Click");
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Hide();
                new LoginForm().Show();

                Console.WriteLine($"fullName => {fullName}");
                Console.WriteLine($"roleID => {roleID}");

                //_logger.Log(fullName, "Logs out", roleID);
            }
        }
    }
}
