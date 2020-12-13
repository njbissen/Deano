using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using models = Deano.Data;

namespace Deano.Models.Prototypes.Users
{
	public class Forum : Deano.Models.Prototypes.Shared.Prototype
	{
		public int ForumId { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		[Required(ErrorMessage = "Please enter a Title.")]
		[DisplayName("Title")]
		public string Subject { get; set; }

		public string Tags { get; set; }

		public List<Thread> Threads { get; set; }

		public Forum()
		{
			Threads = new List<Thread>();
		}
		public Forum(Azure.Models.Forum model)
		{
			//ForumId = model.ForumId;
			Threads = new List<Thread>();
			Subject = model.Subject;
			CreatedBy = model.CreatedBy;
			CreatedDate = model.CreatedDate;
			Tags = model.Tags;
		}

		public Forum(models.Forum model)
		{
			ForumId = model.ForumId;
			Threads = new List<Thread>();
			Subject = model.Subject;
			CreatedBy = model.User.Handle;
			CreatedDate = model.CreatedDate;
			Tags = model.Tags;
		}
	}
}
