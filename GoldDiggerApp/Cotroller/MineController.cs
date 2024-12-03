using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class MineController : ControllerBase
{
    private readonly DbConnector _dbConnector;

    public MineController(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    // GET: api/Mine
    [HttpGet]
    public IActionResult GetAllMines()
    {
        var mines = new List<Mine>();
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("SELECT * FROM Mines", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
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

    // POST: api/Mine
    [HttpPost]
    public IActionResult CreateMine([FromBody] Mine mine)
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("INSERT INTO Mines (Name, Standort) VALUES (@name, @standort)", connection);
        command.Parameters.AddWithValue("@name", mine.Name);
        command.Parameters.AddWithValue("@standort", mine.Standort);

        command.ExecuteNonQuery();
        return CreatedAtAction(nameof(GetAllMines), new { id = mine.Id }, mine);
    }

    // PUT: api/Mine/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateMine(int id, [FromBody] Mine mine)
    {
        using var connection = _dbConnector.GetConnection();
        var command = new MySqlCommand("UPDATE Mines SET Name = @name, Standort = @standort WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", mine.Name);
        command.Parameters.AddWithValue("@standort", mine.Standort);

        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0) return NotFound($"Mine with ID {id} not found.");
        return Ok($"Mine with ID {id} updated.");
    }

    // DELETE: api/Mine/{id}
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
