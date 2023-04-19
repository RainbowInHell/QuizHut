namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DAL.Common.Repositories;
    using QuizHut.DAL.Entities;

    public class StudentService : IStudentService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        private readonly IExpressionBuilder expressionBuilder;

        public StudentService(IDeletableEntityRepository<ApplicationUser> userRepository, IExpressionBuilder expressionBuilder)
        {
            this.userRepository = userRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<bool> AddStudentAsync(string email, string teacherId)
        {
            var user = await userRepository
                .AllAsNoTracking()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                var teacher = await userRepository
                    .AllAsNoTracking()
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

        public async Task DeleteFromTeacherListAsync(string studentId, string teacherId)
        {
            var studentToRemove = await userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == studentId)
                .FirstOrDefaultAsync();

            var teacher = await userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == teacherId)
                .FirstOrDefaultAsync();

            studentToRemove.TeacherId = null;
            teacher.Students.Remove(studentToRemove);

            userRepository.Update(studentToRemove);
            userRepository.Update(teacher);

            await userRepository.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAllStudentsAsync<T>(string teacherId = null, string groupId = null)
        {
            var query = userRepository.AllAsNoTracking();

            if (groupId != null)
            {
                // var assignedstudentsIds = await studentsGroupsService.GetAllStudentsIdsByGroupIdAsync(groupId);
                // query = query.Where(x => !assignedstudentsIds.Contains(x.Id));
                query = query.Where(x => !x.StudentsInGroups.Select(x => x.GroupId).Contains(groupId));
            }

            if (teacherId != null)
            {
                query = query.Where(x => x.TeacherId == teacherId);
            }

            return await query.Where(x => !x.Roles.Any()).To<T>().ToListAsync();
        }

        public async Task<IList<T>> GetAllInRolesPerPageAsync<T>(
            int page,
            int countPerPage,
            string searchCriteria = null,
            string searchText = null,
            string roleId = null)
        {
            var query = userRepository.AllAsNoTracking();

            query = roleId != null
                ? query.Where(x => x.Roles.Any(x => x.RoleId == roleId))
                : query.Where(x => x.Roles.Any());

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<ApplicationUser>(searchCriteria, searchText, roleId);
                query = query.Where(filter);
            }

            return await query.OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetAllInRolesCountAsync(string searchCriteria = null, string searchText = null, string roleId = null)
        {
            var query = userRepository.AllAsNoTracking().Where(x => x.Roles.Any());
            query = roleId != null
                           ? query.Where(x => x.Roles.Any(x => x.RoleId == roleId))
                           : query.Where(x => x.Roles.Any());

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<ApplicationUser>(searchCriteria, searchText, roleId);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId)
        {
            return await userRepository
                .AllAsNoTracking()
                .Where(x => x.StudentsInGroups.Select(x => x.GroupId).Contains(groupId))
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetAllStudentsCountAsync(string teacherId = null, string searchCriteria = null, string searchText = null)
        {
            var query = userRepository.AllAsNoTracking().Where(x => !x.Roles.Any());

            if (teacherId != null)
            {
                query = query.Where(x => x.TeacherId == teacherId);
            }

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<ApplicationUser>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<T>> GetAllStudentsPerPageAsync<T>(
            int page,
            int countPerPage,
            string teacherId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = userRepository.AllAsNoTracking().Where(x => !x.Roles.Any());

            if (teacherId != null)
            {
                query = query.Where(x => x.TeacherId == teacherId);
            }

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<ApplicationUser>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();
        }
    }
}