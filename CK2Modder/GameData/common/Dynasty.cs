using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CK2Modder.GameData.common
{
    public class Dynasty : INotifyPropertyChanged
    {
        private int _id;
        public int ID 
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }

        private String _name;
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
        public CoatOfArms COA
        {
            get { return _coa; }
            set { _coa = value; }
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
