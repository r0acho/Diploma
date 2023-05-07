using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Infrastructure.Implementations
{
    public class RecurPaymentsRepository : IDictBaseRepository<RecurringPaymentModel>
    {
        private readonly Dictionary<ulong, RecurringPaymentModel> _recurPaymentsRepository = new();

        public bool Add(ulong id, RecurringPaymentModel? value)
        {
            return _recurPaymentsRepository.TryAdd(id, value!);
        }

        public bool Contains(ulong id)
        {
            throw new NotImplementedException();
        }

        public RecurringPaymentModel Get(ulong id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ulong id)
        {
            throw new NotImplementedException();
        }
    }
}
