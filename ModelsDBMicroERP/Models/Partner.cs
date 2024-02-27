using System;
using static ModelsDBMicroERP.Models.Enumerables;

namespace ModelsDBMicroERP.Models
{
    public class Partner : IMicroERP
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public PartnerType PartnerType { get; set; }

    }
}
