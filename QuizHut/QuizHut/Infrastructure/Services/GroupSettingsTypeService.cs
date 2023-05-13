namespace QuizHut.Infrastructure.Services
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;

    internal class GroupSettingsTypeService : IGroupSettingsTypeService
    {
        private GroupViewType? groupViewType;
        public GroupViewType? GroupViewType
        {
            get => groupViewType;
            set
            {
                groupViewType = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}