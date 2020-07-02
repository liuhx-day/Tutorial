namespace Tutorial.Web
{
    public class WelcomeService : IWelcomeService
    {
        public string GetMessage()
        {
            return "Hello from IWelcome service";
        }
    }
}