using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsDBMicroERP.Models
{
    public class Seller : IMicroERP
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

    }
}
