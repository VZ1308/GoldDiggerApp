using Microsoft.AspNetCore.Mvc; // Für Controller- und API-Funktionalität
using MySql.Data.MySqlClient;    // Zugriff auf MySQL

[Route("api/[controller]")]  // Basis-Route für diesen Controller: api/Mine
[ApiController]               // Kennzeichnet diese Klasse als API-Controller

public class MineController : ControllerBase // bietet Funktionen für den Zugriff auf HTTP-Anfragen und das Erstellen von Antworten
{
    private readonly DbConnector _dbConnector; // Verbindung zur DB

    // Konstruktor zur Injektion der Datenbankverbindung
    public MineController(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    // GET: api/Mine - Alle Minen abrufen (READ)
    [HttpGet]
    public IActionResult GetAllMines() // IActionResult ist ein Interface in ASP.NET Core, das verwendet wird, um Ergebnisse von API-Methoden zurückzugeben
    {
        var mines = new List<Mine>(); // Liste zum Speichern der Ergebnisse
        using var connection = _dbConnector.GetConnection(); // Verbindung öffnen
        var command = new MySqlCommand("SELECT * FROM Mines", connection); // SQL-Befehl
        using var reader = command.ExecuteReader(); // Ergebnis ausführen und lesen

        while (reader.Read()) // Jede Zeile durchlaufen
        {
            mines.Add(new Mine
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Standort = reader.GetString("Standort")
            });
        }
        return Ok(mines); 
    }

    // POST: api/Mine - Neue Mine erstellen (CREATE)
    [HttpPost]
    public IActionResult CreateMine([FromBody] Mine mine) // FromBody konvertiert die JSON-Daten automatisch in ein Mine-Objekt
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("INSERT INTO Mines (Name, Standort) VALUES (@name, @standort)", connection);
        command.Parameters.AddWithValue("@name", mine.Name); // Parameter binden
        command.Parameters.AddWithValue("@standort", mine.Standort);

        command.ExecuteNonQuery(); // Führt den SQL-Befehl aus, um die Daten in die Datenbank einzufügen. Es wird verwendet, weil kein Ergebnis zurückgegeben wird
        return CreatedAtAction(nameof(GetAllMines), new { id = mine.Id }, mine); //  Gibt eine HTTP 201 Created-Antwort zurück und erstellt eine URL, die auf die neue Ressource verweist
    }

    // PUT: api/Mine/{id} - Mine aktualisieren (UPDATE)
    [HttpPut("{id}")]
    public IActionResult UpdateMine(int id, [FromBody] Mine mine)
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("UPDATE Mines SET Name = @name, Standort = @standort WHERE Id = @id", connection);
        //command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", mine.Name);
        command.Parameters.AddWithValue("@standort", mine.Standort);

        var rowsAffected = command.ExecuteNonQuery(); // Anzahl betroffener Zeilen prüfen
        if (rowsAffected == 0) return NotFound($"Mine with ID {id} not found.");
        return Ok($"Mine with ID {id} updated.");
    }

    // DELETE: api/Mine/{id} - Mine löschen (DELETE)
    [HttpDelete("{id}")]
    public IActionResult DeleteMine(int id)
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("DELETE FROM Mines WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0) return NotFound($"Mine with ID {id} not found.");
        return Ok($"Mine with ID {id} deleted.");
    }
}
