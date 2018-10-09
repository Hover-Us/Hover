using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Hover.Models
{
    public class HoverDb : IDisposable
    {
        public MySqlConnection Hconn;
                
        public HoverDb(string connString)
        {
            Hconn = new MySqlConnection(connString);
        }

        public async Task ReadyConnection()
        {
            if (Hconn.State == ConnectionState.Broken)
                Hconn.Close();

            if (Hconn.State == ConnectionState.Closed)
                await Hconn.OpenAsync();
        }

        public void Dispose()
        {
            if (Hconn != null) Hconn.Close();
        }
    }
}