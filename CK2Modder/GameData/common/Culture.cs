using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CK2Modder.Util;

namespace CK2Modder.GameData.common
{
    public class Culture : ModResource
    {
        #region Fields

        private String _name = "";
        private List<Culture> _subCultures;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the culture
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
                _name = value.Trim();
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// If this is a culture group there will be any number of cultures that belong to it
        /// </summary>
        public List<Culture> SubCultures
        {
            get { return _subCultures; }
            set { _subCultures = value; }
        }

        /// <summary>
        /// How the culture is displayed internally in this app
        /// </summary>
        public override string Display
        {
            get { return Name; }
        }

        #endregion

        #region Methods

        public Culture()
        {
            _subCultures = new List<Culture>();
        }

        public override string ToString()
        {
            return Raw;
        }

        public static Culture Load(List<String> lines)
        {
            Culture culture = new Culture();

            // need a minimum of two lines
            if (lines.Count < 2)
                return null;

            // Load the culture name, it is always on the first line
            KeyValuePair<String, String> name = Helpers.ReadStringData(lines[0]);
            culture.Name = name.Key;

            // if a bad ID is returned return null
            if (String.IsNullOrWhiteSpace(culture.Name))
                return null;

            // loop through each line and handle them appropriately
            for (int i = 0; i < lines.Count; i++)
            {
                culture.Raw += lines[i] + System.Environment.NewLine;

                // Load subcultures
                if (lines[i].Contains("= {") && !lines[i].StartsWith("#") && !lines[i].Contains("male_names") && !lines[i].Contains("female_names") && !lines.Contains("color"))
                {
                    /*
                    // Load the subculture lines into a new list
                    List<String> subCultureLines = Helpers.ReadStringSequence(lines, i);

                    // Load the subculture using a recursive call to this function
                    Culture subCulture = Culture.Load(subCultureLines);

                    // Add the subculture to the current culture if it is valid
                    if (subCulture != null)
                    {
                        culture.SubCultures.Add(subCulture);
                        culture.Raw += subCulture.Raw + System.Environment.NewLine;
                    }

                    // make sure the current loop advances to skip over the subCulture lines
                    i += subCultureLines.Count - 1;
                     */
                }
                // load in the values, but not events which will have the opening {
                else if (lines[i].Contains("=") && !lines[i].Contains("{"))
                {
                    // use the helper to load the value
                    KeyValuePair<String, String> data = Helpers.ReadStringData(lines[i]);

                    switch (data.Key.ToLower())
                    {
                        // nothing of value to load here, at the moment this is just a placeholder
                        default:
                            break;
                    }
                }
            }

            return culture;
        }

        #endregion
    }
}
