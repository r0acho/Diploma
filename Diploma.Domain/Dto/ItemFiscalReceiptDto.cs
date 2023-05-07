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
        public int Qty { get; init; }
        public int TaxId { get; init; }
        public int PayAttribute { get; init; }
    }
}
