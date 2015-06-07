using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientFTP
{
    class File
    {
        public string Name { get; private set; }
        private int _size;
        private string _rights;


        public File(string name, string rights, int size)
        {
            Name = name;
            _rights = rights;
            _size = size;
        }


        public string ToString()
        {
            return Name;
        }
    
    }
}
