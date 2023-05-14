namespace QuizHut.BLL.Services.Contracts
{
    public interface IStudentsService
    {
        Task<IList<T>> GetAllStudentsAsync<T>(
            string teacherId = null,
            string groupId = null,
            string searchCriteria = null,
            string searchText = null);

        Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId);

        Task<bool> AddStudentAsync(string email, string teacherId);

        Task DeleteFromTeacherListAsync(string studentId, string teacherId);
    }
}