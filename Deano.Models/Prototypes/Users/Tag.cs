using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using models = Deano.Data;

namespace Deano.Models.Prototypes.Users
{
    public class Tag : Deano.Models.Prototypes.Shared.Prototype
    {
        public string Name { get; set; }
        public int TypeId { get; set; }

        public Tag()
        {

        }

        public Tag(models.Tag model)
        {
            Name = model.Name;
            TypeId = model.TagTypeId;
        }
    }
}
