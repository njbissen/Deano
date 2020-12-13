using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deano.Azure.Entities
{
    public class ForumEntity : TableEntity
    {
        public ForumEntity(int id, DateTime createdDate)
        {
            this.RowKey = id.ToString();
            this.PartitionKey = createdDate.ToString();
        }

        public ForumEntity()
        {

        }

        public string Name { get; set; }
        public string Tags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
