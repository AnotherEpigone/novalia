using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Novalia.Serialization
{
    public class SaveManager : ISaveManager
    {
        private const string FileName = "save.nova";
        private readonly JsonSerializerSettings _jsonSettings;

        public SaveManager()
        {
            _jsonSettings = new JsonSerializerSettings()
            { 
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new GameStateContract(),
            };
        }

        public void Write(GameState save)
        {
            var saveFolder = GetSaveFolder();
            Directory.CreateDirectory(saveFolder);

            var saveFilePath = Path.Combine(saveFolder, FileName);
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            using var file = File.OpenWrite(saveFilePath);
            using var writer = new StreamWriter(file);

            var json = JsonConvert.SerializeObject(save, _jsonSettings);
            writer.Write(json);
        }

        public (bool, GameState) Read()
        {
            var saveFolder = GetSaveFolder();
            var saveFilePath = Path.Combine(saveFolder, FileName);
            if (!File.Exists(saveFilePath))
            {
                return (false, null);
            }

            using var file = File.OpenRead(saveFilePath);
            using var reader = new StreamReader(file);
            var save = JsonConvert.DeserializeObject<GameState>(reader.ReadToEnd(), _jsonSettings);

            return (true, save);
        }

        public bool SaveExists()
        {
            var mcFolder = GetSaveFolder();
            var saveFilePath = Path.Combine(mcFolder, FileName);
            return File.Exists(saveFilePath);
        }

        private string GetSaveFolder()
        {
            var appDataFolder = Environment.GetEnvironmentVariable(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "LocalAppData"
                    : "Home");
            return Path.Combine(appDataFolder, "Novalia");
        }
    }
}
