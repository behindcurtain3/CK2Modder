using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CK2Modder.GameData.common
{
    public class Culture : INotifyPropertyChanged
    {
        private String _name = "";
        public String Name
        {
            get { return _name; }
            set 
            { 
                _name = value.Trim();
                NotifyPropertyChanged("Name");
            }
        }

        private String _graphical_culture = "";
        public String Graphical_Culture
        {
            get { return _graphical_culture; }
            set 
            { 
                _graphical_culture = value;
                NotifyPropertyChanged("Graphical_Culture");
            }
        }

        private String _color = "";
        public String Color
        {
            get { return _color; }
            set 
            { 
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        private String _maleNames = "";
        public String MaleNames
        {
            get { return _maleNames; }
            set 
            { 
                _maleNames = value;
                NotifyPropertyChanged("MaleNames");
            }
        }

        private String _femaleNames = "";
        public String FemaleNames
        {
            get { return _femaleNames; }
            set 
            { 
                _femaleNames = value;
                NotifyPropertyChanged("FemaleNames");
            }
        }

        private String _malePatronym = "";
        public String MalePatronym
        {
            get { return _malePatronym; }
            set
            {
                _malePatronym = value;
                NotifyPropertyChanged("MalePatronym");
            }
        }

        private String _femalePatronym = "";
        public String FemalePatronym
        {
            get { return _femalePatronym; }
            set
            {
                _femalePatronym = value;
                NotifyPropertyChanged("FemalePatronym");
            }
        }

        private Boolean _isSuffix = false;
        public Boolean IsSuffix
        {
            get { return _isSuffix; }
            set
            {
                _isSuffix = value;
                NotifyPropertyChanged("IsSuffix");
            }
        }

        private String _dynastyPrefix = "";
        public String DynastyPrefix
        {
            get { return _dynastyPrefix; }
            set 
            { 
                _dynastyPrefix = value;
                NotifyPropertyChanged("DynastyPrefix");
            }
        }

        private String _bastardPrefix = "";
        public String BastardPrefix
        {
            get { return _bastardPrefix; }
            set 
            { 
                _bastardPrefix = value;
                NotifyPropertyChanged("BastardPrefix");
            }
        }

        private int _patGFChance = 0;
        public int PaternalGrandFather
        {
            get { return _patGFChance; }
            set 
            { 
                _patGFChance = value;
                NotifyPropertyChanged("PaternalGrandFather");
            }
        }

        private int _matGFChance = 0;
        public int MaternalGrandFather
        {
            get { return _matGFChance; }
            set 
            { 
                _matGFChance = value;
                NotifyPropertyChanged("MaternalGrandFather");
            }
        }

        private int _father = 0;
        public int Father
        {
            get { return _father; }
            set 
            { 
                _father = value;
                NotifyPropertyChanged("Father");
            }
        }

        private int _patGMChance = 0;
        public int PaternalGrandMother
        {
            get { return _patGMChance; }
            set 
            { 
                _patGMChance = value;
                NotifyPropertyChanged("PaternalGrandMother");
            }
        }

        private int _matGMChance = 0;
        public int MaternalGrandMother
        {
            get { return _matGMChance; }
            set 
            { 
                _matGMChance = value;
                NotifyPropertyChanged("MaternalGrandMother");
            }
        }

        private int _mother = 0;
        public int Mother
        {
            get { return _mother; }
            set 
            { 
                _mother = value;
                NotifyPropertyChanged("Mother");
            }
        }

        private String _modifier = "default_culture_modifier";
        public String Modifier
        {
            get { return _modifier; }
            set 
            { 
                _modifier = value;
                NotifyPropertyChanged("Modifier");
            }
        }

        private List<Culture> _subCultures;
        public List<Culture> SubCultures
        {
            get { return _subCultures; }
            set { _subCultures = value; }
        }

        public Culture()
        {
            _subCultures = new List<Culture>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            String tabPrefix = (SubCultures.Count == 0 ? "\t" : "");

            String result = tabPrefix + Name + " = {\r\n";
            result += tabPrefix + "\tgraphical_culture = " + Graphical_Culture + "\r\n";

            if (SubCultures.Count == 0)
            {
                result += "\r\n";

                if(!Color.Equals(""))
                    result += tabPrefix + "\tcolor = { " + Color + " }\r\n";

                if (!MaleNames.Equals(""))
                {
                    result += tabPrefix + "\tmale_names = {\r\n";
                    MaleNames = MaleNames.Replace("\r", "");
                    result += tabPrefix + "\t\t" + MaleNames.Replace("\n", System.Environment.NewLine + "\t\t\t") + "\r\n";
                    result += tabPrefix + "\t}\r\n";
                }

                if (!FemaleNames.Equals(""))
                {
                    result += tabPrefix + "\tfemale_names = {\r\n";
                    FemaleNames = FemaleNames.Replace("\r", "");
                    result += tabPrefix + "\t\t" + FemaleNames.Replace("\n", System.Environment.NewLine + "\t\t\t") + "\r\n";
                    result += tabPrefix + "\t}\r\n";
                }

                if (!DynastyPrefix.Equals(""))
                {
                    result += "\r\n";
                    result += tabPrefix + "\tfrom_dynasty_prefix = \"" + DynastyPrefix + "\"\r\n";
                }
                if (!BastardPrefix.Equals(""))
                {
                    result += "\r\n";
                    result += tabPrefix + "\tbastard_dynasty_prefix = \"" + BastardPrefix + "\"\r\n";
                }

                result += "\r\n";
                if(!MalePatronym.Equals(""))
                    result += tabPrefix + "\tmale_patronym = \"" + MalePatronym + "\"\r\n";
                if(!FemalePatronym.Equals(""))
                    result += tabPrefix + "\tfemale_patronym = \"" + FemalePatronym + "\"\r\n";
                if (!MalePatronym.Equals("") || !FemalePatronym.Equals(""))
                    result += tabPrefix + "\tprefix = " + (IsSuffix ? "yes" : "no") + "\r\n";

                result += "\r\n";
                result += tabPrefix + "\tpat_grf_name_chance = " + PaternalGrandFather.ToString() + "\r\n";
                result += tabPrefix + "\tmat_grf_name_chance = " + MaternalGrandFather.ToString() + "\r\n";
                result += tabPrefix + "\tfather_name_chance = " + Father.ToString() + "\r\n";

                result += "\r\n";
                result += tabPrefix + "\tpat_grm_name_chance = " + PaternalGrandMother.ToString() + "\r\n";
                result += tabPrefix + "\tmat_grm_name_chance = " + MaternalGrandMother.ToString() + "\r\n";
                result += tabPrefix + "\tmother_name_chance = " + Mother.ToString() + "\r\n";

                result += "\r\n";
                result += tabPrefix + "\tmodifier = " + Modifier + "\r\n";

            }
            else
            {
                foreach (Culture c in SubCultures)
                {
                    result += "\r\n";
                    result += c.ToString();
                }
            }

            result += tabPrefix + "}\r\n";
            return result;
        }
    }
}
