using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StoredProcedureLogin
{
    public partial class AdminLogsPage : Form
    {

        private BindingSource StudentBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;

        // Enable of one search at a time only, don't want it to be complicated 
        bool isRoleClicked = false;
        bool isNameClicked = false;
        bool isDateClicked = false;
        public AdminLogsPage()
        {
            InitializeComponent();

            dtpDateFrom.Format = DateTimePickerFormat.Custom;
            dtpDateTo.Format = DateTimePickerFormat.Custom;

            dtpDateFrom.CustomFormat = "MM/dd/yyyy";
            dtpDateTo.CustomFormat = "MM/dd/yyyy";
        }

        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";


        public void GetData(string GetCommand)
        {
            try
            {
                //string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                // Create data adapter
                dataAdapter = new SqlDataAdapter(GetCommand, connectionString);

                // Fill dataset and bind to BindingSource
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                StudentBindingSource.DataSource = table;

                // Bind DataGridView to BindingSource
                dgvLogs.DataSource = StudentBindingSource;

                // Set Fill mode for all columns except one
                foreach (DataGridViewColumn column in dgvLogs.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Set the last column to fill remaining space
                int lastColumnIndex = dgvLogs.Columns.Count - 1;
                dgvLogs.Columns[lastColumnIndex].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;

                dgvLogs.AutoResizeColumns();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }


       
        private void AdminLogsPage_Load(object sender, EventArgs e)
        {
            GetData(@"
                SELECT * FROM dbo.DisplayLogs()
            ");
        }

        private void UpdateUIBasedOnSelections()
        {
            // Enable/disable controls based on selections
            btnReset.Enabled = isRoleClicked || isNameClicked || isDateClicked;

            // Visual feedback for selected filters
            cbxFilterRoles.BackColor = isRoleClicked ? Color.LightBlue : SystemColors.Window;
            txtName.BackColor = isNameClicked ? Color.LightBlue : SystemColors.Window;

            if (isDateClicked)
            {
                dtpDateFrom.BackColor = Color.LightBlue;
                dtpDateTo.BackColor = Color.LightBlue;
            }
            else
            {
                dtpDateFrom.BackColor = SystemColors.Window;
                dtpDateTo.BackColor = SystemColors.Window;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //string SelectedRole = String.Empty;
            //string SearchName = String.Empty;

            //if (cbxFilterRoles.SelectedIndex == 0)
            //{
            //    SelectedRole = "Admin";
            //}

            //else if (cbxFilterRoles.SelectedIndex == 1)
            //{
            //    SelectedRole = "Teacher";
            //}

            //else
            //{
            //    SelectedRole = "Student";
            //}

            // Clear previous filters
            isRoleClicked = false;
            isNameClicked = false;
            isDateClicked = false;

            // Start with all rows visible
            foreach (DataGridViewRow row in dgvLogs.Rows)
            {
                row.Visible = true;
            }

            // Apply role filter if selected
            if (cbxFilterRoles.SelectedIndex > -1)
            {
                string SelectedRole = cbxFilterRoles.SelectedItem.ToString();
                foreach (DataGridViewRow row in dgvLogs.Rows)
                {
                    if (row.Cells["RoleName"].Value?.ToString() != SelectedRole)
                    {
                        row.Visible = false;
                    }
                }
                isRoleClicked = true;
            }

            // Apply name filter if text exists
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                string searchName = txtName.Text.ToLower();
                foreach (DataGridViewRow row in dgvLogs.Rows)
                {
                    if ((bool)!row.Cells["Name"].Value?.ToString().ToLower().Contains(searchName))
                    {
                        row.Visible = false;
                    }
                }
                isNameClicked = true;
            }

            // Apply date range filter if dates are set
            if (dtpDateFrom.Value != DateTime.MinValue && dtpDateTo.Value != DateTime.MinValue)
            {
                DateTime startDate = dtpDateFrom.Value;
                DateTime endDate = dtpDateTo.Value;

                foreach (DataGridViewRow row in dgvLogs.Rows)
                {
                    DateTime logDate;
                    if (DateTime.TryParse(row.Cells["LogDate"].Value?.ToString(), out logDate))
                    {
                        if (logDate < startDate || logDate > endDate)
                        {
                            row.Visible = false;
                        }
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
                isDateClicked = true;



                //if (isRoleClicked)
                //{

                //}

                //else if (isNameClicked)
                //{

                //}

                //else if (isDateClicked)
                //{

                //}

                //else {
                //    MessageBox.Show("Please select a filter option before searching.", "No Filter Selected!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
            }
        }

        private void cbxFilterRoles_Click(object sender, EventArgs e)
        {
            isRoleClicked = true; 

        }

        private void txtName_Click(object sender, EventArgs e)
        {
            isNameClicked = true; 
        }

        private void dtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            isDateClicked = true;
            Console.WriteLine($"Date From => {dtpDateFrom.Text}");
        }

        private void dtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            isDateClicked = true;
            Console.WriteLine($"Date To => {dtpDateTo.Text}");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            GetData(@"
                SELECT * FROM dbo.DisplayLogs()
            ");
        }
    }

}
