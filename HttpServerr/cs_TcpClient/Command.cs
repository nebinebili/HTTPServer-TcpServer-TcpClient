using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cs_TcpClient
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

        public void GetMethod(Command cmd,string str,string response,BinaryWriter bw,BinaryReader br)
        {
            cmd = new Command { Text = str };
            bw.Write(JsonSerializer.Serialize(cmd));

            response = br.ReadString();

            var cars = JsonSerializer.Deserialize<List<Car>>(response);
            foreach (var c in cars)
            Console.WriteLine($"Car id: {c.Id} Car vendor:{c.Vendor} Car model: {c.Model} Car year: {c.Year} ");
           
        }

        public void PostMethod(Command cmd, string str, string response, BinaryWriter bw)
        {
            Console.Write("Vendor:");
            var vendor = Console.ReadLine();
            Console.Write("Model:");
            var model = Console.ReadLine();
            Console.Write("Year:");
            var year = Console.ReadLine();

            var car = new Car { Model = model, Vendor = vendor, Year = 2018 };
            cmd = new Command { Text = str, Value = car };
            bw.Write(JsonSerializer.Serialize(cmd));
        }

        public void DeleteMethod(Command cmd, string str, string response, BinaryWriter bw)
        {
            Console.Write("Enter Delete Car Id:");
            var id = Console.ReadLine();

            cmd = new Command { Text = str, DeleteId = Convert.ToInt32(id) };
            bw.Write(JsonSerializer.Serialize(cmd));
        }

        public void PutMethod(Command cmd, string str, string response, BinaryWriter bw)
        {
            Console.Write("Enter Update Car Id:");
            var id1 = Console.ReadLine();

            Console.Write("Vendor:");
            var newvendor = Console.ReadLine();
            Console.Write("Model:");
            var newmodel = Console.ReadLine();
            Console.Write("Year:");
            var newyear = Console.ReadLine();

            var car_u = new Car { Vendor = newvendor, Model = newmodel, Year = Convert.ToInt32(newyear) };
            cmd = new Command { Text = str, UpdateId = Convert.ToInt32(id1), Value = car_u };
            bw.Write(JsonSerializer.Serialize(cmd));
        }
    }
}
