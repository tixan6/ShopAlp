using Npgsql;
namespace ShopAlp.Scripts
{
    public class ConnectToDatabase
    {
        string query;
        NpgsqlConnection connection;

        private static string Host = "localhost";
        private static string User = "postgres";
        private static string DBname = "alpgore";
        private static string Password = "159753";
        private static string Port = "5432";

        string connString =
               String.Format(
                   "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                   Host,
                   User,
                   DBname,
                   Port,
                   Password);



        public object take(string query)
        {
            
            

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader npgSqlDataReader = command.ExecuteReader();

            if (npgSqlDataReader.HasRows)
            {
                
                return npgSqlDataReader;
            }
            else
            {
                return new Exception("Нет данных");
            }
        }

        public void Close() { connection.Close(); }
        public void Open() {
            connection = new NpgsqlConnection(connString);
            connection.Open(); 
        }

        public void put(string query)
        {
            connection = new NpgsqlConnection(connString);
            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader npgSqlDataReader = command.ExecuteReader();
            connection.Close();    
        }







    }
}
