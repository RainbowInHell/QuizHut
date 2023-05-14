namespace QuizHut.Infrastructure.Services.Contracts
{
    public interface ISharedDataStore
    {
        string SelectedGroupId { get; set; }

        public string SelectedCategoryId { get; set; }
    }
}