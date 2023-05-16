namespace QuizHut.BLL.Helpers.Contracts
{
    public interface IDateTimeConverter
    {
        public string GetDurationString(DateTime activationDateAndTime, TimeSpan duration);

        public string GetDate(DateTime dateAndTime);
    }
}