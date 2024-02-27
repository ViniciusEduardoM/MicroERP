using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ModelsDBMicroERP.Models.Enumerables;

namespace ModelsDBMicroERP.Models
{
    public class Item : IMicroERP
    {
        [Key]
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public ItemType? ItemType { get; set; }
    }
}
