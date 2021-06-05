using IniParser;
using IniParser.Model;
using System.IO;

namespace Novalia.Serialization.Settings
{
    public class AppSettings : IAppSettings
    {
        private const string SettingsIniKey = "settings";
        private const string FullScreenSettingKey = "fullscreen";
        private const string DebugPasswordSettingKey = "debug-pw";
        private const string DebugPassword = "Extra ecclesiam nulla salus";
        private const string ViewportSettingKey = "viewport";
        private const string ViewportDefault = "1280x720";

        private const string ConfigPath = "config.ini";

        private bool _fullScreen;
        private bool _debug;
        private (int width, int height) _viewport;

        public AppSettings()
        {
            if (File.Exists(ConfigPath))
            {
                Read();
            }
            else
            {
                _fullScreen = false;
                _debug = false;
                _viewport = (1280, 720);

                Write();
            }

        }

        public bool FullScreen
        {
            get { return _fullScreen; }
            set
            {
                _fullScreen = value;
                Write();
            }
        }

        public bool Debug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                Write();
            }
        }

        public (int width, int height) Viewport
        {
            get { return _viewport; }
            set
            {
                _viewport = value;
                Write();
            }
        }

        private void Read()
        {
            var iniParser = new FileIniDataParser();
            var data = iniParser.ReadFile(ConfigPath);
            var settings = data[SettingsIniKey];

            if (settings.ContainsKey(FullScreenSettingKey))
            {
                _fullScreen = bool.Parse(settings[FullScreenSettingKey]);
            }

            if (settings.ContainsKey(DebugPasswordSettingKey))
            {
                _debug = settings[DebugPasswordSettingKey] == DebugPassword;
            }

            var viewportString = settings.ContainsKey(ViewportSettingKey)
                ? settings[ViewportSettingKey]
                : ViewportDefault;
            var viewportSplit = viewportString.Split('x');
            _viewport = (int.Parse(viewportSplit[0]), int.Parse(viewportSplit[1]));
        }

        private void Write()
        {
            var data = new IniData();
            data[SettingsIniKey][FullScreenSettingKey] = _fullScreen.ToString();

            if (_debug)
            {
                data[SettingsIniKey][DebugPasswordSettingKey] = DebugPassword;
            }

            data[SettingsIniKey][ViewportSettingKey] = $"{_viewport.width}x{_viewport.height}";

            var iniParser = new FileIniDataParser();
            iniParser.WriteFile(ConfigPath, data);
        }
    }
}
