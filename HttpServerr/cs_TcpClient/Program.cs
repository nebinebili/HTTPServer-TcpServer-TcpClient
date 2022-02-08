using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace cs_TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TcpClient();
            client.Connect("127.0.0.1", 45678);
            var stream = client.GetStream();
            var br = new BinaryReader(stream);
            var bw = new BinaryWriter(stream);
            while (true)
            {
                Console.Write("Write Request:");
                
                var str = Console.ReadLine();

                Command cmd =new Command();
                string response = null;

                switch (str)
                {
                    case Command.Get:
                        cmd.GetMethod(cmd,str,response,bw,br);
                        break;
                    case Command.Post:
                        cmd.PostMethod(cmd, str, response, bw);
                        break;
                    case Command.Delete:
                        cmd.DeleteMethod(cmd, str, response, bw);
                        break;
                    case Command.Put:
                        cmd.PutMethod(cmd, str, response, bw);
                        break;
                }
            }
        }
    }
}
