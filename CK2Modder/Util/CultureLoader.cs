using System;
using System.Collections.Generic;
using CK2Modder.GameData.common;

namespace CK2Modder.Util
{
    public class CultureLoader
    {
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
                    Culture subCulture = CultureLoader.Load(subCultureLines);

                    // Add the subculture to the current culture if it is valid
                    if(subCulture != null)
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
                        case "graphical_culture":
                            culture.Graphical_Culture = data.Value;
                            break;

                        case "color":
                            culture.Color = data.Value;
                            break;

                        case "from_dynasty_prefix":
                            culture.DynastyPrefix = data.Value;
                            break;

                        case "bastard_dynasty_prefix":
                            culture.BastardPrefix = data.Value;
                            break;

                        case "male_patronym":
                            culture.MalePatronym = data.Value;
                            break;

                        case "female_patronym":
                            culture.FemalePatronym = data.Value;
                            break;

                        case "prefix":
                            culture.IsSuffix = data.Value.Equals("yes");
                            break;

                        case "pat_grf_name_chance":
                            culture.PaternalGrandFather = Helpers.ParseInt(data.Value);
                            break;

                        case "mat_grf_name_chance":
                            culture.MaternalGrandFather = Helpers.ParseInt(data.Value);
                            break;

                        case "father_name_chance":
                            culture.Father = Helpers.ParseInt(data.Value);
                            break;

                        case "pat_grm_name_chance":
                            culture.PaternalGrandMother = Helpers.ParseInt(data.Value);
                            break;

                        case "mat_grm_name_chance":
                            culture.MaternalGrandMother = Helpers.ParseInt(data.Value);
                            break;

                        case "mother_name_chance":
                            culture.Mother = Helpers.ParseInt(data.Value);
                            break;

                        case "modifier":
                            culture.Modifier = data.Value;
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
                    if (nameLines[0].Contains("female_names"))
                        culture.FemaleNames = holder;
                    else
                        culture.MaleNames = holder;

                    // advance the current line position
                    i += nameLines.Count - 1;
                }
            }

            return culture;
        }
    }
}
