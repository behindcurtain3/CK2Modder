using System;
using System.ComponentModel;
using System.IO;
using CK2Modder.GameData.Interfaces;
using System.Collections.Generic;

namespace CK2Modder.GameData.history.characters
{
    [DefaultPropertyAttribute("ID")]
    public class Character : INotifyPropertyChanged, IFileResource
    {
        /// <summary>
        /// The name of the file the character is stored in
        /// </summary>
        private String _belongsTo;
        [BrowsableAttribute(false)]
        public String BelongsTo
        {
            get { return _belongsTo; }
            set
            {
                _belongsTo = value;
                NotifyPropertyChanged("BelongsTo");
            }
        }   

        /// <summary>
        /// The characters ID, must be unique
        /// </summary>
        private int _id = 0;
        [CategoryAttribute("Character"), DescriptionAttribute("The characters ID, must be unique")]
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
        [CategoryAttribute("Character"), DescriptionAttribute("The characters name")]
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private Boolean _female = false;
        [CategoryAttribute("Details"), DescriptionAttribute("Is this character a female?")]
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
        private int _dynasty = -1;
        [CategoryAttribute("Details"), DescriptionAttribute("The ID of the dynasty this character belongs to, not required")]
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
        [CategoryAttribute("Details"), DescriptionAttribute("The religion of the character")]
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
        [CategoryAttribute("Details"), DescriptionAttribute("The culture of the character")]
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
        [CategoryAttribute("Details"), DescriptionAttribute("The nickname of the character")]
        public String Nickname
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                NotifyPropertyChanged("Nickname");
            }
        }

        private int _martial = -1;
        [CategoryAttribute("Stats"), DescriptionAttribute("The martial attribute for the character, set to -1 for it to not be included in the output")]
        public int Martial
        {
            get { return _martial; }
            set
            {
                _martial = value;
                NotifyPropertyChanged("Martial");
            }
        }

        private int _diplomacy = -1;
        [CategoryAttribute("Stats"), DescriptionAttribute("The diplomacy attribute for the character, set to -1 for it to not be included in the output")]
        public int Diplomacy
        {
            get { return _diplomacy; }
            set
            {
                _diplomacy = value;
                NotifyPropertyChanged("Diplomacy");
            }
        }

        private int _stewardship = -1;
        [CategoryAttribute("Stats"), DescriptionAttribute("The stewardship attribute for the character, set to -1 for it to not be included in the output")]
        public int Stewardship
        {
            get { return _stewardship; }
            set
            {
                _stewardship = value;
                NotifyPropertyChanged("Stewardship");
            }
        }

        private int _intrigue = -1;
        [CategoryAttribute("Stats"), DescriptionAttribute("The intrigue attribute for the character, set to -1 for it to not be included in the output")]
        public int Intrigue
        {
            get { return _intrigue; }
            set
            {
                _intrigue = value;
                NotifyPropertyChanged("Intrigue");
            }
        }

        private int _learning = -1;
        [CategoryAttribute("Stats"), DescriptionAttribute("The learning attribute for the character, set to -1 for it to not be included in the output")]
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
        [CategoryAttribute("Stats"), DescriptionAttribute("The traits this character has")]
        public BindingList<String> Traits
        {
            get { return _traits; }
            set
            {
                _traits = value;
                NotifyPropertyChanged("Traits");
            }
        }

        private int _father = -1;
        [CategoryAttribute("Family"), DescriptionAttribute("The ID this characters father")]
        public int Father
        {
            get { return _father; }
            set
            {
                _father = value;
                NotifyPropertyChanged("Father");
            }
        }

        private int _mother = -1;
        [CategoryAttribute("Family"), DescriptionAttribute("The ID this characters mother")]
        public int Mother
        {
            get { return _mother; }
            set
            {
                _mother = value;
                NotifyPropertyChanged("Mother");
            }
        }

        private String _raw;
        [BrowsableAttribute(false)]
        public String Raw
        {
            get { return _raw; }
            set
            {
                _raw = value;
                NotifyPropertyChanged("Raw");
            }
        }

        private String _dna = "";
        [CategoryAttribute("Character Appearance"), DescriptionAttribute("The DNA sequence for this character")]
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
        [CategoryAttribute("Character Appearance")]
        public String Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                NotifyPropertyChanged("Properties");
            }
        }

        [BrowsableAttribute(false)]
        public String InternalDisplay
        {
            get { return String.Format("{0} - {1}", ID, Name); }
        }

        public override string ToString()
        {
            String result = "";

            result += ID.ToString() + " = {\r\n";
            result += "\tname=\"" + Name + "\"\r\n";
            if (Female)                 result += "\tfemale=yes\r\n";
            if (Dynasty != -1)           result += "\tdynasty=" + Dynasty.ToString() + "\r\n";
            if (!Religion.Equals(""))   result += "\treligion=\"" + Religion + "\"\r\n";
            if (!Culture.Equals(""))    result += "\tculture=\"" + Culture + "\"\r\n";
            if (Martial >= 0)            result += "\tmartial=" + Martial.ToString() + "\r\n";
            if (Diplomacy >= 0)          result += "\tdiplomacy=" + Diplomacy.ToString() + "\r\n";
            if (Intrigue >= 0)           result += "\tintrigue=" + Intrigue.ToString() + "\r\n";
            if (Stewardship >= 0)        result += "\tstewardship=" + Stewardship.ToString() + "\r\n";
            if (Learning >= 0)           result += "\tlearning=" + Learning.ToString() + "\r\n";
            if (!DNA.Equals(""))        result += "\tdna=\"" + DNA + "\"\r\n";
            if (!Properties.Equals("")) result += "\tproperties=\"" + Properties + "\"\r\n";
            if (!Nickname.Equals(""))   result += "\tgive_nickname=" + Nickname + "\r\n";

            foreach (String trait in Traits)
            {
                result += "\tadd_trait=\"" + trait + "\"\r\n";
            }

            if (Father >= 0) result += "\tfather=" + Father.ToString() + "\r\n";
            if (Mother >= 0) result += "\tmother=" + Mother.ToString() + "\r\n";

            // Life events
            if (!String.IsNullOrWhiteSpace(Raw))
            {
                // Format the output correctly
                String output = Raw.EndsWith("\r\n") ? Raw.Substring(0, Raw.Length - 2) : Raw;
                    
                output = output.Replace("\r\n", System.Environment.NewLine + "\t");

                result += "\t" + output + "\r\n";
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
