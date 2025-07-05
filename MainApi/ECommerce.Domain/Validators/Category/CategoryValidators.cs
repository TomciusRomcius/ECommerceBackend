using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using FluentValidation;

namespace ECommerce.Domain.Validators.Category
{
    public class CategoryEntityValidators : AbstractValidator<CategoryEntity>
    {
        public CategoryEntityValidators()
        {
            RuleFor(x => x.Name).IsCategoryName();
        }
    }

    public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
    {
        public UpdateCategoryModelValidator()
        {
            RuleFor(x => x.Name).IsCategoryName().When(x => x.Name != null);
        }
    }
}
