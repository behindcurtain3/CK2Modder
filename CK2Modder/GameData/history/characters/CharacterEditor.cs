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
        public Button CloseButton { get { return closeButton; } }
        public Button AddTraitButton { get { return addTraitButton; } }

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
        public TextBox Learning { get { return learningTextBox; } }
        public CheckBox Female { get { return femaleCheckBox; } }
        public TextBox Father { get { return fatherTextBox; } }
        public TextBox Mother { get { return motherTextBox; } }
        public ListBox LifeEvents { get { return characterLifeEventsListBox; } }
        public ComboBox TraitComboBox { get { return traitComboBox; } }
        public TextBox DNA { get { return dnaTextBox; } }
        public TextBox Properties { get { return propertiesTextBox; } }
        public TextBox Nickname { get { return nicknameTextBox; } }

        public CharacterEditor()
        {
            InitializeComponent();            
        }        

        private void traitComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Submit the form
                addTraitButton.PerformClick();
            }
        }
    }
}
