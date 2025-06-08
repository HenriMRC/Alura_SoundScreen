using Microsoft.Data.SqlClient;
using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound.database
{
    internal class Connection
    {
        private const string CONNECTION_STRING =
            "Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=ScreenSound;" +
            "Integrated Security=True;" +
            //"Connect Timeout=30;" +
            "Encrypt=False;" +
            "Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False";

        public static SqlConnection GetConnection() => new SqlConnection(CONNECTION_STRING);


    }
}
