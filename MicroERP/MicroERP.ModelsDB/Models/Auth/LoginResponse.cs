using System;

namespace MicroERP.ModelsDB.Models 
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public DateTime ValidTo { get; set; }

        public DateTime ValidFrom { get;set;}
    }
}
