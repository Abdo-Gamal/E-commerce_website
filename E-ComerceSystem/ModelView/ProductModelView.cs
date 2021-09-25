using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_ComerceSystem.ModelView
{
    public class ProductModelView
    {
   
        public int ID { get; set; }
        [
            Required(ErrorMessage = "Product name required "),
            Display(Name = "Product name"),
           // MinLength(1),
            RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
             ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=40")
            ]
        public string Name { get; set; }
        [
            Required(ErrorMessage = "Product Cost required "),
            Display(Name="Product Cost"),
             RegularExpression(@"^[0-9]{1,}$",

             ErrorMessage = "you can use number only and min length =1 "),
              //Column(TypeName = "decimal(18, 2)"),
             DataType(DataType.Currency),
            ]
        public double Cost { get; set; }
        [
            Required(ErrorMessage = "Product price required "),
            Display(Name = "Product price"),
             RegularExpression(@"^[0-9]{1,}$",
             ErrorMessage = "you can use number only and min length =1 "),
            //  Column(TypeName = "decimal(18, 2)"),
             DataType(DataType.Currency),
            ]
        public double price { get; set; }

             [
          
          Display(Name = "Product Image"),
         
          ]
        public string Image { get; set; }

        [NotMapped, Required(ErrorMessage = "Product Image required "),]
        public HttpPostedFileBase ImageFile { get; set; }

           [
            //Required(ErrorMessage = "Product Amount required "),
            Display(Name = "Product Amount"),
             RegularExpression(@"^[0-9]{1,}$",
             ErrorMessage = "you can use number only and min length =1 ")
            ]
        public int Amount { get; set; }
        [
         Required(ErrorMessage = "Totle Amount required "),
          Display(Name = "Product total Amount"),
         RegularExpression(@"^[0-9]{1,}$",
          ErrorMessage = "you can use number only and min length =1 ")
        ]
        public int TotleAmount { get; set; }// not undestand
        [
           Required(ErrorMessage = "Product Provider Name required "),
           Display(Name = "Product Provider Name"),
           MinLength(1),
           RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
            ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=100")
           ]
        public string ProviderName { get; set; }
        [
         Required(ErrorMessage = "Product Description required "),
         Display(Name = "Product Description"),
         MinLength(1),
         RegularExpression(@"^[a-zA-Z''-'\s]{1,9000}$",
          ErrorMessage = "you can use Characters, '-' only \n MinLength=1 \n, maxLength=9000")
         ]
        public string Description { get; set; }

        [
         Required(ErrorMessage = "Product Description required "),
         Display(Name = "Product Category"),
         ]
        public int Category_id { get; set; }

        [
        //Required(ErrorMessage = "Product Description required "),
        Display(Name = "Product Category"),
        ]
        public string CategoryType { get; set; }
        
    }
}