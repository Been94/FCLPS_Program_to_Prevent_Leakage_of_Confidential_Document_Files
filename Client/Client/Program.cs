using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 9999;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("client connected!!");
            NetworkStream ns = client.GetStream();

            string s;
            while (true)
            {
                s = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(s);
                ns.Write(buffer, 0, buffer.Length);
                byte[] receivedBytes = new byte[1024];
                int byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length);
                byte[] formated = new byte[byte_count];
                //handle  the null characteres in the byte array
                Array.Copy(receivedBytes, formated, byte_count);
                string data = Encoding.UTF8.GetString(formated);
                Console.WriteLine(data);
            }
            ns.Close();
            client.Close();
            Console.WriteLine("disconnect from server!!");
            Console.ReadKey();
        }
    }
    }
