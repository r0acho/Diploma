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
        public string ClientId { get; init; }
        public uint TaxMode { get; init; }
        public uint MaxDocumentsInTurn { get; init; }
        public string Group { get; init; }
        public int Password { get; init; }
        public string RequestId { get; init; }
        public (int, int, int) NonCash { get; init; }
        public string Device { get; init; }
        public bool FullResponse { get; init; }
        public string Place { get; init; }
        public List<ItemFiscalReceiptDto> Items { get; init; } = new();
    }
}
