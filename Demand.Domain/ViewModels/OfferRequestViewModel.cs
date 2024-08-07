﻿using Demand.Domain.Entities.ProductCategoryEntity;
using Demand.Domain.NebimModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class OfferRequestViewModel
    {
        public long? OfferRequestId { get; set; }
        public long RequestInfoId { get; set; }
        public long DemandId { get; set; }
        public long DemandOfferId { get; set; }
        public int? NebimCategoryId { get; set; }
        public int? NebimSubCategoryId { get; set; }
        public string ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string?  Currency{ get; set; }
        public int? ProductCategoryId  { get; set; }
        public List<NebimCategoryModel>? NebimCategoryModels { get; set; }
        public List<NebimSubCategoryModel>? NebimSubCategoryModels { get; set; }
        public List<NebimProductModel>? NebimProductModels { get; set; }
        public List<ProductCategoryEntity>? ProductCategories { get; set; }
    }
}
