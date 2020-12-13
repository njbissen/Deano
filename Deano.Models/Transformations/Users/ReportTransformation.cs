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

    public class ReportTransformation
    {
        public static IEnumerable<prototypes.Users.Report> Transform(IQueryable<Deano.Azure.Models.Report> models, int? yearId, int? monthId, string tag)
        {
            IEnumerable<prototypes.Users.Report> result = (from m in models.ToList()
                                                           select new prototypes.Users.Report(m)).ToList();

            if (yearId.HasValue && monthId.HasValue && yearId.GetValueOrDefault() > 0 && monthId.GetValueOrDefault() > 0)
            {
                result = result.Where(m => m.YearId == yearId.Value && m.MonthId == monthId.Value);
            }
            return Transformation<prototypes.Users.Report>.Transform(result, tag);
        }

        public static T Transform<T>(data.Report model) where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { model });
        }
    }
}
