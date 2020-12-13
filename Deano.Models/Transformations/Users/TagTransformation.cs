using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
//using Deano.Models.Transformations.Shared;
using prototypes = Deano.Models.Prototypes;
using data = Deano.Data;
using Deano.Models.Prototypes.Shared;
using Deano.Models.Transformations.Shared;

namespace Deano.Models.Transformations.Users
{

    public class TagTransformation
    {
        public static IEnumerable<prototypes.Users.Tag> Transform(IQueryable<data.Tag> models, string tag)
        {
            IEnumerable<prototypes.Users.Tag> result = (from m in models.ToList()
                                                           select new prototypes.Users.Tag(m)).ToList();

            return Transformation<prototypes.Users.Tag>.Transform(result, tag);
        }

        public static T Transform<T>(data.Report model) where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { model });
        }
    }
}
