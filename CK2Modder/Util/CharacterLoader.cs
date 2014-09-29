using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CK2Modder.GameData.history.characters;
using System.IO;
using System.Text.RegularExpressions;

namespace CK2Modder.Util
{
    public class CharacterLoader
    {
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
            for (int i = 1; i < lines.Count; i++)
            {
                // load in the values, but not events which will have the opening {
                if (lines[i].Contains("=") && !lines[i].Contains("{"))
                {
                    // use the helper to load the value
                    KeyValuePair<String, String> data = Helpers.ReadStringData(lines[i]);

                    // figure out what to do with it
                    switch (data.Key.ToLower())
                    {
                        case "name":
                            c.Name = data.Value;
                            break;

                        case "female":
                            c.Female = data.Value.Equals("yes");
                            break;

                        case "culture":
                            c.Culture = data.Value;
                            break;

                        case "religion":
                            c.Religion = data.Value;
                            break;

                        case "give_nickname":
                            c.Nickname = data.Value;
                            break;

                        case "dynasty":
                            c.Dynasty = Helpers.ParseInt(data.Value);
                            break;

                        case "add_trait":
                            c.Traits.Add(data.Value);
                            break;

                        case "father":
                            c.Father = Helpers.ParseInt(data.Value);
                            break;

                        case "mother":
                            c.Mother = Helpers.ParseInt(data.Value);
                            break;

                        case "dna":
                            c.DNA = data.Value;
                            break;

                        case "properties":
                            c.Properties = data.Value;
                            break;

                        case "martial":
                            c.Martial = Helpers.ParseInt(data.Value);
                            break;

                        case "diplomacy":
                            c.Diplomacy = Helpers.ParseInt(data.Value);
                            break;

                        case "intrigue":
                            c.Intrigue = Helpers.ParseInt(data.Value);
                            break;

                        case "stewardship":
                            c.Stewardship = Helpers.ParseInt(data.Value);
                            break;

                        case "learning":
                            c.Learning = Helpers.ParseInt(data.Value);
                            break;
                    }
                }
                // an event sequence
                else if (lines[i].Contains("{"))
                {
                    List<String> eventLines = Helpers.ReadStringSequence(lines, i);

                    // trim the first tab off, this will be added back on when exported
                    for (int j = 0; j < eventLines.Count; j++)
                    {
                        eventLines[j] = new Regex("\t").Replace(eventLines[j], "", 1);
                        c.Raw += eventLines[j] + System.Environment.NewLine;
                    }
                    
                    // advance the current line position
                    i += eventLines.Count - 1;
                }
            }

            return c;
        }
    }
}
