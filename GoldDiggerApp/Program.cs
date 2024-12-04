public class Program
{
    public static void Main(string[] args)
    {
        // Erstellt einen neuen Webanwendungs-Builder, der Konfigurationen und Dienste verwaltet
        var builder = WebApplication.CreateBuilder(args);

        // Konfiguration laden: L�dt die "appsettings.json"-Datei, die die Verbindung zur DB enth�lt
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory()) // Sucht die Datei im aktuellen Verzeichnis
            .AddJsonFile("appsettings.json");             // L�dt die Einstellungen aus dieser Datei

        // Dienste registrieren (Dependency Injection)
        builder.Services.AddSingleton<DbConnector>();    // Erstellt eine einzelne Instanz der DB-Verbindung
        builder.Services.AddControllers();               // Aktiviert Controller-Unterst�tzung f�r die API
        builder.Services.AddEndpointsApiExplorer();      // F�gt Endpunkt-Explorer hinzu (f�r Swagger)
        builder.Services.AddSwaggerGen();                // Generiert API-Dokumentation mit Swagger

        // Erstellt die Webanwendung basierend auf den obigen Einstellungen
        var app = builder.Build();

        // Middleware konfigurieren (Schichten zur Verarbeitung von Anfragen)
        if (app.Environment.IsDevelopment()) // �berpr�ft, ob die App im Entwicklungsmodus l�uft
        {
            app.UseSwagger();    // Aktiviert Swagger-UI f�r die API-Dokumentation
            app.UseSwaggerUI();  // Zeigt die interaktive Swagger-Oberfl�che im Browser an
        }

        app.UseHttpsRedirection(); // Erzwingt HTTPS, um sichere Verbindungen zu verwenden
        app.UseAuthorization();    // Aktiviert die Autorisierung (hier nicht konfiguriert)

        // Verbindet Controller-Endpunkte (z.B. api/Mine) mit der Anwendung
        app.MapControllers();

        // Startet die Anwendung und wartet auf eingehende HTTP-Anfragen
        app.Run();
    }
}
