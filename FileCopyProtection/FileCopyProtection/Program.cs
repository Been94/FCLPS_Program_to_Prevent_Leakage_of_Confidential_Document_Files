using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileCopyProtection
{
    class Program
    {
        public static string file_key = null;
        public static string rsa_private_key = null;
        public static string rsa_public_key = null;


        static void Main(string[] args)
        {
            rsa_private_key = rsa.RSAPrivateKeyGen();
            rsa_public_key = rsa.RSAPublicKeyGen();

            Console.WriteLine(rsa_public_key);
            Console.WriteLine(rsa_private_key);

            file_key = Files.RandomString();
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();
        }
        public static void Run()
        {
            ArrayList arrList = new ArrayList();
            Thread t2 = null;
            Boolean works = true;
            int no_device_cnt = 0;
            while (true)
            {
                DriveInfo[] Drivers = DriveInfo.GetDrives();



                foreach (DriveInfo drive in Drivers)
                {
                    if (drive.DriveType == DriveType.Removable && drive.IsReady)
                    {

                        //Console.WriteLine("Removable Device found!! \n");
                        no_device_cnt = 0;
                        string file_name = drive.Name;

                        if (arrList.Contains(file_name) == false)
                        {
                            arrList.Add(file_name);
                            works = true;
                        }

                        if (works == true)
                        {
                            works = false;

                            t2 = new Thread(() => detect(drive.Name));
                            t2.Start();

                        }

                    }
                    else
                    {
                        if (no_device_cnt == Drivers.Length)
                        {
                            Console.WriteLine("No removable devices found {0}\n", no_device_cnt);
                            no_device_cnt = 0;
                            t2.Abort();
                            arrList.Clear();
                            works = true;
                        }

                        no_device_cnt++;
                    }

                }

                System.Threading.Thread.Sleep(1000);
            }

        }

        public static void detect(string tmp)
        {
            string path = tmp;//@"H:\\";

            // 폴더가 없으면 FileSystemWatcher 를 생성할때 오류가 발생함.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Watcher watcher = new Watcher(path);

            //  Console.WriteLine("종료하려면 앤터를 입력하세요.");

            Console.ReadLine();
        }


    }
}
