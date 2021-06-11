using Autofac;
using log4net;
using log4net.Config;
using Novalia.Entities;
using Novalia.Logging;
using Novalia.Serialization.Settings;
using Novalia.Ui;

namespace Novalia
{
    public static class AutofacSetup
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UiManager>()
                .As<IUiManager>()
                .SingleInstance();
            builder.RegisterType<GameManager>()
                .As<IGameManager>()
                .SingleInstance();
            builder.RegisterType<EntityFactory>()
                .As<IEntityFactory>()
                .SingleInstance();
            ////builder.RegisterType<SaveManager>()
            ////    .As<ISaveManager>()
            ////    .SingleInstance();

            builder.RegisterType<Logger>()
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterType<AppSettings>()
                .As<IAppSettings>()
                .SingleInstance();

            //log4net
            builder.Register(
                _ =>
                {
                    XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
                    return LogManager.GetLogger(typeof(Novalia));
                })
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<Novalia>().AsSelf();
            return builder.Build();
        }
    }
}
