using System;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface IStepDeployerConfiguration
    {
        TimeSpan Interval { get; }
        TimeSpan Timeout { get; }
    }
}