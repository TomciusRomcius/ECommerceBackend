using FluentValidation;

namespace ECommerce.Domain.Validators.User
{
    internal static class UserValidationRules
    {
        internal static IRuleBuilderOptions<T, string?> IsUserId<T>(this IRuleBuilder<T, string?> rule)
        {
            return rule
                .NotEmpty()
                .Length(36);
        }

        internal static IRuleBuilderOptions<T, string?> IsUserEmail<T>(this IRuleBuilder<T, string?> rule)
        {
            // TODO: check if it is an actual email
            return rule
                .NotEmpty();
        }

        internal static IRuleBuilderOptions<T, string?> IsFirstname<T>(this IRuleBuilder<T, string?> rule)
        {
            return rule
                .NotEmpty();
        }

        internal static IRuleBuilderOptions<T, string?> IsLastname<T>(this IRuleBuilder<T, string?> rule)
        {
            return rule
                .NotEmpty();
        }

        internal static IRuleBuilderOptions<T, string?> IsPasswordHash<T>(this IRuleBuilder<T, string?> rule)
        {
            // TODO: add right length
            return rule
                .NotEmpty();
        }
    }
}
