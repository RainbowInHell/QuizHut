namespace QuizHut.Infrastructure.Registrars
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Services;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Factory;
    using QuizHut.ViewModels.MainViewModels;
    using QuizHut.ViewModels.MainViewModels.CategoryViewModels;
    using QuizHut.ViewModels.MainViewModels.EventViewModels;
    using QuizHut.ViewModels.MainViewModels.GroupViewModels;
    using QuizHut.ViewModels.MainViewModels.QuizViewModels;
    using QuizHut.ViewModels.StartViewModels;
    using QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels;
    using QuizHut.ViewModels.MainViewModels.ResultViewModels;

    public static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<CreateViewModel<AuthorizationViewModel>>(services => () => CreateAuthorizationViewModel(services));
            services.AddSingleton<CreateViewModel<ResetPasswordViewModel>>(services => () => CreateResetPasswordViewModel(services));
            services.AddSingleton<CreateViewModel<StudentRegistrationViewModel>>(services => () => CreateStudentRegistrationViewModel(services));
            services.AddSingleton<CreateViewModel<TeacherRegistrationViewModel>>(services => () => CreateTeacherRegistrationViewModel(services));

            services.AddSingleton<CreateViewModel<HomeViewModel>>(services => () => CreateHomeViewModel(services));
            services.AddSingleton<CreateViewModel<UserProfileViewModel>>(services => () => services.GetRequiredService<UserProfileViewModel>());

            services.AddSingleton<CreateViewModel<ResultsViewModel>>(services => () => CreateResultsViewModel(services));
            services.AddSingleton<CreateViewModel<ActiveEventsViewModel>>(services => () => CreateActiveEventsViewModel(services));
            services.AddSingleton<CreateViewModel<EndedEventsViewModel>>(services => () => CreateEndedEventsViewModel(services));
            services.AddSingleton<CreateViewModel<ResultsForEventViewModel>>(services => () => CreateResultsForEventViewModel(services));

            services.AddSingleton<CreateViewModel<EventsViewModel>>(services => () => CreateEventsViewModel(services));
            services.AddSingleton<CreateViewModel<EventActionsViewModel>>(services => () => CreateEventActionsViewModel(services));
            services.AddSingleton<CreateViewModel<EventSettingsViewModel>>(services => () => CreateEventSettingsViewModel(services));

            services.AddSingleton<CreateViewModel<GroupsViewModel>>(services => () => CreateGroupsViewModel(services));
            services.AddSingleton<CreateViewModel<GroupActionsViewModel>>(services => () => CreateGroupActionsViewModel(services));
            services.AddSingleton<CreateViewModel<GroupSettingsViewModel>>(services => () => CreateGroupSettingsViewModel(services));

            services.AddSingleton<CreateViewModel<CategoriesViewModel>>(services => () => CreateCategoriesViewModel(services));
            services.AddSingleton<CreateViewModel<CategoryActionsViewModel>>(services => () => CreateCategoryActionsViewModel(services));
            services.AddSingleton<CreateViewModel<CategorySettingsViewModel>>(services => () => CreateCategorySettingsViewModel(services));

            services.AddSingleton<CreateViewModel<QuizzesViewModel>>(services => () => CreateQuizzesViewModel(services));
            services.AddSingleton<CreateViewModel<AddEditQuizViewModel>>(services => () => CreateAddEditQuizViewModel(services));
            services.AddSingleton<CreateViewModel<AddEditQuestionViewModel>>(services => () => CreateAddEditQuestionViewModel(services));
            services.AddSingleton<CreateViewModel<QuizSettingsViewModel>>(services => () => CreateQuizSettingsViewModel(services));
            services.AddSingleton<CreateViewModel<AddEditAnswerViewModel>>(services => () => CreateAddEditAnswerViewModel(services));
            services.AddSingleton<CreateViewModel<StartQuizViewModel>>(services => () => CreateStartQuizViewModel(services));
            services.AddSingleton<CreateViewModel<TakingQuizViewModel>>(services => () => CreateTakingQuizViewModel(services));
            services.AddSingleton<CreateViewModel<EndQuizViewModel>>(services => () => CreateEndQuizViewModel(services));
            
            services.AddSingleton<CreateViewModel<StudentsViewModel>>(services => () => services.GetRequiredService<StudentsViewModel>());

            services.AddTransient<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<UserProfileViewModel>();

            services.AddTransient<ResultsViewModel>();
            services.AddTransient<ActiveEventsViewModel>();
            services.AddTransient<EndedEventsViewModel>();
            services.AddTransient<ResultsForEventViewModel>();

            services.AddTransient<EventsViewModel>();
            services.AddTransient<EventActionsViewModel>();
            services.AddTransient<EventSettingsViewModel>();

            services.AddTransient<GroupsViewModel>();
            services.AddTransient<GroupActionsViewModel>();
            services.AddTransient<GroupSettingsViewModel>();

            services.AddTransient<CategoriesViewModel>();
            services.AddTransient<CategoryActionsViewModel>();
            services.AddTransient<CategorySettingsViewModel>();

            services.AddTransient<QuizzesViewModel>();
            services.AddTransient<AddEditQuizViewModel>();
            services.AddTransient<AddEditQuestionViewModel>();
            services.AddTransient<QuizSettingsViewModel>();
            services.AddTransient<AddEditAnswerViewModel>();
            services.AddTransient<StartQuizViewModel>();
            services.AddTransient<TakingQuizViewModel>();
            services.AddTransient<EndQuizViewModel>();
            
            services.AddTransient<StudentsViewModel>();

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();

            services.AddSingleton<ViewModelRenavigate<AuthorizationViewModel>>();
            services.AddSingleton<ViewModelRenavigate<StudentRegistrationViewModel>>();
            services.AddSingleton<ViewModelRenavigate<TeacherRegistrationViewModel>>();
            services.AddSingleton<ViewModelRenavigate<ResetPasswordViewModel>>();
            services.AddSingleton<ViewModelRenavigate<HomeViewModel>>();

            services.AddSingleton<ViewModelRenavigate<EventsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<EventActionsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<EventSettingsViewModel>>();

            services.AddSingleton<ViewModelRenavigate<GroupsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<GroupActionsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<GroupSettingsViewModel>>();

            services.AddSingleton<ViewModelRenavigate<QuizzesViewModel>>();
            services.AddSingleton<ViewModelRenavigate<AddEditQuizViewModel>>();
            services.AddSingleton<ViewModelRenavigate<AddEditQuestionViewModel>>();
            services.AddSingleton<ViewModelRenavigate<QuizSettingsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<AddEditAnswerViewModel>>();
            services.AddSingleton<ViewModelRenavigate<StartQuizViewModel>>();
            services.AddSingleton<ViewModelRenavigate<TakingQuizViewModel>>();
            services.AddSingleton<ViewModelRenavigate<EndQuizViewModel>>();

            services.AddSingleton<ViewModelRenavigate<ResultsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<ActiveEventsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<EndedEventsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<ResultsForEventViewModel>>();

            services.AddSingleton<ViewModelRenavigate<CategoriesViewModel>>();
            services.AddSingleton<ViewModelRenavigate<CategoryActionsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<CategorySettingsViewModel>>();

            return services;
        }

        private static AuthorizationViewModel CreateAuthorizationViewModel(IServiceProvider services)
        {
            return new AuthorizationViewModel(
                services.GetRequiredService<IUserAccountService>(),
                services.GetRequiredService<LoginRequestValidator>(),
                services.GetRequiredService<ViewModelRenavigate<StudentRegistrationViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<TeacherRegistrationViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<ResetPasswordViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<HomeViewModel>>());
        }

        private static ResetPasswordViewModel CreateResetPasswordViewModel(IServiceProvider services)
        {
            return new ResetPasswordViewModel(
                services.GetRequiredService<IUserAccountService>(),
                services.GetRequiredService<EmailRequestValidator>(),
                services.GetRequiredService<PasswordRequestValidator>(),
                services.GetRequiredService<ViewModelRenavigate<AuthorizationViewModel>>());
        }

        private static TeacherRegistrationViewModel CreateTeacherRegistrationViewModel(IServiceProvider services)
        {
            return new TeacherRegistrationViewModel(
                services.GetRequiredService<ViewModelRenavigate<AuthorizationViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<StudentRegistrationViewModel>>());
        }

        private static StudentRegistrationViewModel CreateStudentRegistrationViewModel(IServiceProvider services)
        {
            return new StudentRegistrationViewModel(
                services.GetRequiredService<IUserAccountService>(),
                services.GetRequiredService<RegisterRequestValidator>(),
                services.GetRequiredService<ViewModelRenavigate<AuthorizationViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<TeacherRegistrationViewModel>>());
        }

        private static GroupsViewModel CreateGroupsViewModel(IServiceProvider services)
        {
            return new GroupsViewModel(
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<GroupActionsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<GroupSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static GroupActionsViewModel CreateGroupActionsViewModel(IServiceProvider services)
        {
            return new GroupActionsViewModel(
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IStudentsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<GroupsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<GroupSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static GroupSettingsViewModel CreateGroupSettingsViewModel(IServiceProvider services)
        {
            return new GroupSettingsViewModel(
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IStudentsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<GroupActionsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<GroupActionsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static CategoriesViewModel CreateCategoriesViewModel(IServiceProvider services)
        {
            return new CategoriesViewModel(
                services.GetRequiredService<ICategoriesService>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<CategoryActionsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<CategorySettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static CategorySettingsViewModel CreateCategorySettingsViewModel(IServiceProvider services)
        {
            return new CategorySettingsViewModel(
                services.GetRequiredService<ICategoriesService>(),
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<CategoryActionsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static CategoryActionsViewModel CreateCategoryActionsViewModel(IServiceProvider services)
        {
            return new CategoryActionsViewModel(
                services.GetRequiredService<ICategoriesService>(),
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<CategoriesViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<CategorySettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static EventsViewModel CreateEventsViewModel(IServiceProvider services)
        {
            return new EventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<ViewModelRenavigate<EventActionsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<EventSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>(),
                services.GetRequiredService<IExporter>());
        }

        private static EventActionsViewModel CreateEventActionsViewModel(IServiceProvider services)
        {
            return new EventActionsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<EventsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<EventSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static EventSettingsViewModel CreateEventSettingsViewModel(IServiceProvider services)
        {
            return new EventSettingsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<EventActionsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<EventActionsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<QuizSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static QuizzesViewModel CreateQuizzesViewModel(IServiceProvider services)
        {
            return new QuizzesViewModel(
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<ICategoriesService>(),
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuestionViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<QuizSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static AddEditQuizViewModel CreateAddEditQuizViewModel(IServiceProvider services)
        {
            return new AddEditQuizViewModel(
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<QuizzesViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuestionViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static AddEditQuestionViewModel CreateAddEditQuestionViewModel(IServiceProvider services)
        {
            return new AddEditQuestionViewModel(
                services.GetRequiredService<IQuestionsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<QuizzesViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<QuizSettingsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditAnswerViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static AddEditAnswerViewModel CreateAddEditAnswerViewModel(IServiceProvider services)
        {
            return new AddEditAnswerViewModel(
                services.GetRequiredService<IAnswersService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditAnswerViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<QuizSettingsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuestionViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static QuizSettingsViewModel CreateQuizSettingsViewModel(IServiceProvider services)
        {
            return new QuizSettingsViewModel(
                services.GetRequiredService<IQuestionsService>(),
                services.GetRequiredService<IAnswersService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuestionViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditAnswerViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuestionViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditAnswerViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<IShuffler>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<StartQuizViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static StartQuizViewModel CreateStartQuizViewModel(IServiceProvider services)
        {
            return new StartQuizViewModel(
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<TakingQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<HomeViewModel>>());
        }

        private static TakingQuizViewModel CreateTakingQuizViewModel(IServiceProvider services)
        {
            return new TakingQuizViewModel(
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<TakingQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<EndQuizViewModel>>());
        }

        private static EndQuizViewModel CreateEndQuizViewModel(IServiceProvider services)
        {
            return new EndQuizViewModel(
                services.GetRequiredService<IQuestionsService>(),
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<IResultHelper>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<HomeViewModel>>());
        }

        private static ResultsViewModel CreateResultsViewModel(IServiceProvider services)
        {
            return new ResultsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<ActiveEventsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<EndedEventsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<ResultsForEventViewModel>>());
        }

        private static ActiveEventsViewModel CreateActiveEventsViewModel(IServiceProvider services)
        {
            return new ActiveEventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>());
        }

        private static EndedEventsViewModel CreateEndedEventsViewModel(IServiceProvider services)
        {
            return new EndedEventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>());
        }

        private static ResultsForEventViewModel CreateResultsForEventViewModel(IServiceProvider services)
        {
            return new ResultsForEventViewModel(
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<ISharedDataStore>());
        }
    }
}