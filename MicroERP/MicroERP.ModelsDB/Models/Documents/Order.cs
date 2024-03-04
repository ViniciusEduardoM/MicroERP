using System;
using System.Collections.Generic;
using System.Text;

namespace MicroERP.ModelsDB.Models.Documents
{
    public class Order : Document<OrderLine>
    {
        public override ICollection<OrderLine> DocumentLines { get ; set; }
    }

    public class OrderLine : DocumentLine
    {
    }
}
