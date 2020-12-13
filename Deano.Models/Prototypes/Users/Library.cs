using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deano.Models.Prototypes.Users
{
    public class Library : Deano.Models.Prototypes.Shared.Prototype
    {
        public int LibraryId { get; set; }

        public string Title { get; set; }

        public List<Report> Reports { get; set; }

        public Library()
        {
            Reports = new List<Report>();
        }
    }
}
