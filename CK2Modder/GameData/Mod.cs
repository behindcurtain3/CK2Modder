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
    public class Mod : ModResource
    {
        #region Fields

        private String _name;
        private String _path;
        private Boolean _useArchive;
        private String _userDirectory = "";
        private String _dependencies;
        private BindingList<String> _replacePaths = new BindingList<String>();

        #endregion

        #region Properties

        public List<Dynasty> Dynasties { get; private set; }
        public List<Culture> Cultures { get; private set; }
        public List<Character> Characters { get; private set; }

        // List of files that are in used
        public List<String> CharacterFiles { get; private set; }
        public List<String> DynastyFiles { get; private set; }
        public List<String> CultureFiles { get; private set; }

        // Queue's that hold files to load since only one file is loaded at a time
        public Queue<String> CharacterFilesToLoad { get; private set; }
        public Queue<String> DynastyFilesToLoad { get; private set; }
        public Queue<String> CultureFilesToLoad { get; private set; }

        // File storage
        public String StorageLocation { get; set; }
        public String ModRootDirectory { get; set; }

        /// <summary>
        /// The name of the mod
        /// </summary>
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

                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// The relative path to the mod
        /// </summary>
        public String Path
        {
            get { return _path; }
            private set { _path = value; }
        }

        /// <summary>
        /// Does this mod use an archive?
        /// </summary>
        public Boolean UseArchive
        {
            get { return _useArchive; }
            set { _useArchive = value; }
        }

        /// <summary>
        /// The user directory specified for this mod
        /// </summary>
        public String UserDirectory
        {
            get { return _userDirectory; }
            set 
            { 
                _userDirectory = value;
                NotifyPropertyChanged("UserDirectory");
            }
        }

        /// <summary>
        /// This mod depends on these dependencies
        /// </summary>
        public String Dependencies
        {
            get { return _dependencies; }
            set 
            { 
                _dependencies = value;
                NotifyPropertyChanged("Dependencies");
            }
        }

        /// <summary>
        /// The list of the paths the mod replaces
        /// </summary>
        public BindingList<String> ReplacePaths
        {
            get { return _replacePaths; }
            set
            {
                _replacePaths = value;
                NotifyPropertyChanged("ReplacePaths");
            }
        }

        /// <summary>
        /// How the mod is displayed in the app
        /// </summary>
        public override string Display
        {
            get { return Name; }
        }
        #endregion

        #region Methods

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

        public static Mod LoadFromFile(String file)
        {
            if (!File.Exists(file))
                return null;

            StreamReader stream = File.OpenText(file);

            String line;
            Mod mod = new Mod("Loaded Mod");

            while ((line = stream.ReadLine()) != null)
            {
                mod.Raw += line + System.Environment.NewLine;

                if (line.Contains("=") && !line.StartsWith("#"))
                {
                    KeyValuePair<String, String> data = Helpers.ReadStringData(line);

                    switch (data.Key)
                    {
                        case "name":
                            mod.Name = data.Value;
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

        #endregion

    }
}
