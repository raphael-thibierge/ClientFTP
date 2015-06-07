using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace ClientFTP
{
    class Directory
    {
        public string Name { get; private set; }
        private int _size;
        private string _rights;

        public List<File> FilesList { get; private set; }
        public List<Directory> DirectoriesList { get; private set; }


        public Directory(string name, string rights, int size)
        {
            Name = name;
            _rights = rights;
            _size = size;
            
            FilesList = new List<File>();
            DirectoriesList = new List<Directory>();
        }
        

        public string ToString()
        {
            return Name;
        }

    }
}
