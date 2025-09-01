using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Core.CustomAttributes
{
    public class RequiredInProductionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var env = (IHostEnvironment?)validationContext.GetService(typeof(IHostEnvironment));

            if (env != null && !env.IsDevelopment() && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required in production.");
            }

            return ValidationResult.Success;
        }
    }
}
