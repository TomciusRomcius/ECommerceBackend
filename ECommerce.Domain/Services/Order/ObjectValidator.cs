using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Services
{
    public interface IObjectValidator
    {
        IEnumerable<ResultError> Validate(object obj);
    }

    public class ObjectValidator : IObjectValidator
    {
        public IEnumerable<ResultError> Validate(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var errors = new List<ValidationResult>();

            Validator.TryValidateObject(obj, validationContext, errors, true);
            return errors.Select((error) => new ResultError(ResultErrorType.VALIDATION_ERROR, error.ErrorMessage));
        }
    }
}