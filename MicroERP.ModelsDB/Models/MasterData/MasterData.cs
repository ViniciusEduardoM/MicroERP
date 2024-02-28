﻿using System.ComponentModel.DataAnnotations;

namespace MicroERP.ModelsDB.Models.MasterData
{
    public abstract class MasterData : IMicroERP
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
