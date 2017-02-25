using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface IDeploySnapshotSteps
    {
        void Deploy(SnapshotStep step, string environmentId);
    }
}