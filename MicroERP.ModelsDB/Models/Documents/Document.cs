using MicroERP.ModelsDB.Models.MasterData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroERP.ModelsDB.Models.Documents
{
    public class Document<TLine> : IMicroERP where TLine : DocumentLine
    {
        public int Id { get; set; }
        public string Number { get; set; }
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }
        public virtual ICollection<TLine> DocumentLines { get; set; }
    }

    public class DocumentLine
    {
        public int DocumentId { get; set; }
        public int Line { get; set; }
        public virtual Item Item { get; set; }
        public double UnitPrice { get; set; }
        public double TotalLine { get; set; }
    }
}
