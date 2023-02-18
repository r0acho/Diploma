using Diploma.Data.Enums;
using Diploma.Data.Interfaces;

namespace Diploma.Data.Models
{
    public class Return : Payment, IRequestingBank
    {
        protected override TrType trType { get; } = TrType.Return;

        protected override List<string> RequestKeys { get; init; } = new List<string>()
        {
            "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN", "INT_REF", "TRTYPE",
            "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE", "NOTIFY_URL", "P_SIGN"
        };

        protected override List<string> PSignOrder { get; init; } = new List<string>()
        {
            "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN", 
            "INT_REF", "TRTYPE", "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE"
        };
    }
}
