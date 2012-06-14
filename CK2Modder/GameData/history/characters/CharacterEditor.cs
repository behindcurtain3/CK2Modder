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
        // Expose some of the controls
        public Button CloseButton
        {
            get { return closeButton; }
        }

        public TextBox ID { get { return idTextBox; } }
        public TextBox CharacterName { get { return nameTextBox; } }
        public TextBox Dynasty { get { return dynastyTextBox; } }
        public TextBox Religion { get { return religionTextBox; } }
        public TextBox Culture { get { return cultureTextBox; } }
        public ListBox Traits { get { return traitsListBox; } }
        public TextBox Martial { get { return martialTextBox; } }
        public TextBox Diplomacy { get { return diplomacyTextBox; } }
        public TextBox Intrigue { get { return intrigueTextBox; } }
        public TextBox Stewardship { get { return stewardshipTextBox; } }
        public CheckBox Female { get { return femaleCheckBox; } }
        public TextBox Father { get { return fatherTextBox; } }
        public TextBox Mother { get { return motherTextBox; } }
        public ListBox LifeEvents { get { return characterLifeEventsListBox; } }

        public CharacterEditor()
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
