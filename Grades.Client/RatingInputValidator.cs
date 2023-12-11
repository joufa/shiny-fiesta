using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Grades
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("No value provided.");
            }

            if (!(value is string stringValue))
            {
                return new ValidationResult("The input must be a string.");
            }

            if (IsValidNumber(stringValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid input. Accepted values are single numbers from 4-10 or a decimal with values after the comma: 25, 50, 75.");
            }
        }

        private static bool IsValidNumber(string input)
        {
            input = input.Replace(',', '.');
            // Try parsing the input as a decimal
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal number))
            {
                var integerPart = (int)number;
                decimal fractionalPart = number % 1;

                bool isIntergerOk = false;

                // Check if the decimal is within the range 4-10
                if (integerPart >= 4 && integerPart <= 10)
                {
                    isIntergerOk = true;
                }

                if (isIntergerOk && fractionalPart == 0)
                {
                    return true;
                }

                if (integerPart == 10 && fractionalPart != 0)
                {
                    return false;
                }

                return isIntergerOk && fractionalPart == 0.25m || fractionalPart == 0.50m || fractionalPart == 0.75m;
            }

            return false;
        }
    }
}
