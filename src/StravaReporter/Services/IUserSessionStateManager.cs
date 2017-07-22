namespace StravaReporter.Services
{
    public interface IUserSessionStateManager
    {
        UserSessionStateManager.Wrapper Current { get; }
    }
}