using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ClientFTP
{
    class Client
    {
        private string _userID;
        private string _password;
        private string _host;
        private string _port;

        private bool passiv;

        private TcpClient _clientSocket;
        private TcpClient _clientSocketPassive;

        private StreamWriter _sw;
        private StreamReader _sr;

        public string IpAfterConnect { get; private set; }
        private int _portAfterConnect; // needed for passive connection

        public Client(string id, string pwd, string host, string port)
        {
            _userID = id;
            _password = pwd;
            _host = host;
            _port = port;

            _clientSocket = new TcpClient();
            passiv = true;

        }

        public bool connect()
        {
            // ============= ISSUE DU PROJET POP3 ==============
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

            _clientSocket.Connect(adresse, 21);

            if (_clientSocket.Connected)
            {
                IpAfterConnect = adresse.ToString();

                // connexion ok,setting streams to read and write
                _sr = new StreamReader(_clientSocket.GetStream(), Encoding.Default);
                _sw = new StreamWriter(_clientSocket.GetStream(), Encoding.Default);
                _sw.AutoFlush = true;

                // Reading header
                

                if (!readLineWithCode("220"))
                {
                    Console.WriteLine("Serveur not found !");
                    return false;
                }
                Console.WriteLine("Serveur found !");


                /* ======= LOGIN ======== */
                _sw.WriteLine("USER " + _userID);

                if (!readLineWithCode("331"))
                {
                    Console.WriteLine("Bad User");
                    return false;
                }


                /* ======= PASSWORD ======== */
                _sw.WriteLine("PASS " + _password);

                if (!readLineWithCode("230"))
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
            //on indique qu'on est en connexion passive
            _sw.WriteLine("PASV");

            string ligne = _sr.ReadLine();
            Console.WriteLine(ligne);
            if (ligne.Contains("227")) //code de réponse
            {

                /************ IP *************/

                //on découpe la ligne avec les ,
                string[] num = ligne.Split(','); 

                string[] couper = num[0].Split('(');

                string ip1 = couper[1];

                IpAfterConnect = ip1 + '.' + num[1] + '.' + num[2] + '.' + num[3];

                Console.WriteLine("IP = " + IpAfterConnect);

                /************ PORT  *************/
                    
                num[5] = num[5].Remove(num[5].Length - 1); //on enlève la ")" à la fin de ka chaine de charactère

                _portAfterConnect = int.Parse(num[4]) * 256 + int.Parse(num[5]); //formule d'après la consigne
                
                Console.WriteLine("PORT = " + _portAfterConnect);


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

        public List<String> getListResult()
        {


            ReconnectToPassive();
            StreamReader sr = new StreamReader(_clientSocketPassive.GetStream(), Encoding.Default);
            _sw.WriteLine("LIST");
            List<String> result = new List<string>();

            if (readLineWithCode("150"))
            {
                string line;
                
                Console.WriteLine(sr.ReadLine());
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    Console.WriteLine(line);
                    result.Add(line);

                }
            }

            readLineWithCode("226");
                
            return result;
        }

        public bool moveToDirectory(string name)
        {
            Console.WriteLine("Changing to directory " + name);
            _sw.WriteLine("CWD " + name);
            if (readLineWithCode("250"))
                return true;
            return false;
        }

        private bool readLineWithCode(string code, bool verbose = true)
        {
            // return true if code found and streal read 
            // else return false if code wanted not found

            string line = _sr.ReadLine();
            if (verbose)
                Console.WriteLine(line);
            
            
            if (line.Contains(code))
            {
                if (line.Contains(code + '-'))
                {
                    line = _sr.ReadLine();
                    if (verbose)
                        Console.WriteLine(line);
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

            return true;
        }

        public bool quit()
        {
            _sw.WriteLine("QUIT");

            if (this.readLineWithCode("221"))
            {
                Console.WriteLine("Deconnection ok");
                return true;
            }
            else
                Console.WriteLine("ER<RRRRRRRRRRRRRROORORO");
            return false;
        }
    }
}
