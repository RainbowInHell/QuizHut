namespace QuizHut.ViewModels.Factory
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.StartViewModels;
    using QuizHut.ViewModels.MainViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.GroupViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels.PassingQuizViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.ResultViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.CategoryViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.EventViewModels;
    using QuizHut.ViewModels.MainViewModels.TeacherPartViewModels;
    using QuizHut.ViewModels.MainViewModels.StudentPartViewModels;
    using QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels;

    internal class ViewModelFactory : IViewModelFactory
    {
        private readonly CreateViewModel<AuthorizationViewModel> createAuthorizationViewModel;
        private readonly CreateViewModel<StudentRegistrationViewModel> createStudentRegistrationViewModel;
        private readonly CreateViewModel<TeacherRegistrationViewModel> createTeacherRegistrationViewModel;
        private readonly CreateViewModel<ResetPasswordViewModel> createResetPasswordViewModel;

        private readonly CreateViewModel<HomeViewModel> createHomeViewModel;
        private readonly CreateViewModel<StudentHomeViewModel> createStudentHomeViewModel;

        private readonly CreateViewModel<CategoriesViewModel> createCategoryViewModel;
        private readonly CreateViewModel<CategoryActionsViewModel> createCategoryActionsViewModel;
        private readonly CreateViewModel<CategorySettingsViewModel> createCategorySettingsViewModel;

        private readonly CreateViewModel<StudentActiveEventsViewModel> createStudentActiveEventsViewModel;
        private readonly CreateViewModel<StudentPendingEventsViewModel> createStudentPendingEventsViewModel;
        private readonly CreateViewModel<StudentEndedEventsViewModel> createStudentEndedEventsViewModel;

        private readonly CreateViewModel<EventsViewModel> createEventViewModel;
        private readonly CreateViewModel<EventActionsViewModel> createEventActionsViewModel;
        private readonly CreateViewModel<EventSettingsViewModel> createEventSettingsViewModel;

        private readonly CreateViewModel<GroupsViewModel> createGroupViewModel;
        private readonly CreateViewModel<GroupActionsViewModel> createGroupActionsViewModel;
        private readonly CreateViewModel<GroupSettingsViewModel> createGroupSettingsViewModel;
        
        private readonly CreateViewModel<QuizzesViewModel> createQuizzesViewModel;
        private readonly CreateViewModel<AddEditQuizViewModel> createAddEditQuizViewModel;
        private readonly CreateViewModel<AddEditQuestionViewModel> createAddEditQuestionViewModel;
        private readonly CreateViewModel<QuizSettingsViewModel> createQuizSettingsViewModel;
        private readonly CreateViewModel<AddEditAnswerViewModel> createAddEditAnswerViewModel;
        private readonly CreateViewModel<StartQuizViewModel> createStartQuizViewModel;
        private readonly CreateViewModel<TakingQuizViewModel> createTakingQuizViewModel;
        private readonly CreateViewModel<EndQuizViewModel> createEndQuizViewModel;

        private readonly CreateViewModel<ResultsViewModel> createResultViewModel;
        private readonly CreateViewModel<ActiveEndedEventsViewModel> createActiveEndedEventsViewModel;
        private readonly CreateViewModel<ResultsForEventViewModel> createResultsForEventViewModel;

        private readonly CreateViewModel<OwnResultsViewModel> createOwnResultsViewModel;

        private readonly CreateViewModel<StudentsViewModel> createStudentViewModel;
        private readonly CreateViewModel<UserProfileViewModel> createUserProfileViewModel;

        public ViewModelFactory(CreateViewModel<AuthorizationViewModel> createAuthorizationViewModel,
                                            CreateViewModel<StudentRegistrationViewModel> createStudentRegistrationViewModel,
                                            CreateViewModel<TeacherRegistrationViewModel> createTeacherRegistrationViewModel,
                                            CreateViewModel<ResetPasswordViewModel> createResetPasswordViewModel,

                                            CreateViewModel<HomeViewModel> createHomeViewModel,
                                            CreateViewModel<StudentHomeViewModel> createStudentHomeViewModel,

                                            CreateViewModel<CategoriesViewModel> createCategoryViewModel,
                                            CreateViewModel<CategoryActionsViewModel> createCategoryActionsViewModel,
                                            CreateViewModel<CategorySettingsViewModel> createCategorySettingsViewModel,

                                            CreateViewModel<EventsViewModel> createEventViewModel,
                                            CreateViewModel<EventActionsViewModel> createEventActionsViewModel,
                                            CreateViewModel<EventSettingsViewModel> createEventSettingsViewModel,

                                            CreateViewModel<StudentActiveEventsViewModel> createStudentActiveEventsViewModel,
                                            CreateViewModel<StudentPendingEventsViewModel> createStudentPendingEventsViewModel,
                                            CreateViewModel<StudentEndedEventsViewModel> createStudentEndedEventsViewModel,

                                            CreateViewModel<GroupsViewModel> createGroupViewModel,
                                            CreateViewModel<GroupActionsViewModel> createGroupActionsViewModel,
                                            CreateViewModel<GroupSettingsViewModel> createGroupSettingsViewModel,

                                            CreateViewModel<QuizzesViewModel> createQuizzesViewModel,
                                            CreateViewModel<AddEditQuizViewModel> createAddEditQuizViewModel,
                                            CreateViewModel<AddEditQuestionViewModel> createAddEditQuestionViewModel,
                                            CreateViewModel<QuizSettingsViewModel> createQuizSettingsViewModel,
                                            CreateViewModel<AddEditAnswerViewModel> createAddEditAnswerViewModel,
                                            CreateViewModel<StartQuizViewModel> createStartQuizViewModel,
                                            CreateViewModel<TakingQuizViewModel> createTakingQuizViewModel,
                                            CreateViewModel<EndQuizViewModel> createEndQuizViewModel,

                                            CreateViewModel<ResultsViewModel> createResultViewModel,
                                            CreateViewModel<ActiveEndedEventsViewModel> createActiveEndedEventsViewModel,
                                            CreateViewModel<ResultsForEventViewModel> createResultsForEventViewModel,

                                            CreateViewModel<OwnResultsViewModel> createOwnResultsViewModel,

                                            CreateViewModel<StudentsViewModel> createStudentViewModel,
                                            CreateViewModel<UserProfileViewModel> createUserProfileViewModel)
        {
            this.createAuthorizationViewModel = createAuthorizationViewModel;
            this.createStudentRegistrationViewModel = createStudentRegistrationViewModel;
            this.createTeacherRegistrationViewModel = createTeacherRegistrationViewModel;
            this.createResetPasswordViewModel = createResetPasswordViewModel;

            this.createHomeViewModel = createHomeViewModel;
            this.createStudentHomeViewModel = createStudentHomeViewModel;

            this.createCategoryViewModel = createCategoryViewModel;
            this.createCategoryActionsViewModel = createCategoryActionsViewModel;
            this.createCategorySettingsViewModel = createCategorySettingsViewModel;

            this.createEventViewModel = createEventViewModel;
            this.createEventActionsViewModel = createEventActionsViewModel;
            this.createEventSettingsViewModel = createEventSettingsViewModel;

            this.createStudentActiveEventsViewModel = createStudentActiveEventsViewModel;
            this.createStudentPendingEventsViewModel = createStudentPendingEventsViewModel;
            this.createStudentEndedEventsViewModel = createStudentEndedEventsViewModel;

            this.createGroupViewModel = createGroupViewModel;
            this.createGroupActionsViewModel = createGroupActionsViewModel;
            this.createGroupSettingsViewModel = createGroupSettingsViewModel;

            this.createQuizzesViewModel = createQuizzesViewModel;
            this.createAddEditQuizViewModel = createAddEditQuizViewModel;
            this.createAddEditQuestionViewModel = createAddEditQuestionViewModel;
            this.createQuizSettingsViewModel = createQuizSettingsViewModel;
            this.createAddEditAnswerViewModel = createAddEditAnswerViewModel;
            this.createStartQuizViewModel = createStartQuizViewModel;
            this.createTakingQuizViewModel = createTakingQuizViewModel;
            this.createEndQuizViewModel = createEndQuizViewModel;

            this.createResultViewModel = createResultViewModel;
            this.createActiveEndedEventsViewModel = createActiveEndedEventsViewModel;
            this.createResultsForEventViewModel = createResultsForEventViewModel;

            this.createOwnResultsViewModel = createOwnResultsViewModel;

            this.createStudentViewModel = createStudentViewModel;
            this.createUserProfileViewModel = createUserProfileViewModel;
        }

        public ViewModel CreateViewModel(ViewType viewType)
        {
            return viewType switch
            {
                ViewType.Authorization => createAuthorizationViewModel(),
                ViewType.StudentRegistration => createStudentRegistrationViewModel(),
                ViewType.TeacherRegistration => createTeacherRegistrationViewModel(),
                ViewType.ResetPassword => createResetPasswordViewModel(),

                ViewType.Home => createHomeViewModel(),
                ViewType.StudentHome => createStudentHomeViewModel(),

                ViewType.Category => createCategoryViewModel(),
                ViewType.CategoryActions => createCategoryActionsViewModel(),
                ViewType.CategorySettings => createCategorySettingsViewModel(),

                ViewType.Event => createEventViewModel(),
                ViewType.EventActions => createEventActionsViewModel(),
                ViewType.EventSettings => createEventSettingsViewModel(),

                ViewType.StudentActiveEvents => createStudentActiveEventsViewModel(),
                ViewType.StudentPendingEvents => createStudentPendingEventsViewModel(),
                ViewType.StudentEndedEvents => createStudentEndedEventsViewModel(),

                ViewType.Group => createGroupViewModel(),
                ViewType.GroupActions => createGroupActionsViewModel(),
                ViewType.GroupSettings => createGroupSettingsViewModel(),

                ViewType.Quiz => createQuizzesViewModel(),
                ViewType.AddEditQuiz => createAddEditQuizViewModel(),
                ViewType.AddEditQuestion => createAddEditQuestionViewModel(),
                ViewType.QuizSettings => createQuizSettingsViewModel(),
                ViewType.AddEditAnswer => createAddEditAnswerViewModel(),
                ViewType.StartQuiz => createStartQuizViewModel(),
                ViewType.TakingQuiz => createTakingQuizViewModel(),
                ViewType.EndQuiz => createEndQuizViewModel(),

                ViewType.Result => createResultViewModel(),
                ViewType.ActiveEndedResults => createActiveEndedEventsViewModel(),
                ViewType.ResultsForEvent => createResultsForEventViewModel(),

                ViewType.OwnResult => createOwnResultsViewModel(),

                ViewType.Student => createStudentViewModel(),
                ViewType.UserProfile => createUserProfileViewModel(),

                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", nameof(viewType)),
            };
        }
    }
}