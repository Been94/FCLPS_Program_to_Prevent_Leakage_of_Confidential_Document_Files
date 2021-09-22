using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

            int count = 1;


            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 9999);
            ServerSocket.Start();

            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();
                list_clients.Add(count, client);

                count++;
                Box box = new Box(client, list_clients);

                Thread t = new Thread(handle_clients);
                t.Start(box);
            }

        }

        public static void handle_clients(object o)
        {
            Box box = (Box)o;
            Dictionary<int, TcpClient> list_connections = box.list;

            while (true)
            {
                NetworkStream stream = box.c.GetStream();



                Socket c = box.c.Client;
                IPEndPoint ip_point = (IPEndPoint)c.RemoteEndPoint;
                string ip = ip_point.Address.ToString();
                string port = ip_point.Port.ToString();

                Console.WriteLine("{0} {1} connected!!", ip, port);

                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                byte[] formated = new Byte[byte_count];
                //handle  the null characteres in the byte array
                Array.Copy(buffer, formated, byte_count);
                string data = Encoding.UTF8.GetString(formated);


                broadcast(ip_point.Address.ToString(), ip_point.Port.ToString(),list_connections, data);
                Console.WriteLine(data);

            }
        }

        public static void broadcast(string ipadress, string port, Dictionary<int, TcpClient> conexoes, string data)
        {
            Socket s;
            foreach (TcpClient c in conexoes.Values)
            {

                s = c.Client;
                IPEndPoint ip_point = (IPEndPoint)s.RemoteEndPoint;

                if (!ip_point.Address.ToString().Equals(ipadress))
                {

                    if (!ip_point.Port.ToString().Equals(port))
                    {
                        NetworkStream stream = c.GetStream();

                        byte[] buffer = Encoding.UTF8.GetBytes(data);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                } 


            }
        }

    }
    class Box
    {
        public TcpClient c;
        public Dictionary<int, TcpClient> list;

        public Box(TcpClient c, Dictionary<int, TcpClient> list)
        {
            this.c = c;
            this.list = list;
        }

    }
}
