using Octopus.Client.Model;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface IWaitForExecutedTaskResources
    {
        TaskResource Wait(TaskResource task);
    }
}