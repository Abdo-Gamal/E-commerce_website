using ITIMVCProjectV1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace E_ComerceSystem
{
    public class ApplicationUserModelView
    {

     
        public string Id { get; set; }
        [
        Required(ErrorMessage = "User Name  required "),
        Display(Name = "User Name "),
        RegularExpression(@"^[a-zA-Z'\s]{1,100}$",
       ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=100"),
            DatabaseGenerated(DatabaseGeneratedOption.Identity),//not work
            ]

        public string UserName { get; set; }
         [
         Required(ErrorMessage = "Email Address  required "),
         Display(Name = "Email Address  "),
            DataType(DataType.EmailAddress),
            DatabaseGenerated(DatabaseGeneratedOption.Identity),
         ]
              
        //[Remote(action: "CheackEmail",
        //    controller: "Customer",
        //    AdditionalFields = "Id",
        //    ErrorMessage = "Email eready exsit"),

        //    ]
        public string Email { get; set; }
        [
          Required(ErrorMessage = "Phone Number   required "),
          Display(Name = "user Phone   "),
             DataType(DataType.PhoneNumber),
            MinLength(11),
            MaxLength(11),
          ]
        public string PhoneNumber { get; set; }
           [
            Required(ErrorMessage = "Password  required "),
             Display(Name = "user Password   "),
             DataType(DataType.Password),
             MinLength(5,ErrorMessage = "Min Length is 5 char"),
          ]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
        [
            Required(ErrorMessage = "Password Confirmed  required "),
             Display(Name = " user Password Confirmed  "),
             DataType(DataType.Password),
             MinLength(5, ErrorMessage = "Min Length is 5 char"),
          ]
        public string PasswordConfirmed { get; set; }

        [
          
            Display(Name = " user Image  "),
         ]
        public string Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

    }
}