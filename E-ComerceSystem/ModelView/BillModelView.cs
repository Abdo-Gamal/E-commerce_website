using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_ComerceSystem.ModelView
{
    public class BillModelView //use
    {
        public int  SuborderId { get; set; }
        [
            Required(ErrorMessage = "Resevation Date  required"),
            Display(Name = "Resevation Date"),
            DataType(DataType.Date),
            ]
           public DateTime ResevationDate { get; set; }
            [
              Required(ErrorMessage = "Delivery Date required"),
             Display(Name = " Delivery Date "),
             DataType(DataType.Date),
             ]
        public DateTime DeliveryDate { get; set; }
           [
          Required(ErrorMessage = "Cost  required"),
           Display(Name = "Cost Cost"),
             Column(TypeName = "decimal(18, 2)"),
             DataType(DataType.Currency),
        RegularExpression(@"^[0-9]{1,}$",
            ErrorMessage = "you can use number only and min length =1 ")
           ]
        public double Cost { get; set; }
            [
           Required(ErrorMessage = "destination  required"),
            Display(Name = "destination:city,street and numberOfBulid "),
            MinLength(1),
            RegularExpression(@"^[a-z0-9''-'\s]{3,200}$",
             ErrorMessage = "you can use Characters, '-' only \n MinLength=3 \n, maxLength=200")
            ]
        public string destination { get; set; }
        //   [
        //   Required,
        //   Display(Name = "Is Confirmed"), 
        //   ]
        //public bool IsConfirmed { get; set; }

        [
          Required,
          Display(Name = "Product Name"),
          ]
        public string ProductName { get; set; }
        [
        Required,
          Display(Name = "Product Amount "),
          ]
        public int ProductAmount { get; set; }
        [
         Required,
        Display(Name = "Product Provider Name "),
        ]
        public string ProviderName { get; set; }
       
    }
}