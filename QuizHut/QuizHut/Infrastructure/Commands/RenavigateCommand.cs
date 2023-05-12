namespace QuizHut.Infrastructure.Commands
{
    using System;

    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Services.Contracts;

    internal class RenavigateCommand : Command
    {
        private readonly IRenavigator renavigator;

        private IGroupSettingsTypeService groupSettingsTypeService;

        private readonly GroupViewType groupViewType;

        public RenavigateCommand(IRenavigator renavigator)
        {
            this.renavigator = renavigator;
        }

        public RenavigateCommand(IRenavigator renavigator, GroupViewType groupViewType, IGroupSettingsTypeService groupSettingsTypeService)
        {
            this.renavigator = renavigator;
            this.groupViewType = groupViewType;
            this.groupSettingsTypeService = groupSettingsTypeService;
        }

        public event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            renavigator.Renavigate();

            if (groupViewType != null)
            {
                groupSettingsTypeService.GroupViewType = groupViewType;
            }
        }
    }
}