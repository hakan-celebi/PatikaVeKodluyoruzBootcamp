namespace WebApi.Services
{
    public class ConsoleLogger : ISingleLogger
    {
        public void Log(string message)
        {
            Console.WriteLine("[ConsoleLogger]: " + message);
        }
    }
}