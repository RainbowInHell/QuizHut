namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(loginRequest => loginRequest.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(loginRequest => loginRequest.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 8 characters long.");
        }
    }
}