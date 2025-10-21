namespace StoredProcedureLogin
{
    partial class AdminReportsPage
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
            this.lblAdminReportsPage = new System.Windows.Forms.Label();
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.btnActiveStudents = new System.Windows.Forms.Button();
            this.btnActiveTeachers = new System.Windows.Forms.Button();
            this.btnStudentsPerSubject = new System.Windows.Forms.Button();
            this.btnStudentsPerTeacher = new System.Windows.Forms.Button();
            this.btnAllSubjects = new System.Windows.Forms.Button();
            this.lblActiveStudentsNumber = new System.Windows.Forms.Label();
            this.lblActiveTeachersNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAdminReportsPage
            // 
            this.lblAdminReportsPage.AutoSize = true;
            this.lblAdminReportsPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminReportsPage.Location = new System.Drawing.Point(337, 9);
            this.lblAdminReportsPage.Name = "lblAdminReportsPage";
            this.lblAdminReportsPage.Size = new System.Drawing.Size(183, 22);
            this.lblAdminReportsPage.TabIndex = 1;
            this.lblAdminReportsPage.Text = "Reports Dashboard";
            // 
            // dgvReports
            // 
            this.dgvReports.AllowUserToAddRows = false;
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Location = new System.Drawing.Point(12, 41);
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.RowHeadersWidth = 51;
            this.dgvReports.RowTemplate.Height = 24;
            this.dgvReports.Size = new System.Drawing.Size(883, 364);
            this.dgvReports.TabIndex = 2;
            // 
            // btnActiveStudents
            // 
            this.btnActiveStudents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActiveStudents.Location = new System.Drawing.Point(12, 489);
            this.btnActiveStudents.Name = "btnActiveStudents";
            this.btnActiveStudents.Size = new System.Drawing.Size(172, 71);
            this.btnActiveStudents.TabIndex = 3;
            this.btnActiveStudents.Text = "Active Students";
            this.btnActiveStudents.UseVisualStyleBackColor = true;
            this.btnActiveStudents.Click += new System.EventHandler(this.btnActiveStudents_Click);
            // 
            // btnActiveTeachers
            // 
            this.btnActiveTeachers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActiveTeachers.Location = new System.Drawing.Point(190, 489);
            this.btnActiveTeachers.Name = "btnActiveTeachers";
            this.btnActiveTeachers.Size = new System.Drawing.Size(172, 71);
            this.btnActiveTeachers.TabIndex = 4;
            this.btnActiveTeachers.Text = "Active Teachers";
            this.btnActiveTeachers.UseVisualStyleBackColor = true;
            this.btnActiveTeachers.Click += new System.EventHandler(this.btnActiveTeachers_Click);
            // 
            // btnStudentsPerSubject
            // 
            this.btnStudentsPerSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudentsPerSubject.Location = new System.Drawing.Point(368, 489);
            this.btnStudentsPerSubject.Name = "btnStudentsPerSubject";
            this.btnStudentsPerSubject.Size = new System.Drawing.Size(172, 71);
            this.btnStudentsPerSubject.TabIndex = 5;
            this.btnStudentsPerSubject.Text = "Students Per Subject";
            this.btnStudentsPerSubject.UseVisualStyleBackColor = true;
            this.btnStudentsPerSubject.Click += new System.EventHandler(this.btnStudentsPerSubject_Click);
            // 
            // btnStudentsPerTeacher
            // 
            this.btnStudentsPerTeacher.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudentsPerTeacher.Location = new System.Drawing.Point(546, 489);
            this.btnStudentsPerTeacher.Name = "btnStudentsPerTeacher";
            this.btnStudentsPerTeacher.Size = new System.Drawing.Size(172, 71);
            this.btnStudentsPerTeacher.TabIndex = 6;
            this.btnStudentsPerTeacher.Text = "Students Per Teacher";
            this.btnStudentsPerTeacher.UseVisualStyleBackColor = true;
            this.btnStudentsPerTeacher.Click += new System.EventHandler(this.btnStudentsPerTeacher_Click);
            // 
            // btnAllSubjects
            // 
            this.btnAllSubjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllSubjects.Location = new System.Drawing.Point(724, 489);
            this.btnAllSubjects.Name = "btnAllSubjects";
            this.btnAllSubjects.Size = new System.Drawing.Size(172, 71);
            this.btnAllSubjects.TabIndex = 7;
            this.btnAllSubjects.Text = "All Subjects";
            this.btnAllSubjects.UseVisualStyleBackColor = true;
            this.btnAllSubjects.Click += new System.EventHandler(this.btnAllSubjects_Click);
            // 
            // lblActiveStudentsNumber
            // 
            this.lblActiveStudentsNumber.AutoSize = true;
            this.lblActiveStudentsNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveStudentsNumber.Location = new System.Drawing.Point(78, 435);
            this.lblActiveStudentsNumber.Name = "lblActiveStudentsNumber";
            this.lblActiveStudentsNumber.Size = new System.Drawing.Size(31, 32);
            this.lblActiveStudentsNumber.TabIndex = 8;
            this.lblActiveStudentsNumber.Text = "0";
            // 
            // lblActiveTeachersNumber
            // 
            this.lblActiveTeachersNumber.AutoSize = true;
            this.lblActiveTeachersNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveTeachersNumber.Location = new System.Drawing.Point(261, 435);
            this.lblActiveTeachersNumber.Name = "lblActiveTeachersNumber";
            this.lblActiveTeachersNumber.Size = new System.Drawing.Size(31, 32);
            this.lblActiveTeachersNumber.TabIndex = 9;
            this.lblActiveTeachersNumber.Text = "0";
            // 
            // AdminReportsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 572);
            this.Controls.Add(this.lblActiveTeachersNumber);
            this.Controls.Add(this.lblActiveStudentsNumber);
            this.Controls.Add(this.btnAllSubjects);
            this.Controls.Add(this.btnStudentsPerTeacher);
            this.Controls.Add(this.btnStudentsPerSubject);
            this.Controls.Add(this.btnActiveTeachers);
            this.Controls.Add(this.btnActiveStudents);
            this.Controls.Add(this.dgvReports);
            this.Controls.Add(this.lblAdminReportsPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminReportsPage";
            this.Text = "AdminReportsPage";
            this.Load += new System.EventHandler(this.AdminReportsPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAdminReportsPage;
        private System.Windows.Forms.DataGridView dgvReports;
        private System.Windows.Forms.Button btnActiveStudents;
        private System.Windows.Forms.Button btnActiveTeachers;
        private System.Windows.Forms.Button btnStudentsPerSubject;
        private System.Windows.Forms.Button btnStudentsPerTeacher;
        private System.Windows.Forms.Button btnAllSubjects;
        private System.Windows.Forms.Label lblActiveStudentsNumber;
        private System.Windows.Forms.Label lblActiveTeachersNumber;
    }
}