namespace QuizHut.Infrastructure.EntityViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class GroupListViewModel : IMapFrom<Group>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int StudentsCount { get; set; }

        public int EventsCount { get; set; }

        public string CreatedOnDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public IList<StudentGroup> StudentsGroups { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Group, GroupListViewModel>()
                .ForMember(
                    x => x.StudentsCount,
                    opt => opt.MapFrom(x => x.StudentsGroups.Count))
                .ForMember(
                    x => x.EventsCount,
                    opt => opt.MapFrom(x => x.EventsGroups.Count));
        }
    }
}