using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deano.Azure.Entities
{
    public class PostEntity : TableEntity
    {
        public PostEntity(int id, string threadId)
        {
            this.RowKey = id.ToString();
            this.PartitionKey = threadId;
        }

        public PostEntity()
        {

        }

        public string Content { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
