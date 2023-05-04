using QuizHut.Infrastructure.Services.Contracts;
using QuizHut.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizHut.ViewModels.Factory
{
    internal interface ISimpleTraderViewModelFactory
    {
        ViewModel CreateViewModel(ViewType viewType);
    }
}
