using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CK2Modder.GameData.history.characters
{
    public partial class CharacterEditor : UserControl
    {
        public Button CloseButton
        {
            get { return closeButton; }
        }

        public CharacterEditor()
        {
            InitializeComponent();
        }
    }
}
