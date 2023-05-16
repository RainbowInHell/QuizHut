namespace QuizHut.DLL.Entities
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseEntity<TKey>
    {
        public BaseEntity()
        {
            CreatedOn = DateTime.Now;
        }

        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}