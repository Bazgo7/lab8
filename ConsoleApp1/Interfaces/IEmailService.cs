namespace ConsoleApp1
{
    public interface IEmailService
    {
        Task SendResultAsync(string triangleType, List<(int, int)> coordinates);
    }
}