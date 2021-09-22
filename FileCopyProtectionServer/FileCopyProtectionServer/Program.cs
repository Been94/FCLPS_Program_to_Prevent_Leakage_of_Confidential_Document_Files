using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileCopyProtectionServer
{
    class Program
    {
        public static string private_key, public_key;
        static List<Socket> clientSockets = new List<Socket>();
        static void Main(string[] args)
        {
             private_key = rsa.RSAPrivateKeyGen();
             public_key = rsa.RSAPublicKeyGen();

            Console.WriteLine(public_key);

            Console.WriteLine("Server\n");
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            try
            {
                TcpListener listener = new TcpListener(ip, 5001);
                listener.Start();
                while (true)
                {
                    Socket socket = listener.AcceptSocket();
                    notice(socket, public_key);
                    new Thread(() => chat(socket)).Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        static void notice(Socket socket,string msg)
        {
            clientSockets.Add(socket);

            Encoding encoding = Encoding.GetEncoding("euc-kr");

            NetworkStream stream = new NetworkStream(clientSockets[clientSockets.Count()-1]);
            StreamWriter writer = new StreamWriter(stream, encoding) { AutoFlush = true };
            writer.WriteLine(msg);
            writer.Close();

        }
        static void chat(Socket socket)
        {
            Encoding encoding = Encoding.GetEncoding("euc-kr");
            try{
                
                StreamReader reader = new StreamReader(new NetworkStream(socket), encoding);
                string line;
                while ((line = readLine(reader)) != null){
                    Console.WriteLine(line);
                    foreach (Socket clientSocket in clientSockets){

                        if(clientSocket != socket)
                        {
                            NetworkStream stream = new NetworkStream(clientSocket);
                            StreamWriter writer = new StreamWriter(stream, encoding) { AutoFlush = true };
                            writer.WriteLine(line);
                            writer.Close();
                        }


                    }
                }
            } catch {
            } finally{
                clientSockets.Remove(socket);
                socket.Close();
                socket = null;
            }
        }
        static string readLine(StreamReader reader)
        {
            try {

                string tmp = string.Format("{0}", reader.ReadLine());

                tmp = rsa.RSADecrypt(tmp, private_key);

                return tmp;
            }catch{
                return null;
            }
        }

    }
}
