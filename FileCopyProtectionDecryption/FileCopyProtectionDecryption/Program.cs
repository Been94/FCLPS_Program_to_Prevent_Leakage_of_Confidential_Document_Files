using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyProtectionDecryption
{
    class Program
    {
        private static Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값
        public readonly byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        public const int iterations = 4096;
        static void Main(string[] args)
        {
            Console.Write("파일명 : ");
            string inputFile = Console.ReadLine();
            Console.Write("패스워드 : ");
            string password = Console.ReadLine();

            DecryptFile(password, inputFile, inputFile + ".dec");


        }


        public static void DecryptFile(string password, string inputFile, string outputFile)
        {
            
                //string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] tmp = UE.GetBytes(password);
                byte[] key = new byte[16];
                System.Array.Clear(key, 0, key.Length);
                for (int i = 0; i < key.Length; i++)
                {
                    key[i] = tmp[i];
                }

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, key),
                CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);



                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            
        }
    }
}
