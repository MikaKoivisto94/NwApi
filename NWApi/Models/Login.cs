using System;
using System.Collections.Generic;

#nullable disable

namespace NWApi.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
