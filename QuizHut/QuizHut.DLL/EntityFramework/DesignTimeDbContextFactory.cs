namespace QuizHut.DLL.EntityFramework
{
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            const string QH_DATABASE_CONNECTION_STRING = "QH_DATABASE_CONNECTION_STRING";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = Environment.GetEnvironmentVariable(QH_DATABASE_CONNECTION_STRING, EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"The {QH_DATABASE_CONNECTION_STRING} environment variable is not set.");
            }

            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(builder.Options);
        }
    }
}