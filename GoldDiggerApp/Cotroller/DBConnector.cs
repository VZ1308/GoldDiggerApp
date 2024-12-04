using MySql.Data.MySqlClient;  // Bibliothek für MySQL-Datenbankzugriff

public class DbConnector
{
    private readonly string _connectionString; // Speichert den Verbindungstext zur DB

    // Konstruktor erhält Konfiguration und lädt Verbindungszeichenfolge
    public DbConnector(IConfiguration configuration) // IConfiguration: Ermöglicht den Zugriff auf die appsettings.json
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // Öffnet und gibt eine Verbindung zur Datenbank zurück
    public MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_connectionString); // Erstellen einer neuen Verbindung
        connection.Open(); // Öffnet die Verbindung zur Datenbank
        return connection; // Rückgabe der offenen Verbindung
    }
}
