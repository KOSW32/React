using FluentValidation;
using UserManager.Models;

namespace UserManager.Validators
{
    public class UserQueryParametersValidator : AbstractValidator<UserQueryParameters>
    {
        public UserQueryParametersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page має бути не менше 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize має бути від 1 до 100");

            RuleFor(x => x.Search)
                .MaximumLength(100).WithMessage("Search не може бути довшим за 100 символів");

            RuleFor(x => x.SortBy)
                .Must(value => UserQueryParameters.SortFields.Contains(value, StringComparer.OrdinalIgnoreCase))
                .WithMessage($"Невідоме поле сортування. Доступні значення: {string.Join(", ", UserQueryParameters.SortFields)}");

            RuleFor(x => x.SortDir)
                .Must(value => value.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            || value.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortDir має бути asc або desc");

            RuleFor(x => x.CreatedTo)
                .Must((query, createdTo) => query.CreatedFrom is null || createdTo is null || createdTo >= query.CreatedFrom)
                .WithMessage("CreatedFrom не може бути пізніше за CreatedTo");
        }
    }
}
