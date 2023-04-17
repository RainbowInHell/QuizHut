namespace QuizHut.ViewModels
{
    using QuizHut.ViewModels.Base;

    class ResetPasswordViewModel : ViewModel
    {
        private string? email;

        private string? token;

        private string? password;

        private string? emailErrorMessage;

        private string? tokenErrorMessage;

        private string? passwordErrorMessage;

        public string? Email 
        { 
            get => email; 
            set => Set(ref  email, value); 
        }
        public string? Token 
        { 
            get => token; 
            set => Set(ref token, value);
        }
        public string? Password 
        { 
            get => password; 
            set => Set(ref password, value); 
        }
        public string? EmailErrorMessage 
        { 
            get => emailErrorMessage; 
            set => Set(ref emailErrorMessage, value); 
        }
        public string? TokenErrorMessage 
        { 
            get => tokenErrorMessage; 
            set => Set(ref tokenErrorMessage, value); 
        }
        public string? PasswordErrorMessage 
        { 
            get => passwordErrorMessage; 
            set => Set(ref passwordErrorMessage, value); 
        }
    }
}