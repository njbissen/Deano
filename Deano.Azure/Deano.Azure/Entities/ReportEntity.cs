using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deano.Azure.Entities
{
    public class ReportEntity : TableEntity
    {
        public ReportEntity(int id, DateTime createdDate)
        {
            this.RowKey = id.ToString();
            this.PartitionKey = createdDate.ToString();
        }

        public ReportEntity()
        {

        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
