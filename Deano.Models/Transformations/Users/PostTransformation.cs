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

    public class PostTransformation
    {
        public static IEnumerable<prototypes.Users.Post> Transform(IQueryable<Azure.Models.Post> models, string tag)
        {
            IEnumerable<prototypes.Users.Post> result = (from m in models.ToList()
                                                         select new prototypes.Users.Post(m)).ToList();

            return Transformation<prototypes.Users.Post>.Transform(result, tag);
        }

        public static T Transform<T>(data.Post model) where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { model });
        }
    }
}
