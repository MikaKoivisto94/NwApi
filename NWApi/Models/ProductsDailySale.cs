using System;
using System.Collections.Generic;

#nullable disable

namespace NWApi.Models
{
    public partial class ProductsDailySale
    {
        public DateTime? OrderDate { get; set; }
        public string ProductName { get; set; }
        public double? DailySales { get; set; }
    }
}
