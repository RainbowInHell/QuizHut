namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;
    using QuizHut.BLL.Dto.Requests;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.Email).ValidateEmail();

            RuleFor(loginRequest => loginRequest.Password).ValidatePassword();
        }
    }
}