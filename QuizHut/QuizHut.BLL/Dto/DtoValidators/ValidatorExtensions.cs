namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Длина пароля должна быть не менее 6 символов")
                .Matches("[0-9]").WithMessage("Пароль должен содержать как минимум одну цифру");
        }

        public static IRuleBuilder<T, string> ValidateEmail<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Электронная почта обязательна")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Неверный формат электронной почты");
        }
    }
}