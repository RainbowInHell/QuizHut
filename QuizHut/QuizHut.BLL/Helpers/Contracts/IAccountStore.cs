namespace QuizHut.BLL.Helpers.Contracts
{
    using QuizHut.DLL.Entities;

    public enum UserRole
    {
        Unauthorised,
        Student,
        Teacher
    }

    public interface IAccountStore
    {
        ApplicationUser CurrentUser { get; set; }

        UserRole CurrentUserRole { get; set; }

        event Action StateChanged;
    }
}