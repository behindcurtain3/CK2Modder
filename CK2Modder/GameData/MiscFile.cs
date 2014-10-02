using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder.GameData
{
    public class MiscFile : ModResource
    {
        /// <summary>
        /// Return the file name this belongs to for the display
        /// </summary>
        public override string Display
        {
            get { return BelongsTo; }
        }

        public static MiscFile Load(List<String> lines)
        {
            MiscFile file = new MiscFile();

            foreach (String line in lines)
            {
                file.Raw += line + System.Environment.NewLine;
            }

            return file;
        }
    }
}
