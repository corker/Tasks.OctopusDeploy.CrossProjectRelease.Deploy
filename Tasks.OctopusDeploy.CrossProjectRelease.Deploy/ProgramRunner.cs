using System.Reflection;
using log4net;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy
{
    public class ProgramRunner
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDeploySnapshots _deployer;
        private readonly IReadSnapshots _reader;

        public ProgramRunner(IDeploySnapshots deployer, IReadSnapshots reader)
        {
            _deployer = deployer;
            _reader = reader;
        }

        public void Run()
        {
            Log.Debug("Snapshot deployment process started");

            var snapshot = _reader.Read();
            _deployer.Deploy(snapshot);

            Log.Debug("Snapshot deployment process finished");
        }
    }
}