namespace QuizHut.BLL.Services.Contracts
{
    public interface IStudentsGroupsService
    {
        Task CreateStudentGroupAsync(string groupId, string studentId);

        Task DeleteStudentGroupAsync(string groupId, string studentId);
    }
}