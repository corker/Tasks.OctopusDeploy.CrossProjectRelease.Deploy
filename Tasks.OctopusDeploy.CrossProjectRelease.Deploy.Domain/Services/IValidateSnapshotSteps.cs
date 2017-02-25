using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface IValidateSnapshotSteps
    {
        bool Validate(SnapshotStep step, string environmentId);
    }
}