namespace QuizHut.BLL.Helpers.Contracts
{
    public interface IShuffler
    {
        IList<T> Shuffle<T>(IList<T> list);
    }
}