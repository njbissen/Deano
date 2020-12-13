using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deano.Models.Prototypes.Users
{
    public class Month : Deano.Models.Prototypes.Shared.Prototype
    {
        public string Name { get; set; }
        public int MonthId { get; set; }
        public int YearId { get; set; }
    }
}
