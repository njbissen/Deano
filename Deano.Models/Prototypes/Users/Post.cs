using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using models = Deano.Data;
using Deano.Data;

namespace Deano.Models.Prototypes.Users
{
    public class Post : Deano.Models.Prototypes.Shared.Prototype
    {
        public int ThreadId { get; set; }

        public int PostId { get; set; }

        public int ForumId { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string Tags { get; set; }

        public string TemporaryId { get; set; }

        public List<Picture> Images { get; set; }

        [Required(ErrorMessage = "Please enter a Title.")]
        [DisplayName("Title")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter a Message.")]
        [DisplayName("Message")]
        public string Body { get; set; }

        public bool AllowEdit { get; set; }

        public bool Favorite { get; set; }

        public Post()
        {
            Images = new List<Picture>();
        }

        public Post(Azure.Models.Post model)
        {
            Images = new List<Picture>();
           // PostId = model.PostId;
           // ThreadId = model.Id;
           // ForumId = model.Thread.ForumId;
            Subject = model.Subject;
            Body = model.Body;
            CreatedBy = model.CreatedBy;
            CreatedDate = model.CreatedDate.ToString("d");
            Tags = model.Tags;
        }
            public Post(models.Post model)
        {
            Images = new List<Picture>();
            PostId = model.PostId;
            ThreadId = model.ThreadId;
            ForumId = model.Thread.ForumId;
            Subject = model.Subject;
            Body = model.Body;
            CreatedBy = model.User.Handle;
            CreatedDate = model.CreatedDate.ToString("d");
            TemporaryId = model.TemporaryId.ToString();
            Tags = model.Tags;

            if (model.PostPictures != null && model.PostPictures.Count > 0)
            {
                foreach (PostPicture pp in model.PostPictures)
                {
                    if (pp.Picture != null)
                    {
                        if (!string.IsNullOrWhiteSpace(pp.Picture.Path))
                        {
                            Images.Add(new Picture(pp.Picture));
                        }
                    }
                }
            }
        }
    }
}
