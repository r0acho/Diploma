using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Domain.Extensions
{
    public static class EntityExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairsString(this PaymentModel payment, List<string> keys)
        {
            var properties = TypeDescriptor.GetProperties(payment);
            var idnMapping = new IdnMapping();
            foreach (PropertyDescriptor prop in properties)
            {
                var displayName = prop.DisplayName;

                if (keys.Contains(displayName))
                {
                    var value = prop.GetValue(payment);
                    if (value is decimal decimalValue)
                    {
                        yield return new KeyValuePair<string, string>(displayName, decimalValue.ToString(CultureInfo.InvariantCulture));
                    }
                    else if (value is string stringValue) 
                    {
                        try
                        {
                            stringValue = idnMapping.GetAscii(stringValue);
                        }
                        catch { }
                        yield return new KeyValuePair<string, string>(displayName, stringValue);
                    }
                    else if (value is TrType trType)
                    {
                        yield return new KeyValuePair<string, string>(displayName, ((int)trType).ToString());
                    }
                    else
                        yield return new KeyValuePair<string, string>(displayName, value?.ToString()! ?? string.Empty);
                }
            }
        }
    }
}
