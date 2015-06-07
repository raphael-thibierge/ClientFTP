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
        private string _name;
        private int _size;
        private string _rights;


        public File(string lineFromList)
        {
            string[] tmp = lineFromList.Split(' ');

            _rights = tmp[0];
            _rights.Remove(0);

            _size = 0;
            _name = tmp[tmp.Count() - 1];  
        }

        public string ToString()
        {
            return _name;
        }
    
    }
}
