namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    public class EmailRequestValidator : AbstractValidator<EmailRequest>
    {
        public EmailRequestValidator()
        {
            RuleFor(request => request.Email).ValidateEmail();
        }
    }
}