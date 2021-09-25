using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ITIMVCProjectV1.Models
{
    public class SubOrder
    {
        public int ID { get; set; }

        public int Amount { get; set; }

        [ForeignKey("Order")]
        public int Order_id { get; set; }

        public Order Order { get; set; }
        [ForeignKey("Product")]
        public int Product_id { get; set; }
        public virtual Product Product { get; set; }

    }
}