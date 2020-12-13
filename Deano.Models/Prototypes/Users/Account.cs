using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Globalization;
using System.Web.Mvc;

using data = Deano.Data;
using Deano.Data;

namespace Deano.Models.Prototypes.Users
{
    public class Account : Deano.Models.Prototypes.Shared.Prototype
    {
        public Account()
        {

        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter your Phone Number.")]
        [DisplayName("Phone #")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your Screen Name.")]
        [DisplayName("Screen Name")]
        public string Handle { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address.")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        public Account(data.User model)
        {
            Phone = model.Phone;
            Handle = model.Handle;
            Email = model.Username;
            UserId = model.UserId;
        }
    }
}
