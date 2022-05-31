using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWApi.Models
{
    public class LoggedUser
    {
        public string Username { get; set; }
        public int Accesslevelid { get; set; }
        public string? Token { get; set; }
    }
}
