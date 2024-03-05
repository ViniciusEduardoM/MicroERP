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

    public class Register
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class RecoverPassword
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class RenewPassword
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public int CodeRecovery { get; set; }
    }
}
