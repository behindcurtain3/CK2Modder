using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder.GameData.Interfaces
{
    /// <summary>
    /// Used for any game data that belongs in a file that is read from or saved to
    /// </summary>
    public interface IFileResource
    {
        // The file this resource belongs to
        String BelongsTo { get; set; }

        // The display that can be used for this resource without modifying it
        String InternalDisplay { get; }
    }
}
