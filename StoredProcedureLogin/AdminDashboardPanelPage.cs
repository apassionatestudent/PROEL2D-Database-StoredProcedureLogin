using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Forms.DataVisualization.Charting; 
using System.Windows.Markup;

namespace StoredProcedureLogin
{
    public partial class AdminDashboardPanelPage : Form
    {

        private BindingSource EnrollmentBindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter;


        private readonly PieChart _pieChart;
        private LiveCharts.WinForms.PieChart pieChart;

        //private LiveCharts.WinForms.PieChart pieChart;
        public AdminDashboardPanelPage()
        {
            InitializeComponent();
        }


        private void GetData(string selectCommand)
        {
            try
            {
                string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

                // Create data adapter
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Fill dataset and bind to BindingSource
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                EnrollmentBindingSource.DataSource = table;

                // Bind DataGridView to BindingSource
                dgvEnrollments.DataSource = EnrollmentBindingSource;

                // Adjust column widths
                //dgvStudents.AutoResizeColumns(
                //    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

                // Set Fill mode for all columns except one
                foreach (DataGridViewColumn column in dgvEnrollments.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Set the last column to fill remaining space
                int lastColumnIndex = dgvEnrollments.Columns.Count - 1;
                dgvEnrollments.Columns[lastColumnIndex].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;

                dgvEnrollments.AutoResizeColumns();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }

        }

        private static DataTable GetDataChart(string query)
        {
            string constr = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }




        private void AdminDashboardPanelPage_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'storedProcedureDataSet1.DisplayEnrollmentData' table. You can move, or remove it, as needed.
            //this.displayEnrollmentDataTableAdapter.Fill(this.storedProcedureDataSet1.DisplayEnrollmentData);
            //InitPieChart();

            GetData(@"
                SELECT * FROM dbo.DisplayEnrollmentData()
            ");

            //string newquery = "SELECT * FROM dbo.DisplayEnrollmentData()";
            string newquery2 = "EXEC [dbo].[SP_GetConsolidatedEnrollments]";
            DataTable dt = GetDataChart(newquery2);

            //Get the names of courses
            string[] x = (from p in dt.AsEnumerable()
                          orderby p.Field<string>("CourseCode") ascending
                          select p.Field<string>("CourseName")).ToArray();

            //Get the Total enrollment
            int[] y = (from p in dt.AsEnumerable()
                       orderby p.Field<string>("CourseName") ascending
                       select p.Field<int>("StudentCount")).ToArray();

            chart1.Series[0].ChartType = SeriesChartType.Pie;
            chart1.Series[0].Points.DataBindXY(x, y);
            chart1.Legends[0].Enabled = true;
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;


        }


    }
}
