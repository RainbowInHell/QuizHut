namespace QuizHut.ViewModels.Factory
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.StartViewModels;
    using QuizHut.ViewModels.MainViewModels;
    using QuizHut.ViewModels.MainViewModels.GroupViewModels;
    using QuizHut.ViewModels.MainViewModels.CategoryViewModels;
    using QuizHut.ViewModels.MainViewModels.EventViewModels;
    using QuizHut.ViewModels.MainViewModels.QuizViewModels;
    using QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels;
    using QuizHut.ViewModels.MainViewModels.ResultViewModels;

    internal class ViewModelFactory : IViewModelFactory
    {
        private readonly CreateViewModel<AuthorizationViewModel> createAuthorizationViewModel;
        private readonly CreateViewModel<StudentRegistrationViewModel> createStudentRegistrationViewModel;
        private readonly CreateViewModel<TeacherRegistrationViewModel> createTeacherRegistrationViewModel;
        private readonly CreateViewModel<ResetPasswordViewModel> createResetPasswordViewModel;

        private readonly CreateViewModel<HomeViewModel> createHomeViewModel;

        private readonly CreateViewModel<CategoriesViewModel> createCategoryViewModel;
        private readonly CreateViewModel<CategoryActionsViewModel> createCategoryActionsViewModel;
        private readonly CreateViewModel<CategorySettingsViewModel> createCategorySettingsViewModel;

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
        private readonly CreateViewModel<ActiveEventsViewModel> createActiveEventsViewModel;
        private readonly CreateViewModel<EndedEventsViewModel> createEndedEventsViewModel;
        private readonly CreateViewModel<ResultsForEventViewModel> createResultsForEventViewModel;

        private readonly CreateViewModel<StudentsViewModel> createStudentViewModel;
        private readonly CreateViewModel<UserProfileViewModel> createUserProfileViewModel;

        public ViewModelFactory(CreateViewModel<AuthorizationViewModel> createAuthorizationViewModel,
                                            CreateViewModel<StudentRegistrationViewModel> createStudentRegistrationViewModel,
                                            CreateViewModel<TeacherRegistrationViewModel> createTeacherRegistrationViewModel,
                                            CreateViewModel<ResetPasswordViewModel> createResetPasswordViewModel,

                                            CreateViewModel<HomeViewModel> createHomeViewModel,

                                            CreateViewModel<CategoriesViewModel> createCategoryViewModel,
                                            CreateViewModel<CategoryActionsViewModel> createCategoryActionsViewModel,
                                            CreateViewModel<CategorySettingsViewModel> createCategorySettingsViewModel,

                                            CreateViewModel<EventsViewModel> createEventViewModel,
                                            CreateViewModel<EventActionsViewModel> createEventActionsViewModel,
                                            CreateViewModel<EventSettingsViewModel> createEventSettingsViewModel,

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
                                            CreateViewModel<ActiveEventsViewModel> createActiveEventsViewModel,
                                            CreateViewModel<EndedEventsViewModel> createEndedEventsViewModel,
                                            CreateViewModel<ResultsForEventViewModel> createResultsForEventViewModel,

                                            CreateViewModel<StudentsViewModel> createStudentViewModel,
                                            CreateViewModel<UserProfileViewModel> createUserProfileViewModel)
        {
            this.createAuthorizationViewModel = createAuthorizationViewModel;
            this.createStudentRegistrationViewModel = createStudentRegistrationViewModel;
            this.createTeacherRegistrationViewModel = createTeacherRegistrationViewModel;
            this.createResetPasswordViewModel = createResetPasswordViewModel;

            this.createHomeViewModel = createHomeViewModel;

            this.createCategoryViewModel = createCategoryViewModel;
            this.createCategoryActionsViewModel = createCategoryActionsViewModel;
            this.createCategorySettingsViewModel = createCategorySettingsViewModel;

            this.createEventViewModel = createEventViewModel;
            this.createEventActionsViewModel = createEventActionsViewModel;
            this.createEventSettingsViewModel = createEventSettingsViewModel;

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
            this.createActiveEventsViewModel = createActiveEventsViewModel;
            this.createEndedEventsViewModel = createEndedEventsViewModel;
            this.createResultsForEventViewModel = createResultsForEventViewModel;

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

                ViewType.Category => createCategoryViewModel(),
                ViewType.CategoryActions => createCategoryActionsViewModel(),
                ViewType.CategorySettings => createCategorySettingsViewModel(),

                ViewType.Event => createEventViewModel(),
                ViewType.EventActions => createEventActionsViewModel(),
                ViewType.EventSettings => createEventSettingsViewModel(),

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
                ViewType.ActiveResults => createActiveEventsViewModel(),
                ViewType.EndedResults => createEndedEventsViewModel(),
                ViewType.ResultsForEvent => createResultsForEventViewModel(),

                ViewType.Student => createStudentViewModel(),
                ViewType.UserProfile => createUserProfileViewModel(),

                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", nameof(viewType)),
            };
        }
    }
}