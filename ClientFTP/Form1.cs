using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientFTP
{
    public partial class Form1 : Form
    {
        private Client _client;

        public Form1()
        {
            InitializeComponent();
            // default Connextion textBox Values
            Host_textBox.Text = "ftp.lip6.fr";
            Port_textBox.Text = "21";
            UserID_textBox.Text = "anonymous";
            Password_textBox.Text = "toto";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Connect_button_Click(object sender, EventArgs e)
        {
            // get connexion details to connect client to the serveur
            string host = Host_textBox.Text;
            string port = Port_textBox.Text;
            string user = UserID_textBox.Text;
            string pwd = Password_textBox.Text;
            // init client
            _client = new Client(user, pwd, host, port);
            // connexion
            if (_client.connect())
            {
                ConnexionState_label.Text = "Connecté !";
            }
            else
            {
                ConnexionState_label.Text = "Non Connecté !";
            }
        }
    }
}
