namespace QuizHut.DataAccess.Entities
{
    using QuizHut.DataAccess.Common.Models;

    public class Password : BaseModel<int>
    {
        public string Content { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}