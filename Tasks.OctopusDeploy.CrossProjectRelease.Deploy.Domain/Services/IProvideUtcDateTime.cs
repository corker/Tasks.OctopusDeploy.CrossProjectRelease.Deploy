using System;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public interface IProvideUtcDateTime
    {
        DateTime Now { get; }
    }
}