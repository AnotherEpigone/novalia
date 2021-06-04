using Autofac;

namespace Novalia
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var container = AutofacSetup.CreateContainer();

            var game = container.Resolve<Novalia>();
            game.Run();
        }
    }
}
