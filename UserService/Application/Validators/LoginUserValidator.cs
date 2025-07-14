using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
    public sealed class LoginUserValidator: AbstractValidator<UserLoginDTO>
    {
        public LoginUserValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}