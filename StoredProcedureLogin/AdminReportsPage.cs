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
    public partial class AdminReportsPage : Form
    {

        private BindingSource ReportsBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;
        private DataTable table;
        public AdminReportsPage()
        {
            InitializeComponent();

            int ActiveStudentsNumber = 0, ActiveTeachersNumber = 0;
            
        }

        public void FillTable()
        {
            // Clear existing bindings, especially each time you pull up other tables via other buttons 
            dgvReports.DataSource = null;
            ReportsBindingSource.DataSource = null;

            // Set Fill mode for all columns except one
            foreach (DataGridViewColumn column in dgvReports.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Bind data in correct order
            ReportsBindingSource.DataSource = table;
            dgvReports.DataSource = ReportsBindingSource;

            // Set the last column to fill remaining space
            int lastColumnIndex = dgvReports.Columns.Count - 1;
            dgvReports.Columns[lastColumnIndex].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            // Force refresh of column order
            dgvReports.AutoResizeColumns();
            dgvReports.Refresh();

        }

        private void GetData(string selectCommand)
        {
            try
            {
                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                // Create data adapter
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                table = new DataTable();  // initialize table here
                dataAdapter.Fill(table);

                Console.WriteLine($"Total rows in table: {table.Rows.Count}");
                if (table.Rows.Count > 0)
                {
                    Console.WriteLine($"Column names: {string.Join(", ", table.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }

        private void AdminReportsPage_Load(object sender, EventArgs e)
        {
            // load numbers here for students and teachers 
            GetData(@"
                SELECT * FROM dbo.DisplayReportsNumbers()
            ");
            Console.WriteLine("GetData is worked on!");
            // need to display each number to lblActiveStudentsNumber and lblActiveTeachersNumber

            // Display the numbers in the labels
            if (dataAdapter != null)
            {
                DataTable table = new DataTable();
                dataAdapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    lblActiveStudentsNumber.Text = table.Rows[0]["ActiveStudents"].ToString();
                    Console.WriteLine($"Stuents: {table.Rows[0]["ActiveStudents"].ToString()}");
                    lblActiveTeachersNumber.Text = table.Rows[0]["ActiveTeachers"].ToString();
                    Console.WriteLine($"Teachers: {table.Rows[0]["ActiveTeachers"].ToString()}");
                }
                else
                {
                    lblActiveStudentsNumber.Text = "0";
                    lblActiveTeachersNumber.Text = "0";
                }
            }
            // this thing doesn't work, I'll focus on the other ones at the moment 
        }
        private void btnActiveStudents_Click(object sender, EventArgs e)
        {
            // fixed SQL query and can't be modified 
            GetData(@"
                SELECT * FROM dbo.DisplayReportsStudentData()
            ");

            FillTable();
            Console.WriteLine("Clicks Reports => ActiveStudents ");
        }

        private void btnActiveTeachers_Click(object sender, EventArgs e)
        {
            // fixed SQL query and can't be modified 
            GetData(@"
                SELECT * FROM dbo.DisplayReportsTeacherData()
            ");
            FillTable();
            Console.WriteLine("Clicks Reports => ActiveTeachers ");
        }

        private void btnStudentsPerSubject_Click(object sender, EventArgs e)
        {
            // fixed SQL query and can't be modified 
            GetData(@"
                SELECT * FROM dbo.DisplayReportsStudentsPerSubject()
            ");
            FillTable();
            Console.WriteLine("Clicks Reports => StudentsPerSubject ");
        }

        private void btnStudentsPerTeacher_Click(object sender, EventArgs e)
        {
            // fixed SQL query and can't be modified 
            GetData(@"
                SELECT * FROM dbo.DisplayReportsStudentsPerTeacher()
            ");
            FillTable();
            Console.WriteLine("Clicks Reports => StudentsPerTeacher ");
        }

        private void btnAllSubjects_Click(object sender, EventArgs e)
        {
            // fixed SQL query and can't be modified 
            GetData(@"
                SELECT * FROM dbo.DisplayAllSubjects()
            ");
            FillTable();
            Console.WriteLine("Clicks Reports => AllSubjects ");
        }

    }
}
