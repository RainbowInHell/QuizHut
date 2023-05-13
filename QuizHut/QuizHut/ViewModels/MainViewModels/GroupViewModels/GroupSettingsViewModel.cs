namespace QuizHut.ViewModels.MainViewModels.GroupViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class GroupSettingsViewModel : ViewModel
    {
        public GroupSettingsViewModel(IRenavigator addStudentRenavigator, IRenavigator addEventsRenavigator, IGroupSettingsTypeService groupSettingsTypeService)
        {
            NavigateAddStudentsCommand = new RenavigateCommand(addStudentRenavigator, GroupViewType.AddStudents, groupSettingsTypeService);
            NavigateAddEventsCommand = new RenavigateCommand(addEventsRenavigator, GroupViewType.AddEvents, groupSettingsTypeService);
        }

        public ICommand NavigateAddStudentsCommand { get; }

        public ICommand NavigateAddEventsCommand { get; }

    }
}