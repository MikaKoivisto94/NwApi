using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWApi.Models
{
    public class ProductData
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "default";
        public string CategoryName { get; set; } = "default";
        public string SupplierName { get; set; } = "default";
    }
}
