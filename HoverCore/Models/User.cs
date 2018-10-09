using System;
using System.Threading.Tasks;

namespace Hover.Models
{
    public class User
    {
        public string hash;
        public string username;
        public bool optout;
        public static HoverDb Db { get; set; }

        public static async Task<User> CreateAsync(string userHash, HoverDb db)
        {
            Db = db;
            User x = new User();
            await x.InitializeAsync(userHash);
            return x;
        }

        // No one but the Create function calls the constructor:
        private User() {}

        private async Task InitializeAsync(string userHash)
        {
            using (var cmd = Db.Hconn.CreateCommand())
            {
                await Db.ReadyConnection();
                cmd.CommandText = "UserGet";
                cmd.Parameters.AddWithValue("@userHashIn", userHash);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        hash = userHash;
                        username = reader["username"].ToString();
                        optout = reader["optout"] != DBNull.Value;
                    }
                    else
                        username = "Not Found";
                }
            }
        }

        public static async Task<string> UserOptOut(string userHash)
        {
            if (userHash.Length != 44) return "Incorrect userHash length";

            using (var cmd = Db.Hconn.CreateCommand())
            {
                await Db.ReadyConnection();
                cmd.CommandText = "UserOptOut";
                cmd.Parameters.AddWithValue("@userHashIn", userHash);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                object result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
        }


        public async Task<User> GetUser(string userHash)
        {
            using (var cmd = Db.Hconn.CreateCommand())
            {
                await Db.ReadyConnection();
                cmd.CommandText = "UserGet";
                cmd.Parameters.AddWithValue("@userHashIn", userHash);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (var user = await cmd.ExecuteReaderAsync())
                {
                    hash = userHash;
                    username = user["name"].ToString();
                    optout = Convert.ToBoolean(user["optout"]);
                }
            }

            return this;
        }
    }
}
