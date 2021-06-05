namespace Novalia.Serialization.Settings
{
    public interface IAppSettings
    {
        bool Debug { get; set; }

        bool FullScreen { get; set; }

        (int width, int height) Viewport { get; set; }
    }
}
