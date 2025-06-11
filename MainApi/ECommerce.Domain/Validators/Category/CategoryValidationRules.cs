using FluentValidation;

namespace ECommerce.Domain.Validators.Category
{
    public static class CategoryValidationRules
    {
        public static IRuleBuilderOptions<T, string?> IsCategoryName<T>(
            this IRuleBuilder<T, string?> ruleBuilder) where T : class
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Category name cannot be empty!")
                .MinimumLength(3).WithMessage("Category name be atleast 3 characters long!")
                .MaximumLength(20).WithMessage("Category name length must not exceed 20 characters!");
        }
    }
}
