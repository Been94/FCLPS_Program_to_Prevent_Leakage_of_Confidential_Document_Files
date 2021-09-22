using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyProtection
{
    class Files
    {
        private static Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값
        public readonly byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        public const int iterations = 4096;
        public static void DecryptFile(string password, string inputFile, string outputFile)
        {
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
        public static void EncryptFile(string password, string inputFile, string outputFile)
        {
            try
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


                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                // Console.WriteLine(RMCrypto.IV.Length + "\r");
                //  Console.WriteLine(key.Length);

                CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                {
                    cs.WriteByte((byte)data);
                }



                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

                Watcher.files_delete(inputFile);

                System.IO.File.Move(outputFile, inputFile);

                Console.WriteLine("\t기밀 파일 의심\n\t\t파일명 = {0}, 조치사항: 대상 파일 -> 암호화", inputFile);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static string RandomString(int _nLength = 20)
        {
            const string tmp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  //문자 생성 풀
            string strPool = tmp;
            strPool = strPool.ToLower();
            char[] chRandom = new char[_nLength];

            for (int i = 0; i < _nLength; i++)
            {
                chRandom[i] = strPool[random.Next(strPool.Length)];
            }
            string strRet = new String(chRandom);   // char to string
            return strRet;
        }


    }
}
