using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CK2Modder.Util;

namespace CK2Modder.GameData.history.characters
{
    public class Character : ModResource
    {
        #region Fields

        /// <summary>
        /// Only use the fields that we care about being able to search or use some way to help
        /// the user navigate through the app
        /// </summary>
        private int _id = 0;        
        private int _dynasty = -1;        
        private int _father = -1;
        private int _mother = -1;
        private String _name = String.Empty;
        private String _religion = "";
        private String _culture = "";
        private String _nickName = "";

        #endregion

        #region Properties

        /// <summary>
        /// The characters ID, must be unique
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
        /// The characters name
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
        /// The ID of the dynasty the character belongs to, not required
        /// </summary>
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
        /// The nickname of the character
        /// </summary>
        public String Nickname
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                NotifyPropertyChanged("Nickname");
            }
        }

        /// <summary>
        /// The ID of this characters father
        /// </summary>
        public int Father
        {
            get { return _father; }
            set
            {
                _father = value;
                NotifyPropertyChanged("Father");
            }
        }

        /// <summary>
        /// The ID of this characters mother
        /// </summary>
        public int Mother
        {
            get { return _mother; }
            set
            {
                _mother = value;
                NotifyPropertyChanged("Mother");
            }
        }

        /// <summary>
        /// The internal display for this resource
        /// </summary>
        public override string  Display
        {
            get { return String.Format("{0} - {1}", ID, Name); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a character from a list of strings
        /// </summary>
        /// <param name="lines">List of strings that contain the raw data for the character</param>
        /// <returns>An instance of the Character class filled in with the given data, Null if invalid data is passed in</returns>
        public static Character Load(List<String> lines)
        {
            Character c = new Character();

            // need a minimum of two lines to have a character
            if (lines.Count < 2)
                return null;

            // Load the ID, it is always on the first line
            c.ID = Helpers.ParseInt(lines[0]);

            // if a bad ID is returned return null
            if (c.ID == -1)
                return null;

            // loop through each line and handle them appropriately
            for (int i = 0; i < lines.Count; i++)
            {
                // add the text to the raw output and make sure there is a new line added to the end of each
                c.Raw += lines[i] + System.Environment.NewLine;

                // load in the values, but not events which will have the opening {
                if (lines[i].Contains("=") && !lines[i].Contains("{"))
                {
                    // use the helper to load the value
                    KeyValuePair<String, String> data = Helpers.ReadStringData(lines[i]);

                    // figure out what to do with it
                    switch (data.Key.ToLower())
                    {
                        case "name":
                            // if the name already has a value don't override it
                            if(String.IsNullOrWhiteSpace(c.Name))
                                c.Name = data.Value;
                            break;

                        case "culture":
                            // if the cultuer already has a value don't override it
                            if (String.IsNullOrWhiteSpace(c.Culture))
                                c.Culture = data.Value;
                            break;

                        case "religion":
                            // if the religion already has a value don't override it
                            if (String.IsNullOrWhiteSpace(c.Religion))
                                c.Religion = data.Value;
                            break;

                        case "give_nickname":
                            c.Nickname = data.Value;
                            break;

                        case "dynasty":
                            c.Dynasty = Helpers.ParseInt(data.Value);
                            break;

                        case "father":
                            c.Father = Helpers.ParseInt(data.Value);
                            break;

                        case "mother":
                            c.Mother = Helpers.ParseInt(data.Value);
                            break;
                    }
                }
            }

            return c;
        }

        #endregion
    }
}
