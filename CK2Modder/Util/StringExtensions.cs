using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder.Util
{
    public class StringExtensions
    {
        /// <summary>
        /// Counts the number of times the pattern appears in the text
        /// </summary>
        /// <param name="text">The string to search</param>
        /// <param name="pattern">The pattern to look for</param>
        /// <returns>Number of times the pattern appears in the text</returns>
        public static int CountOccurences(String text, String pattern)
        {
            int count = 0;
            int i = 0;
            
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }

            return count;
        }
    }
}
