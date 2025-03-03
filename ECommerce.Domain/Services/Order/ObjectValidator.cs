using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Services
{
    public interface IObjectValidator
    {
        List<ValidationResult> Validate(object obj);
    }

    public class ObjectValidator : IObjectValidator
    {
        public List<ValidationResult> Validate(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var errors = new List<ValidationResult>();

            Validator.TryValidateObject(obj, validationContext, errors, true);
            return errors;
        }
    }
}