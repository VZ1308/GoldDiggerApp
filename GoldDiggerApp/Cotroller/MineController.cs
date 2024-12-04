using Microsoft.AspNetCore.Mvc; // Für Controller- und API-Funktionalität
using MySql.Data.MySqlClient;    // Zugriff auf MySQL

[Route("api/[controller]")]  // Basis-Route für diesen Controller: api/Mine
[ApiController]               // Kennzeichnet diese Klasse als API-Controller

public class MineController : ControllerBase
{
    private readonly DbConnector _dbConnector; // Verbindung zur DB

    // Konstruktor zur Injektion der Datenbankverbindung
    public MineController(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    // GET: api/Mine - Alle Minen abrufen (READ)
    [HttpGet]
    public IActionResult GetAllMines()
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
        return Ok(mines); // Rückgabe der Minen als Antwort
    }

    // POST: api/Mine - Neue Mine erstellen (CREATE)
    [HttpPost]
    public IActionResult CreateMine([FromBody] Mine mine)
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("INSERT INTO Mines (Name, Standort) VALUES (@name, @standort)", connection);
        command.Parameters.AddWithValue("@name", mine.Name); // Parameter binden
        command.Parameters.AddWithValue("@standort", mine.Standort);

        command.ExecuteNonQuery(); // SQL-Befehl ausführen
        return CreatedAtAction(nameof(GetAllMines), new { id = mine.Id }, mine);
    }

    // PUT: api/Mine/{id} - Mine aktualisieren (UPDATE)
    [HttpPut("{id}")]
    public IActionResult UpdateMine(int id, [FromBody] Mine mine)
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("UPDATE Mines SET Name = @name, Standort = @standort WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
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
