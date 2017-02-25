namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Configuration
{
    public interface IOctopusServerConfiguration
    {
        string Url { get; }
        string ApiKey { get; }
    }
}