using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace cs_TcpListener
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Database

            //var connection = new SQLiteConnection("Data Source=CarDB.db ");
            //if (!File.Exists("./CarDB.db"))
            //{
            //    System.Console.WriteLine();
            //    SQLiteConnection.CreateFile("CarDB.db");
            //}

            var ip = IPAddress.Parse("127.0.0.1");

            var listener = new TcpListener(ip, 45678);
            listener.Start(100);

            while (true)
            {
               
                var client = listener.AcceptTcpClient();
                var stream = client.GetStream();
                var br = new BinaryReader(stream);
                var bw = new BinaryWriter(stream);

                while (true)
                {
                    var input = br.ReadString();

                    var command = JsonSerializer.Deserialize<Command>(input);
                    
                    if (command == null) continue;

                    switch (command.Text)
                    {
                        case Command.Get:                          
                            command.GetMethod(bw);
                            Console.WriteLine("Cars Send Succesfully!!");
                            break;
                        case Command.Post:                           
                            command.PostMethod(command.Value);
                            Console.WriteLine("Car Add Succesfully!!");                           
                            break;
                        case Command.Delete:
                            command.DeleteMethod(command.DeleteId);
                            Console.WriteLine("Car Delete Succesfully!!");
                            break;
                        case Command.Put:                          
                            command.PutMethod(command.Value, command.UpdateId);
                            Console.WriteLine("Car Update Succesfully!!");
                            break;
                    }
                }
            }
        }
    }
}
