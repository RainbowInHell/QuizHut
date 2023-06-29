namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    using QuizHut.BLL.Dto.Requests;

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(request => request.Email).ValidateEmail();

            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .Length(2, 20).WithMessage("Длина имени от 2 до 20 символов")
                .Matches("^[А-Я][а-я]*$").WithMessage("Необходима первая заглавная буква и русский язык");

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .Length(2, 20).WithMessage("Длина фамилии от 2 до 20 символов")
                .Matches("^[А-Я][а-я]*$").WithMessage("Необходима первая заглавная буква и русский язык");

            RuleFor(request => request.Password).ValidatePassword();
        }
    }
}