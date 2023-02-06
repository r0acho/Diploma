using Diploma.Data.Enums;
using Diploma.Data.Interfaces;

namespace Diploma.Data.Models
{
    public class EndOfCalculation : Return, IRequestingBank
    {
        protected override TrType trType { get; } = TrType.EndOfCalculation;
    }
}
