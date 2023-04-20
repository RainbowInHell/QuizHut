namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(loginRequest => loginRequest.Email)
                .NotEmpty().WithMessage("Почта обязательна.")
                .EmailAddress().WithMessage("Почта должна быть допустимым адресом.");

            RuleFor(loginRequest => loginRequest.Password)
                .NotEmpty().WithMessage("Пароль обязателен.")
                .MinimumLength(6).WithMessage("Длина пароля должна быть не менее 6 символов.");
        }
    }
}