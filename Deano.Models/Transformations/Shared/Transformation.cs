using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deano.Models.Prototypes.Shared;
using System.Collections;

namespace Deano.Models.Transformations.Shared
{
    public class Transformation<T> where T : Prototype
    {
        public static IEnumerable Transform(IQueryable<T> models)
        {
            return Transform(models, string.Empty);
        }

        public static IEnumerable<T> Transform(IEnumerable<T> models, string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                foreach (T model in models)
                {
                    model.Tag = tag;
                }
            }
            return models;
        }

        public static object Transform(object model)
        {
            return model;
        }
    }
}
