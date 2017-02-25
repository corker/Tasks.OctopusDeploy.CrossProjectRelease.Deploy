using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services
{
    public interface IReadSnapshots
    {
        Snapshot Read();
    }
}