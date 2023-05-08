using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Domain.Dto
{
    public class ItemFiscalReceiptDto
    {
        public string? Description { get; init; }  
        public uint Price { get; init; }
        public decimal QtyDecimal { get; init; }
        public ushort TaxId { get; init; }
        public uint PayAttribute { get; init; }
    }
}
