using System;
using System.IO;
using Renci.SshNet;

namespace SFTPConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Список содержимого sftp: \r\n");
            ListDirectory("host", " username",  "password");
            //string localFile = Console.ReadLine();
            string localFile = Console.ReadLine();
            localFile = Convert.ToString(Directory.GetCurrentDirectory() + localFile);
            DownloadFile(localFile, "host", " username", "password");
            if (File.Exists(Convert.ToString( Directory.GetCurrentDirectory()+localFile)))
            {
                Console.WriteLine("success");
            }
            else { Console.WriteLine("fail!"); }
        }
        /// <summary>
        /// This sample will download a file on the remote system to your local machine.
        /// </summary>
        public static void DownloadFile(string localFile, string host, string username, string password)
        {
            string localFileName = System.IO.Path.GetFileName(localFile);
            string remoteFileName = "";

            using (var sftp = new SftpClient(host, username, password))
            {
                sftp.Connect();

                using (var file = File.OpenWrite(localFileName))
                {
                    sftp.DownloadFile(remoteFileName, file);
                }

                sftp.Disconnect();
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
