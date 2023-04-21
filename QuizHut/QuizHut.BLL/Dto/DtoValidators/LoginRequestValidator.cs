namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Электронная почта обязательна")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Неверный формат электронной почты");

            RuleFor(loginRequest => loginRequest.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Длина пароля должна быть не менее 6 символов");
        }
    }
}