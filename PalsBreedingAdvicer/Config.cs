using System.Text.Json;
using System.IO;

namespace PalsBreedingAdvicer
{
    public sealed class Config
    {
        private static readonly Lazy<Config> instance =
            new Lazy<Config>(() => new Config());

        public static Config Instance => instance.Value;


        private Config()
        {
            if (!File.Exists(configPath))
                GenerateConfigFile();
            iniEditor = new Ini(configPath);

            SetLanguage(iniEditor);
            SetDefaultSavePath(iniEditor);
        }



        private string configPath => "config.ini";
        private Ini iniEditor;
        private Dictionary<LanguageCode, string> languageCodes = new Dictionary<LanguageCode, string>()
        {
            { LanguageCode.EN, "en-US" },
            { LanguageCode.RU, "ru-RU" },
        };




        private readonly Dictionary<string, string> defaultValues = new Dictionary<string, string>() {
            { "Language", "EN" },
            { "DefaultSavePath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Pal\\Saved\\SaveGames") }
        };

        private void GenerateConfigFile()
        {
            if (File.Exists(configPath))
                File.Delete(configPath);

            iniEditor = new Ini(configPath);

            foreach (var value in defaultValues) {
                iniEditor.WriteValue(value.Key, value.Value);
            }

            iniEditor.Save();
        }




        public string BreedingExclusionsFile => "Data\\BreedingExclusions.json";
        public string BreedingDataFile => "Data\\BreedingData.json";
        public string WeightSetsFilePath => "Data\\PassiveSkillsWeightSets.json";




        //Language settings
        public LanguageCode Language { get; private set; }
        private LanguageCode newLanguage;
        public LanguageCode NewLanguage
        {
            get { return newLanguage; }
            set {
                newLanguage = value;
                iniEditor.WriteValue("Language", value.ToString());
            }
        }

        private void SetLanguage(Ini ini)
        {
            var languageStr = ini.GetValue("Language");
            if (Enum.TryParse<LanguageCode>(languageStr, out var languageCode)) {
                Language = languageCode;
                newLanguage = languageCode;
            } else {
                languageStr = defaultValues["Language"];
                var defaultLanguage = Enum.Parse<LanguageCode>(languageStr);
                Language = defaultLanguage;
                newLanguage = defaultLanguage;
            }
        }





        public string DefaultSavePath { get; private set; } = "";

        private void SetDefaultSavePath(Ini ini)
        {
            var savePath = ini.GetValue("DefaultSavePath");

            if (savePath == null) {
                DefaultSavePath = defaultValues["DefaultSavePath"];
                ini.WriteValue("DefaultSavePath", DefaultSavePath);
                return;
            }

            if (Directory.Exists(savePath)) {
                DefaultSavePath = savePath;
                return;
            }

            if (File.Exists(savePath)) {
                DefaultSavePath = Path.GetDirectoryName(savePath)!;
                return;
            }

            DefaultSavePath = defaultValues["DefaultSavePath"];
            ini.WriteValue("DefaultSavePath", DefaultSavePath);
        }




        public JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };





        public void Save()
        {
            iniEditor.Save();
        }


        public string GetCultureCode(LanguageCode languageCode) => languageCodes[languageCode];
        public string GetCurrentCultureCode() => languageCodes[Language];
    }
}
