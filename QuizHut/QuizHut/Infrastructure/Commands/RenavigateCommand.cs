using QuizHut.Infrastructure.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuizHut.Infrastructure.Commands
{
    internal class RenavigateCommand : ICommand
    {
        private readonly IRenavigator renavigator;

        public RenavigateCommand(IRenavigator renavigator)
        {
            this.renavigator = renavigator;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            renavigator.Renavigate();
        }
    }
}
