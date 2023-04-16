namespace QuizHut.BLL.Services
{
    using System.Text;

    using QuizHut.BLL.Services.Contracts;

    public class StringEncoderDecoder : IStringEncoderDecoder
    {
        public string Encode(string message)
        {
            byte[] encodedBytes = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(encodedBytes);
        }

        public string Decode(string encodedMessage)
        {
            byte[] decodedBytes = Convert.FromBase64String(encodedMessage);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}