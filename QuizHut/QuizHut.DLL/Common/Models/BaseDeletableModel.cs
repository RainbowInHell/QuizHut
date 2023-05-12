namespace QuizHut.DLL.Common.Models
{
    using QuizHut.DLL.Common.Models.Contracts;

    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}