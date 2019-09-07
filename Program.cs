using System;
using System.IO;
using Renci.SshNet;
using System.Text;

namespace SFTPConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("L - list directory \r\nD - downlad file from sftp\r\nDEL - delete file from sftp\r\nU - upload file to sftp\r\nE- exit from app");
            string choose = "";
            do
            {
                Console.WriteLine("L - list directory \r\nD - downlad file from sftp\r\nDEL - delete file from sftp\r\nU - upload file to sftp\r\nE- exit from app");
                choose = Console.ReadLine();
                //Console.WriteLine("filename:");
                string localFile = "";
                //localFile = Convert.ToString(Directory.GetCurrentDirectory() + localFile);
                switch (choose)
                {
                    case "L":
                        ListDirectory("host",  "username",  "password");
                        break;
                    case "D":
                        localFile = "";
                        DownloadFile(localFile, "host", "username", "password");
                        break;
                    case "U":
                        Console.Write("filename: ");
                        localFile = "\\" + Console.ReadLine();
                        localFile = Convert.ToString(Directory.GetCurrentDirectory() + localFile);
                        UploadFile(localFile, "host", "username", "password");
                        break;
                    case "DEL":
                        Console.Write("filename: ");
                        localFile = "\\" + Console.ReadLine();
                        localFile = Convert.ToString(Directory.GetCurrentDirectory() + localFile);
                        DeleteFile(localFile, "host", "username", "password");
                        break;
                }
                Console.WriteLine("-----------------------------------------");
            } while (choose!="E");
         Console.Read();
        }
        /// <summary>
        /// This sample will download a file on the remote system to your local machine.
        /// </summary>
        public static void DownloadFile(string localFile, string host, string username, string password)
        {
            try
            {
                Console.WriteLine("Enter local filename: ");
                localFile = "\\" + Console.ReadLine();
                localFile = Convert.ToString(Directory.GetCurrentDirectory() + localFile);
                // Delete the file if it exists.
                if (File.Exists(localFile))
                {
                    File.Delete(localFile);
                }

                //Create a file to write to.
                
                string localFileName = System.IO.Path.GetFileName(localFile);
                Console.WriteLine("Enter sftp filename: ");
                string remoteFileName = Console.ReadLine();

                using (var sftp = new SftpClient(host, username, password))
                {
                    sftp.Connect();

                    using (var file = File.OpenWrite(localFile))
                    {
                        sftp.DownloadFile(remoteFileName, file);
                    }

                    sftp.Disconnect();
                    sftp.Dispose();
                }
            }
            catch { }
        }
        public static void UploadFile(string localFile, string host, string username, string password)
        {
            FileInfo f = new FileInfo(localFile);
            string uploadfile = f.FullName;
            Console.WriteLine(f.Name);
            Console.WriteLine("uploadfile: " + uploadfile+"\r\n");

            //Passing the sftp host without the "sftp://"
            var client = new SftpClient(host, username, password);
            client.Connect();
            if (client.IsConnected)
            {

                var fileStream = new FileStream(uploadfile, FileMode.Open);
                if (fileStream != null)
                {
                    //If you have a folder located at sftp://ftp.example.com/share
                    //then you can add this like:
                    client.UploadFile(fileStream, "/root/" + f.Name, null);
                    client.Disconnect();
                    client.Dispose();
                }
                fileStream.Close();
            }
        }
        public static void DeleteFile(string localFile, string host, string username, string password)
        {
            FileInfo f = new FileInfo(localFile);
            string uploadfile = f.FullName;
            Console.WriteLine(f.Name);
            Console.WriteLine("deletefile: " + uploadfile + "\r\n");

            //Passing the sftp host without the "sftp://"
            var client = new SftpClient(host, username, password);
            client.Connect();
            if (client.IsConnected)
            {

                var fileStream = new FileStream(uploadfile, FileMode.Open);
                if (fileStream != null)
                {
                    //If you have a folder located at sftp://ftp.example.com/share
                    //then you can add this like:
                    client.DeleteFile("/root/" + f.Name);
                    client.Disconnect();
                    client.Dispose();
                }
                fileStream.Close();
            }
        }

        /// <summary>
        /// This will list the contents of the current directory.
        /// </summary>
        public static void ListDirectory(string host, string username, string password)
        {
            string remoteDirectory = "."; // . always refers to the current directory.

            using (var sftp = new SftpClient(host, username, password))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(remoteDirectory);
                foreach (var file in files)
                {
                    Console.WriteLine(file.FullName);
                }
                sftp.Disconnect();
            }
        }
    }
}
