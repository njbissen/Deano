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
    public class Report : Deano.Models.Prototypes.Shared.Prototype
    {
        public int UserId { get; set; }

        public int ReportId { get; set; }

        public int MonthId { get; set; }

        public int YearId { get; set; }

        public string TemporaryId { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        [Required(ErrorMessage = "Please enter a Title.")]
        [DisplayName("Title")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter a Message.")]
        [DisplayName("Message")]
        public string Body { get; set; }

        public bool Favorite { get; set; }

        public List<Picture> Images { get; set; }

        public string Tags { get; set; }

        public Report()
        {

        }
        public Report(models.Report model)
        {
            TemporaryId = model.TemporaryId.ToString();
            Images = new List<Picture>();
            ReportId = model.ReportId;
            Subject = model.Subject;
            Body = model.Body;
            CreatedBy = model.User.Handle;
            CreatedDate = model.CreatedDate.ToString("d");
            Tags = model.Tags;
            YearId = model.CreatedDate.Year;
            MonthId = model.CreatedDate.Month;

            if (model.ReportPictures != null && model.ReportPictures.Count > 0)
            {
                foreach (ReportPicture pp in model.ReportPictures)
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

        public Report(Deano.Azure.Models.Report model)
        {
           // TemporaryId = model.TemporaryId.ToString();
            Images = new List<Picture>();
           // ReportId = model.ReportId;
            Subject = model.Subject;
            Body = model.Body;
            CreatedBy = model.CreatedBy;
            CreatedDate = model.CreatedDate.ToString("d");
            Tags = model.Tags;
            YearId = model.CreatedDate.Year;
            MonthId = model.CreatedDate.Month;

            //if (model.ReportPictures != null && model.ReportPictures.Count > 0)
            //{
            //    foreach (ReportPicture pp in model.ReportPictures)
            //    {
            //        if (pp.Picture != null)
            //        {
            //            if (!string.IsNullOrWhiteSpace(pp.Picture.Path))
            //            {
            //                Images.Add(new Picture(pp.Picture));
            //            }
            //        }
            //    }
            //}
        }
    }
}
