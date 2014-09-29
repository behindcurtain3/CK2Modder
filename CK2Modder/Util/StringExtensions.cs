using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder.Util
{
    public class StringExtensions
    {
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
