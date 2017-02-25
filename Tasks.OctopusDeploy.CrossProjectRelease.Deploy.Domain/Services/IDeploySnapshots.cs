using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface IDeploySnapshots
    {
        void Deploy(Snapshot snapshot);
    }
}