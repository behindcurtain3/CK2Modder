using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CK2Modder.GameData.history.characters
{
    public partial class CharacterFileForm : Form
    {
        public String FileName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        public CharacterFileForm()
        {
            InitializeComponent();

            FileName = "";
        }

        public CharacterFileForm(String defaultName)
        {
            InitializeComponent();

            FileName = defaultName;            
        }
    }
}
