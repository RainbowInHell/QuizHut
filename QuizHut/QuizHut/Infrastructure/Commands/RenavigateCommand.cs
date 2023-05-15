namespace QuizHut.Infrastructure.Commands
{
    using System;

    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Services.Contracts;

    internal class RenavigateCommand : Command
    {
        private readonly IRenavigator renavigator;

        private IViewDisplayTypeService viewDisplayTypeService;

        private readonly ViewDisplayType? viewDisplayType;

        public RenavigateCommand(IRenavigator renavigator)
        {
            this.renavigator = renavigator;
        }

        public RenavigateCommand(IRenavigator renavigator, ViewDisplayType viewDisplayType, IViewDisplayTypeService viewDisplayTypeService)
        {
            this.renavigator = renavigator;
            this.viewDisplayType = viewDisplayType;
            this.viewDisplayTypeService = viewDisplayTypeService;
        }

        public event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            renavigator.Renavigate();

            if (viewDisplayType != null)
            {
                viewDisplayTypeService.ViewDisplayType = viewDisplayType;
            }
        }
    }
}