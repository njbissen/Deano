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
//using System.Web.Mvc;

using data = Deano.Data;
using Deano.Data;

namespace Deano.Models.Prototypes.Users
{
    public class Credentials : Deano.Models.Prototypes.Shared.Prototype
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter your Username.")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your Password.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your Password.")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords are not the same.")]
        public string VerifiedPassword { get; set; }

        public Credentials()
        {

        }

        public Credentials(data.User model)
        {
            Username = model.Username;
            Password = model.Password;
            UserId = model.UserId;
        }
    }
}
