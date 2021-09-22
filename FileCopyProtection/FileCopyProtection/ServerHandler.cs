using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyProtection
{
    class ServerHandler
    {
        public static string public_key = null;
        StreamReader reader = null;
        public ServerHandler(StreamReader reader)
        {
            this.reader = reader;
        }

        public void chat()
        {
            /*
            if (reader.ReadLine().Contains("RSAKeyValue"))
            {
                public_key = reader.ReadLine();
                Console.WriteLine(public_key);
            }
            else
            {
                Console.WriteLine(reader.ReadLine());
            }
            */
            string tmp = string.Format("{0}", reader.ReadLine());
            if (public_key == null)
            {

                if (tmp.Contains("RSAKeyValue"))
                {
                    public_key = tmp;
                }
                else
                {
                    Console.WriteLine(tmp);
                }
            }
            else
            {
                Console.WriteLine(tmp);
            }


        }
    }
}
