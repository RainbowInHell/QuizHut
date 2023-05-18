namespace QuizHut.Infrastructure.Services.Contracts
{
    public interface ISharedDataStore
    {
        string SelectedGroupId { get; set; }

        string SelectedCategoryId { get; set; }

        string SelectedEventId { get; set; }

        string SelectedQuizId { get; set; }

        string SelectedQuestionId { get; set; }

        string SelectedAnswerId { get; set; }
    }
}