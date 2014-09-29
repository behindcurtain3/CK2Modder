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

        
        

        public Culture()
        {
            _subCultures = new List<Culture>();
        }

        public override string ToString()
        {
            return Raw;
            /*

            String tabPrefix = (SubCultures.Count == 0 ? "\t" : "");

            String result = tabPrefix + Name + " = {\r\n";
            result += tabPrefix + "\tgraphical_culture = " + Graphical_Culture + "\r\n";

            if (SubCultures.Count == 0)
            {
                result += "\r\n";

                if(!Color.Equals(""))
                    result += tabPrefix + "\tcolor = { " + Color + " }\r\n";

                if (!MaleNames.Equals(""))
                {
                    result += tabPrefix + "\tmale_names = {\r\n";
                    MaleNames = MaleNames.Replace("\r", "");
                    result += tabPrefix + "\t\t" + MaleNames.Replace("\n", System.Environment.NewLine + "\t\t\t") + "\r\n";
                    result += tabPrefix + "\t}\r\n";
                }

                if (!FemaleNames.Equals(""))
                {
                    result += tabPrefix + "\tfemale_names = {\r\n";
                    FemaleNames = FemaleNames.Replace("\r", "");
                    result += tabPrefix + "\t\t" + FemaleNames.Replace("\n", System.Environment.NewLine + "\t\t\t") + "\r\n";
                    result += tabPrefix + "\t}\r\n";
                }

                if (!DynastyPrefix.Equals(""))
                {
                    result += "\r\n";
                    result += tabPrefix + "\tfrom_dynasty_prefix = \"" + DynastyPrefix + "\"\r\n";
                }
                if (!BastardPrefix.Equals(""))
                {
                    result += "\r\n";
                    result += tabPrefix + "\tbastard_dynasty_prefix = \"" + BastardPrefix + "\"\r\n";
                }

                result += "\r\n";
                if(!MalePatronym.Equals(""))
                    result += tabPrefix + "\tmale_patronym = \"" + MalePatronym + "\"\r\n";
                if(!FemalePatronym.Equals(""))
                    result += tabPrefix + "\tfemale_patronym = \"" + FemalePatronym + "\"\r\n";
                if (!MalePatronym.Equals("") || !FemalePatronym.Equals(""))
                    result += tabPrefix + "\tprefix = " + (IsSuffix ? "yes" : "no") + "\r\n";

                result += "\r\n";
                result += tabPrefix + "\tpat_grf_name_chance = " + PaternalGrandFather.ToString() + "\r\n";
                result += tabPrefix + "\tmat_grf_name_chance = " + MaternalGrandFather.ToString() + "\r\n";
                result += tabPrefix + "\tfather_name_chance = " + Father.ToString() + "\r\n";

                result += "\r\n";
                result += tabPrefix + "\tpat_grm_name_chance = " + PaternalGrandMother.ToString() + "\r\n";
                result += tabPrefix + "\tmat_grm_name_chance = " + MaternalGrandMother.ToString() + "\r\n";
                result += tabPrefix + "\tmother_name_chance = " + Mother.ToString() + "\r\n";

                result += "\r\n";
                result += tabPrefix + "\tmodifier = " + Modifier + "\r\n";

            }
            else
            {
                foreach (Culture c in SubCultures)
                {
                    result += "\r\n";
                    result += c.ToString();
                }
            }

            result += tabPrefix + "}\r\n";
            return result;
             */
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
            for (int i = 1; i < lines.Count; i++)
            {
                // Load subcultures
                if (lines[i].Contains("= {") && !lines[i].StartsWith("#") && !lines[i].Contains("male_names") && !lines[i].Contains("female_names"))
                {
                    // Load the subculture lines into a new list
                    List<String> subCultureLines = Helpers.ReadStringSequence(lines, i);

                    // Load the subculture using a recursive call to this function
                    Culture subCulture = Culture.Load(subCultureLines);

                    // Add the subculture to the current culture if it is valid
                    if (subCulture != null)
                        culture.SubCultures.Add(subCulture);

                    // make sure the current loop advances to skip over the subCulture lines
                    i += subCultureLines.Count - 1;
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
                // an event sequence
                else if (lines[i].Contains("{"))
                {
                    List<String> nameLines = Helpers.ReadStringSequence(lines, i);
                    String holder = "";

                    foreach (String s in nameLines)
                    {
                        if (!holder.Equals(""))
                        {
                            holder += "\r\n";

                            if (!s.Contains("{"))
                            {
                                if (!s.Contains("}"))
                                    holder += "\t";
                            }
                        }
                        holder += s.Trim();
                    }

                    // Add the names to the correct gender
                    //if (nameLines[0].Contains("female_names"))
                    //    culture.FemaleNames = holder;
                    //else
                    //    culture.MaleNames = holder;

                    // advance the current line position
                    i += nameLines.Count - 1;
                }
            }

            return culture;
        }
    }
}
