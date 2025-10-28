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
    public partial class TeacherSubjects : Form
    {

        private BindingSource CoursesBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;

        string userName;
        public TeacherSubjects( string username)
        {
            InitializeComponent();
            userName = username;
        }

        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

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

        private void TeacherSubjects_Load(object sender, EventArgs e)
        {
            // Load the subjects under the said teacher 
            LoadCourses("EXEC [dbo].[SP_LoadTeacherCourses] @Username");
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

                    // 0 to check the first column only, which is CourseID 
                    string currentValue = dgvCourses.Rows[rowIndex].Cells[0].Value?.ToString()?.ToLower();

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
                    "Information found!" :
                    "No matching records found. Please ensure it's a correct course ID.",
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
    }
}
