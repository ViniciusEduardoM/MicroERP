using System;
using System.Collections.Generic;
using System.Text;

namespace MicroERP.API.Models
{
    public class Login
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
