using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CK2Modder.GameData.common;
using CK2Modder.GameData.history.characters;

namespace CK2Modder.GameData
{
    public class Mod : INotifyPropertyChanged
    {
        public BindingList<Dynasty> Dynasties;
        public List<Culture> Cultures;
        public List<String> CharacterFiles;        
        public BindingList<Character> Characters;

        // Queue that holds character files to load since we only load one at a time store them here
        public Queue<String> CharacterFilesToLoad;

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
            Dynasties = new BindingList<Dynasty>();
            Cultures = new List<Culture>();
            CharacterFiles = new List<String>();
            Characters = new BindingList<Character>();

            CharacterFilesToLoad = new Queue<String>();
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
                if (line.StartsWith("name = ") || line.StartsWith("name="))
                {
                    int start = line.IndexOf('"') + 1;
                    int end = line.IndexOf('"', start);

                    mod = new Mod(line.Substring(start, end - start));
                }
                else if (line.Contains("user_dir"))
                {
                    int start = line.IndexOf('"') + 1;
                    int end = line.IndexOf('"', start);

                    mod.UserDirectory = line.Substring(start, end - start);
                }
                else if (line.Contains("replace_path"))
                {
                    int start = line.IndexOf('"') + 1;
                    int end = line.IndexOf('"', start);

                    mod.ReplacePaths.Add(line.Substring(start, end - start));
                }
                else if (line.StartsWith("# areCulturesImported = "))
                {
                    int start = line.IndexOf("=") + 1;
                    String answer = line.Substring(start, line.Length - start);
                    mod.AreCulturesImported = Boolean.Parse(answer.Trim());
                }
                else if (line.StartsWith("# areDynastiesImported = "))
                {
                    int start = line.IndexOf("=") + 1;
                    String answer = line.Substring(start, line.Length - start);
                    mod.AreDynastiesImported = Boolean.Parse(answer.Trim());
                }
                else if (line.StartsWith("# areCharactersImported = "))
                {
                    int start = line.IndexOf("=") + 1;
                    String answer = line.Substring(start, line.Length - start);
                    mod.AreCharactersImported = Boolean.Parse(answer.Trim());
                }
                
            }

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
