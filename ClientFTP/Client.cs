using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
        private StreamReader _srp;

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
            //IPAddress adresse;
            
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
                // connexion ok,setting streams to read and write
                _sr = new StreamReader(_clientSocket.GetStream(), Encoding.Default);
                _sw = new StreamWriter(_clientSocket.GetStream(), Encoding.Default);
                _sw.AutoFlush = true;
                
                //on envoie le login
                _sw.WriteLine("USER " + _userID);

                //on envoie le mdp
                _sw.WriteLine("PASS " + _password);

                if (passiv)
                {
                    _sw.WriteLine("PASV"); //on indique qu'on est en connexion passive

                }

                string ligne = _sr.ReadLine();
                while (!_sr.EndOfStream || ligne=="") //on parcourt le stream reçu
                {
                    Console.WriteLine(ligne);

                    if (ligne.Contains("503") || ligne.Contains("530") || ligne.Contains("500"))
                    {  //erreur si il y a un mauvais login, pas de login ou erreur de saisi
                        Console.WriteLine("Erreur saisi");
                        return false;
                    }

                    if (ligne.Contains("230"))
                    { //le login est correct
                        if (passiv)
                        {
                            //on indique qu'on est en connexion passive
                            _sw.WriteLine("PASV"); 

                            ligne = _sr.ReadLine();
                            while (!_sr.EndOfStream || ligne == "")
                            {
                                Console.WriteLine(ligne);

                                if (ligne.Contains("227")) //code de réponse
                                {
                                    Console.WriteLine(ligne);

                                    /************ IP *************/

                                    string[] num = ligne.Split(','); //on découpe la ligne avec les ,

                                    string[] couper = num[0].Split('(');

                                    string ip1 = couper[1].ToString();

                                    for(int i = 0; i<num.Length; i++)
                                        Console.WriteLine(num[i]);

                                    Console.WriteLine("découpe :" + ip1);
                                    IpAfterConnect = ip1 + '.' + num[1] + '.' + num[2] + '.' + num[3];

                                    Console.WriteLine("IP=" + IpAfterConnect);
                                    
                                    num[5] = num[5].Remove(num[5].Length-1); //on enlève la ")" 
                                    int port = int.Parse(num[4].ToString()) * 256 + int.Parse(num[5].ToString()); //formule d'après la consigne
                                    _portAfterConnect = port;
                                    Console.WriteLine("PORT=" + port);

                                    Console.WriteLine("Co OK !");
                                    Console.WriteLine("END 1");
                                    
                                    if (ReconnectToPassive())
                                        return true;
                                    return false;
                                }
                                ligne = _sr.ReadLine();
                            }
                        }
                    }
                    ligne = _sr.ReadLine();
                }
                Console.WriteLine("END 2");
            }
            
            return false;
        }

        public bool Connected()
        {
            return _clientSocket.Connected;
        }

        private bool ReconnectToPassive()
        {
            _clientSocketPassive = new TcpClient();

            try
            {
                Console.WriteLine(">>>> " + IpAfterConnect + " / " + _portAfterConnect.ToString());
                _clientSocketPassive.Connect(IPAddress.Parse(IpAfterConnect), _portAfterConnect);
            }
            catch (Exception)
            {
                Console.WriteLine(">>>> Le serveur a refusé la connextion");
                return false;
            }

            if (_clientSocketPassive.Connected)
            {
                _srp = new StreamReader(_clientSocketPassive.GetStream(), Encoding.Default);
                Console.WriteLine(">>>> Connexion passive ok");

                return true;
            }

            return false;
        }
       

    }
}
