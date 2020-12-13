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

    public class ThreadTransformation
    {
        public static IEnumerable<prototypes.Users.Thread> Transform(IQueryable<Azure.Models.Thread> models, string tag)
        {
            IEnumerable<prototypes.Users.Thread> result = (from m in models.ToList()
                                                          select new prototypes.Users.Thread(m)).ToList();

            return Transformation<prototypes.Users.Thread>.Transform(result, tag);
        }

        public static T Transform<T>(data.Thread model) where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { model });
        }
    }
}
