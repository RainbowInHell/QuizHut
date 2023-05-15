namespace QuizHut.Infrastructure.Services
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;

    internal class ViewDisplayTypeService : IViewDisplayTypeService
    {
        private ViewDisplayType? viewDisplayType;
        public ViewDisplayType? ViewDisplayType
        {
            get => viewDisplayType;
            set
            {
                viewDisplayType = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}