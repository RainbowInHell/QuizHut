namespace QuizHut.Infrastructure.Services
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;

    internal class ViewDisplayTypeService : IViewDisplayTypeService
    {
        private ViewDisplayType? currentViewDisplayType;
        public ViewDisplayType? CurrentViewDisplayType
        {
            get => currentViewDisplayType;
            set
            {
                currentViewDisplayType = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}