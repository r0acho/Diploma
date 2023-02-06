using Diploma.Data.Enums;
using Diploma.Data.Interfaces;

namespace Diploma.Data.Models
{
    public class PreAuthorization : Payment, IRequestingBank
    {
        protected override TrType trType { get; } = TrType.PreAuthorization;
    }
}
