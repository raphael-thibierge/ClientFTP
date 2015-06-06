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
        StreamWriter _sw;
        StreamReader _sr;

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
                
                _sw.WriteLine("USER " + _userID);

                _sw.WriteLine("PASS " + _password);
                

                if (passiv)
                {
                    _sw.WriteLine("PASV");
                }

                string ligne = _sr.ReadLine();
                while (!_sr.EndOfStream || ligne=="")
                {
                    Console.WriteLine(ligne);
                    if (ligne.Contains("503") || ligne.Contains("530") || ligne.Contains("500"))
                    {
                        Console.WriteLine("Erreur saisi");
                        return false;
                    }
                    if (ligne.Contains("230"))
                    {
                        Console.WriteLine("Co OK !");
                        Console.WriteLine("END 1");
                        return true;
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
    }
}
