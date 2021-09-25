using ITIMVCProjectV1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace E_ComerceSystem.ModelView
{
    public class loginViewModel
    {

        [
        Required(ErrorMessage = "Email Address  required "),
        Display(Name = "Email Address  "),
           DataType(DataType.EmailAddress),
           
        ]
        public string Email { get; set; }
    
        [
         Required(ErrorMessage = "Password  required "),
          Display(Name = "user Password   "),
          DataType(DataType.Password),
          MinLength(5, ErrorMessage = "Min Length is 5 char"),
       ]
        public string Password { get; set; }
    }
}