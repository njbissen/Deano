using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using models = Deano.Data;

namespace Deano.Models.Prototypes.Users
{
    public class Picture : Deano.Models.Prototypes.Shared.Prototype
    {

        public string Name { get; set; }

        public string Path { get; set; }

        public string GalleryPath { get; set; }

        public string Tags { get; set; }

        public Picture()
        {
        }

        public Picture(Deano.Data.Picture model)
        {
            Name = model.Path;
            Tags = model.Tags;
        }
    }
}
