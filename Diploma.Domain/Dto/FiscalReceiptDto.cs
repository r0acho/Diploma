using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Domain.Dto
{
    public class FiscalReceiptDto
    {
        public string PhoneOrEmail { get; init; }
        public uint TaxMode { get; init; }
        public int Password { get; init; } = 1;
        public string RequestId => Guid.NewGuid().ToString("N");
        public IEnumerable<uint> NonCash { get; init; }
        public string Device => "auto";
        public bool FullResponse { get; init; } = false;
        public string Place { get; init; }
        public IEnumerable<ItemFiscalReceiptDto> Items { get; init; }
    }
}
