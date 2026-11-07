using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using UserManager.Data;
using UserManager.DTOs;
using UserManager.Models;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserStore _store;
        private readonly IValidator<UserCreateDto> _createValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;
        private readonly IValidator<UserQueryParameters> _queryValidator;

        public UsersController(
            UserStore store,
            IValidator<UserCreateDto> createValidator,
            IValidator<UserUpdateDto> updateValidator,
            IValidator<UserQueryParameters> queryValidator)
        {
            _store = store;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _queryValidator = queryValidator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<User>>> GetAll([FromQuery] UserQueryParameters query)
        {
            var validation = await _queryValidator.ValidateAsync(query);

            if (!validation.IsValid)
            {
                return ValidationError(validation);
            }

            var sortBy = UserQueryParameters.SortFields.First(f => f.Equals(query.SortBy, StringComparison.OrdinalIgnoreCase));
            var isDescending = query.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase);

            IQueryable<User> users = _store.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(u =>
                    u.FullName.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
            }

            if (query.Role.HasValue)
            {
                users = users.Where(u => u.Role == query.Role.Value);
            }

            if (query.IsActive.HasValue)
            {
                users = users.Where(u => u.IsActive == query.IsActive.Value);
            }

            if (query.CreatedFrom.HasValue)
            {
                users = users.Where(u => u.CreatedAt >= query.CreatedFrom.Value);
            }

            if (query.CreatedTo.HasValue)
            {
                var createdTo = query.CreatedTo.Value;

                if (createdTo.TimeOfDay == TimeSpan.Zero)
                {
                    var nextDay = createdTo.AddDays(1);
                    users = users.Where(u => u.CreatedAt < nextDay);
                }
                else
                {
                    users = users.Where(u => u.CreatedAt <= createdTo);
                }
            }

            var sorted = ApplySorting(users, sortBy, isDescending).ThenBy(u => u.Id);

            var totalCount = sorted.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

            var items = sorted
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return Ok(new PagedResult<User>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
        }

        [HttpGet("{id:int}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _store.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] UserCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                return ValidationError(validation);
            }

            var user = new User
            {
                Id = _store.Users.Count == 0 ? 1 : _store.Users.Max(u => u.Id) + 1,
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone,
                BirthDate = dto.BirthDate.Date,
                Role = dto.Role,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            _store.Users.Add(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var user = _store.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            dto.Id = id;

            var validation = await _updateValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                return ValidationError(validation);
            }

            user.FullName = dto.FullName.Trim();
            user.Email = dto.Email.Trim();
            user.Phone = dto.Phone;
            user.BirthDate = dto.BirthDate.Date;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _store.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            _store.Users.Remove(user);

            return NoContent();
        }

        private ActionResult ValidationError(ValidationResult result)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).Distinct().ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Помилки валідації"
            });
        }

        private static IOrderedQueryable<User> ApplySorting(IQueryable<User> source, string sortBy, bool descending)
        {
            return sortBy switch
            {
                "fullName" => descending ? source.OrderByDescending(u => u.FullName) : source.OrderBy(u => u.FullName),
                "email" => descending ? source.OrderByDescending(u => u.Email) : source.OrderBy(u => u.Email),
                "role" => descending ? source.OrderByDescending(u => u.Role) : source.OrderBy(u => u.Role),
                "createdAt" => descending ? source.OrderByDescending(u => u.CreatedAt) : source.OrderBy(u => u.CreatedAt),
                _ => descending ? source.OrderByDescending(u => u.Id) : source.OrderBy(u => u.Id)
            };
        }
    }
}
