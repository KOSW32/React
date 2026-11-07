using FluentValidation;
using UserManager.Data;
using UserManager.DTOs;

namespace UserManager.Validators
{
    public class UserUpdateValidator : UserBaseValidator<UserUpdateDto>
    {
        public UserUpdateValidator(UserStore store)
        {
            RuleFor(x => x.Email)
                .Must((dto, email) => !store.EmailExists(email, dto.Id))
                .WithMessage("Користувач з таким Email вже існує")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
