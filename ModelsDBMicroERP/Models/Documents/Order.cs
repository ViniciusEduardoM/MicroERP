using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsDBMicroERP.Models.Documents
{
    public class Order : Document<OrderLine>
    {
        public override ICollection<OrderLine> DocumentLines { get ; set; }
    }

    public class OrderLine : DocumentLine
    {
    }
}
