using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CK2Modder
{
    public partial class NewModForm : Form
    {
        Form1 mainForm;

        public NewModForm(Form1 form)
        {
            InitializeComponent();

            mainForm = form;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Equals(""))
            {
                MessageBox.Show(this, "Please enter a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check to see if a mod like this already exists
            String directory = UserPreferences.Default.WorkingLocation + "\\mod\\";
            if (File.Exists(directory + nameTextBox.Text + ".mod"))
            {
                MessageBox.Show(this, "A mod with this name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mainForm.SaveMod();
            mainForm.CloseMod();

            Mod mod = new Mod(nameTextBox.Text);

            mainForm.setCurrentMod(mod);
            this.Close();
        }
    }
}
