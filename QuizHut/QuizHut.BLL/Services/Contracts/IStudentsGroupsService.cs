namespace QuizHut.BLL.Services.Contracts
{
    public interface IStudentsGroupsService
    {
        Task CreateStudentGroupAsync(string groupId, string studentId);

        Task DeleteAsync(string groupId, string studentId);
    }
}