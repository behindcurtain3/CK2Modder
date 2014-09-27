using System;
using System.ComponentModel;
using System.IO;

namespace CK2Modder.GameData.history.characters
{
    public interface ICharacterBinding
    {
        int ID { get; set; }
        String Name { get; set; }
        int Dynasty { get; set; }
        String Religion { get; set; }
        String Culture { get; set; }
    }

    public class Character : INotifyPropertyChanged, ICharacterBinding
    {
        /// <summary>
        /// The name of the file the character is stored in
        /// </summary>
        private String _file;
        public String File
        {
            get { return _file; }
            set
            {
                _file = value;
                NotifyPropertyChanged("File");
            }
        }

        /// <summary>
        /// The characters ID, must be unique
        /// </summary>
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }
        
        /// <summary>
        /// The characters name
        /// </summary>
        private String _name = "";
        public String Name
        {
            get { return _name; }
            set
            {
                /*
                if (value.Equals(""))
                {
                    MessageBox.Show("The character name cannot be blank. Character ID: " + ID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                */
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private Boolean _female = false;
        public Boolean Female
        {
            get { return _female; }
            set
            {
                _female = value;
                NotifyPropertyChanged("Female");
            }
        }

        /// <summary>
        /// The ID of the dynasty the character belongs to, not required
        /// </summary>
        private int _dynasty = 0;
        public int Dynasty
        {
            get { return _dynasty; }
            set
            {
                _dynasty = value;
                NotifyPropertyChanged("Dynasty");
            }
        }

        /// <summary>
        /// The religion of the character
        /// </summary>
        private String _religion = "";
        public String Religion
        {
            get { return _religion; }
            set
            {
                _religion = value;
                NotifyPropertyChanged("Religion");
            }
        }

        /// <summary>
        /// The culture the character belongs to
        /// </summary>
        private String _culture = "";
        public String Culture
        {
            get { return _culture; }
            set
            {
                _culture = value;
                NotifyPropertyChanged("Culture");
            }
        }

        private String _nickName = "";
        public String Nickname
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                NotifyPropertyChanged("Nickname");
            }
        }

        private int _martial;
        public int Martial
        {
            get { return _martial; }
            set
            {
                _martial = value;
                NotifyPropertyChanged("Martial");
            }
        }

        private int _diplomacy;
        public int Diplomacy
        {
            get { return _diplomacy; }
            set
            {
                _diplomacy = value;
                NotifyPropertyChanged("Diplomacy");
            }
        }

        private int _stewardship;
        public int Stewardship
        {
            get { return _stewardship; }
            set
            {
                _stewardship = value;
                NotifyPropertyChanged("Stewardship");
            }
        }

        private int _intrigue;
        public int Intrigue
        {
            get { return _intrigue; }
            set
            {
                _intrigue = value;
                NotifyPropertyChanged("Intrigue");
            }
        }

        private int _learning;
        public int Learning
        {
            get { return _learning; }
            set
            {
                _learning = value;
                NotifyPropertyChanged("Learning");
            }
        }

        /// <summary>
        /// A string array that holds the traits of this character
        /// </summary>
        private BindingList<String> _traits = new BindingList<String>();
        public BindingList<String> Traits
        {
            get { return _traits; }
            set
            {
                _traits = value;
                NotifyPropertyChanged("Traits");
            }
        }

        private int _father;
        public int Father
        {
            get { return _father; }
            set
            {
                _father = value;
                NotifyPropertyChanged("Father");
            }
        }

        private int _mother;
        public int Mother
        {
            get { return _mother; }
            set
            {
                _mother = value;
                NotifyPropertyChanged("Mother");
            }
        }

        private String _events = "";
        public String Events
        {
            get { return _events; }
            set
            {
                _events = value;
                NotifyPropertyChanged("Events");
            }
        }

        private String _dna = "";
        public String DNA
        {
            get { return _dna; }
            set
            {
                _dna = value;
                NotifyPropertyChanged("DNA");
            }
        }

        private String _properties = "";
        public String Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                NotifyPropertyChanged("Properties");
            }
        }

        public override string ToString()
        {
            String result = "";

            result += ID.ToString() + " = {\r\n";
            result += "\tname=\"" + Name + "\"\r\n";
            if (Female)                 result += "\tfemale=yes\r\n";
            if (Dynasty != 0)           result += "\tdynasty=" + Dynasty.ToString() + "\r\n";
            if (!Religion.Equals(""))   result += "\treligion=\"" + Religion + "\"\r\n";
            if (!Culture.Equals(""))    result += "\tculture=\"" + Culture + "\"\r\n";
            if (Martial > 0)            result += "\tmartial=" + Martial.ToString() + "\r\n";
            if (Diplomacy > 0)          result += "\tdiplomacy=" + Diplomacy.ToString() + "\r\n";
            if (Intrigue > 0)           result += "\tintrigue=" + Intrigue.ToString() + "\r\n";
            if (Stewardship > 0)        result += "\tstewardship=" + Stewardship.ToString() + "\r\n";
            if (Learning > 0)           result += "\tlearning=" + Learning.ToString() + "\r\n";
            if (!DNA.Equals(""))        result += "\tdna=\"" + DNA + "\"\r\n";
            if (!Properties.Equals("")) result += "\tproperties=\"" + Properties + "\"\r\n";
            if (!Nickname.Equals(""))   result += "\tgive_nickname=" + Nickname + "\r\n";

            foreach (String trait in Traits)
            {
                result += "\tadd_trait=\"" + trait + "\"\r\n";
            }

            if (Father > 0) result += "\tfather=" + Father.ToString() + "\r\n";
            if (Mother > 0) result += "\tmother=" + Mother.ToString() + "\r\n";

            // Life events
            if (!Events.Equals(""))
            {                
                Events = Events.Replace("\r", "");
                result += "\t" + Events.Replace("\n", System.Environment.NewLine + "\t") + "\r\n";
            }            

            result += "}\r\n";

            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
