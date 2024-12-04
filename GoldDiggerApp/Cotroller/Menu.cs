public static class Menu
{
    private static MineController mineController; // Speichert die Instanz des MineControllers

    // Initialisiert den MineController mit der DB-Verbindung
    public static void Initialize(DbConnector dbConnector)
    {
        mineController = new MineController(dbConnector); // Erstellt einen neuen Controller mit der Verbindung
    }

    // Zeigt das Hauptmenü für CRUD-Operationen
    public static void DisplayMenu()
    {
        while (true) // Endlosschleife für das Menü
        {
            Console.Clear(); // Löscht die Konsole für bessere Lesbarkeit
            Console.WriteLine("CRUD-Operationen für die Mines-Tabelle");
            Console.WriteLine("1. Alle Einträge anzeigen (READ)");
            Console.WriteLine("2. Neuen Eintrag hinzufügen (CREATE)");
            Console.WriteLine("3. Eintrag aktualisieren (UPDATE)");
            Console.WriteLine("4. Eintrag löschen (DELETE)");
            Console.WriteLine("5. Beenden");
            Console.Write("Wählen Sie eine Option: ");
            string choice = Console.ReadLine(); // Eingabe des Benutzers lesen

            // Auswahl prüfen und entsprechende Methode ausführen
            switch (choice)
            {
                case "1":
                    mineController.GetAllMines(); 
                    break;
                case "2":
                    CreateMineMenu(); 
                    break;
                case "3":
                    UpdateMineMenu(); 
                    break;
                case "4":
                    DeleteMineMenu(); 
                    break;
                case "5":
                    return; 
                default:
                    Console.WriteLine("Ungültige Auswahl. Bitte erneut versuchen.");
                    break;
            }

            Console.WriteLine("Drücken Sie eine beliebige Taste, um fortzufahren...");
            Console.ReadKey(); // Warte, bis der Benutzer eine Taste drückt
        }
    }

    // Untermenü für das Erstellen einer neuen Mine
    private static void CreateMineMenu()
    {
        Console.Write("Geben Sie den Namen der Mine ein: ");
        string name = Console.ReadLine(); 

        Console.Write("Geben Sie den Standort der Mine ein: ");
        string standort = Console.ReadLine(); 

        var mine = new Mine { Name = name, Standort = standort }; // Neues Mine-Objekt erstellen
        mineController.CreateMine(mine); // Aufruf der Create-Methode im Controller
    }

    private static void UpdateMineMenu()
    {
        Console.Write("Geben Sie die ID der zu aktualisierenden Mine ein: ");
        int id = int.Parse(Console.ReadLine()); // ID der Mine eingeben

        Console.Write("Geben Sie den neuen Namen der Mine ein: ");
        string name = Console.ReadLine(); 

        Console.Write("Geben Sie den neuen Standort der Mine ein: ");
        string standort = Console.ReadLine(); 

        var mine = new Mine { Name = name, Standort = standort }; // Neues Mine-Objekt erstellen
        mineController.UpdateMine(id, mine); // Aufruf der Update-Methode im Controller
    }

    // Untermenü zum Löschen einer Mine
    private static void DeleteMineMenu()
    {
        Console.Write("Geben Sie die ID der zu löschenden Mine ein: ");
        int id = int.Parse(Console.ReadLine()); // ID der Mine eingeben
        mineController.DeleteMine(id); // Aufruf der Delete-Methode im Controller
    }
}
