using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder
{
    public class Mod
    {
        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _path;
        public String Path
        {
            get { return _path; }
        }

        private Boolean _useArchive;
        public Boolean UseArchive
        {
            get { return _useArchive; }
            set { _useArchive = value; }
        }

        private String _userDirectory;
        public String UserDirectory
        {
            get { return _userDirectory; }
            set { _userDirectory = value; }
        }

        public Mod(String name)
        {
            Name = name;
            
            // Set the path to the name, all lower case with no spaces
            _path = "mod/" + name.ToLower().Replace(" ", "");
        }
    }
}
