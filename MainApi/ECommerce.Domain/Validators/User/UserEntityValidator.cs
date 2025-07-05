using ECommerce.Domain.src.Entities;
using FluentValidation;

namespace ECommerce.Domain.Validators.User
{
    public class UserEntityValidator : AbstractValidator<UserEntity>
    {
        public UserEntityValidator()
        {
            RuleFor(x => x.UserId).IsUserId();
            RuleFor(x => x.Email).IsUserEmail();
            RuleFor(x => x.Firstname).IsFirstname();
            RuleFor(x => x.PasswordHash).IsPasswordHash();
        }
    }
}
