namespace screensound.test.utils;

public static class TestUtils
{
    public static string GetSqlConnectionString(string dbName)
    {
        return "Data Source=(localdb)\\MSSQLLocalDB;" +
              $"Initial Catalog={dbName};" +
               "Integrated Security=True;" +
               "Connect Timeout=30;" +
               "Encrypt=False;" +
               "Trust Server Certificate=False;" +
               "Application Intent=ReadWrite;" +
               "Multi Subnet Failover=False";
    }
}
