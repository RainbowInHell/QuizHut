namespace QuizHut.ViewModels.Factory
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.StartViewModels;
    using QuizHut.ViewModels.MainViewModels;
    using QuizHut.ViewModels.MainViewModels.GroupViewModels;
    using QuizHut.ViewModels.MainViewModels.CategoryViewModels;

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

        private readonly CreateViewModel<GroupsViewModel> createGroupViewModel;
        private readonly CreateViewModel<GroupActionsViewModel> createGroupActionsViewModel;
        private readonly CreateViewModel<GroupSettingsViewModel> createGroupSettingsViewModel;
        
        private readonly CreateViewModel<QuizzesViewModel> createQuizViewModel;
        private readonly CreateViewModel<ResultsViewModel> createResultViewModel;
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

                                            CreateViewModel<GroupsViewModel> createGroupViewModel,
                                            CreateViewModel<GroupActionsViewModel> createGroupActionsViewModel,
                                            CreateViewModel<GroupSettingsViewModel> createGroupSettingsViewModel,

                                            CreateViewModel<QuizzesViewModel> createQuizViewModel,
                                            CreateViewModel<ResultsViewModel> createResultViewModel,
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

            this.createGroupViewModel = createGroupViewModel;
            this.createGroupActionsViewModel = createGroupActionsViewModel;
            this.createGroupSettingsViewModel = createGroupSettingsViewModel;

            this.createQuizViewModel = createQuizViewModel;
            this.createResultViewModel = createResultViewModel;
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

                ViewType.Group => createGroupViewModel(),
                ViewType.GroupActions => createGroupActionsViewModel(),
                ViewType.GroupSettings => createGroupSettingsViewModel(),

                ViewType.Quiz => createQuizViewModel(),
                ViewType.Result => createResultViewModel(),
                ViewType.Student => createStudentViewModel(),
                ViewType.UserProfile => createUserProfileViewModel(),

                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", nameof(viewType)),
            };
        }
    }
}