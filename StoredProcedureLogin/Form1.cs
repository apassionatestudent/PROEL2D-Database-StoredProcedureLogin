using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace StoredProcedureLogin
{
    public partial class LoginForm : Form
    {

        private readonly Logger _logger;
        private readonly PictureLoader _pictureLoader = new PictureLoader();
        public string profilePictureString;
        private System.Windows.Forms.Timer countdownTimer;

        int? roleId = null;

        public LoginForm()
        {
            InitializeComponent();

            // Initialize password visibility
            txtPassword.PasswordChar = '*';
            txtPassword.Text = string.Empty;

            // connect => Logger.cs
            _logger = new Logger();

            // conect => PictureLoader.cs
            _pictureLoader = new PictureLoader();

            // Initialize timer
            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000; // 1 second interval
            countdownTimer.Tick += DisableLogin;
            countdownTimer.Tag = 10; // 10 seconds countdown
        }

        public string FullName { get; private set; }

        int LoginAttemptCounter = 0;

        private readonly string connectingString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

        private async Task<int?> GetUserRoleAsync(string username, string password)
        {
            int? roleId = null;

            using (SqlConnection connection = new SqlConnection(connectingString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SP_LoginForm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); // Empty password not used

                    try
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                roleId = Convert.ToInt32(reader["role"]);
                                string firstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FirstName"));
                                string lastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? string.Empty : reader.GetString(reader.GetOrdinal("LastName"));
                                FullName = $"{firstName} {lastName}".Trim();
                                Console.WriteLine($"FullName upon login => {FullName}");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Database Error");
                    }
                }
            }

            return roleId;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            int errorCounter = 0;
            string errorMessage = string.Empty;

            try
            {
                // Input validation
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    errorMessage += "- Username is required. \n";
                    errorCounter++;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    errorMessage += "- Password is required. \n";
                    errorCounter++;
                }

                if (errorCounter > 0)
                {
                    MessageBox.Show(errorMessage, "Validation Error");
                    return;
                }

                // Attempt login
                bool loginSuccess = await AttemptLogin(txtUsername.Text, txtPassword.Text);

                if (loginSuccess)
                {
                    int? roleId = await GetUserRoleAsync(txtUsername.Text, txtPassword.Text);

                    //Console.WriteLine($"Role ID: {roleId}");

                    if (roleId.HasValue)
                    {

                        profilePictureString = _pictureLoader.GetImageFromDatabase(txtUsername.Text);
                        string picLoaderLog = Convert.ToString(roleId.Value);
                        Console.WriteLine($"picLoaderLog => {picLoaderLog}");

                        // Create and show the appropriate dashboard based on RoleID
                        Form dashboard = null;

                        switch (roleId.Value)
                        {
                            case 1: // Admin
                                dashboard = new AdminDashboard(txtUsername.Text, profilePictureString, FullName, roleId.Value);
                                //Console.WriteLine($"Role ID: {roleId}");
                                break;
                            case 2: // Teacher
                                dashboard = new TeacherDashboard(txtUsername.Text, profilePictureString, FullName, roleId.Value);
                                //Console.WriteLine($"Role ID: {roleId}");
                                break;
                            case 3: // Student
                                dashboard = new Dashboard(txtUsername.Text, profilePictureString, FullName, roleId.Value);
                                Console.WriteLine($"profilePictureString Login => {profilePictureString}");
                                //Console.WriteLine($"Role ID: {roleId}");
                                break;
                            default:
                                MessageBox.Show("Invalid RoleID", "Error");
                                return;
                        }

                        if (dashboard != null)
                        {
                            dashboard.Show();
                            Console.WriteLine($"Log profilePictureString {profilePictureString}");

                            Console.WriteLine($"FullName => {FullName}");
                            // logs successful login
                            _logger.Log(FullName, "Logs in", roleId.Value);
                            Console.WriteLine($"RoleID upon logging => {roleId}");

                            this.Hide();
                            ClearForm();
                        }

                        LoginAttemptCounter = 0;
                    }
                    else
                    {

                        MessageBox.Show($"Invalid username or password ", "Login Failed");
                        Console.WriteLine($"{loginSuccess} within the if/else statement");

                        if (LoginAttemptCounter > 3)
                        {
                            Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                LoginAttempts();
                MessageBox.Show($"Database error: {ex.Message}. Number of attempts: {LoginAttemptCounter}. " +
                        $"It will be closed on the 3rd incorrect attempt!", "Database Error");

            }
            catch (Exception ex)
            {
                LoginAttempts();
                MessageBox.Show($"An error occurred: {ex.Message}. Number of attempts: {LoginAttemptCounter}. " +
                    $"$\"It will be closed on the 3rd incorrect attempt!", "Error | LoginButton  ");
            }
        }


        private async Task<bool> AttemptLogin(string username, string password)
        {
            bool loginSuccess = false;

            using (SqlConnection connection = new SqlConnection(connectingString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SP_LoginForm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Use Add method instead of AddWithValue for better type safety
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, -1).Value = password;

                    try
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // Check HasRows first, then read
                            if (await reader.ReadAsync())
                            {
                                loginSuccess = true;

                                // Verify all columns exist
                                string storedUsername = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString(reader.GetOrdinal("Username"));
                                string storedPassword = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString(reader.GetOrdinal("Password"));

                                if (storedUsername != username || storedPassword != password)
                                    loginSuccess = false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        LoginAttempts();
                        _logger.Log(FullName, "Unable to log in", roleId.Value);
                        MessageBox.Show($"Database error: {ex.Message}. Number of attempts: {LoginAttemptCounter}. " +
                            $"It will be closed on the 3rd incorrect attempt!", "Database Error | AttemptLogin");
                        return false;
                    }
                }
            }
 
            return loginSuccess;
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        // only allows 3 login attempts 
        private void LoginAttempts()
        {
            LoginAttemptCounter++;

            // Start countdown on 3rd failed attempt, 10 seconds => 0 second
            if (LoginAttemptCounter == 3)
            {
                StartCountdown();
            }
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbShowPassword.Checked ? '\0' : '*';
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Registration registrationForm = new Registration();
            registrationForm.Show();
            Hide();
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            // Need to input recovery code
            ForgotPassword forgotPasswordForm = new ForgotPassword(this, txtUsername.Text);
            forgotPasswordForm.Show();

            // Disable Login Form for now until the ForgotPassword form is closed
            Enabled = false;
        }

        // For disabling the login button for 10 seconds after 3 failed attempts
        private void DisableLogin(object sender, EventArgs e)
        {

            int remainingSeconds = (int)(countdownTimer.Tag ?? 0);

            if (remainingSeconds <= 0)
            {
                countdownTimer.Stop();
                btnLogin.Enabled = true;
                countdownTimer.Tag = 10; // Reset for next time
                Console.WriteLine($"Remaining seconds: {remainingSeconds}");
            }
            else
            {
                countdownTimer.Tag = remainingSeconds - 1;
                btnLogin.Enabled = false;
                Console.WriteLine($"Remaining seconds: {remainingSeconds}");
            }
        }

        private void StartCountdown()
        {
            countdownTimer.Tag = 10; // Set initial count
            countdownTimer.Start();
        }
    }
}
