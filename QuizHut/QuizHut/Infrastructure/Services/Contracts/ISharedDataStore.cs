namespace QuizHut.Infrastructure.Services.Contracts
{
    public interface ISharedDataStore
    {
        string SelectedGroupId { get; set; }
    }
}