using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Data.Validation
{
    public abstract class ValidatableBase : IValidatable
    {
        public ILookup<string,string> Validate()
        {
            return Validate(this);
        }

        private ILookup<string, string> Validate(object value, string prefix = null)
        {
            var results = new List<ValidationError>();
            var properties = value.GetType().GetProperties();
            foreach (var Property in properties)
            {
                var validationAttributes = Property.GetCustomAttributes(typeof(ValidationAttribute), true).Select(va => (ValidationAttribute)va).ToArray();
                var currentValue = Property.GetValue(value);
                foreach (var validationAttribute in validationAttributes)
                {
                    var validationResults = validationAttribute.GetValidationResult(currentValue, new ValidationContext(value) { MemberName = Property.Name });
                    if (validationResults != null)
                        results.Add(new ValidationError()
                        {
                            Key = (prefix ?? string.Empty) + string.Join(",", validationResults.MemberNames),
                            ErrorMessage = validationResults.ErrorMessage
                        });
                }
                if (currentValue != null && !Property.PropertyType.IsValueType && !(currentValue is string))
                {
                    if (Property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                    {
                        var index = 0;
                        foreach (var item in currentValue as IEnumerable)
                        {
                            var subResults = Validate(item, (prefix ?? string.Empty) + Property.Name + "[" + index + "]" + ".");
                            if (subResults != null)
                                results.AddRange(subResults.SelectMany(sr => sr.Select(sr2 => new ValidationError() { Key = sr.Key, ErrorMessage = sr2 })));
                            index++;
                        }
                    }
                    else
                    {
                        var subResults = Validate(currentValue, (prefix ?? string.Empty) + Property.Name + ".");
                        if (subResults != null)
                            results.AddRange(subResults.SelectMany(sr => sr.Select(sr2 => new ValidationError() { Key = sr.Key, ErrorMessage = sr2 })));
                    }
                }
            }

            return results.Any() ? results.ToLookup(r => r.Key, r => r.ErrorMessage) : null;
        }
    }
}
