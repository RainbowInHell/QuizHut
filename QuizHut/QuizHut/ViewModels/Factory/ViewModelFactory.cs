namespace QuizHut.ViewModels.Factory
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.LoginViewModels;
    using QuizHut.ViewModels.MainViewModels;

    internal class ViewModelFactory : IViewModelFactory
    {
        private readonly CreateViewModel<AuthorizationViewModel> createAuthorizationViewModel;
        private readonly CreateViewModel<StudentRegistrationViewModel> createStudentRegistrationViewModel;
        private readonly CreateViewModel<TeacherRegistrationViewModel> createTeacherRegistrationViewModel;
        private readonly CreateViewModel<ResetPasswordViewModel> createResetPasswordViewModel;

        private readonly CreateViewModel<HomeViewModel> createHomeViewModel;
        private readonly CreateViewModel<CategoriesViewModel> createCategoryViewModel;
        private readonly CreateViewModel<EventsViewModel> createEventViewModel;
        private readonly CreateViewModel<GroupsViewModel> createGroupViewModel;
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
                                            CreateViewModel<EventsViewModel> createEventViewModel, 
                                            CreateViewModel<GroupsViewModel> createGroupViewModel, 
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
            this.createEventViewModel = createEventViewModel;
            this.createGroupViewModel = createGroupViewModel;
            this.createQuizViewModel = createQuizViewModel;
            this.createResultViewModel = createResultViewModel;
            this.createStudentViewModel = createStudentViewModel;
            this.createUserProfileViewModel = createUserProfileViewModel;
        }

        public ViewModel CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Authorization:
                    return createAuthorizationViewModel();
                case ViewType.StudentRegistration:
                    return createStudentRegistrationViewModel();
                case ViewType.TeacherRegistration:
                    return createTeacherRegistrationViewModel();
                case ViewType.ResetPassword:
                    return createResetPasswordViewModel();

                case ViewType.Home:
                    return createHomeViewModel();
                case ViewType.Category:
                    return createCategoryViewModel();
                case ViewType.Event:
                    return createEventViewModel();
                case ViewType.Group:
                    return createGroupViewModel();
                case ViewType.Quiz:
                    return createQuizViewModel();
                case ViewType.Result:
                    return createResultViewModel();
                case ViewType.Student:
                    return createStudentViewModel();
                case ViewType.UserProfile:
                    return createUserProfileViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", nameof(viewType));
            }
        }
    }
}