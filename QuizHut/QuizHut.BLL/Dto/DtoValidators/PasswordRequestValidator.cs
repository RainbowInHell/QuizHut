namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;
    using QuizHut.BLL.Dto.Requests;

    public class PasswordRequestValidator : AbstractValidator<PasswordRequest>
    {
        public PasswordRequestValidator()
        {
            RuleFor(request => request.Password).ValidatePassword();
        }
    }
}