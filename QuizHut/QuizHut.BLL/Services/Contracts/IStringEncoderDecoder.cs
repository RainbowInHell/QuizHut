namespace QuizHut.BLL.Services.Contracts
{
    public interface IStringEncoderDecoder
    {
        string Encode(string message);
        string Decode(string encodedMessage);
    }
}