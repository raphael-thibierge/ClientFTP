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
        private Directory _rootDirectory;
        private Directory _currentDirectory;

        public Form1()
        {
            InitializeComponent();

            // default Connextion textBox Values
            Host_textBox.Text = "ftp.lip6.fr";
            Port_textBox.Text = "21";
            UserID_textBox.Text = "anonymous";
            Password_textBox.Text = "";

            Refresh_button.Hide();


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
                Refresh_button.Show();

                _client.ReconnectToPassive();

                ConnexionState_label.Text = _client.IpAfterConnect + " connecté !";
                
                _rootDirectory = new Directory("/", "rwxrwxrdx", 0, null);
                _currentDirectory = _rootDirectory;

                treatListResult(_client.getListResult());
                

            }
            else
            {
                ConnexionState_label.Text = "Non Connecté !";
            }
        }


        private void Files_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treatListResult(string[] listStrings)
        {
            _currentDirectory.DirectoriesList.Clear();
            _currentDirectory.FilesList.Clear();

            foreach (string line in listStrings)
            {
                // split line
                string[] tmp = line.Split(' ');

                // get name
                string name = tmp[tmp.Count() - 1];

                // get rights
                string rights = tmp[0];
                char type = rights[0];
                rights.Remove(0);
                
                // get size
                int size = 0;

                // create file or directory
                if (type == 'd')
                {
                    _currentDirectory.DirectoriesList.Add(new Directory(name, rights, size, _currentDirectory));
                }
                else if (type == '-')
                {
                    _currentDirectory.FilesList.Add(new File(name, rights,size));
                }
            }
            
            // update listBox
            updateDirectoryContentListBox();
        }

        private void updateDirectoryContentListBox()
        {
            Directories_listBox.Items.Clear();
            Files_listBox.Items.Clear();

            if (_currentDirectory.ParentDirectory != null)
            {
                Directories_listBox.Items.Add("..");
            }

            foreach (Directory directory in _currentDirectory.DirectoriesList)
            {
                Directories_listBox.Items.Add(directory.Name);
            }

            foreach (File file in _currentDirectory.FilesList)
            {
                Files_listBox.Items.Add(file.Name);
            }
            
        }

        public void moveToDirectory(string name)
        {
            // change directory in host
            _client.moveToDirectory(name);
            _currentDirectory = _currentDirectory.getDirectory(name);
            Console.WriteLine("current directory" + _currentDirectory.Name);
            // update directory's content
            treatListResult(_client.getListResult());
        }


        private void Refresh_button_Click(object sender, EventArgs e)
        {
            updateDirectoryContentListBox();
        }

        private void Directories_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Directories_listBox.SelectedItem != null)
            {
                moveToDirectory(Directories_listBox.SelectedItem.ToString());
            }
        }


    }
}
