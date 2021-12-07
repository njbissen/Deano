using System;
using System.Collections.Generic;
using System.Text;

namespace Deano.Azure.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string Password { get; set; }
	}
}
