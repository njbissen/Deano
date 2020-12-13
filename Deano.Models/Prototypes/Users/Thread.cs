using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using models = Deano.Data;

namespace Deano.Models.Prototypes.Users
{
    public class Thread : Deano.Models.Prototypes.Shared.Prototype
    {
        public int ForumId { get; set; }

        public int ThreadId { get; set; }

        public int Total { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string TemporaryId { get; set; }

        public string Tags { get; set; }

        [Required(ErrorMessage = "Please enter a Title.")]
        [DisplayName("Title")]
        public string Subject { get; set; }

        public List<Post> Posts { get; set; }

        [Required(ErrorMessage = "Please enter a Message.")]
        [DisplayName("Message")]
        public string Body { get; set; }

        public Thread()
        {
            Posts = new List<Post>();
        }
        public Thread(Azure.Models.Thread model)
        {
           // ForumId = model.ForumId;
            //ThreadId = model.ThreadId;
            Posts = new List<Post>();
            Subject = model.Subject;
            CreatedBy = model.CreatedBy;
            CreatedBy = string.Format("by {0} at {1}", model.CreatedBy, model.CreatedDate.ToString());
            CreatedDate = model.CreatedDate;
            //Total = model.Posts.Count;
            //models.Post lastPost = model.Posts.OrderByDescending(m => m.CreatedBy).FirstOrDefault();
            //if (lastPost != null)
            //{
            //    EditedBy = string.Format("{0} at {1}", lastPost.User.Handle, lastPost.CreatedDate.ToString());
            //}
            Tags = model.Tags;
        }
        public Thread(models.Thread model)
        {
            ForumId = model.ForumId;
            ThreadId = model.ThreadId;
            Posts = new List<Post>();
            Subject = model.Subject;
            CreatedBy = model.User.Handle;
            CreatedBy = string.Format("by {0} at {1}", model.User.Handle, model.CreatedDate.ToString());
            CreatedDate = model.CreatedDate;
            Total = model.Posts.Count;
            models.Post lastPost = model.Posts.OrderByDescending(m => m.CreatedBy).FirstOrDefault();
            if (lastPost != null)
            {
                EditedBy = string.Format("{0} at {1}", lastPost.User.Handle, lastPost.CreatedDate.ToString());
            }
            Tags = model.Tags;
        }
    }
}
