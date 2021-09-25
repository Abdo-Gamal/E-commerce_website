using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_ComerceSystem.ModelView
{
    public class CustomerAddToCardModelView
    {
       // no attribute 
        public int ProductId { get; set; }

        //[
        // Required(ErrorMessage = "Amount   required"),
        // Display(Name = "Amount "),
         
        // ]
        public int Amount { get; set; }

    }
}