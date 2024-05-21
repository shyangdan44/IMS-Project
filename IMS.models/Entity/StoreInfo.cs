﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IMS.models.Entity
{
    public class StoreInfo : BaseEntity
    {
        [Required]
        [Display(Name ="Store Name")]
        public string StoreName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
        [Required]
		[Display(Name = "Registration No")]
		public string RegistrationNo { get; set; }
        [Required]
		[Display(Name = "Pan No")]
		public string PanNo { get; set; }

        public bool IsActive { get; set; }
        public  DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set;}
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set;}
		public virtual ICollection<CategoryInfo> CategoryInfos { get; set; }
		public virtual ICollection<CustomerInfo> CustomerInfos { get; set; }
		public virtual ICollection<ProductInfo> ProductInfos { get; set; }
		public virtual ICollection<ProductInvoiceInfo> ProductInvoiceInfos { get; set; }
		public virtual ICollection<ProductRateInfo> ProductRateInfos { get; }
		public virtual ICollection<RackInfo> RackInfos { get; set; }
		public virtual ICollection<StockInfo> StockInfos { get; set; }
		public virtual ICollection<SupplierInfo> SupplierInfos { get; set; }
		public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }


	}
}
