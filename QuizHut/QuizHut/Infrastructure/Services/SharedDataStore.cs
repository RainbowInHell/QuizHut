namespace QuizHut.Infrastructure.Services
{
    using QuizHut.Infrastructure.Services.Contracts;

    public class SharedDataStore : ISharedDataStore
    {
        public string SelectedGroupId { get; set; }

        public string SelectedCategoryId { get; set; }

        public string SelectedEventId { get; set; }

        public string SelectedQuizId { get; set; }

        public string SelectedQuestionId { get; set; }

        public string SelectedAnswerId { get; set; }
    }
}