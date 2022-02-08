using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cs_TcpListener
{
    public class Command
    {
        public const string Get = "get";
        public const string Post = "post";
        public const string Put = "put";
        public const string Delete = "delete";

        public string Text { get; set; }
        public int DeleteId { get; set; }
        public int UpdateId { get; set; }
        public Car Value { get; set; }

        public void GetMethod(BinaryWriter bw)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=CarDB.db "))
            {
                connection.Open();
                

                string request = @"Select * From Cars";
                var cmd = new SQLiteCommand(request, connection);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                List<Car> temp = new List<Car>();
                while (rdr.Read())
                {
                    temp.Add(new Car { Id = rdr.GetInt32(0), Vendor = rdr.GetString(1), Model = rdr.GetString(2), Year = rdr.GetInt32(3)});
                }
                bw.Write(JsonSerializer.Serialize(temp));
            }
        }

        public void PostMethod(Car car)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=CarDB.db "))
            {
                connection.Open();

                using(var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "INSERT INTO Cars(Vendor,Model,Year) VALUES(@vendor,@model,@year)";

                    cmd.Parameters.AddWithValue("@vendor",car.Vendor);
                    cmd.Parameters.AddWithValue("@model",car.Model);
                    cmd.Parameters.AddWithValue("@year", car.Year);
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void PutMethod(Car car,int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=CarDB.db "))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "UPDATE Cars SET Vendor=@vendor, Model=@model, Year=@year WHERE Id=@id";

                    cmd.Parameters.Add("@vendor",DbType.String).Value=car.Vendor;
                    cmd.Parameters.Add("@model", DbType.String).Value = car.Model;
                    cmd.Parameters.Add("@year",DbType.Int32).Value=car.Year;
                    cmd.Parameters.Add("@id", DbType.Int32).Value=id;
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMethod(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=CarDB.db "))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                   
                    cmd.CommandText = "DELETE FROM Cars  WHERE Id=@id";                  
                    cmd.Parameters.Add("@id", DbType.Int32).Value = id;

                    cmd.ExecuteNonQuery();

                }
            }
        }
    }
}
