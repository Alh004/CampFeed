using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampLib.Model
{
    public class Setting
    {

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public Setting(string key, string value, string description)
        {
            Key = key;
            Value = value;
            Description = description;
        }

        public override string ToString()
        {
            return $"{{{nameof(Key)}={Key}, {nameof(Value)}={Value}, {nameof(Description)}={Description}}}";
        }
    }
}
