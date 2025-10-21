using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace StoredProcedureLogin
{
    internal class Logger
    {
        private readonly string connectingString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";


        // add to Logs Table 
        public async Task Log(string actor, string action, int roleId)
        {
            Console.WriteLine($"Actor: {actor}, Action: {action} => Log Function enabled!");


            using (SqlConnection connection = new SqlConnection(connectingString))
            {
                await connection.OpenAsync();

                try
                {
                    using (SqlCommand command = new SqlCommand("SP_LoggingOperations", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Actor", actor);
                        command.Parameters.AddWithValue("@Action", action);
                        command.Parameters.AddWithValue("@RoleID", roleId);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Database Error");
                }


            }

        }

    }
}
