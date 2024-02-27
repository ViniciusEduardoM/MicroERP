using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDBMicroERP.Models.Documents
{
    public class Document<TLine> : IMicroERP
    {
        public int Id { get; set; }

        public string Number { get; set; }

        [ForeignKey("PartnerId")]
        public Partner Partner { get; set; }

        public virtual ICollection<TLine> DocumentLines { get; set; }
    }

    public class DocumentLine
    {
        public int DocumentId { get; set; }

        public int Line { get; set; }

        public Item Item { get; set; }
        public double UnitPrice { get; set; }

        public double TotalLine { get; set; }
    }
}
