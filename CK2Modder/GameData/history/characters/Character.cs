using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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

        /// <summary>
        /// A string array that holds the traits of this character
        /// </summary>
        private List<String> _traits = new List<String>();
        public List<String> Traits
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

        private List<String> _events = new List<String>();
        public List<String> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                NotifyPropertyChanged("Events");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
