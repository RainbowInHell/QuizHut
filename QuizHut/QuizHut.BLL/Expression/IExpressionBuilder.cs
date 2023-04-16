namespace QuizHut.BLL.Expression
{
    using System.Linq.Expressions;

    public interface IExpressionBuilder
    {
        Expression<Func<T, bool>> GetExpression<T>(string queryType, string queryValue, string roleId = null);
    }
}