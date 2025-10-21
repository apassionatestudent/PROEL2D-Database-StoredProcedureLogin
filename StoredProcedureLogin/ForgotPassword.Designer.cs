namespace StoredProcedureLogin
{
    partial class ForgotPassword
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
            this.lblEnterCode = new System.Windows.Forms.Label();
            this.txtRecoveryCode = new System.Windows.Forms.TextBox();
            this.btnEnterRecoveryCode = new System.Windows.Forms.Button();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.btnEnterNewPassword = new System.Windows.Forms.Button();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblEnterCode
            // 
            this.lblEnterCode.AutoSize = true;
            this.lblEnterCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnterCode.Location = new System.Drawing.Point(109, 27);
            this.lblEnterCode.Name = "lblEnterCode";
            this.lblEnterCode.Size = new System.Drawing.Size(242, 29);
            this.lblEnterCode.TabIndex = 0;
            this.lblEnterCode.Text = "Enter Recovery Code";
            // 
            // txtRecoveryCode
            // 
            this.txtRecoveryCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecoveryCode.Location = new System.Drawing.Point(114, 59);
            this.txtRecoveryCode.Name = "txtRecoveryCode";
            this.txtRecoveryCode.Size = new System.Drawing.Size(237, 34);
            this.txtRecoveryCode.TabIndex = 1;
            // 
            // btnEnterRecoveryCode
            // 
            this.btnEnterRecoveryCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnterRecoveryCode.Location = new System.Drawing.Point(172, 99);
            this.btnEnterRecoveryCode.Name = "btnEnterRecoveryCode";
            this.btnEnterRecoveryCode.Size = new System.Drawing.Size(114, 49);
            this.btnEnterRecoveryCode.TabIndex = 2;
            this.btnEnterRecoveryCode.Text = "Enter";
            this.btnEnterRecoveryCode.UseVisualStyleBackColor = true;
            this.btnEnterRecoveryCode.Click += new System.EventHandler(this.btnEnterRecoveryCode_Click);
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Enabled = false;
            this.txtNewPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPassword.Location = new System.Drawing.Point(96, 207);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(237, 34);
            this.txtNewPassword.TabIndex = 3;
            // 
            // btnEnterNewPassword
            // 
            this.btnEnterNewPassword.Enabled = false;
            this.btnEnterNewPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnterNewPassword.Location = new System.Drawing.Point(339, 200);
            this.btnEnterNewPassword.Name = "btnEnterNewPassword";
            this.btnEnterNewPassword.Size = new System.Drawing.Size(114, 49);
            this.btnEnterNewPassword.TabIndex = 4;
            this.btnEnterNewPassword.Text = "Enter";
            this.btnEnterNewPassword.UseVisualStyleBackColor = true;
            this.btnEnterNewPassword.Click += new System.EventHandler(this.btnEnterNewPassword_Click);
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewPassword.Location = new System.Drawing.Point(12, 166);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(176, 29);
            this.lblNewPassword.TabIndex = 5;
            this.lblNewPassword.Text = "New Password";
            // 
            // ForgotPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 253);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.btnEnterNewPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.btnEnterRecoveryCode);
            this.Controls.Add(this.txtRecoveryCode);
            this.Controls.Add(this.lblEnterCode);
            this.Name = "ForgotPassword";
            this.Text = "Forgot Password";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ForgotPassword_FormClosed);
            this.Load += new System.EventHandler(this.ForgotPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEnterCode;
        private System.Windows.Forms.TextBox txtRecoveryCode;
        private System.Windows.Forms.Button btnEnterRecoveryCode;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Button btnEnterNewPassword;
        private System.Windows.Forms.Label lblNewPassword;
    }
}