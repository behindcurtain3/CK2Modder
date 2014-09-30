using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CK2Modder.Util
{
    public class Helpers
    {
        /// <summary>
        /// Takes in a string that contains a key and a value. Splits the string
        /// and returns a KeyValuePair with the data.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>KeyValuePair containing the key and the value contained in the string passed in.</returns>
        public static KeyValuePair<String, String> ReadStringData(String line)
        {
            // split the incoming string
            String[] values = line.Split('=');
            
            // clean up each part
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

                // we only want the value inside quotes if there are any
                if (values[i].Contains('"'))
                {
                    int start = values[i].IndexOf('"') + 1;
                    int end = values[i].IndexOf('"', start);
                    values[i] = values[i].Substring(start, end - start);
                }
            }

            // return the result
            if (values.Length < 2)
                return new KeyValuePair<String, String>();
            else
                return new KeyValuePair<String, String>(values[0], values[1]);
        }

        /// <summary>
        /// Attempts to parse and integer value from the given string.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The integer value passed in and as int. If invalid string returns -1.</returns>
        public static int ParseInt(String line)
        {
            try
            {
                return Int32.Parse(Regex.Match(line, @"\d+").Value);
            }
            catch
            {
                return -1;
            }
        }

        public static List<String> ReadStringSequence(List<String> lines, int startIndex)
        {
            List<String> data = new List<String>();
            int bracketCount = 0;

            // Loop through the lines
            for (int i = startIndex; i < lines.Count; i++)
            {
                //if(lines[i].StartsWith("#"))
                //   continue;

                if (lines[i].Contains("{"))
                    bracketCount += StringExtensions.CountOccurences(lines[i], "{");
                if (lines[i].Contains("}"))
                    bracketCount -= StringExtensions.CountOccurences(lines[i], "}");

                data.Add(lines[i]);

                if(lines[i].Contains("}") && bracketCount == 0)
                {
                    // exit the loop
                    break;
                }
            }

            return data;
        }
    }
}
