using System;
using System.IO;
using System.Reflection;
using Autofac;
using log4net;
using log4net.Config;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy
{
    internal class Program
    {
        private const string Log4NetConfigFileName = "log4net.config";

        private const int ExitCodeError = -1;
        private const int ExitCodeOk = 0;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Log4NetConfigFileName));
            Log.Info("Deploying a cross project release.");
            try
            {
                using (var container = ProgramContainerFactory.Create())
                {
                    container.Resolve<ProgramRunner>().Run();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception:", ex);
                Environment.Exit(ExitCodeError);
            }
            Log.Info("Done.");
            Environment.Exit(ExitCodeOk);
        }
    }
}