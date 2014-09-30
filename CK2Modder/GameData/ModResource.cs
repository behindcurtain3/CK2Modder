using System;
using System.ComponentModel;

namespace CK2Modder.GameData
{
    /// <summary>
    /// Used for any game data that belongs in a file that is read from or saved to
    /// </summary>
    public abstract class ModResource: INotifyPropertyChanged
    {
        #region Fields

        private string _raw = String.Empty;

        #endregion

        #region Properties

        // The file this resource belongs to
        public String BelongsTo { get; set; }

        // Any complex text sequence that needs to be easily editable
        // IE: the text associated with this resource
        public String Raw 
        {
            get { return _raw; }
            set
            {
                _raw = value;
                NotifyPropertyChanged("Raw");
            }
        }

        // The display that can be used for this resource without modifying it
        public abstract String Display { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Override the ToString method and return the raw string instead
        /// Used for easily writing resources to files
        /// </summary>
        /// <returns>Raw text output</returns>
        public override string ToString()
        {
            return Raw;
        }

        // Used for data bindings
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
