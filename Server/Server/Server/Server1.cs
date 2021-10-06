using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO ;
using System.Threading;
using System.Collections;

namespace Server
{
    class Server2
    {
        //public static Hashtable clientsList = new Hashtable();

        static IPEndPoint end;
        static Socket sock;
        public static string path= "C:/Users/ADMIN/Desktop/";
        public static string path1 = "C:/Users/ADMIN/Desktop/";
        public static string MessageCurrent = "Stopped";
        public static string name=null;
        public Server2()
        {           
            end = new IPEndPoint(IPAddress.Any, 2000);
            sock = new Socket (AddressFamily .InterNetwork , SocketType.Stream , ProtocolType .IP);
            sock.Bind(end);
            sock.Listen(5);
            Thread ctThread = new Thread(Accept1);
            ctThread.Start();
        }
        
        public void Accept1()
        {
            while (true)
            {
                Socket clientSock = sock.Accept();
                byte[] data = new byte[15];
                clientSock.Receive(data);
                ASCIIEncoding encoding = new ASCIIEncoding();
                name = encoding.GetString(data);
                Thread.Sleep(2000);
                //clientsList.Add(name, clientSock);
                handleClinet client = new handleClinet();
                client.startClient(clientSock, name);
            }
        }
        

        public class handleClinet
        {
            Socket clientSocket;
            string clNo;

            public void startClient(Socket inClientSocket, string clineNo)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                Thread ctThread = new Thread(dosend);
                ctThread.Start();
            }

            private void dosend()
            {

                while ((true))
                {
                    try
                    {
                        byte[] clientData = new byte[1024 * 5000];
                        int receiveByteLen = clientSocket.Receive(clientData);
                        int fNameLen = BitConverter.ToInt32(clientData, 0);
                        string fName = Encoding.ASCII.GetString(clientData, 4, fNameLen);
                        BinaryWriter write = new BinaryWriter(File.Open(path1 + "/" +clNo+"/" + fName, FileMode.Create));
                        write.Write(clientData, 4 + fNameLen, receiveByteLen - 4 - fNameLen);
                        write.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        } 
    }
}
