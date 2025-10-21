using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace StoredProcedureLogin
{
    public partial class ForgotPassword : Form
    {

        private LoginForm loginForm;
        private string userName;
        public ForgotPassword(LoginForm owner, string Username)
        {
            InitializeComponent();
            loginForm = owner;
            userName = Username;
        }

        string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";


        private bool VerifyRecoveryCode(string recoveryCode)
        {

            string query = @"
                SELECT CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM Users 
                        WHERE Username = @username AND RecoveryCode = @recoveryCode
                    ) THEN 1 
                    ELSE 0 
                END";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", userName);
                        command.Parameters.AddWithValue("@recoveryCode", recoveryCode);

                        connection.Open();
                        return Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnEnterRecoveryCode_Click(object sender, EventArgs e)
        {
            //// Logic for handling the recovery code submission
            //string recoveryCode = txtRecoveryCode.Text;
            //if (string.IsNullOrWhiteSpace(recoveryCode))
            //{
            //    MessageBox.Show("Please enter the recovery code.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //// Here you would typically verify the recovery code against your data source
            //// For demonstration purposes, let's assume the code is "123456"
            //if (recoveryCode == "123456")
            //{
            //    MessageBox.Show("Recovery code accepted. You can now reset your password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    // Proceed to password reset logic
            //}
            //else
            //{
            //    MessageBox.Show("Invalid recovery code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            string recoveryCode = txtRecoveryCode.Text.Trim();

            if (string.IsNullOrWhiteSpace(recoveryCode))
            {
                MessageBox.Show("Please enter the recovery code.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (VerifyRecoveryCode(recoveryCode))
            {
                MessageBox.Show("Recovery code accepted. You can now reset your password.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Add your password reset logic here

                txtRecoveryCode.Enabled = false; btnEnterRecoveryCode.Enabled = false;
                txtNewPassword.Enabled = true; btnEnterNewPassword.Enabled = true;

            }
            else
            {
                MessageBox.Show("Invalid recovery code. Please try again or contact admin.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ForgotPassword_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine($"ForgotPassword_FormClosed is enabled.");

            loginForm.Enabled = true;
        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {

        }

        private void btnEnterNewPassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter a new password.", "Input Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.SP_ChangePassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", userName);
                        command.Parameters.AddWithValue("@Password", newPassword);

                        connection.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Password has been successfully reset.", "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Close the form and return to login
                        this.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error occurred: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
