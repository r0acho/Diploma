using Diploma.Data.Interfaces;

namespace Diploma.Data.Models
{
    public class PreAuthorization : Payment, IRequestingBank
    {
        protected override void PrepareSendingData(IDictionary<string, object> model)
        {
            model["TRTYPE"] = 12;
        }
    }
}
