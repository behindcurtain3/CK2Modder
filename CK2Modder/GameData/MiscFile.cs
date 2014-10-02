using System;
using System.Collections.Generic;
using System.IO;

namespace CK2Modder.GameData
{
    /// <summary>
    /// This class allows the app to open and view any text file that is not specifically tracked
    /// by the app.
    /// </summary>
    public class MiscFile : ModResource
    {
        /// <summary>
        /// Return the file name this belongs to for the display
        /// </summary>
        public override string Display
        {
            get { return BelongsTo; }
        }

        /// <summary>
        /// FileInfo for the misc file
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Load the list of strings passed into a MiscFile object.
        /// </summary>
        /// <param name="lines">The list of strings that contains the text in a file</param>
        /// <returns>A MiscFile object populated with the strings passed in</returns>
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
