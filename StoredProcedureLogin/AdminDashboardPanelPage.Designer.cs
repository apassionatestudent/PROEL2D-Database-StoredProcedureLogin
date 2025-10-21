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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblAdminDashboardPage = new System.Windows.Forms.Label();
            this.chartActiveStudents = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblActivevsInactiveStudents = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartActiveStudents)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAdminDashboardPage
            // 
            this.lblAdminDashboardPage.AutoSize = true;
            this.lblAdminDashboardPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminDashboardPage.Location = new System.Drawing.Point(337, 9);
            this.lblAdminDashboardPage.Name = "lblAdminDashboardPage";
            this.lblAdminDashboardPage.Size = new System.Drawing.Size(168, 22);
            this.lblAdminDashboardPage.TabIndex = 0;
            this.lblAdminDashboardPage.Text = "Admin Dashboard";
            // 
            // chartActiveStudents
            // 
            chartArea2.Name = "ChartArea1";
            this.chartActiveStudents.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartActiveStudents.Legends.Add(legend2);
            this.chartActiveStudents.Location = new System.Drawing.Point(12, 34);
            this.chartActiveStudents.Name = "chartActiveStudents";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartActiveStudents.Series.Add(series2);
            this.chartActiveStudents.Size = new System.Drawing.Size(883, 291);
            this.chartActiveStudents.TabIndex = 1;
            this.chartActiveStudents.Text = "chart1";
            // 
            // lblActivevsInactiveStudents
            // 
            this.lblActivevsInactiveStudents.AutoSize = true;
            this.lblActivevsInactiveStudents.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivevsInactiveStudents.Location = new System.Drawing.Point(281, 343);
            this.lblActivevsInactiveStudents.Name = "lblActivevsInactiveStudents";
            this.lblActivevsInactiveStudents.Size = new System.Drawing.Size(320, 29);
            this.lblActivevsInactiveStudents.TabIndex = 2;
            this.lblActivevsInactiveStudents.Text = "Active vs Inactive Students";
            // 
            // AdminDashboardPanelPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 572);
            this.Controls.Add(this.lblActivevsInactiveStudents);
            this.Controls.Add(this.chartActiveStudents);
            this.Controls.Add(this.lblAdminDashboardPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminDashboardPanelPage";
            this.Text = "AdminDashboardPanelPage";
            ((System.ComponentModel.ISupportInitialize)(this.chartActiveStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAdminDashboardPage;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartActiveStudents;
        private System.Windows.Forms.Label lblActivevsInactiveStudents;
    }
}