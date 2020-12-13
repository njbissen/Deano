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

    public class UserTransformation
    {
        public static IEnumerable<prototypes.Users.User> Transform(IQueryable<data.User> models, string tag)
        {
            IEnumerable<prototypes.Users.User> result = (from m in models.ToList()
                                                         select new prototypes.Users.User(m)).ToList();

            return Transformation<prototypes.Users.User>.Transform(result, tag);
        }

        //public static prototypes.Users.User Transform(data.User model)
        //{
        //    return new prototypes.Users.User(model);
        //}

        public static T Transform<T>(data.User model) where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { model });
        }
    }
}
