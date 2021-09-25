using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_ComerceSystem.ModelView
{
    public class FeedbackModelView
    {

        [
        Required(ErrorMessage = "Comment  required "),
        Display(Name = "Comment "),
        RegularExpression(@"^[a-zA-Z'\s]{1,100}$",
         ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=100")
        ]
        public string Comment { get; set; }

        [
        Required(ErrorMessage = "Rate  required "),
        Display(Name = " Rate "),
        RegularExpression(@"^[0-9]{1,2}$",
         ErrorMessage = " ")
        ]
        public int Rate { get; set; }

        [
           
           Display(Name = "Customer Name "),
          RegularExpression(@"^[a-zA-Z'\s]{1,100}$",
          ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=100")
      ]
        public string CustomerName { get; set; }
        
       [Required(ErrorMessage = "Rate  required ")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Customer Id  required ")]
        public string CustomerId { get; set; }
        
    }
}