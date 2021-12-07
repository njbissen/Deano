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
    [MetadataType(typeof(UserMetadata))]
    public partial class User : Deano.Models.Prototypes.Shared.Prototype
    {
        public User()
        {

        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Handle { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

        public User(Azure.Models.User model)
        {
            Phone = model.PhoneNumber;
            Username = model.Name;
            Password = model.Password;
            //Handle = model.Handle;
           // UserId = model.Id;
           // CreatedDate = model.CreatedDate;
        }
        public User(data.User model)
        {
            Phone = model.Phone;
            Username = model.Username;
            Password = model.Password;
            Handle = model.Handle;
            UserId = model.UserId;
            CreatedDate = model.CreatedDate;
        }

        public string VerifiedPassword { get; set; }
    }

    public partial class UserMetadata
    {
        [Required(ErrorMessage = "Please enter your Username.")]
        [DisplayName("Username")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Please enter your Screen Name.")]
        [DisplayName("Handle")]
        public String Handle { get; set; }
        
        [Required(ErrorMessage = "Please enter your Password.")]
        [DisplayName("Password")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Please confirm your Password.")]
        [DisplayName("VerifiedPassword")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public String VerifiedPassword { get; set; }
    }
}
