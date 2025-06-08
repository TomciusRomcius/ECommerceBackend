using ECommerce.Domain.Models;
using FluentValidation;

namespace ECommerce.Domain.Validators.User
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(x => x.UserId).IsUserId();
            RuleFor(x => x.Email).IsUserEmail().When(x => x.Email != null);
            RuleFor(x => x.Email).IsFirstname().When(x => x.Firstname != null);
            RuleFor(x => x.Email).IsLastname().When(x => x.Lastname != null);
            RuleFor(x => x.Email).IsPasswordHash().When(x => x.PasswordHash != null);
        }
    }
}
