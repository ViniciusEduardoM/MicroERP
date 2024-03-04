using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MicroERP.ModelsDB.Models.MasterData.Users
{
    public class User : MasterData
    {
        public string Email { get; set; }

        public string Password { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

    }
}
