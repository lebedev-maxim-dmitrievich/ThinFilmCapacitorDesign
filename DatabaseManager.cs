using System.Data;
using System.Data.SQLite;

public class DatabaseManager
{
    private SQLiteConnection connection;

    public DatabaseManager()
    {
        // Создание подключения к базе данных
        connection = new SQLiteConnection("Data Source=mydatabase.db;Version=3;");
    }

    public void CreateDatabase()
    {
        // Открытие подключения
        connection.Open();

        // Создание таблицы
        string createTableQuery = "CREATE TABLE IF NOT EXISTS Capacitor (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "DielectricMaterial TEXT," +
            "CoverMaterial TEXT," +
            "CoverResistivity DOUBLE," +
            "CapacitanceDensity DOUBLE," +
            "ElectricStrength DOUBLE," +
            "DielectricPermittivity DOUBLE," +
            "TgDelta DOUBLE," +
            "WorkingFrequency DOUBLE," +
            "TKE DOUBLE" +
            ");";

        SQLiteCommand command = new SQLiteCommand(createTableQuery, connection);
        command.ExecuteNonQuery();

        // Закрытие подключения
        connection.Close();
    }

    public void InsertData(string dielectricMaterial, string coverMaterial, double coverResistivity, double capacitanceDensity, double electricStrength, double dielectricPermittivity, double tgDelta, double workingFrequency, double TKE)
    {
        // Открытие подключения
        connection.Open();

        string insertQuery = "INSERT INTO Capacitor (DielectricMaterial, CoverMaterial, CoverResistivity, CapacitanceDensity, ElectricStrength, DielectricPermittivity, TgDelta, WorkingFrequency, TKE) " +
            "VALUES (@DielectricMaterial, @CoverMaterial, @CoverResistivity, @CapacitanceDensity, @ElectricStrength, @DielectricPermittivity, @TgDelta, @WorkingFrequency, @TKE);";

        SQLiteCommand command = new SQLiteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@DielectricMaterial", dielectricMaterial);
        command.Parameters.AddWithValue("@CoverMaterial", coverMaterial);
        command.Parameters.AddWithValue("@CoverResistivity", coverResistivity);
        command.Parameters.AddWithValue("@CapacitanceDensity", capacitanceDensity);
        command.Parameters.AddWithValue("@ElectricStrength", electricStrength);
        command.Parameters.AddWithValue("@DielectricPermittivity", dielectricPermittivity);
        command.Parameters.AddWithValue("@TgDelta", tgDelta);
        command.Parameters.AddWithValue("@WorkingFrequency", workingFrequency);
        command.Parameters.AddWithValue("@TKE", TKE);

        command.ExecuteNonQuery();

        // Закрытие подключения
        connection.Close();
    }

    public DataTable GetMaterialData(string material)
    {
        // Открытие подключения
        connection.Open();

        string selectQuery = "SELECT * FROM Capacitor WHERE DielectricMaterial = @Material;";

        SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@Material", material);

        SQLiteDataReader reader = command.ExecuteReader();

        DataTable dataTable = new DataTable();
        dataTable.Load(reader);

        reader.Close();

        // Закрытие подключения
        connection.Close();

        return dataTable;
    }

    public DataTable GetAllData()
    {
        // Открытие подключения
        connection.Open();

        string selectQuery = "SELECT * FROM Capacitor;";

        SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
        SQLiteDataReader reader = command.ExecuteReader();

        DataTable dataTable = new DataTable();
        dataTable.Load(reader);

        reader.Close();

        // Закрытие подключения
        connection.Close();

        return dataTable;
    }
}
