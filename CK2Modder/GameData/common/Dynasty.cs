using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CK2Modder.Util;

namespace CK2Modder.GameData.common
{
    public class Dynasty : ModResource
    {
        #region Fields

        private int _id;
        private String _name;
        private String _culture;

        #endregion

        #region Properties

        /// <summary>
        /// The dynasties ID
        /// </summary>
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
        /// The dynasties name
        /// </summary>
        public String Name 
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// The dynasties culture
        /// </summary>
        public String Culture
        {
            get { return _culture; }
            set
            {
                _culture = value;
                NotifyPropertyChanged("Culture");
            }
        }
        
        /// <summary>
        /// The internal display of the dynasty
        /// </summary>
        public override String Display
        {
            get { return String.Format("{0} - {1}", ID, Name); }
        }

        #endregion

        public Dynasty()
        {
            ID = 0;
            Name = "";
            Culture = "";
        }

        public Dynasty(int id, String name, String culture)
        {
            ID = id;
            Name = name;
            Culture = culture;
        }

        public override string ToString()
        {
            return Raw;            
        }

        public static Dynasty Load(List<String> lines)
        {
            Dynasty dynasty = new Dynasty();

            // need a minimum of two lines
            if (lines.Count < 2)
                return null;

            // Load the ID, it is always on the first line
            dynasty.ID = Helpers.ParseInt(lines[0]);

            // if a bad ID is returned return null
            if (dynasty.ID == -1)
                return null;

            // loop through each line and handle them appropriately
            for (int i = 0; i < lines.Count; i++)
            {
                // add the text to the raw output and make sure there is a new line added to the end of each
                dynasty.Raw += lines[i] + System.Environment.NewLine;

                // load in the values, but not events which will have the opening {
                if (lines[i].Contains("=") && !lines[i].Contains("{"))
                {
                    // use the helper to load the value
                    KeyValuePair<String, String> data = Helpers.ReadStringData(lines[i]);

                    switch (data.Key.ToLower())
                    {
                        case "name":
                            dynasty.Name = data.Value;
                            break;

                        case "culture":
                            dynasty.Culture = data.Value;
                            break;
                    }
                }
            }

            return dynasty;
        }
    }
}
