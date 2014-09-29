using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CK2Modder.GameData.common;
using CK2Modder.GameData.history.characters;
using CK2Modder.Util;

namespace CK2Modder.GameData
{
    public class Mod : INotifyPropertyChanged
    {
        public List<Dynasty> Dynasties;
        public List<Culture> Cultures;
        public List<Character> Characters;

        // List of files that are in used
        public List<String> CharacterFiles;
        public List<String> DynastyFiles;
        public List<String> CultureFiles;

        // Queue's that hold files to load since only one file is loaded at a time
        public Queue<String> CharacterFilesToLoad;
        public Queue<String> DynastyFilesToLoad;
        public Queue<String> CultureFilesToLoad;

        public String StorageLocation { get; set; }
        public String ModRootDirectory { get; set; }

        private String _rawOutput;
        public String RawOutput 
        {
            get { return _rawOutput; }
            set
            {
                _rawOutput = value;
                NotifyPropertyChanged("RawOutput");
            }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set 
            {
                if (value.Equals(""))
                {
                    MessageBox.Show("The name cannot be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _name = value;

                // Update the path also
                // Set the path to the name, all lower case with no spaces
                _path = "mod/" + _name; // _name.ToLower().Replace(" ", "");

                UpdateRawOutput();
                NotifyPropertyChanged("Name");
            }
        }

        private String _path;
        public String Path
        {
            get { return _path; }
            private set { _path = value; }
        }

        private Boolean _useArchive;
        public Boolean UseArchive
        {
            get { return _useArchive; }
            set { _useArchive = value; }
        }

        private String _userDirectory = "";
        public String UserDirectory
        {
            get { return _userDirectory; }
            set 
            { 
                _userDirectory = value;
                NotifyPropertyChanged("UserDirectory");
            }
        }

        private String _dependencies;
        public String Dependencies
        {
            get { return _dependencies; }
            set 
            { 
                _dependencies = value;
                UpdateRawOutput();
                NotifyPropertyChanged("Dependencies");
            }
        }

        private BindingList<String> _replacePaths = new BindingList<String>();
        public BindingList<String> ReplacePaths
        {
            get { return _replacePaths; }
            set
            {
                _replacePaths = value;
                NotifyPropertyChanged("ReplacePaths");
            }
        }

        private Boolean _areCulturesImported = true;
        public Boolean AreCulturesImported
        {
            get { return _areCulturesImported; }
            set
            {
                _areCulturesImported = value;
                UpdateRawOutput();
                NotifyPropertyChanged("AreCulturesImported");
            }
        }

        private Boolean _areDynastiesImported = true;
        public Boolean AreDynastiesImported
        {
            get { return _areDynastiesImported; }
            set
            {
                _areDynastiesImported = value;
                UpdateRawOutput();
                NotifyPropertyChanged("AreDynastiesImported");
            }
        }

        private Boolean _areCharactersImported = true;
        public Boolean AreCharactersImported
        {
            get { return _areCharactersImported; }
            set
            {
                _areCharactersImported = value;
                UpdateRawOutput();
                NotifyPropertyChanged("AreCharactersImported");
            }
        }

        public Mod(String name)
        {
            Name = name;
            Dependencies = "";
            UserDirectory = "";
            UseArchive = false;

            // Setup dynasty list
            Dynasties = new List<Dynasty>();
            Cultures = new List<Culture>();
            Characters = new List<Character>();

            CharacterFiles = new List<String>();
            DynastyFiles = new List<String>();
            CultureFiles = new List<String>();

            CharacterFilesToLoad = new Queue<String>();
            DynastyFilesToLoad = new Queue<String>();
            CultureFilesToLoad = new Queue<String>();
        }

        public void UpdateRawOutput()
        {
            RawOutput = "name = \"" + Name + "\"\r\n";
            RawOutput += "path = \"" + Path + "\"\r\n";
            if (!UserDirectory.Equals("")) RawOutput += "user_dir = \"" + UserDirectory + "\"\r\n";
            RawOutput += "\r\n";

            foreach (String path in ReplacePaths)
            {
                RawOutput += "replace_path = \"" + path + "\"\r\n";
            }            

            if (Dependencies != null && !Dependencies.Equals(""))
            {
                RawOutput += "\r\n";
                RawOutput += "dependencies = { " + Dependencies + " }\r\n";
            }

            RawOutput += "\r\n";
            RawOutput += "### CK2 Modder settings ###\r\n";            
            RawOutput += "# areCulturesImported = " + AreCulturesImported.ToString() + "\r\n";
            RawOutput += "# areDynastiesImported = " + AreDynastiesImported.ToString() + "\r\n";
            RawOutput += "# areCharactersImported = " + AreCharactersImported.ToString() + "\r\n";
        }

        public static Mod LoadFromFile(String file)
        {
            if (!File.Exists(file))
                return null;

            StreamReader stream = File.OpenText(file);

            String line;
            Mod mod = null;

            while ((line = stream.ReadLine()) != null)
            {
                if (line.Contains("=") && !line.StartsWith("#"))
                {
                    KeyValuePair<String, String> data = Helpers.ReadStringData(line);

                    switch (data.Key)
                    {
                        case "name":
                            mod = new Mod(data.Value);
                            mod.StorageLocation = System.IO.Path.GetDirectoryName(file);
                            mod.ModRootDirectory = mod.StorageLocation + "/" + mod.Name;
                            break;

                        case "user_dir":
                            mod.UserDirectory = data.Value;
                            break;

                        case "replace_path":
                            mod.ReplacePaths.Add(data.Value);
                            break;
                    }
                }
            }

            // close the file stream
            stream.Close();

            return mod;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
