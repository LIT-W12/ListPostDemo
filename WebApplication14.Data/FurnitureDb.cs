using Microsoft.Data.SqlClient;

namespace WebApplication14.Data
{

    public class FurnitureDb
    {
        private readonly string _connectionString;
        public FurnitureDb(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<FurnitureItem> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Furniture";
            connection.Open();
            var list = new List<FurnitureItem>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new FurnitureItem
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Color = (string)reader["Color"],
                    Price = (decimal)reader["Price"],
                    QuantityInStock = (int)reader["QuantityInStock"],
                });
            }
            return list;
        }

        public void DeleteMultiple(List<int> ids)
        {
            using var connection = new SqlConnection(_connectionString);

            List<string> parameters = new List<string>();
            for (int i = 0; i < ids.Count; i++)
            {
                parameters.Add($"@id{i}");
            }

            string sql = $"DELETE FROM Furniture WHERE Id IN ({string.Join(",", parameters)})";

            using SqlCommand cmd = new SqlCommand(sql, connection);
            for (int i = 0; i < ids.Count; i++)
            {
                cmd.Parameters.AddWithValue($"@id{i}", ids[i]);
            }
            connection.Open();
            cmd.ExecuteNonQuery();
        }


        public void AddMultiple(List<FurnitureItem> items)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();

            cmd.CommandText = "INSERT INTO Furniture (Name, Color, Price, QuantityInStock, PreAssembled) VALUES" +
                "(@name, @color, @price, @qty, 1)";
            connection.Open();
            foreach(var item in items)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@color", item.Color);
                cmd.Parameters.AddWithValue("@price", item.Price);
                cmd.Parameters.AddWithValue("@qty", item.QuantityInStock);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
