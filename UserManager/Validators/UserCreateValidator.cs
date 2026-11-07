using FluentValidation;
using UserManager.Data;
using UserManager.DTOs;

namespace UserManager.Validators
{
    public class UserCreateValidator : UserBaseValidator<UserCreateDto>
    {
        public UserCreateValidator(UserStore store)
        {
            RuleFor(x => x.Email)
                .Must(email => !store.EmailExists(email))
                .WithMessage("Користувач з таким Email вже існує")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
