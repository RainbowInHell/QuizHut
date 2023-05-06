namespace QuizHut.ViewModels.Base
{
    using System;

    class DialogViewModel : ViewModel
    {
        public event EventHandler? DialogComplete;

        protected virtual void OnDialogComplete(EventArgs e) => DialogComplete?.Invoke(this, e);
    }
}