using System;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services
{
    public class DateTimeProvider : IProvideUtcDateTime
    {
        DateTime IProvideUtcDateTime.Now => DateTime.UtcNow;
    }
}