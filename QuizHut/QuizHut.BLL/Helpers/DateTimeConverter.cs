using QuizHut.BLL.Helpers.Contracts;
using TimeZoneConverter;

namespace QuizHut.BLL.Helpers
{
    public class DateTimeConverter : IDateTimeConverter
    {
        public const string TimeZoneIana = "Europe/Minsk";

        public string GetDate(DateTime dateAndTime)
        {
            var activationDateAndTimeToUserLocalTime = this.GetDateTimeLocalToTheUser(dateAndTime);
            return activationDateAndTimeToUserLocalTime.Date.ToString("dd/MM/yyyy");
        }

        public string GetDurationString(DateTime activationDateAndTime, TimeSpan duration)
        {
            var activationDateAndTimeToUserLocalTime = this.GetDateTimeLocalToTheUser(activationDateAndTime);

            return $"{activationDateAndTimeToUserLocalTime.Hour.ToString("D2")}" +
                   $":{activationDateAndTimeToUserLocalTime.Minute.ToString("D2")}" +
                   $" - {activationDateAndTimeToUserLocalTime.Add(duration).Hour.ToString("D2")}" +
                   $":{activationDateAndTimeToUserLocalTime.Add(duration).Minute.ToString("D2")}";
        }

        private DateTime GetDateTimeLocalToTheUser(DateTime dateAndTime)
        {
            var windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(TimeZoneIana));
            return TimeZoneInfo.ConvertTimeFromUtc(dateAndTime, windowsTimeZone);
        }
    }
}