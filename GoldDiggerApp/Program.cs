public class Program
{
    public static void Main(string[] args)
    {
        // Erstellt einen neuen Webanwendungs-Builder, der Konfigurationen und Dienste verwaltet
        var builder = WebApplication.CreateBuilder(args);

        // Konfiguration laden: Lädt die "appsettings.json"-Datei, die die Verbindung zur DB enthält
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory()) // Sucht die Datei im aktuellen Verzeichnis
            .AddJsonFile("appsettings.json");             // Lädt die Einstellungen aus dieser Datei

        // Dienste registrieren (Dependency Injection)
        builder.Services.AddSingleton<DbConnector>();    // Erstellt eine einzelne Instanz der DB-Verbindung
        builder.Services.AddControllers();               // Aktiviert Controller-Unterstützung für die API
        builder.Services.AddEndpointsApiExplorer();      // Fügt Endpunkt-Explorer hinzu (für Swagger)
        builder.Services.AddSwaggerGen();                // Generiert API-Dokumentation mit Swagger

        // Erstellt die Webanwendung basierend auf den obigen Einstellungen
        var app = builder.Build();

        // Middleware konfigurieren (Schichten zur Verarbeitung von Anfragen)
        if (app.Environment.IsDevelopment()) // Überprüft, ob die App im Entwicklungsmodus läuft
        {
            app.UseSwagger();    // Aktiviert Swagger-UI für die API-Dokumentation
            app.UseSwaggerUI();  // Zeigt die interaktive Swagger-Oberfläche im Browser an
        }

        app.UseHttpsRedirection(); // Erzwingt HTTPS, um sichere Verbindungen zu verwenden
        app.UseAuthorization();    // Aktiviert die Autorisierung (hier nicht konfiguriert)

        // Verbindet Controller-Endpunkte (z.B. api/Mine) mit der Anwendung
        app.MapControllers();

        // Startet die Anwendung und wartet auf eingehende HTTP-Anfragen
        app.Run();
    }
}
