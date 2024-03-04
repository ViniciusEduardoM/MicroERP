using static MicroERP.ModelsDB.Models.Enumerables;

namespace MicroERP.ModelsDB.Models.MasterData
{
    public class Partner : MasterData
    {
        public string Description { get; set; }
        public PartnerType PartnerType { get; set; }
    }
}
