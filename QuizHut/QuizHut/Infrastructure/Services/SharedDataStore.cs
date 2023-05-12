namespace QuizHut.Infrastructure.Services
{
    using QuizHut.Infrastructure.Services.Contracts;

    public class SharedDataStore : ISharedDataStore
    {
        public string SelectedGroupId { get; set; }
    }
}