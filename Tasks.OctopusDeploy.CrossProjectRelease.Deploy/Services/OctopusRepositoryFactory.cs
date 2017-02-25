using Octopus.Client;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Configuration;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services
{
    public class OctopusRepositoryFactory
    {
        private readonly IOctopusServerConfiguration _configuration;

        public OctopusRepositoryFactory(IOctopusServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IOctopusRepository Create()
        {
            var endpoint = new OctopusServerEndpoint(_configuration.Url, _configuration.ApiKey);
            return new OctopusRepository(endpoint);
        }
    }
}