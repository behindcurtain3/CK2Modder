using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using CK2Modder.GameData.common;
using CK2Modder.GameData.history.characters;

namespace CK2Modder
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

        private String _userDirectory;
        public String UserDirectory
        {
            get { return _userDirectory; }
            set { _userDirectory = value; }
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

        private Boolean _replaceCommonPath;
        public Boolean ReplaceCommonPath
        {
            get { return _replaceCommonPath; }
            set 
            { 
                _replaceCommonPath = value;
                NotReplaceCommonPath = !value;
                UpdateRawOutput();
                NotifyPropertyChanged("ReplaceCommonPath");
            }
        }

        /// <summary>
        /// This looks wierd but its used to disable buttons when the replace
        /// common path variable is true.
        /// </summary>
        private Boolean _notReplaceCommonPath;
        public Boolean NotReplaceCommonPath
        {
            get { return _notReplaceCommonPath; }
            set
            {
                _notReplaceCommonPath = value;
                NotifyPropertyChanged("NotReplaceCommonPath");
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

        public Mod(String name)
        {
            Name = name;
            Dependencies = "";
            UserDirectory = "";
            UseArchive = false;
            ReplaceCommonPath = false;

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
            
            if (ReplaceCommonPath)
            {
                RawOutput += "\r\n";
                RawOutput += "replace_path = \"common\"\r\n";
            }

            if (Dependencies != null && !Dependencies.Equals(""))
            {
                RawOutput += "\r\n";
                RawOutput += "dependencies = { " + Dependencies + " }\r\n";
            }

            RawOutput += "\r\n";
            RawOutput += "### CK2 Modder settings ###\r\n";
            RawOutput += "# areDynastiesImported = " + AreDynastiesImported.ToString() + "\r\n";
            RawOutput += "# areCulturesImported = " + AreCulturesImported.ToString() + "\r\n";
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
                if (line.StartsWith("name = "))
                {
                    int start = line.IndexOf('"') + 1;
                    int end = line.IndexOf('"', start);

                    mod = new Mod(line.Substring(start, end - start));
                }
                else if (line.Equals("replace_path = \"common\""))
                {
                    mod.ReplaceCommonPath = true;
                }
                else if (line.StartsWith("# areDynastiesImported = "))
                {
                    int start = line.IndexOf("=") + 1;
                    String answer = line.Substring(start, line.Length - start);
                    mod.AreDynastiesImported = Boolean.Parse(answer.Trim());
                }
                else if (line.StartsWith("# areCulturesImported = "))
                {
                    int start = line.IndexOf("=") + 1;
                    String answer = line.Substring(start, line.Length - start);
                    mod.AreCulturesImported = Boolean.Parse(answer.Trim());
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
