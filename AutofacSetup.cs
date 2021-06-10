using Autofac;
using Novalia.Entities;
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

            ////builder.RegisterType<LogManager>()
            ////    .As<ILogManager>()
            ////    .SingleInstance();

            builder.RegisterType<AppSettings>()
                .As<IAppSettings>()
                .SingleInstance();

            builder.RegisterType<Novalia>().AsSelf();
            return builder.Build();
        }
    }
}
