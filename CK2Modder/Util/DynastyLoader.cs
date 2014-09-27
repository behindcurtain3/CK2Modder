using System;
using System.Collections.Generic;
using CK2Modder.GameData.common;

namespace CK2Modder.Util
{
    public class DynastyLoader
    {
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
            for (int i = 1; i < lines.Count; i++)
            {
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
