using PalworldSaveDecoding;
using System.IO;
using System.Text.Json;

namespace PalsBreedingAdvicer
{
    public class BreedingCalculator
    {
        private static readonly List<BreedingSet> breedingExclusions;
        private static readonly Dictionary<PalTribeId, PalBreedingInfo> breedingData;



        static BreedingCalculator()
        {
            //Читаем исключения скрещивания
            var breedingExclusionsFile = Config.Instance.BreedingExclusionsFile;
            if (!File.Exists(breedingExclusionsFile))
                throw new FileNotFoundException($"File {breedingExclusionsFile} not found");

            using (var file = new StreamReader(breedingExclusionsFile)) {
                var jsonString = file.ReadToEnd();
                if (jsonString == null)
                    throw new InvalidDataException($"File {breedingExclusionsFile} reading error");
                var list = (List<BreedingSet>?)JsonSerializer.Deserialize(jsonString, typeof(List<BreedingSet>));
                if (list == null)
                    throw new InvalidDataException($"Error with {breedingExclusionsFile} deserialization");
                breedingExclusions = list;
            }


            //Читаем данные палов, необходимые для скрещивания
            var breedingDataFile = Config.Instance.BreedingDataFile;
            if (!File.Exists(breedingDataFile))
                throw new FileNotFoundException($"File {breedingDataFile} not found");

            using (var file = new StreamReader(breedingDataFile)) {
                var jsonString = file.ReadToEnd();
                if (jsonString == null)
                    throw new InvalidDataException($"File {breedingDataFile} reading error");
                var dict = JsonSerializer.Deserialize<Dictionary<PalTribeId, PalBreedingInfo>>(jsonString);
                if (dict == null)
                    throw new InvalidDataException($"Error with {breedingDataFile} deserialization");
                breedingData = dict;
            }
        }



        public static List<ParentsDraft> GetPossibleParents(PalTribeId targetTribeId, PalInfo[] malePals, PalInfo[] femalePals)
        {
            var result = new List<ParentsDraft>();

            //Получаем подсписок исключений для указанного потомка
            var exclBreedingSets = new List<BreedingSet>(breedingExclusions.Where(p => p.Child == targetTribeId));
            //Если такие исключения существуют, то заполняем результат палами из исключений и возвращаем результат
            if (exclBreedingSets.Count > 0) {
                foreach (var exclBreedingSet in exclBreedingSets) {
                    foreach (var maleParent in malePals.Where(p => p.TribeId == exclBreedingSet.Parents.MaleParent))
                        foreach (var femaleParent in femalePals.Where(p => p.TribeId == exclBreedingSet.Parents.FemaleParent))
                            result.Add(new ParentsDraft(maleParent, femaleParent));
                }
                return result;
            }


            //Если для целевого пала не удалось получить его информацию о скрещивании
            //Или целевой пал не выводится согласно общим правилам, то возвращаем пустой массив
            if (!breedingData.TryGetValue(targetTribeId, out var targetBreedingInfo) || !targetBreedingInfo.FollowBreedingRule)
                return result;


            //Определеяем интервал BreedeingPower, который даст целевого пала
            //Получаем список палов, которых можно вывести по общим плавилам скрещивания и сортируем его по BreedingPower
            var noExcl = new List<PalBreedingInfo>(breedingData.Where(x => x.Value.FollowBreedingRule).Select(x => x.Value));
            noExcl = noExcl.OrderBy(x => x.BreedingPower).ToList();
            //Получаем индекс целевого пала в этом списке
            var index = noExcl.FindIndex(p => p.BreedingPower == targetBreedingInfo.BreedingPower);
            //Получаем список только значений BreedingPower
            var breedingPowers = breedingData.Select(x => x.Value.BreedingPower);

            //Формируем интервал в зависимости от TieBreakerOrder палов
            float leftBorder, rightBorder;
            bool isLeftClosed, isRightClosed;
            //Определяем параметры левой границы интервала
            if (index == 0) {
                leftBorder = breedingPowers.Min();
                isLeftClosed = true;
            } else {
                leftBorder = (noExcl[index].BreedingPower + noExcl[index - 1].BreedingPower) / 2f;
                isLeftClosed = noExcl[index].TieBreakOrder < noExcl[index - 1].TieBreakOrder;
            }

            //Определяем параметры правой границы интервала
            if (index == noExcl.Count - 1) {
                rightBorder = breedingPowers.Max();
                isRightClosed = true;
            } else {
                rightBorder = (noExcl[index].BreedingPower + noExcl[index + 1].BreedingPower) / 2f;
                isRightClosed = noExcl[index].TieBreakOrder < noExcl[index + 1].TieBreakOrder;
            }

            var interval = new Interval(leftBorder, rightBorder, isLeftClosed, isRightClosed);


            //Из списка исключений берём пары родителей, т.к. они в любом случае не дадут целевого пала
            var exclParents = new List<ParentsSet>(breedingExclusions.Select(a => a.Parents));
            //Перебираем всех палов, переданных в параметрах
            foreach (var malePalInfo in malePals) {
                var maleBreedingInfo = GetBreedingInfo(malePalInfo.TribeId);

                foreach (var femalePalInfo in femalePals) {
                    var femaleBreedingInfo = GetBreedingInfo(femalePalInfo.TribeId);

                    //Если полученная пара родителей находится в исключениях, то пропускаем
                    if (exclParents.Contains(new(malePalInfo.TribeId, femalePalInfo.TribeId)))
                        continue;

                    //Если среднее арифметическое BreedingPower родителей попадает в интервал, то добавляем их в результат
                    var avgBreedingPower = Math.Round((maleBreedingInfo.BreedingPower + femaleBreedingInfo.BreedingPower) / 2f, MidpointRounding.AwayFromZero);
                    if (interval.Contains((float)avgBreedingPower))
                        result.Add(new(malePalInfo, femalePalInfo));
                }
            }

            return result;
        }


        public static Task<List<ParentsDraft>> GetPossibleParentsAsync(PalTribeId targetTribeId, PalInfo[] malePals, PalInfo[] femalePals)
        {
            return Task.Run(() => GetPossibleParents(targetTribeId, malePals, femalePals));
        }



        public static PalBreedingInfo GetBreedingInfo(PalTribeId palTribeId) => breedingData[palTribeId];
    }
}
