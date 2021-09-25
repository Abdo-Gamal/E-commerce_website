using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITIMVCProjectV1.Models
{
    public class Feedback
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("Product")]
        public int Product_ID { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Customer")]
        public string Customer_ID { get; set; }
        public virtual ApplicationUser Customer { get; set; }
    } 
}