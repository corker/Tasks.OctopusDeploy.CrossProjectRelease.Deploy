using System;
using System.Configuration;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Configuration;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy
{
    public class ProgramConfiguration :
        IOctopusServerConfiguration,
        ISnapshotEnvironmentConfiguration,
        ISnapshotReaderConfiguration,
        IStepDeployerConfiguration
    {
        string IOctopusServerConfiguration.Url
            => GetValueFromAppSettings("OctopusDeploy.Url");

        string IOctopusServerConfiguration.ApiKey
            => GetValueFromAppSettings("OctopusDeploy.ApiKey");

        string ISnapshotEnvironmentConfiguration.Name
            => GetValueFromAppSettings("Tasks.OctopusDeploy.CrossProjectRelease.Environment");

        public string FileName
            => GetValueFromAppSettings("Tasks.OctopusDeploy.CrossProjectRelease.FileName");

        public TimeSpan Interval
            => GetTimeSpanFromAppSettings("Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Interval");

        public TimeSpan Timeout
            => GetTimeSpanFromAppSettings("Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Timeout");

        private static TimeSpan GetTimeSpanFromAppSettings(string key)
        {
            var stringValue = GetValueFromAppSettings(key);
            var value = TimeSpan.Parse(stringValue);
            return value;
        }

        private static string GetValueFromAppSettings(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                string message = $"{key} is not defined in AppSettings section of the application configuration file.";
                throw new InvalidOperationException(message);
            }
            return value;
        }
    }
}