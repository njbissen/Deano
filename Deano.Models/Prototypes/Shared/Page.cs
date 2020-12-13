using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deano.Models.Prototypes.Shared
{
    public class Page : Prototype
    {
        public int Value { get; set; }

        public string Name { get; set; }

        public bool Visible { get; set; }

        public Page()
        {
            Name = string.Empty;
            Visible = false;
        }

        public Page(string name, int value, bool visible)
        {
            Name = name;
            Value = value;
            Visible = visible;
        }
    }
}
