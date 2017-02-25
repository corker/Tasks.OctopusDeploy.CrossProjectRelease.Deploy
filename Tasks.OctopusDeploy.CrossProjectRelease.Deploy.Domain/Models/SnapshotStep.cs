namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models
{
    public class SnapshotStep
    {
        public int Index { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ReleaseVersion { get; set; }
        public string ReleaseId { get; set; }
    }
}