namespace WebApi.Services
{
    public class DBLogger : ISingleLogger
    {
        public void Log(string message)
        {
            Console.WriteLine("[DBLogger]: " + message);
        }
    }
}