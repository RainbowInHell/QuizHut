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
                .Matches("^[А-Я][а-я]*$").WithMessage("Имя должно начинаться с заглавной буквы и содержать только буквы");

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .Length(2, 20).WithMessage("Длина фамилии от 2 до 20 символов")
                .Matches("^[А-я][а-я]*$").WithMessage("Фамилия должна начинаться с заглавной буквы и содержать только буквы");

            RuleFor(request => request.Password).ValidatePassword();
        }
    }
}