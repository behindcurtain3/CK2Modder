using System;
using System.ComponentModel;
using CK2Modder.GameData.Interfaces;

namespace CK2Modder.GameData.common
{
    public class Dynasty : INotifyPropertyChanged, IFileResource
    {
        private int _id;
        [CategoryAttribute("Dynasty"), DescriptionAttribute("The dynasties ID, must be unique")]
        public int ID 
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }

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

        private String _raw = String.Empty;
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

        private String _name;
        [CategoryAttribute("Dynasty"), DescriptionAttribute("The dynasties name")]
        public String Name 
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private String _culture;
        [CategoryAttribute("Information"), DescriptionAttribute("The dynasties culture")]
        public String Culture
        {
            get { return _culture; }
            set
            {
                _culture = value;
                NotifyPropertyChanged("Culture");
            }
        }

        private CoatOfArms _coa;
        [CategoryAttribute("Information"), DescriptionAttribute("The dynasties Coat of Arms")]
        public CoatOfArms COA
        {
            get { return _coa; }
            set { _coa = value; }
        }

        [BrowsableAttribute(false)]
        public String InternalDisplay
        {
            get { return String.Format("{0} - {1}", ID, Name); }
        }

        public Dynasty()
        {
            ID = 0;
            Name = "";
            Culture = "";
            COA = null;
        }

        public Dynasty(int id, String name, String culture)
        {
            ID = id;
            Name = name;
            Culture = culture;
            COA = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            String result = ID + " = {\r\n";
            result += "\tname=\"" + Name + "\"\r\n";
            result += "\tculture = " + Culture + "\r\n";

            if(COA != null)
                result += COA.ToString();

            result += "}\r\n";
            return result;
        }
    }
}
