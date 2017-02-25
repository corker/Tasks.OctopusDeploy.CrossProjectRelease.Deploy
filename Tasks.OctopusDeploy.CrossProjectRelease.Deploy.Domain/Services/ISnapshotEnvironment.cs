namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface ISnapshotEnvironment
    {
        string Id { get; }
        string Name { get; }
    }
}