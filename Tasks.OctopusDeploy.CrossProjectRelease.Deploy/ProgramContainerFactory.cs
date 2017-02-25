using Autofac;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy
{
    public static class ProgramContainerFactory
    {
        public static IContainer Create()
        {
            var builder = new ContainerBuilder();

            // Program
            builder.RegisterType<ProgramRunner>();
            builder.RegisterType<ProgramConfiguration>().AsImplementedInterfaces();

            // Services
            builder.RegisterType<DateTimeProvider>().AsImplementedInterfaces();
            builder.RegisterType<OctopusRepositoryFactory>();
            builder.RegisterType<SnapshotEnvironmentProvider>().AsImplementedInterfaces();
            builder.RegisterType<SnapshotReader>().AsImplementedInterfaces();
            builder.Register(context => context.Resolve<OctopusRepositoryFactory>().Create()).InstancePerLifetimeScope();

            // Domain
            builder.RegisterType<Domain.Services.SnapshotDeployer>().AsImplementedInterfaces();
            builder.RegisterType<SnapshotStepValidator>().AsImplementedInterfaces();
            builder.RegisterType<SnapshotStepDeployer>().AsImplementedInterfaces();
            builder.RegisterType<TaskResourceFactory>().AsImplementedInterfaces();
            builder.RegisterType<ExecutedTaskResourceWaiter>().AsImplementedInterfaces();

            return builder.Build();
        }
    }
}