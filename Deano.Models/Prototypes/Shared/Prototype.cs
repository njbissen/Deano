using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Deano.Models.Prototypes.Shared
{
    public class Prototype
    {
        public Prototype()
        {

        }

        public bool AllowBookmark { get; set; }

        public bool Selected { get; set; }

        public string Tag { get; set; }

        public override string ToString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}
