using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_ComerceSystem.ModelView
{
    public class CategoryModelView
    {
        [
          Required(ErrorMessage = "Category ID required "),
          Display(Name = " product ID "),
          RegularExpression(@"[0-9]",
          ErrorMessage = "use number only")
         ]

        public int ID { get; set; }

        [
       Required(ErrorMessage = "Category Type required "),
       Display(Name = " product Type "),
       RegularExpression(@"^[a-zA-Z'\s]{1,100}$",
        ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=100")
       ]
        [Remote(action: "CheackType",
            controller: "Category",
            AdditionalFields ="ID",
            ErrorMessage ="name eready exsit"),
            
            ]
       
        public string Type { get; set; }
    }
}