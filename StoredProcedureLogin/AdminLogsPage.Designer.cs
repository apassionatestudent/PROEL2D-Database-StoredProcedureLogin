namespace StoredProcedureLogin
{
    partial class AdminLogsPage
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
            this.lblAdminLogsPage = new System.Windows.Forms.Label();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblFilterRole = new System.Windows.Forms.Label();
            this.cbxFilterRoles = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAdminLogsPage
            // 
            this.lblAdminLogsPage.AutoSize = true;
            this.lblAdminLogsPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminLogsPage.Location = new System.Drawing.Point(337, 9);
            this.lblAdminLogsPage.Name = "lblAdminLogsPage";
            this.lblAdminLogsPage.Size = new System.Drawing.Size(156, 22);
            this.lblAdminLogsPage.TabIndex = 2;
            this.lblAdminLogsPage.Text = "Logs Dashboard";
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Location = new System.Drawing.Point(12, 34);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.RowHeadersWidth = 51;
            this.dgvLogs.Size = new System.Drawing.Size(883, 355);
            this.dgvLogs.TabIndex = 3;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.Location = new System.Drawing.Point(12, 468);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(184, 32);
            this.lblSearch.TabIndex = 4;
            this.lblSearch.Text = "Search Logs";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(221, 522);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(194, 27);
            this.txtName.TabIndex = 5;
            this.txtName.Click += new System.EventHandler(this.txtName_Click);
            // 
            // lblSearchName
            // 
            this.lblSearchName.AutoSize = true;
            this.lblSearchName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchName.Location = new System.Drawing.Point(217, 489);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(128, 20);
            this.lblSearchName.TabIndex = 6;
            this.lblSearchName.Text = "Search Name:";
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "MMMM dd, yyyy";
            this.dtpDateFrom.Location = new System.Drawing.Point(452, 443);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(200, 22);
            this.dtpDateFrom.TabIndex = 7;
            this.dtpDateFrom.ValueChanged += new System.EventHandler(this.dtpDateFrom_ValueChanged);
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateFrom.Location = new System.Drawing.Point(448, 420);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(104, 20);
            this.lblDateFrom.TabIndex = 8;
            this.lblDateFrom.Text = "Date From:";
            // 
            // lblDateTo
            // 
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTo.Location = new System.Drawing.Point(448, 489);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(82, 20);
            this.lblDateTo.TabIndex = 9;
            this.lblDateTo.Text = "Date To:";
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.CustomFormat = "MMMM dd, yyyy";
            this.dtpDateTo.Location = new System.Drawing.Point(452, 527);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(200, 22);
            this.dtpDateTo.TabIndex = 10;
            this.dtpDateTo.ValueChanged += new System.EventHandler(this.dtpDateTo_ValueChanged);
            // 
            // lblFilterRole
            // 
            this.lblFilterRole.AutoSize = true;
            this.lblFilterRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterRole.Location = new System.Drawing.Point(217, 420);
            this.lblFilterRole.Name = "lblFilterRole";
            this.lblFilterRole.Size = new System.Drawing.Size(113, 20);
            this.lblFilterRole.TabIndex = 11;
            this.lblFilterRole.Text = "Filter Roles:";
            // 
            // cbxFilterRoles
            // 
            this.cbxFilterRoles.FormattingEnabled = true;
            this.cbxFilterRoles.Items.AddRange(new object[] {
            "Admins",
            "Teachers",
            "Students"});
            this.cbxFilterRoles.Location = new System.Drawing.Point(221, 443);
            this.cbxFilterRoles.Name = "cbxFilterRoles";
            this.cbxFilterRoles.Size = new System.Drawing.Size(194, 24);
            this.cbxFilterRoles.TabIndex = 12;
            this.cbxFilterRoles.SelectedIndexChanged += new System.EventHandler(this.cbxFilterRoles_SelectedIndexChanged);
            this.cbxFilterRoles.Click += new System.EventHandler(this.cbxFilterRoles_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(672, 420);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(223, 60);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(672, 500);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(223, 60);
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // AdminLogsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 572);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.cbxFilterRoles);
            this.Controls.Add(this.lblFilterRole);
            this.Controls.Add(this.dtpDateTo);
            this.Controls.Add(this.lblDateTo);
            this.Controls.Add(this.lblDateFrom);
            this.Controls.Add(this.dtpDateFrom);
            this.Controls.Add(this.lblSearchName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.dgvLogs);
            this.Controls.Add(this.lblAdminLogsPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminLogsPage";
            this.Text = "AdminLogsPage";
            this.Load += new System.EventHandler(this.AdminLogsPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAdminLogsPage;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label lblFilterRole;
        private System.Windows.Forms.ComboBox cbxFilterRoles;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
    }
}