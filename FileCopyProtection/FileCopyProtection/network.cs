using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileCopyProtection
{
    class network
    {
        public static void run(string tmp_data)
        {
            TcpClient client = null;
            try
            {
                Console.WriteLine("Client\n");
                client = new TcpClient();
                client.Connect("localhost", 5001);
                NetworkStream stream = client.GetStream();
                Encoding encode = System.Text.Encoding.GetEncoding("euc-kr");
                StreamReader reader = new StreamReader(stream, encode);
                StreamWriter writer = new StreamWriter(stream, encode)
                {
                    AutoFlush = true
                };
                ServerHandler serverHandler = new ServerHandler(reader);
                Thread t = new Thread(new ThreadStart(serverHandler.chat));
                t.Start();
                // string dataToSend = Console.ReadLine();
                //writer.WriteLine(Program.rsa_public_key);

                string tmp = rsa.RSAEncrypt(tmp_data, ServerHandler.public_key);

                writer.WriteLine(tmp);

                //rsa.RSAEncrypt(dataToSend, ServerHandler.public_key);
                /*
                while (true)
                {
                    string tmp = rsa.RSAEncrypt(dataToSend, ServerHandler.public_key);
                    //Console.WriteLine(rsa.RSAEncrypt(dataToSend, ServerHandler.public_key));
                    writer.WriteLine(tmp);
                    if (dataToSend.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                    dataToSend = Console.ReadLine();
                }
                */



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                client.Close();
                client = null;
            }



        }
    }
}
