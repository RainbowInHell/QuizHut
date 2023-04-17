using System.Text;

class Program
{
    static void Main()
    {
        string message = "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";

        // Encode the message
        byte[] encodedBytes = Encoding.UTF8.GetBytes(message);
        string encodedMessage = Convert.ToBase64String(encodedBytes);

        Console.WriteLine("Encoded message: " + encodedMessage);

        // Decode the message
        byte[] decodedBytes = Convert.FromBase64String(encodedMessage);
        string decodedMessage = Encoding.UTF8.GetString(decodedBytes);

        Console.WriteLine("Decoded message: " + decodedMessage);
    }
}