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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


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

   
        // Helps the search process, null by default
        string roleName = string.Empty;
        //string name = null;
        //string dateFrom;
        //string dateTo;

        private BindingSource LogsBindingSource = new BindingSource();

        public AdminLogsPage()
        {
            InitializeComponent();

            dtpDateFrom.Format = DateTimePickerFormat.Custom;
            dtpDateTo.Format = DateTimePickerFormat.Custom;

            //dtpDateFrom.CustomFormat = "YYYY/mm/dd | MM/dd/yyyy";
            //dtpDateTo.CustomFormat = "MM/dd/yyyy";

            dtpDateFrom.CustomFormat = "yyyy-MM-dd";
            dtpDateTo.CustomFormat = "yyyy-MM-dd";

            //dtpDateFrom.Value = DateTimePicker.MinimumDateTime;
            //dtpDateTo.Value = DateTimePicker.MinimumDateTime;
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

        private void LoadQueryLogs(string selectCommand)
        {
            try
            {

                // Create connection and command objects
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create command object
                    SqlCommand command = new SqlCommand(selectCommand, connection);

                    // Add parameter safely
                    SqlParameter param = new SqlParameter("@Role", SqlDbType.NVarChar, 50);
                    SqlParameter param2 = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
                    SqlParameter param3 = new SqlParameter("@DateFrom", SqlDbType.NVarChar, 200);
                    SqlParameter param4 = new SqlParameter("@DateTo", SqlDbType.NVarChar, 200);

                    // Assign values to parameters, damn 
                    param.Value = roleName;
                    param2.Value = txtName.Text;
                    param3.Value = dtpDateFrom.Text;
                    param4.Value = dtpDateTo.Text;

                    command.Parameters.Add(param);
                    command.Parameters.Add(param2);
                    command.Parameters.Add(param3);
                    command.Parameters.Add(param4);

                    // Create data adapter with the prepared command
                    dataAdapter = new SqlDataAdapter(command);

                    // Fill dataset and bind to BindingSource
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    LogsBindingSource.DataSource = table;

                    // Bind DataGridView to BindingSource
                    dgvLogs.DataSource = LogsBindingSource;

                    // Set Fill mode for all columns except one
                    foreach (DataGridViewColumn column in dgvLogs.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    // Set the last column to fill remaining space
                    int lastColumnIndex = dgvLogs.Columns.Count - 1;
                    dgvLogs.Columns[lastColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvLogs.AutoResizeColumns();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coures => Error loading data: " + ex.Message);
            }
        }

        private void AdminLogsPage_Load(object sender, EventArgs e)
        {
            GetData(@"
                SELECT * FROM dbo.DisplayLogs() ORDER BY LogID DESC;
            ");
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Prevent incorrect date flow 
            if (dtpDateFrom.Value > dtpDateTo.Value)
            {
                dtpDateFrom.Value = dtpDateTo.Value;
                MessageBox.Show("Start date cannot be later than end date");
                return;
            }

            if (dtpDateTo.Value < dtpDateFrom.Value)
            {
                dtpDateTo.Value = dtpDateFrom.Value;
                MessageBox.Show("End date cannot be earlier than start date");
                return;
            }

            Console.WriteLine($"btnSearch_Click roleName => {roleName}");
            Console.WriteLine($"[SP_QueryLogs] => {roleName}, {txtName.Text}, {dtpDateFrom.Text}, {dtpDateTo.Text}");
            LoadQueryLogs(@"EXEC [dbo].[SP_QueryLogs] @Role, @Name, @DateFrom, @DateTo");
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
                SELECT * FROM dbo.DisplayLogs() ORDER BY LogID DESC;
            ");
        }

        private void cbxFilterRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxFilterRoles.SelectedIndex == -1)
            {
                roleName = null;
                Console.WriteLine(roleName);
            }

            else if (cbxFilterRoles.SelectedIndex == 0)
            {
                roleName = "Admin";
                Console.WriteLine(roleName);
            }

            else if (cbxFilterRoles.SelectedIndex == 1)
            {
                roleName = "Instructor";
                Console.WriteLine(roleName);
            }

            else
            {
                roleName = "Student";
                Console.WriteLine(roleName);
            }
        }
    }

}
