namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class GroupsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Группы";
        public static IconChar IconChar { get; } = IconChar.PeopleGroup;

        public GroupsViewModel(IRenavigator createGroupRenavigator) 
        {
            NavigateCreateGroupCommand = new RenavigateCommand(createGroupRenavigator);
        }

        #region Commands

        public ICommand NavigateCreateGroupCommand { get; }

        #endregion
    }
}