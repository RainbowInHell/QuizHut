namespace QuizHut.DLL.Seeding.Contracts
{
    using QuizHut.DLL.EntityFramework;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}