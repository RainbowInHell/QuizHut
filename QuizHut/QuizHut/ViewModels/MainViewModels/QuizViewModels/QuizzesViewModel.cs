using QuizHut.BLL.Helpers;
using QuizHut.BLL.Services;
using QuizHut.Infrastructure.Commands.Base.Contracts;
using QuizHut.Infrastructure.EntityViewModels.Events;
using System.Threading.Tasks;

namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class QuizzesViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Викторины";
        public static IconChar IconChar { get; } = IconChar.FolderOpen;

        public Dictionary<string, string> SearchCriteriasInEnglish => new()
        {
            { "Название", "FullName" },
            { "Назначен", "FirstName" },
            { "Не назначен", "LastName" }
        };

        private readonly IQuizzesService quizzesService;

        private readonly ICategoriesService categoriesService;

        private readonly IDateTimeConverter dateTimeConverter;

        private readonly ISharedDataStore sharedDataStore;

        public QuizzesViewModel(
            IQuizzesService quizzesService,
            ICategoriesService categoriesService,
            IDateTimeConverter dateTimeConverter,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizRenavigator,
            IRenavigator editQuizRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.quizzesService = quizzesService;
            this.categoriesService = categoriesService;
            this.dateTimeConverter = dateTimeConverter;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuizCommand = new RenavigateCommand(addQuizRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditQuizCommand = new RenavigateCommand(editQuizRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
            DeleteQuizCommandAsync = new ActionCommandAsync(OnDeleteQuizCommandExecutedAsync, CanDeleteQuizCommandExecute);
        }

        #region Fields and properties

        public ObservableCollection<QuizListViewModel> quizzes;
        public ObservableCollection<QuizListViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        public ObservableCollection<CategorySimpleViewModel> categories;
        public ObservableCollection<CategorySimpleViewModel> Categories
        {
            get => categories;
            set => Set(ref categories, value);
        }

        private QuizListViewModel selectedQuiz;
        public QuizListViewModel SelectedQuiz
        {
            get
            {
                sharedDataStore.SelectedQuizId = selectedQuiz is null ? null : selectedQuiz.Id;
                return selectedQuiz;
            }
            set => Set(ref selectedQuiz, value);
        }

        private CategorySimpleViewModel selectedCategory;
        public CategorySimpleViewModel SelectedCategory
        {
            get => selectedCategory;
            set => Set(ref selectedCategory, value);
        }

        private string searchCriteria;
        public string SearchCriteria
        {
            get => searchCriteria;
            set => Set(ref searchCriteria, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddQuizCommand { get; }

        public ICommand NavigateEditQuizCommand { get; }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();

            await LoadCategoriesData();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => true;

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            var selectedCategoryId = SelectedCategory is null ? null : SelectedCategory.Id;
            
            var searchCriteria = SearchCriteria is null ? null : SearchCriteriasInEnglish[SearchCriteria];

            await LoadQuizzesData(searchCriteria, SearchText, selectedCategoryId);

            SelectedCategory = null;
        }

        #endregion

        #region DeleteQuizCommandAsync

        public ICommandAsync DeleteQuizCommandAsync { get; }

        private bool CanDeleteQuizCommandExecute(object p) => true;

        private async Task OnDeleteQuizCommandExecutedAsync(object p)
        {
            //await quizzesService.DeleteByIdAsync(SelectedQuiz.Id);

            //await LoadQuizzesData();
        }

        #endregion

        #region CreateUpdateQuiz

        //#region CreateQuizCommandAsync

        //public ICommandAsync CreateQuizCommandAsync { get; }

        //private bool CanCreateQuizCommandExecute(object p) => true;

        //private async Task OnCreateQuizCommandExecutedAsync(object p)
        //{
        //    var quizWithSamePasswordId = await quizzesService.GetQuizIdByPasswordAsync("Тут проперти пароля вставить");

        //    if (quizWithSamePasswordId != null)
        //    {
        //        // TODO: сообщение об ошибке
        //        return;
        //    }

        //    var quizId = await quizzesService.CreateQuizAsync("имя", "описание", 69, AccountStore.CurrentAdminId, "пароль");

        //    sharedDataStore.SelectedQuizId = quizId;

        //    //навигация к созданию вопроса
        //}

        //#endregion

        //#region UpdateQuizCommandAsync

        //public ICommandAsync UpdateQuizCommandAsync { get; }

        //private bool CanUpdateQuizCommandExecute(object p) => true;

        //private async Task OnUpdateQuizCommandExecutedAsync(object p)
        //{
        //    var quizWithSamePasswordId = await quizzesService.GetQuizIdByPasswordAsync("Тут проперти пароля вставить");

        //    if (quizWithSamePasswordId != null && quizWithSamePasswordId != sharedDataStore.SelectedQuizId)
        //    {
        //        // TODO: сообщение об ошибке
        //        return;
        //    }

        //    await quizzesService.UpdateAsync("Id", "имя", "описание", 69, AccountStore.CurrentAdminId, "пароль");

        //    //навигация к шестеренке квиза
        //}

        #endregion

        #region CreateUpdateDeleteQuestion

        //#region CreateQuestionCommandAsync

        //public ICommandAsync CreateQuestionCommandAsync { get; }

        //private bool CanCreateQuestionCommandExecute(object p) => true;
        //private async Task OnCreateQuestionCommandExecutedAsync(object p)
        //{
        //    var questionId = await questionsService.CreateQuestionAsync(sharedDataStore.SelectedQuizId, "ТЕКСТ ВОПРОСА ИЗ ПРОПЕРТИ");

        //    sharedDataStore.SelectedQuestionId = questionId;

        //    //навигация к созданию ответа
        //}

        //#region UpdateQuestionCommandAsync

        //public ICommandAsync UpdateQuestionCommandAsync { get; }

        //private bool CanUpdateQuestionCommandExecute(object p) => true;

        //private async Task OnUpdateQuestionCommandExecutedAsync(object p)
        //{
        //    await questionsService.Update(sharedDataStore.SelectedQuestionId, "Новый текст");

        //    //навигация на шестеренку
        //}

        //#endregion

        //#endregion

        //#region DeleteQuestionCommandAsync

        //public ICommandAsync DeleteQuestionCommandAsync { get; }

        //private bool CanDeleteQuestionCommandExecute(object p) => true;

        //private async Task OnDeleteQuestionCommandExecutedAsync(object p)
        //{
        //    await questionsService.DeleteQuestionByIdAsync(sharedDataStore.SelectedQuestionId);
        //}

        //#endregion

        #endregion

        #region CreateUpdateDeleteAnswer

        //#region CreateAnswerCommandAsync

        //public ICommandAsync CreateAnswerCommandAsync { get; }

        //private bool CanCreateAnswerCommandExecute(object p) => true;

        //private async Task OnCreateAnswerCommandExecutedAsync(object p)
        //{
        //    await answerService.CreateAnswerAsync("текст ответа", "правильный ли вопрос", sharedDataStore.SelectedQuestionId);

        //    //навигация к созданию ответа
        //}

        //#endregion

        //#region UpdateAnswerCommandAsync

        //public ICommandAsync UpdateAnswerCommandAsync { get; }

        //private bool CanUpdateAnswerCommandExecute(object p) => true;

        //private async Task OnUpdateAnswerCommandExecutedAsync(object p)
        //{
        //    await answerService.UpdateAsync(sharedDataStore.SelectedAnswerId, "текст ответа", "правильный ли вопрос");

        //    //навигация
        //}

        //#endregion

        //#region DeleteAnswerCommandAsync

        //public ICommandAsync DeleteAnswerCommandAsync { get; }

        //private bool CanDeleteAnswerCommandExecute(object p) => true;

        //private async Task OnDeleteAnswerCommandExecutedAsync(object p)
        //{
        //    await answerService.DeleteAsync(sharedDataStore.SelectedAnswerId);

        //    //навигация
        //}

        //#endregion

        #endregion

        private async Task LoadQuizzesData(string searchCriteria = null, string searchText = null, string categoryId = null)
        {
            var quizzes = await quizzesService.GetAllQuizzesAsync<QuizListViewModel>(AccountStore.CurrentAdminId, searchCriteria, searchText, categoryId);

            foreach (var quizz in quizzes)
            {
                quizz.CreatedOnDate = dateTimeConverter.GetDate(quizz.CreatedOn);
            }

            Quizzes = new(quizzes);
        }

        private async Task LoadCategoriesData()
        {
            var categories = await categoriesService.GetAllCategories<CategorySimpleViewModel>(AccountStore.CurrentAdminId);

            Categories = new(categories);
        }
    }
}

//#region GetAllEndedEventsCommandAsync

//public ICommandAsync GetAllEndedEventsCommandAsync { get; }
//private bool CanGetAllEndedEventsCommandExecute(object p) => true;

//private async Task OnGetAllEndedEventsExecutedAsync(object p)
//{
//    await eventsService.GetAllByCreatorIdAndStatus<EventSimpleViewModel>(Status.Ended, AccountStore.CurrentAdminId, SearchText, SearchCriteria);
//}

//#endregion