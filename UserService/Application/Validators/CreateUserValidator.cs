using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
    public sealed class CreateUserValidator: AbstractValidator<UserRegisterDTO>
    {
        public CreateUserValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is required")
                .MinimumLength(8)
                    .WithMessage("Password must be at least 8 characters")
                .MaximumLength(16)
                    .WithMessage("Password must not exceed 16 characters");
        }
    }
}
