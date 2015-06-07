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
        private string _name;
        private int _size;
        private string _rights;

        public List<File> FilesList { get; private set; }
        public List<Directory> DirectoriesList { get; private set; }

        public Directory(string lineFromList)
        {
            string[] tmp = lineFromList.Split(' ');

            _rights = tmp[0];
            _rights.Remove(0);
            
            _size = 0;
            _name = tmp[tmp.Count()-1];    
        }

        public string ToString()
        {
            return _name;
        }

    }
}
