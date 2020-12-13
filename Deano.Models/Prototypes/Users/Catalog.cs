using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deano.Models.Prototypes.Shared;

namespace Deano.Models.Prototypes.Users
{
    public class Catalog<H,I> : Deano.Models.Prototypes.Shared.Prototype
    {
       public Pager Pager { get; set; }

       public List<I> Items { get; set; }

       public H Header { get; set; }

       public Catalog()
       {
           Pager = new Pager();
           Items = new List<I>();
       }
    }
}
