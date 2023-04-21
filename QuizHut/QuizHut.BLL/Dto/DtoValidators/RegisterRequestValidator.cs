namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .Length(2, 20).WithMessage("Длина имени от 2 до 20 символов")
                .Matches("^[А-Я][а-я]*$").WithMessage("Имя должно начинаться с заглавной буквы и содержать только буквы");

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .Length(2, 20).WithMessage("Длина фамилии от 2 до 20 символов")
                .Matches("^[А-я][а-я]*$").WithMessage("Фамилия должно начинаться с заглавной буквы и содержать только буквы");

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Электронная почта обязательна")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Неверный формат электронной почты");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Длина пароля должна быть не менее 6 символов")
                .Matches("[A-Z]").WithMessage("Пароль должен содержать как минимум одну заглавную букву")
                .Matches("[a-z]").WithMessage("Пароль должен содержать как минимум одну строчную букву")
                .Matches("[0-9]").WithMessage("Пароль должен содержать как минимум одну цифру")
                .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен содержать как минимум один неалфавитно-цифровой символ");
        }
    }
}