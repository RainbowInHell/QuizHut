﻿namespace QuizHut.BLL.Services.Contracts
{
    public interface IResultsService
    {
        Task<int> GetResultsCountByStudentIdAsync(string id, string searchCriteria = null, string searchText = null);

        Task<string> CreateResultAsync(string studentId, decimal points, string quizId);

        Task UpdateResultAsync(string id, decimal points);
    }
}