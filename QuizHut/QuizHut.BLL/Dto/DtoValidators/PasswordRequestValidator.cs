namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public class PasswordRequestValidator : AbstractValidator<PasswordRequest>
    {
        public PasswordRequestValidator()
        {
            RuleFor(request => request.Password).ValidatePassword();
        }
    }
}