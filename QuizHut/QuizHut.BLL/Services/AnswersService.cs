namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class AnswersService : IAnswersService
    {
        private readonly IRepository<Answer> repository;

        public AnswersService(IRepository<Answer> repository)
        {
            this.repository = repository;
        }

        public async Task CreateAnswerAsync(string answerText, bool isRightAnswer, string questionId)
        {
            var answer = new Answer
            {
                Text = answerText,
                IsRightAnswer = isRightAnswer,
                QuestionId = questionId,
            };

            await repository.AddAsync(answer);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateAnswerAsync(string id, string text, bool isRightAnswer)
        {
            var answer = await repository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            answer.Text = text;
            answer.IsRightAnswer = isRightAnswer;
            repository.Update(answer);

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(string id)
        {
            var answer = await repository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            repository.Delete(answer);
            
            await repository.SaveChangesAsync();
        }
    }
}