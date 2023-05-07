namespace QuizHut.ViewModels.Base
{
    using System;

    class DialogViewModel : ViewModel
    {
        public event EventHandler? DialogComplete;

        public virtual void OnDialogComplete(EventArgs e) => DialogComplete?.Invoke(this, e);
    }
}