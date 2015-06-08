using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ClientFTP
{
    class Client
    {
        private string _userId;
        private string _password;
        private string _host;
        private int _port;

        private bool _passiv;

        private TcpClient _clientSocket;
        private TcpClient _clientSocketPassive;

        private StreamWriter _sw;
        private StreamReader _sr;

        public string IpAfterConnect { get; private set; }
        private int _portAfterConnect; // needed for passive connection

        public Client(string id, string pwd, string host)
        { // cponstructor
            _userId = id;
            _password = pwd;
            _host = host;
            _port = 21;

            _clientSocket = new TcpClient();
            _passiv = true;

        }

        public bool Connect()
        {// connect to host


            // ============= COME FROM POP3 PROJECT ==============
            // get IP adress and connect

            //(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress adresse = IPAddress.Parse("127.0.0.1");
            
            bool trouve = false;
            IPAddress[] adresses = Dns.GetHostAddresses(_host);
            
            foreach (IPAddress ip in adresses)
            {//on cherche la première adresse IPV4
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    trouve = true;
                    adresse = ip;
                    break;
                }
            }
            if (!trouve)
            {
                return false;
            }

            _clientSocket.Connect(adresse, _port);

            if (_clientSocket.Connected)
            {
                IpAfterConnect = adresse.ToString();

                // connexion ok,setting streams to read and write
                _sr = new StreamReader(_clientSocket.GetStream(), Encoding.Default);
                _sw = new StreamWriter(_clientSocket.GetStream(), Encoding.Default);
                _sw.AutoFlush = true;

                // Reading header
                

                if (!ReadLineWithCode("220"))
                {
                    Console.WriteLine("Serveur not found !");
                    return false;
                }
                Console.WriteLine("Serveur found !");


                /* ======= LOGIN ======== */
                _sw.WriteLine("USER " + _userId);

                if (!ReadLineWithCode("331"))
                {
                    Console.WriteLine("Bad User");
                    return false;
                }


                /* ======= PASSWORD ======== */
                _sw.WriteLine("PASS " + _password);

                if (!ReadLineWithCode("230"))
                {
                    Console.WriteLine("Bad Password");
                    return false;
                }
                Console.WriteLine("User Connected");
            }

            else
            {
                return false;
            }
            
            return true;
        }

        public bool Connected()
        {
            return _clientSocket.Connected;
        }

        private bool ReconnectToPassive()
        {
            //send Passive mode command
            _sw.WriteLine("PASV");

            string ligne = _sr.ReadLine();
            Console.WriteLine(ligne);
            
            // if wanted result code 
            if (ligne.Contains("227"))
            {

                /************ IP *************/

                //split sting with ','
                string[] num = ligne.Split(','); 

                string[] couper = num[0].Split('(');

                string ip1 = couper[1];

                IpAfterConnect = ip1 + '.' + num[1] + '.' + num[2] + '.' + num[3];

                Console.WriteLine("IP = " + IpAfterConnect);

                /************ PORT  *************/

                num[5] = num[5].Remove(num[5].Length - 1); // remove ')' at the end of the string

                _portAfterConnect = int.Parse(num[4]) * 256 + int.Parse(num[5]); //calculate new port
                Console.WriteLine("PORT = " + _portAfterConnect);

                // trying connection in passive mode
                try
                {
                    _clientSocketPassive = new TcpClient();
                    _clientSocketPassive.Connect(IPAddress.Parse(IpAfterConnect), _portAfterConnect);
                }
                catch (Exception)
                {
                    Console.WriteLine(">>>> Le serveur a refusé la connextion !");
                    return false;
                }
                Console.WriteLine(">>>> Le serveur a accepté la connextion !");
                return true;
            }

            return false;
        }

        public List<String> GetListResult()
        {
            // reconnect in passive mode
            ReconnectToPassive();
            StreamReader sr = new StreamReader(_clientSocketPassive.GetStream(), Encoding.Default);
            // send LIST command
            _sw.WriteLine("LIST");

            List<String> result = new List<string>();

            // if good result code
            if (ReadLineWithCode("150"))
            {
                string line;
                // first line isn't a wanted result
                Console.WriteLine(sr.ReadLine());
                // get result until connection will be close
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    Console.WriteLine(line);
                    result.Add(line);

                }
            }

            // read confirmation code
            ReadLineWithCode("226");
                
            return result;
        }

        public bool MoveToDirectory(string name)
        { // move to a directory in host

            _sw.WriteLine("CWD " + name);
            
            // if host has change of directory, return true 
            if (ReadLineWithCode("250"))
            {
                Console.WriteLine("Changing to directory " + name);
                return true;
            }
            return false;
        }

        private bool ReadLineWithCode(string code, bool verbose = true)
        {
            // return true if code found and streal read 
            // else return false if code wanted not found

            string line = _sr.ReadLine();
            if (verbose)
                Console.WriteLine(line);
            
            // if there is the wanted code --> continue, else return false
            if (line.Contains(code))
            {
                // if there is '-' after the code, it mean there is a message on many lines
                if (line.Contains(code + '-'))
                {
                    line = _sr.ReadLine();
                    if (verbose)
                        Console.WriteLine(line);

                    // while there is not the end code --> continue reading
                    while (!line.Contains(code))
                    {
                        line = _sr.ReadLine();
                        if (verbose)
                            Console.WriteLine(line);
                    }
                }
                
            }
            else
            {
                return false;
            }
            // read end
            return true;
        }

        public bool Disconnect()
        { // diconnect client

            _sw.WriteLine("QUIT");
            // if close connextion it's ok
            if (this.ReadLineWithCode("221"))
            {
                Console.WriteLine("Disconnected");
                return true;
            }
            else
                Console.WriteLine("Error in deconnection");
            return false;
        }

        public List<String> DownloadFile(string fileName)
        {
            // reconnect to passive mode and create a new streamReader
            ReconnectToPassive();
            StreamReader sr = new StreamReader(_clientSocketPassive.GetStream(), Encoding.Default);
            
            // send command to download file
            _sw.WriteLine("RETR " + fileName);
            
            // string list to get file content
            List<String> result = new List<string>();

            // if download begin
            if (ReadLineWithCode("150"))
            {
                Console.WriteLine("Transfert en cours, merci de patienter..");
                // get content until Stream will be closed
                while (!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine());
                }
            }
            // read "download completed
            ReadLineWithCode("226");
            return result;
        }
    }
}
