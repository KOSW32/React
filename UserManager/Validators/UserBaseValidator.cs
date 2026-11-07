using FluentValidation;
using UserManager.DTOs;

namespace UserManager.Validators
{
    public abstract class UserBaseValidator<T> : AbstractValidator<T> where T : UserBaseDto
    {
        protected UserBaseValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Вкажіть прізвище та ім'я")
                .MinimumLength(2).WithMessage("Ім'я має містити щонайменше 2 символи")
                .MaximumLength(200).WithMessage("Ім'я не може бути довшим за 200 символів")
                .Matches(@"^[\p{L}'’\- ]+$").WithMessage("Ім'я може містити лише літери, пробіли, дефіс та апостроф")
                .Must(HaveAtLeastTwoWords).WithMessage("Вкажіть щонайменше прізвище та ім'я");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Вкажіть Email")
                .MaximumLength(200).WithMessage("Email не може бути довшим за 200 символів")
                .EmailAddress().WithMessage("Email має некоректний формат");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Вкажіть номер телефону")
                .Matches(@"^\+380\d{9}$").WithMessage("Телефон має бути у форматі +380XXXXXXXXX");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Вкажіть дату народження")
                .Must(date => date.Date <= DateTime.Today.AddYears(-16))
                    .WithMessage("Користувачу має бути не менше 16 років")
                .Must(date => date.Date >= DateTime.Today.AddYears(-120))
                    .WithMessage("Дата народження вказана некоректно");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Невідома роль. Доступні значення: User, Manager, Admin");
        }

        private static bool HaveAtLeastTwoWords(string fullName)
        {
            return fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2;
        }
    }
}
