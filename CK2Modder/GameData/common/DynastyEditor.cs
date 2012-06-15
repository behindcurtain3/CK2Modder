using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CK2Modder.GameData.common
{
    public partial class DynastyEditor : UserControl
    {
        public TextBox ID { get { return idTextBox; } }
        public TextBox DynastyName { get { return nameTextBox; } }
        public TextBox Culture { get { return cultureTextBox; } }
        public DataGridView Characters { get { return charactersDataGridView; } }
        public Button CloseButton { get { return closeButton; } }

        public DynastyEditor()
        {
            InitializeComponent();

            charactersDataGridView.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "ID";
            idColumn.HeaderText = "ID";
            idColumn.MinimumWidth = 50;

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.MinimumWidth = 150;

            DataGridViewTextBoxColumn cultureColumn = new DataGridViewTextBoxColumn();
            cultureColumn.DataPropertyName = "Culture";
            cultureColumn.HeaderText = "Culture";
            cultureColumn.MinimumWidth = 75;

            DataGridViewTextBoxColumn dynastyColumn = new DataGridViewTextBoxColumn();
            dynastyColumn.DataPropertyName = "Dynasty";
            dynastyColumn.HeaderText = "Dynasty";
            dynastyColumn.MinimumWidth = 50;

            DataGridViewTextBoxColumn religionColumn = new DataGridViewTextBoxColumn();
            religionColumn.DataPropertyName = "Religion";
            religionColumn.HeaderText = "Religion";
            religionColumn.MinimumWidth = 50;

            charactersDataGridView.Columns.Add(idColumn);
            charactersDataGridView.Columns.Add(nameColumn);
            charactersDataGridView.Columns.Add(dynastyColumn);
            charactersDataGridView.Columns.Add(religionColumn);
            charactersDataGridView.Columns.Add(cultureColumn);
        }
    }
}
