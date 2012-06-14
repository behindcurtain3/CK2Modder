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
    public partial class LifeEventEditor : Form
    {
        public LifeEventEditor()
        {
            InitializeComponent();
        }

        private void characterLifeEventComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (characterLifeEventComboBox.Text.Equals("birth") || characterLifeEventComboBox.Text.Equals("death"))
            {
                characterLifeEventIDTextBox.Enabled = false;
            }
            else
            {
                characterLifeEventIDTextBox.Enabled = true;
            }
        }

        private void characterLifeEventComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (characterLifeEventComboBox.Text.Equals("birth") || characterLifeEventComboBox.Text.Equals("death"))
            {
                characterLifeEventIDTextBox.Enabled = false;
            }
            else
            {
                characterLifeEventIDTextBox.Enabled = true;
            }
        }
    }
}
