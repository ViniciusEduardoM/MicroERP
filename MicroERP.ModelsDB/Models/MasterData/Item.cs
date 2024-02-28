using static MicroERP.ModelsDB.Models.Enumerables;

namespace MicroERP.ModelsDB.Models.MasterData
{
    public class Item : MasterData
    {
        public ItemType? ItemType { get; set; }
    }
}
