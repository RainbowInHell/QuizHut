namespace QuizHut.Infrastructure.EntityViewModels.Results
{
    using AutoMapper;
    
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using System;

    public class ResultViewModel : IMapFrom<Result>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string StudentName { get; set; }

        public string StudentEmail { get; set; }

        public string Score { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Result, ResultViewModel>()
               .ForMember(
                   x => x.StudentName,
                   opt => opt.MapFrom(x => $"{x.Student.FirstName} {x.Student.LastName}"))
               .ForMember(
                   x => x.StudentEmail,
                   opt => opt.MapFrom(x => x.Student.Email))
               .ForMember(
                   x => x.Score,
                   opt => opt.MapFrom(x => $"{Math.Round((decimal)x.Points,2)}/{x.MaxPoints}"));
        }
    }
}