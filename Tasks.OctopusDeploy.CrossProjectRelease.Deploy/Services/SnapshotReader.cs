using System.IO;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Configuration;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services
{
    public class SnapshotReader : IReadSnapshots
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly JsonSerializer Serializer;
        private readonly ISnapshotReaderConfiguration _configuration;

        static SnapshotReader()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            Serializer = JsonSerializer.Create(settings);
        }

        public SnapshotReader(ISnapshotReaderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Snapshot Read()
        {
            Log.Debug($"Reading snapshot from a file: {_configuration.FileName}");
            using (var reader = File.OpenText(_configuration.FileName))
            using (var jsonReader = new JsonTextReader(reader))
            {
                Log.Debug("Deserializing a snapshot");
                return Serializer.Deserialize<Snapshot>(jsonReader);
            }
        }
    }
}