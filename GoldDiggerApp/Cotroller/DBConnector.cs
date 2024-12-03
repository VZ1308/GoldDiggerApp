using MySql.Data.MySqlClient;

public class DbConnector
{
    private readonly string _connectionString;
    public DbConnector(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
