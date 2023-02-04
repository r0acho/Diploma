namespace Diploma.Data.Models
{
    enum TRTYPE
    {
        PreAuthorization = 12,
        Abort = 22,
        Pay = 1,
        EndOfCalculation = 21,
        Return = 14,
        Reccuring = 171,
        CheckCard = 39
    }

    [Serializable]
    public class PaymentInfo
    {
        private decimal amount;
        private string currency = "RUB";
        private ulong order;
        private string desc;
        private uint terminal;
        private short trType;
        private string merch_name;
        private ulong merchant;
        private string email;
        private ulong timestamp;
        private string nonce;
        private string backref;
        private string notify_url;
        private uint recur_freq;
        private DateTime recur_exp;
        private string p_sign;

    }


}
