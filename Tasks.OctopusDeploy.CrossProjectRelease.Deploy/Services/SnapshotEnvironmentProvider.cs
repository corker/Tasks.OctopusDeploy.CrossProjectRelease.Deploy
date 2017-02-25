using System;
using Octopus.Client;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Configuration;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services
{
    public class SnapshotEnvironmentProvider : ISnapshotEnvironment
    {
        private readonly ISnapshotEnvironmentConfiguration _configuration;
        private readonly IOctopusRepository _repository;
        private readonly Lazy<SnapshotEnvironment> _environment;

        public SnapshotEnvironmentProvider(ISnapshotEnvironmentConfiguration configuration, IOctopusRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
            _environment = new Lazy<SnapshotEnvironment>(GetSnapshotEnvironment);
        }

        private SnapshotEnvironment GetSnapshotEnvironment()
        {
            var resource = _repository.Environments.FindByName(_configuration.Name);
            if (resource == null)
            {
                string message = $"Can't schedule updates from {_configuration.Name}. The environment was not found.";
                throw new ArgumentOutOfRangeException(nameof(_configuration.Name), message);
            }
            return new SnapshotEnvironment(resource.Id, resource.Name);
        }

        public string Id => _environment.Value.Id;
        public string Name => _environment.Value.Name;

        private class SnapshotEnvironment
        {
            public SnapshotEnvironment(string id, string name)
            {
                Id = id;
                Name = name;
            }

            public string Id { get; }
            public string Name { get; }
        }
    }
}