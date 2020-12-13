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

    public class ForumTransformation
    {
        public static IEnumerable<prototypes.Users.Forum> Transform(IQueryable<Azure.Models.Forum> models, string tag)
        {
            IEnumerable<prototypes.Users.Forum> result = (from m in models.ToList()
                                                          select new prototypes.Users.Forum(m)).ToList();

            return Transformation<prototypes.Users.Forum>.Transform(result, tag);
        }

        public static T Transform<T>(data.Forum model) where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { model });
        }
    }
}
