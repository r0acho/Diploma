using Diploma.Data.Interfaces;

namespace Diploma.Data.Models
{
    public class Return : Payment, IRequestingBank
    {
        protected override int TRTYPE { get; } = 14;
        static Return()
        {
            PSignOrder = new List<string>()
            {
                "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN", 
                "INT_REF", "TRTYPE", "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE"
            };

            RequestKeys = new List<string>()
            {
               "ORDER", "AMOUNT", "CURRENCY", "ORG_AMOUNT", "RRN", "INT_REF", "TRTYPE",
                "TERMINAL", "BACKREF", "EMAIL", "TIMESTAMP", "NONCE", "NOTIFY_URL", "P_SIGN"
            };
        }
    }
}
