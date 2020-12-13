using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace AcumenRegistry.Models.Prototypes.Shared
{
    public class JsonResponse
    {
        public JsonResponse()
        {

        }
        public override string ToString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        private List<JsonMessage> _messages = new List<JsonMessage>();

        public bool Success { get; set; }

        public int Id { get; set; }

        public int[] Keys { get; set; }

        public string Area { get; set; }

        public string Role { get; set; }

        public bool Display
        {
            get
            {
                if (Messages != null)
                {
                    return Messages.Where(m => m.Display == true).Count() > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<JsonMessage> Messages
        {
            get { return _messages; }
        }

    }
}
