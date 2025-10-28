namespace StoredProcedureLogin
{
    partial class AdminDashboardPanelPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblAdminDashboardPage = new System.Windows.Forms.Label();
            this.dgvEnrollments = new System.Windows.Forms.DataGridView();
            this.storedProcedureDataSet = new StoredProcedureLogin.StoredProcedureDataSet();
            this.displayAllSubjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.displayAllSubjectsTableAdapter = new StoredProcedureLogin.StoredProcedureDataSetTableAdapters.DisplayAllSubjectsTableAdapter();
            this.storedProcedureDataSet1 = new StoredProcedureLogin.StoredProcedureDataSet1();
            this.displayEnrollmentDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.displayEnrollmentDataTableAdapter = new StoredProcedureLogin.StoredProcedureDataSet1TableAdapters.DisplayEnrollmentDataTableAdapter();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblEnrollmentChart = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storedProcedureDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayAllSubjectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storedProcedureDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayEnrollmentDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAdminDashboardPage
            // 
            this.lblAdminDashboardPage.AutoSize = true;
            this.lblAdminDashboardPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminDashboardPage.Location = new System.Drawing.Point(370, 9);
            this.lblAdminDashboardPage.Name = "lblAdminDashboardPage";
            this.lblAdminDashboardPage.Size = new System.Drawing.Size(168, 22);
            this.lblAdminDashboardPage.TabIndex = 0;
            this.lblAdminDashboardPage.Text = "Admin Dashboard";
            // 
            // dgvEnrollments
            // 
            this.dgvEnrollments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnrollments.Location = new System.Drawing.Point(12, 410);
            this.dgvEnrollments.Name = "dgvEnrollments";
            this.dgvEnrollments.RowHeadersWidth = 51;
            this.dgvEnrollments.RowTemplate.Height = 24;
            this.dgvEnrollments.Size = new System.Drawing.Size(883, 150);
            this.dgvEnrollments.TabIndex = 1;
            // 
            // storedProcedureDataSet
            // 
            this.storedProcedureDataSet.DataSetName = "StoredProcedureDataSet";
            this.storedProcedureDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // displayAllSubjectsBindingSource
            // 
            this.displayAllSubjectsBindingSource.DataMember = "DisplayAllSubjects";
            this.displayAllSubjectsBindingSource.DataSource = this.storedProcedureDataSet;
            // 
            // displayAllSubjectsTableAdapter
            // 
            this.displayAllSubjectsTableAdapter.ClearBeforeFill = true;
            // 
            // storedProcedureDataSet1
            // 
            this.storedProcedureDataSet1.DataSetName = "StoredProcedureDataSet1";
            this.storedProcedureDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // displayEnrollmentDataBindingSource
            // 
            this.displayEnrollmentDataBindingSource.DataMember = "DisplayEnrollmentData";
            this.displayEnrollmentDataBindingSource.DataSource = this.storedProcedureDataSet1;
            // 
            // displayEnrollmentDataTableAdapter
            // 
            this.displayEnrollmentDataTableAdapter.ClearBeforeFill = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(8, 52);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(887, 309);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            // 
            // lblEnrollmentChart
            // 
            this.lblEnrollmentChart.AutoSize = true;
            this.lblEnrollmentChart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnrollmentChart.Location = new System.Drawing.Point(380, 377);
            this.lblEnrollmentChart.Name = "lblEnrollmentChart";
            this.lblEnrollmentChart.Size = new System.Drawing.Size(135, 20);
            this.lblEnrollmentChart.TabIndex = 4;
            this.lblEnrollmentChart.Text = "Enrollment Chart";
            // 
            // AdminDashboardPanelPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 572);
            this.Controls.Add(this.lblEnrollmentChart);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.dgvEnrollments);
            this.Controls.Add(this.lblAdminDashboardPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminDashboardPanelPage";
            this.Text = "AdminDashboardPanelPage";
            this.Load += new System.EventHandler(this.AdminDashboardPanelPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storedProcedureDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayAllSubjectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storedProcedureDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayEnrollmentDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAdminDashboardPage;
        private StoredProcedureDataSet storedProcedureDataSet;
        private System.Windows.Forms.BindingSource displayAllSubjectsBindingSource;
        private StoredProcedureDataSetTableAdapters.DisplayAllSubjectsTableAdapter displayAllSubjectsTableAdapter;
        private System.Windows.Forms.DataGridView dgvEnrollments;
        private StoredProcedureDataSet1 storedProcedureDataSet1;
        private System.Windows.Forms.BindingSource displayEnrollmentDataBindingSource;
        private StoredProcedureDataSet1TableAdapters.DisplayEnrollmentDataTableAdapter displayEnrollmentDataTableAdapter;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label lblEnrollmentChart;
    }
}