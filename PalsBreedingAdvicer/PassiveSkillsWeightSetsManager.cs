using System.IO;
using System.Text.Json;

namespace PalsBreedingAdvicer
{
    internal class PassiveSkillsWeightSetsManager
    {
        public static List<PassiveSkillsWeightSet> ReadSetsFromJson(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException($"{nameof(filePath)} is null");

            if (!File.Exists(filePath))
                return new();


            var jsonString = File.ReadAllText(filePath);

            if (jsonString == null)
                return new();

            var result = JsonSerializer.Deserialize<List<PassiveSkillsWeightSet>>(jsonString);
            if (result == null)
                return new();
            return result;
        }

        public static List<PassiveSkillsWeightSet> ReadSetsFromJson() =>
            ReadSetsFromJson(Config.Instance.WeightSetsFilePath);



        public static Task<List<PassiveSkillsWeightSet>> ReadSetsFromJsonAsync(string filePath) =>
            Task.Run(() => ReadSetsFromJson(filePath));

        public static Task<List<PassiveSkillsWeightSet>> ReadSetsFromJsonAsync() =>
            Task.Run(() => ReadSetsFromJson(Config.Instance.WeightSetsFilePath));




        public static void WriteSetsToJson(string filePath, List<PassiveSkillsWeightSet> weightSets)
        {
            if (filePath == null)
                throw new ArgumentNullException($"{nameof(filePath)} is null");

            var jsonString = JsonSerializer.Serialize(weightSets, Config.Instance.JsonSerializerOptions);

            if (jsonString != null)
                File.WriteAllText(filePath, jsonString);
        }

        public static void WriteSetsToJson(List<PassiveSkillsWeightSet> weightSets) =>
            WriteSetsToJson(Config.Instance.WeightSetsFilePath, weightSets);



        public static Task WriteSetsToJsonAsync(string filePath, List<PassiveSkillsWeightSet> weightSets) =>
            Task.Run(() => WriteSetsToJson(filePath, weightSets));

        public static Task WriteSetsToJsonAsync(List<PassiveSkillsWeightSet> weightSets) =>
            Task.Run(() => WriteSetsToJson(Config.Instance.WeightSetsFilePath, weightSets));



        public static List<PassiveSkillsWeightSet> GenerateDefaultWeightSets()
        {
            var result = new List<PassiveSkillsWeightSet>();
            result.Add(PassiveSkillsWeightSet.MaximizeCraftSpeed);
            result.Add(PassiveSkillsWeightSet.MaximizeMovementSpeed);
            result.Add(PassiveSkillsWeightSet.MaximizeAttack);
            result.Add(PassiveSkillsWeightSet.MaximizeDeffense);
            result.Add(PassiveSkillsWeightSet.MaximizePlayer);
            return result;
        }
    }
}
