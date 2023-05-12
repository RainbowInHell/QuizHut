namespace QuizHut.DLL.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    using QuizHut.DLL.Common.Models.Contracts;

    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}