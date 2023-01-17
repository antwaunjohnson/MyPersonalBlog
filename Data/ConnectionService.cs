using Npgsql;

namespace MyPersonalBlog.Data;

public class ConnectionService
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);

    }

    private static string BuildConnectionString(string databaseURL)
    {
        var databaseUri = new Uri(databaseURL);
        var userInfo = databaseUri.UserInfo.Split(':');

        return new NpgsqlConnectionStringBuilder()
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/'),
            SslMode = SslMode.Prefer,
            TrustServerCertificate = true
        }.ToString();
    }
}
