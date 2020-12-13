using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deano.Models.Prototypes.Users
{
    public class Album : Deano.Models.Prototypes.Shared.Prototype
    {
        public string AlbumId { get; set; }

        public string Title { get; set; }

        public List<Photo> Photos { get; set; }

        public Album()
        {
            Photos = new List<Photo>();
        }
    }
}
