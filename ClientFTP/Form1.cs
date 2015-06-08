using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            UserID_textBox.Text = "anonymous";
            Password_textBox.Text = "";

            Disconnect_button.Hide();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Connect_button_Click(object sender, EventArgs e)
        {   /// connect to host

            // get connexion details to connect client to the serveur
            string host = Host_textBox.Text;
            string user = UserID_textBox.Text;
            string pwd = Password_textBox.Text;
            // init client
            _client = new Client(user, pwd, host);

            // connexion
            if (_client.Connect())
            {
                Disconnect_button.Show();
                Connect_button.Hide();
                ConnexionState_label.Text = " Connected : " + _client.IpAfterConnect;
                
                // create local mirror directory
                _rootDirectory = new Directory("/", "rwxrwxrdx", 0, null);
                _currentDirectory = _rootDirectory;

                // update directory content
                TreatListResult(_client.GetListResult());
                
            }
            else
            {
                ConnexionState_label.Text = "Disconnected !";
            }
        }


        private void Files_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // download selected file

            // create a window to get path
            SaveFileDialog window = new SaveFileDialog();
            window.RestoreDirectory = true;
            window.InitialDirectory = @"c:\";
            window.FileName = Files_listBox.SelectedItem.ToString();
            
            if (window.ShowDialog() == DialogResult.OK)
            {
                string fileName = window.FileName;

                // open stream reader to save file content
                StreamWriter sw = new StreamWriter(fileName);
                List<string> fileContent = _client.DownloadFile(Files_listBox.SelectedItem.ToString());

                foreach (string line in fileContent)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
                MessageBox.Show("Téléchargement terminé");
            }
            

        }

        private void TreatListResult(List<String> listStrings)
        { // create directory or files with string from LIST command in _client

            // reset current directory content
            _currentDirectory.DirectoriesList.Clear();
            _currentDirectory.FilesList.Clear();

            // add content
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
            UpdateDirectoryContentListBox();
        }

        private void UpdateDirectoryContentListBox()
        { // update local directory content --> listbox and _currentDirectory

            Directories_listBox.Items.Clear();
            Files_listBox.Items.Clear();

            // add previous folder in list
            if (_currentDirectory.ParentDirectory != null)
            {
                Directories_listBox.Items.Add("..");
            }
            
            // add other directory
            foreach (Directory directory in _currentDirectory.DirectoriesList)
            {
                Directories_listBox.Items.Add(directory.Name);
            }
            
            // add files
            foreach (File file in _currentDirectory.FilesList)
            {
                Files_listBox.Items.Add(file.Name);
            }
        }

        public void MoveToDirectory(string name)
        {
            // change directory in host
            if (_client.MoveToDirectory(name))
            {
                // change local directory
                _currentDirectory = _currentDirectory.GetDirectory(name);
                Console.WriteLine("current directory : " + _currentDirectory.Name);
                // update directory's content
                TreatListResult(_client.GetListResult());
            }   
        }


        private void Directories_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // move into a directory
            if (Directories_listBox.SelectedItem != null)
            {
                MoveToDirectory(Directories_listBox.SelectedItem.ToString());
            }
        }


        private void Quit_button_Click(object sender, EventArgs e)
        {
            //Disconnect user

            // if client is connected
            if (_client.Connected())
            {
                // if he has been disconned
                if (_client.Disconnect())
                {
                    // update view
                    Directories_listBox.Items.Clear();
                    Files_listBox.Items.Clear();

                    _currentDirectory = null;
                    _rootDirectory = null;
                    
                    ConnexionState_label.Text = "Déconnecté";

                    Disconnect_button.Hide();
                    Connect_button.Show();
                    
                }
            }
        }
    }
}
