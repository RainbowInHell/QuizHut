namespace QuizHut.BLL.Dto.DtoValidators
{
    using FluentValidation;

    using QuizHut.BLL.Dto.Requests;

    public class EmailRequestValidator : AbstractValidator<EmailRequest>
    {
        public EmailRequestValidator()
        {
            RuleFor(request => request.Email).ValidateEmail();
        }
    }
}