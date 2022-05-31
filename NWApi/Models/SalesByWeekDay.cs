using System;
using System.Collections.Generic;

#nullable disable

namespace NWApi.Models
{
    public partial class SalesByWeekDay
    {
        public string Weekday { get; set; }
        public decimal? Total { get; set; }
    }
}
