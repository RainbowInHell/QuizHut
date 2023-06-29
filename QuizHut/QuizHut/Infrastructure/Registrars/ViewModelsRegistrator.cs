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
    using QuizHut.ViewModels.MainViewModels.StudentPartViewModels;
    using QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.CategoryViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.EventViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.GroupViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels.PassingQuizViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.ResultViewModels;
    using QuizHut.ViewModels.StartViewModels;

    public static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<CreateViewModel<AuthorizationViewModel>>(services => () => CreateAuthorizationViewModel(services));
            services.AddSingleton<CreateViewModel<ResetPasswordViewModel>>(services => () => CreateResetPasswordViewModel(services));
            services.AddSingleton<CreateViewModel<StudentRegistrationViewModel>>(services => () => CreateStudentRegistrationViewModel(services));
            services.AddSingleton<CreateViewModel<TeacherRegistrationViewModel>>(services => () => CreateTeacherRegistrationViewModel(services));

            services.AddSingleton<CreateViewModel<HomeViewModel>>(services => () => CreateHomeViewModel(services));
            services.AddSingleton<CreateViewModel<StudentHomeViewModel>>(services => () => CreateStudentHomeViewModel(services));

            services.AddSingleton<CreateViewModel<UserProfileViewModel>>(services => () => CreateUserProfileViewModel(services));

            services.AddSingleton<CreateViewModel<ActiveEndedEventsViewModel>>(services => () => CreateActiveEndedEventsViewModel(services));
            services.AddSingleton<CreateViewModel<ResultsViewModel>>(services => () => CreateResultsViewModel(services));
            services.AddSingleton<CreateViewModel<ResultsForEventViewModel>>(services => () => CreateResultsForEventViewModel(services));

            services.AddSingleton<CreateViewModel<OwnResultsViewModel>>(services => () => CreateOwnResultsViewModel(services));

            services.AddSingleton<CreateViewModel<EventsViewModel>>(services => () => CreateEventsViewModel(services));
            services.AddSingleton<CreateViewModel<EventActionsViewModel>>(services => () => CreateEventActionsViewModel(services));
            services.AddSingleton<CreateViewModel<EventSettingsViewModel>>(services => () => CreateEventSettingsViewModel(services));

            services.AddSingleton<CreateViewModel<StudentActiveEventsViewModel>>(services => () => CreateStudentActiveEventsViewModel(services));
            services.AddSingleton<CreateViewModel<StudentPendingEventsViewModel>>(services => () => CreateStudentPendingEventsViewModel(services));
            services.AddSingleton<CreateViewModel<StudentEndedEventsViewModel>>(services => () => CreateStudentEndedEventsViewModel(services));

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
            
            services.AddSingleton<CreateViewModel<StudentsViewModel>>(services => () => CreateStudentsViewModel(services));

            services.AddTransient<MainViewModel>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<StudentHomeViewModel>();

            services.AddTransient<UserProfileViewModel>();

            services.AddTransient<ResultsViewModel>();
            services.AddTransient<ActiveEndedEventsViewModel>();
            services.AddTransient<ResultsForEventViewModel>();

            services.AddTransient<OwnResultsViewModel>();

            services.AddTransient<EventsViewModel>();
            services.AddTransient<EventActionsViewModel>();
            services.AddTransient<EventSettingsViewModel>();

            services.AddTransient<StudentActiveEventsViewModel>();
            services.AddTransient<StudentPendingEventsViewModel>();
            services.AddTransient<StudentEndedEventsViewModel>();

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
            services.AddSingleton<ViewModelRenavigate<StudentHomeViewModel>>();

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
            services.AddSingleton<ViewModelRenavigate<ActiveEndedEventsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<ResultsForEventViewModel>>();

            services.AddSingleton<ViewModelRenavigate<CategoriesViewModel>>();
            services.AddSingleton<ViewModelRenavigate<CategoryActionsViewModel>>();
            services.AddSingleton<ViewModelRenavigate<CategorySettingsViewModel>>();

            services.AddSingleton<ViewModelRenavigate<StudentsViewModel>>();

            return services;
        }

        private static AuthorizationViewModel CreateAuthorizationViewModel(IServiceProvider services)
        {
            return new AuthorizationViewModel(
                services.GetRequiredService<IUserAccountService>(),
                services.GetRequiredService<LoginRequestValidator>(),
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<StudentRegistrationViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<TeacherRegistrationViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<ResetPasswordViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<HomeViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<StudentHomeViewModel>>());
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
                services.GetRequiredService<IUserAccountService>(),
                services.GetRequiredService<RegisterRequestValidator>(),
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
                services.GetRequiredService<IExporter>(),
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
                services.GetRequiredService<ViewModelRenavigate<StudentsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<EventActionsViewModel>>(),
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
                services.GetRequiredService<IExporter>(),
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
                services.GetRequiredService<ViewModelRenavigate<QuizSettingsViewModel>>(),
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
                services.GetRequiredService<ViewModelRenavigate<CategoryActionsViewModel>>(),
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
                services.GetRequiredService<ViewModelRenavigate<AddEditQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<GroupActionsViewModel>>(),
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
                services.GetRequiredService<ViewModelRenavigate<GroupSettingsViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static QuizzesViewModel CreateQuizzesViewModel(IServiceProvider services)
        {
            return new QuizzesViewModel(
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<ICategoriesService>(),
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<IExporter>(),
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
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<AddEditQuizViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<StartQuizViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }

        private static StartQuizViewModel CreateStartQuizViewModel(IServiceProvider services)
        {
            return new StartQuizViewModel(
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<IShuffler>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<TakingQuizViewModel>>(),
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<ViewModelRenavigate<HomeViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<StudentHomeViewModel>>());
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
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<ViewModelRenavigate<HomeViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<StudentHomeViewModel>>());
        }

        private static ResultsViewModel CreateResultsViewModel(IServiceProvider services)
        {
            return new ResultsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<ActiveEndedEventsViewModel>>(),
                services.GetRequiredService<ViewModelRenavigate<ResultsForEventViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>(),
                services.GetRequiredService<IExporter>());
        }

        private static ResultsForEventViewModel CreateResultsForEventViewModel(IServiceProvider services)
        {
            return new ResultsForEventViewModel(
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<IGroupsService>(),
                services.GetRequiredService<ISharedDataStore>());
        }        
        
        private static ActiveEndedEventsViewModel CreateActiveEndedEventsViewModel(IServiceProvider services)
        {
            return new ActiveEndedEventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<ResultsForEventViewModel>>(),
                services.GetRequiredService<IViewDisplayTypeService>());
        }       
        
        private static UserProfileViewModel CreateUserProfileViewModel(IServiceProvider services)
        {
            return new UserProfileViewModel(
                services.GetRequiredService<IUserAccountService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<IExporter>(),
                services.GetRequiredService<ViewModelRenavigate<AuthorizationViewModel>>());
        }       
        
        private static OwnResultsViewModel CreateOwnResultsViewModel(IServiceProvider services)
        {
            return new OwnResultsViewModel(
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<IExporter>());
        }       
        
        private static StudentActiveEventsViewModel CreateStudentActiveEventsViewModel(IServiceProvider services)
        {
            return new StudentActiveEventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<IExporter>());
        }

        private static StudentPendingEventsViewModel CreateStudentPendingEventsViewModel(IServiceProvider services)
        {
            return new StudentPendingEventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<IExporter>());
        }

        private static StudentEndedEventsViewModel CreateStudentEndedEventsViewModel(IServiceProvider services)
        {
            return new StudentEndedEventsViewModel(
                services.GetRequiredService<IEventsService>(),
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<IDateTimeConverter>(),
                services.GetRequiredService<IExporter>());
        }

        private static StudentHomeViewModel CreateStudentHomeViewModel(IServiceProvider services)
        {
            return new StudentHomeViewModel(
                services.GetRequiredService<IQuizzesService>(),
                services.GetRequiredService<IResultsService>(),
                services.GetRequiredService<ISharedDataStore>(),
                services.GetRequiredService<ViewModelRenavigate<StartQuizViewModel>>());
        } 
        
        private static StudentsViewModel CreateStudentsViewModel(IServiceProvider services)
        {
            return new StudentsViewModel(
                services.GetRequiredService<IStudentsService>(),
                services.GetRequiredService<IExporter>(),
                services.GetRequiredService<ISharedDataStore>());
        }
    }
}