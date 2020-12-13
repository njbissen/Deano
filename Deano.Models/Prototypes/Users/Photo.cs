using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deano.Models.Prototypes.Users
{
    public class Photo : Deano.Models.Prototypes.Shared.Prototype
    {
        public string PhotoId { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public string Image { get; set; }

        public Photo()
        {

        }
    }
}
