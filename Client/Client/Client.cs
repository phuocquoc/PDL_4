using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
namespace Client
{
    class Client
    {
        public static string strComputerName = Environment.MachineName.ToString();
        static ASCIIEncoding encoding = new ASCIIEncoding();
        static IPEndPoint end;
        static readonly Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

        
        public Client()
        {
            IPAddress ip = IPAddress.Parse("192.168.43.5");
            end = new IPEndPoint(ip, 2000);
            sock.Connect(end);  
        }

        public void GetName()
        {
            try
            {   
                byte[] clientData = encoding.GetBytes(strComputerName);               
                sock.Send(clientData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(""+ ex);
            }

        }   
        public void SendFile(string path,string fName)
        {
            try
            {
                //Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                byte[] fNameByte = Encoding.ASCII.GetBytes(fName);
                path = path.Replace("\\", "/");
                byte[] fileData = File.ReadAllBytes(path +"/"+ fName);
                byte[] clientData = new byte[4 + fNameByte.Length + fileData.Length];
                byte[] fNameLen = BitConverter.GetBytes(fNameByte.Length);
                fNameLen.CopyTo(clientData, 0);
                fNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fNameByte.Length);
                sock.Send(clientData );
            }
            catch(Exception ex)
            {
                Console.WriteLine("" + ex);
            }

        }
    }
}
