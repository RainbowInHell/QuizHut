namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class StudentsService : IStudentsService
    {
        private readonly IRepository<ApplicationUser> userRepository;

        private readonly IExpressionBuilder expressionBuilder;

        public StudentsService(
            IRepository<ApplicationUser> userRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.userRepository = userRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<IList<T>> GetAllStudentsAsync<T>(
            string teacherId = null,
            string groupId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = userRepository.AllAsNoTracking();

            if (teacherId != null)
            {
                query = query.Where(x => x.TeacherId == teacherId);
            }

            if (groupId != null)
            {
                query = query.Where(x => !x.StudentsInGroups.Select(x => x.GroupId).Contains(groupId));
            }

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<ApplicationUser>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.Where(x => !x.Roles.Any()).To<T>().ToListAsync();
        }

        public async Task<IList<T>> GetAllStudentsByGroupIdAsync<T>(string groupId)
        {
            return await userRepository
                .AllAsNoTracking()
                .Where(x => x.StudentsInGroups.Select(x => x.GroupId).Contains(groupId))
                .To<T>()
                .ToListAsync();
        }

        public async Task<bool> AddStudentAsync(string email, string teacherId)
        {
            var user = await userRepository
                .All()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

            if (user != null && user.TeacherId != teacherId)
            {
                var teacher = await userRepository
                    .All()
                    .Where(x => x.Id == teacherId)
                    .FirstOrDefaultAsync();

                user.TeacherId = teacherId;
                teacher.Students.Add(user);

                userRepository.Update(user);
                userRepository.Update(teacher);

                await userRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task DeleteStudentFromTeacherListAsync(string studentId, string teacherId)
        {
            var studentToRemove = await userRepository
                .All()
                .Where(x => x.Id == studentId)
                .FirstOrDefaultAsync();

            var teacher = await userRepository
                .All()
                .Where(x => x.Id == teacherId)
                .FirstOrDefaultAsync();

            studentToRemove.TeacherId = null;
            teacher.Students.Remove(studentToRemove);

            userRepository.Update(studentToRemove);
            userRepository.Update(teacher);

            await userRepository.SaveChangesAsync();
        }
    }
}