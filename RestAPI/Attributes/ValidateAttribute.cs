using System.ComponentModel.DataAnnotations;

namespace RestAPI.Attributes
{
    public class ValidateAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;

        public ValidateAttribute(string dependentProperty)
        {
            _dependentProperty = dependentProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dependentValue = validationContext.ObjectType.GetProperty(_dependentProperty)?.GetValue(validationContext.ObjectInstance);

            if (dependentValue?.ToString() != value?.ToString())
            {
                return new ValidationResult(string.Empty);
            }

            return ValidationResult.Success;
        }
    }
}
