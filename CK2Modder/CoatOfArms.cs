using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder
{
    public struct Layer
    {
        public int Texture { get; set; }
        public int Texture_Internal { get; set; }
        public int Emblem { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }

    public class CoatOfArms
    {
        private int _template;
        public int Template
        {
            get { return _template; }
            set { _template = value; }
        }

        public Layer Layer;

        public CoatOfArms()
        {
            Template = 0;
            Layer = new Layer();
            Layer.Texture = 0;
            Layer.Texture_Internal = 0;
            Layer.Emblem = 0;
            Layer.R = 0;
            Layer.G = 0;
            Layer.B = 0;            
        }

        public override string ToString()
        {
            String result = "\tcoat_of_arms = {\n";
            result += "\t\ttemplate = " + Template + "\n";
            result += "\t\tlayer = {\n";
            result += "\t\t\ttexture = " + Layer.Texture + "\n";
            result += "\t\t\ttexture_internal = " + Layer.Texture_Internal + "\n";
            result += "\t\t\temblem = " + Layer.Emblem + "\n";
            result += "\t\t\tcolor = " + Layer.R + "\n";
            result += "\t\t\tcolor = " + Layer.G + "\n";
            result += "\t\t\tcolor = " + Layer.B + "\n";
            result += "\t\t}\n";
            result += "\t}\n";

            return result;
        }
    }
}
