using System;
using System.Data;
using System.Data.SqlClient;

namespace MinionNames
{
    class Program
    {
        public static SqlConnection connection = new SqlConnection("Data Source=(local);Initial Catalog=MinionsDB;Integrated Security=True");

        static void Main(string[] args)
        {
            var id = int.Parse(Console.ReadLine());
            string viliansName = "SELECT Name FROM Villains WHERE id=@VillainId";
            SqlCommand command = new SqlCommand(viliansName, connection);
            command.Parameters.AddWithValue("@VillainId", id);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("There is no villiain with Id "+id +" in the database.");
                    return;
                }
                reader.Read();
                string name = reader[0].ToString();
                Console.WriteLine("Name "+name);
            }
            connection.Close();
            string minions="select m.name, age\n" +
                    "FROM Villains v\n" +
                    "JOIN MinionsVillains mv ON v.Id = mv.VillainId\n" +
                    "JOIN Minions m ON m.id = mv.MinionId\n" +
                    "WHERE v.Id = @villainId";
            command = new SqlCommand(minions, connection);
            command.Parameters.AddWithValue("@VillainId", id);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("No minions in the database");
                    return;
                }
                int count = 1;
                while (reader.Read())
                {
                    Console.WriteLine(count+" "+reader["name"]+" "+reader["age"]);
                    count++;
                }
            }
            connection.Close();
         
        }
    }
}
