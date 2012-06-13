using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CK2Modder.GameData.history.characters
{
    public class Character : INotifyPropertyChanged
    {
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


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
