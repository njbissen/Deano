using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deano.Azure.Entities
{
    public class ThreadEntity : TableEntity
    {
        public ThreadEntity(int id, string forumId)
        {
            this.RowKey = id.ToString();
            this.PartitionKey = forumId;
        }

        public ThreadEntity()
        {

        }

        public string Name { get; set; }
        public string Tags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
