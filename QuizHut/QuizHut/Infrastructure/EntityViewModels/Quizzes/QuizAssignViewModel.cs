namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class QuizAssignViewModel : IMapFrom<Quiz>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public bool IsAssigned { get; set; }
    }
}