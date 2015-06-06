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

            // ============= END ==============
   
            _clientSocket.Connect(adresse, 21);
            if (_clientSocket.Connected)
            {
                // connexion ok,setting streams to read and write
                _sr = new StreamReader(_clientSocket.GetStream(), Encoding.Default);
                _sw = new StreamWriter(_clientSocket.GetStream(), Encoding.Default);

                _sw.WriteLine("USER " + _userID);
                _sw.WriteLine("PASS " + _password);
                

                if (passiv)
                {
                    _sw.WriteLine("PASV");
                }

                ConsolePrint();

            }
            else return false;

            return true;
        }

        public void sendCommand(string tampon)
        {
            _sw.WriteLine(tampon);
        }

        private void ConsolePrint()
        {
            _sr.ReadLine();

            string ligne = "";

            

            int i = 0;
            while (ligne!="" || !_sr.EndOfStream)
            {
                ligne = _sr.ReadLine();
                Console.WriteLine(i + ">> " + ligne);
                Console.WriteLine(i + "  etat = " + !_sr.EndOfStream);
                i++;
            }

            Console.WriteLine(">> END <<");

        }

        public bool Connected()
        {
            return _clientSocket.Connected;
        }
    }
}
