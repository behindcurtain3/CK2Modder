using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK2Modder.GameData.history.characters
{
    public class LifeEvent
    {
        private String _date = "";
        public String Date
        {
            get { return _date; }
            set
            {
                _date = value;
            }
        }

        private List<KeyValuePair<String, object>> _events = new List<KeyValuePair<string,object>>();
        public List<KeyValuePair<String, object>> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public String GetRawOutput()
        {
            String result = "";

            result += "\t" + Date + "={\r\n";

            foreach (KeyValuePair<String, object> pair in Events)
            {
                if (pair.Value is Int32)
                {
                    result += "\t\t" + pair.Key + "=" + pair.Value.ToString() + "\r\n";
                }
                else
                {
                    String value = pair.Value as String;
                    result += "\t\t" + pair.Key + "=\"" + value + "\"\r\n";
                }

            }

            result += "\t}\r\n";

            return result;
        }

        public override string ToString()
        {
            String result = "";

            result += Date + ": ";

            int count = 0;
            foreach (KeyValuePair<String, object> pair in Events)
            {
                if (count >= 1)
                    result += ", ";

                if(pair.Value is Int32)
                {
                    result += "" + pair.Key + "=" + pair.Value.ToString();
                }
                else
                {
                    String value = pair.Value as String;
                    result += "" + pair.Key + "=\"" + value + "\"";
                }

                count++;
            }

            return result;
        }
    }
}
