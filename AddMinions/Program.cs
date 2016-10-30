using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddMinions
{
    class Program
    {
        public static SqlConnection connection = new SqlConnection("Data Source=(local);Initial Catalog=MinionsDB;Integrated Security=True");

   
        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split(' ');
            string minionName = input[1];
            int age = int.Parse(input[2]);
            string town = input[3];
            input = Console.ReadLine().Split(' ');
            string villiansName = input[1];
            var minionTown = "SELECT Id FROM Towns WHERE name=@townName";
            SqlCommand command = new SqlCommand(minionTown, connection);
            command.Parameters.AddWithValue("@townName", town);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                string townToAdd = "INSERT INTO Towns(name,country) VALUES(@townName,NULL)";
                SqlCommand addTown = new SqlCommand(townToAdd, connection);
                addTown.Parameters.AddWithValue("@townName", town);
                addTown.ExecuteNonQuery();
                Console.WriteLine("Town {0} was added to the database.",town);
            }
            reader.Close();
            int townId = (int)command.ExecuteScalar();
            reader.Close();

            string villian = "SELECT * FROM Villains WHERE name=@villians";
            SqlCommand cmd = new SqlCommand(villian, connection);
            cmd.Parameters.AddWithValue("@villians", villiansName);
            reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                string addVillainSQL = "INSERT INTO villains (name, EvilnessFactor) VALUES (@villians, 'evil')";
                SqlCommand comd = new SqlCommand(addVillainSQL, connection);
                comd.Parameters.AddWithValue("@villians",villiansName);
                comd.ExecuteNonQuery();
                Console.WriteLine("Villain {0} was added to the database.",villiansName);
                   
            }
            reader.Close();
            int villianId =(int) cmd.ExecuteScalar();
            reader.Close();

            string addMinion = "INSERT INTO Minions(Name,Age,TownId) VALUES(@name,@age,@townId)";
            SqlCommand comm = new SqlCommand(addMinion, connection);
            comm.Parameters.AddWithValue("@name", minionName);
            comm.Parameters.AddWithValue("@age", age);
            comm.Parameters.AddWithValue("@townId", townId);
            comm.ExecuteNonQuery();
            string getMinionId = "select id from Minions where name=@minionName";
            cmd = new SqlCommand(getMinionId, connection);
            cmd.Parameters.AddWithValue("@minionName", minionName);
            int minionId = (int)cmd.ExecuteScalar();
            string addMinionToVillian = "INSERT INTO MinionsVillains(MinionId,VillainId) VALUES(@minionId, @villainId)";
            SqlCommand add = new SqlCommand(addMinionToVillian, connection);
            add.Parameters.AddWithValue("@minionId", minionId);
            add.Parameters.AddWithValue("@villainId", villianId);
            add.ExecuteNonQuery();
            Console.WriteLine("Successfully added {0} to be minion of {1}.",minionName,villiansName);
        }
    }
}
