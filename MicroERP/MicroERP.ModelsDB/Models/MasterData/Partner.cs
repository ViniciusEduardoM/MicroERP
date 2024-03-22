using System.Collections.Generic;
using static MicroERP.ModelsDB.Models.Enumerables;

namespace MicroERP.ModelsDB.Models.MasterData
{
    public class Partner : MasterData
    {
        public string Description { get; set; }
        public PartnerType PartnerType { get; set; }

        public virtual ICollection<PartnerContact> PartnerContacts { get; set; }

        public virtual ICollection<PartnerAddress> PartnerAddresses { get; set; }

    }

    public class PartnerContact : MasterData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string StreetNo { get; set; }
        public string ZipCode { get; set; }
        public string Observation { get; set; }
    }

    public class PartnerAddress : MasterData
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string StreetNo { get; set; }
        public string ZipCode { get; set; }
        public string Observation { get; set; }

    }
}
