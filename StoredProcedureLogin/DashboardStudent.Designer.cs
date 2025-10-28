namespace StoredProcedureLogin
{
    partial class DashboardStudent
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
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblStudentID = new System.Windows.Forms.Label();
            this.txtStudentID = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.dgvCourses = new System.Windows.Forms.DataGridView();
            this.lblCoursesEnrolled = new System.Windows.Forms.Label();
            this.dgvDataReference = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataReference)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(390, 9);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(90, 29);
            this.lblProfile.TabIndex = 0;
            this.lblProfile.Text = "Profile";
            // 
            // lblStudentID
            // 
            this.lblStudentID.AutoSize = true;
            this.lblStudentID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentID.Location = new System.Drawing.Point(35, 92);
            this.lblStudentID.Name = "lblStudentID";
            this.lblStudentID.Size = new System.Drawing.Size(148, 29);
            this.lblStudentID.TabIndex = 1;
            this.lblStudentID.Text = "Student ID: ";
            // 
            // txtStudentID
            // 
            this.txtStudentID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStudentID.Location = new System.Drawing.Point(186, 92);
            this.txtStudentID.Name = "txtStudentID";
            this.txtStudentID.Size = new System.Drawing.Size(180, 34);
            this.txtStudentID.TabIndex = 2;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.Location = new System.Drawing.Point(28, 172);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(155, 29);
            this.lblFirstName.TabIndex = 3;
            this.lblFirstName.Text = "First Name: ";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.Location = new System.Drawing.Point(32, 239);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(151, 29);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name: ";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Enabled = false;
            this.txtFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(186, 172);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(180, 34);
            this.txtFirstName.TabIndex = 5;
            // 
            // txtLastName
            // 
            this.txtLastName.Enabled = false;
            this.txtLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(186, 239);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(180, 34);
            this.txtLastName.TabIndex = 6;
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.Location = new System.Drawing.Point(107, 313);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(73, 29);
            this.lblAge.TabIndex = 7;
            this.lblAge.Text = "Age: ";
            // 
            // txtAge
            // 
            this.txtAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAge.Location = new System.Drawing.Point(186, 310);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(180, 34);
            this.txtAge.TabIndex = 8;
            // 
            // dgvCourses
            // 
            this.dgvCourses.AllowUserToAddRows = false;
            this.dgvCourses.AllowUserToOrderColumns = true;
            this.dgvCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCourses.Location = new System.Drawing.Point(395, 143);
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.RowHeadersWidth = 51;
            this.dgvCourses.RowTemplate.Height = 24;
            this.dgvCourses.Size = new System.Drawing.Size(473, 256);
            this.dgvCourses.TabIndex = 9;
            // 
            // lblCoursesEnrolled
            // 
            this.lblCoursesEnrolled.AutoSize = true;
            this.lblCoursesEnrolled.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoursesEnrolled.Location = new System.Drawing.Point(390, 92);
            this.lblCoursesEnrolled.Name = "lblCoursesEnrolled";
            this.lblCoursesEnrolled.Size = new System.Drawing.Size(223, 29);
            this.lblCoursesEnrolled.TabIndex = 10;
            this.lblCoursesEnrolled.Text = "Courses Enrolled:";
            // 
            // dgvDataReference
            // 
            this.dgvDataReference.AllowUserToAddRows = false;
            this.dgvDataReference.AllowUserToDeleteRows = false;
            this.dgvDataReference.AllowUserToResizeColumns = false;
            this.dgvDataReference.AllowUserToResizeRows = false;
            this.dgvDataReference.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataReference.Location = new System.Drawing.Point(12, 436);
            this.dgvDataReference.Name = "dgvDataReference";
            this.dgvDataReference.RowHeadersWidth = 51;
            this.dgvDataReference.RowTemplate.Height = 24;
            this.dgvDataReference.Size = new System.Drawing.Size(856, 55);
            this.dgvDataReference.TabIndex = 11;
            // 
            // DashboardStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(880, 517);
            this.Controls.Add(this.dgvDataReference);
            this.Controls.Add(this.lblCoursesEnrolled);
            this.Controls.Add(this.dgvCourses);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.txtStudentID);
            this.Controls.Add(this.lblStudentID);
            this.Controls.Add(this.lblProfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DashboardStudent";
            this.Text = "DashboardStudent";
            this.Load += new System.EventHandler(this.DashboardStudent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataReference)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Label lblStudentID;
        private System.Windows.Forms.TextBox txtStudentID;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.DataGridView dgvCourses;
        private System.Windows.Forms.Label lblCoursesEnrolled;
        private System.Windows.Forms.DataGridView dgvDataReference;
    }
}