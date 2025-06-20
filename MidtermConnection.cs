using MySql.Data.MySqlClient;

public class DBConnection
{
    private static string connStr = "server=localhost;user=root;password=yourpassword;database=yourdatabase";
    public static MySqlConnection GetConnection()
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
        }
        catch (MySqlException ex)
        {
            MessageBox.Show("Connection Error: " + ex.Message);
        }
        return conn;
    }
}
