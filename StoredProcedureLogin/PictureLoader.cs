using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace StoredProcedureLogin
{
    internal class PictureLoader
    {

        public string  GetImageFromDatabase(string username)
        {
            string connectionString = @"Data Source=RICSON\SQLEXPRESS;Initial Catalog=StoredProcedure;Integrated Security=True;";

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    string query = "SELECT PictureDirectory FROM Profiles WHERE ProfileID = @UserId";
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@UserId", userId);

            //        byte[] imageData = (byte[])command.ExecuteScalar();

            //        if (imageData == null || imageData.Length == 0)
            //            return null;

            //        using (MemoryStream ms = new MemoryStream(imageData))
            //        {
            //            return Convert.ToBase64String(imageData);
            //        }
            //    }
            //}

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT PictureDirectory FROM Users WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    // Use ExecuteReader instead of ExecuteScalar for better performance
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check for null values first
                            if (!reader.IsDBNull(0))
                            {
                                return reader.GetString(0);  // Get the value directly as a string
                            }
                        }
                    }
                }
            }

            return null;  // Return null if no row found or value is null
        }
    }
}
