namespace QuizHut.DataAccess.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySQL("server=localhost;port=3306;user=root;password=matvey2003;database=testdb");

            return new ApplicationDbContext(builder.Options);
        }
    }
}