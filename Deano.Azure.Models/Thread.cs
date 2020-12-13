using System;

namespace Deano.Azure.Models
{
	public class Thread
    {

        public string Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Subject { get; set; }

        public string Tags { get; set; }

        public Thread()
        {

        }
    }
}
