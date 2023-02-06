using Diploma.Data.Interfaces;

namespace Diploma.Data.Models
{
    public class PreAuthorization : Payment, IRequestingBank
    {
        protected override int TRTYPE { get; } = 12;
    }
}
