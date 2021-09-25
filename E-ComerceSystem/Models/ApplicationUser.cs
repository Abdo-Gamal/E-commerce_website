using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ITIMVCProjectV1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Image { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

    }
}