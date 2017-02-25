namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models
{
    public class Snapshot
    {
        public string EnvironmentId { get; set; }
        public string EnvironmentName { get; set; }
        public SnapshotStep[] Steps { get; set; }
    }
}