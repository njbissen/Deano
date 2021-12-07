using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deano.Azure.Entities
{
	public class UserEntity : TableEntity
    {
        public UserEntity(string id)
        {
            this.RowKey = id;
            this.PartitionKey = id;
        }

        public UserEntity()
		{

		}

        public string Name { get; set; }
        public string Role { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
