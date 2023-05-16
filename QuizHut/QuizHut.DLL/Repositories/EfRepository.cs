namespace QuizHut.DLL.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.DLL.EntityFramework;
    using QuizHut.DLL.Repositories.Contracts;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => DbSet.AsNoTracking();

        public virtual async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

        //public virtual void Update(TEntity entity) => DbSet.Update(entity);
        public virtual void Update(TEntity entity) => DbSet.Entry(entity).State = EntityState.Modified;

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

        public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context?.Dispose();
            }
        }
    }
}