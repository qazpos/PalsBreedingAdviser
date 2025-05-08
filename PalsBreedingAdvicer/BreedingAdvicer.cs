using System.IO;
using System.Text.RegularExpressions;
using PalsBreedingAdvicer.BaseClasses;
using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    public class BreedingAdvicer
    {
        private List<PalInfo> playerPals;

        private static readonly int PalPassiveSkillsCount = 4;


        public static List<PassiveSkillsWeightSet> PassiveSkillsWeightSets { get; private set; }




        static BreedingAdvicer()
        {
            //Читаем из файла имеющиеся наборы весов пассивных скиллов
            PassiveSkillsWeightSets = PassiveSkillsWeightSetsManager.ReadSetsFromJson();
            //Если их нет в файле, то генерим дефолтный файл
            if (PassiveSkillsWeightSets.Count == 0) {
                PassiveSkillsWeightSets = PassiveSkillsWeightSetsManager.GenerateDefaultWeightSets();
                PassiveSkillsWeightSetsManager.WriteSetsToJson(PassiveSkillsWeightSets);
            }
        }



        public static void UpdatePassiveSkillsWeightSets(List<PassiveSkillsWeightSet> newWeightSet)
        {
            PassiveSkillsWeightSets = newWeightSet;
        }




        public BreedingAdvicer(List<Character> playerCharacters)
        {
            //Из переданного списка палов игрока формируем список с необходимой информацией
            playerPals = new();
            var regex = new Regex(@"^boss_");
            //Для всех Character, которые не являются игроками
            foreach (var character in playerCharacters.Where(x => !x.IsPlayer)) {
                var characterId = character.CharacterId;
                //Если пал считается боссом (рарный или босс), то убираем из его characterId указание, что он босс
                if (regex.IsMatch(characterId!.ToLower()))
                    characterId = characterId.Substring(5);

                //Определяем пол пала
                if (character.Gender == null)
                    throw new InvalidDataException($"Pal {characterId} has no gender");
                var gender = (PalGenderType)character.Gender;

                //Определяем TribeId пала
                if (!Enum.TryParse<PalTribeId>(characterId, out var tribeId)) {
                    throw new InvalidDataException($"Unknown characterId {characterId}");
                }


                //Формируем список пассивных скиллов пала
                var passiveSkills = new List<PalPassiveSkill>();
                if (character.PassiveSkillList != null)
                    foreach (var passiveSkillStr in character.PassiveSkillList) {
                        if (Enum.TryParse<PalPassiveSkill>(passiveSkillStr, out var passiveSkill))
                            passiveSkills.Add(passiveSkill);
                        else
                            throw new InvalidDataException($"Unknown pal passive skill {passiveSkillStr}");
                    }

                playerPals.Add(new PalInfo(tribeId, character.NickName, character.Level.ByteValue??0, gender, character.IsPlayer, passiveSkills));
            }
        }




        public List<ParentsDraftWeighed> GetRecommendedParents(PalTribeId childTribeId, PassiveSkillsWeightSet weights)
        {
            //Получаем список всех возможных родителей
            var parentsDrafts = BreedingCalculator.GetPossibleParents(childTribeId,
                playerPals.Where(p => p.Gender == PalGenderType.Male).ToArray(),
                playerPals.Where(p => p.Gender == PalGenderType.Female).ToArray());

            //Каждую возможную пару родителей взвешиваем согласно указанному набору весов
            List<ParentsDraftWeighed> result = new();
            foreach (var parentDraft in parentsDrafts) {
                result.Add(WeighParents(parentDraft, weights));
            }
            //Сортируем итоговый список по убыванию итогового веса
            result.Sort((a, b) => a.DrawtWeight > b.DrawtWeight ? -1 : 1);
            return result;
        }

        public Task<List<ParentsDraftWeighed>> GetRecommendedParentsAsync(PalTribeId childTribeId, PassiveSkillsWeightSet weights)
        {
            return Task.Run(() => GetRecommendedParents(childTribeId, weights));
        }




        private ParentsDraftWeighed WeighParents(ParentsDraft parents, PassiveSkillsWeightSet weights)
        {
            //Формируем общий пул скиллов родителей
            var parentsSkills = new List<PalPassiveSkill>(parents.ParentMale.PassiveSkills);
            parentsSkills.AddRange(parents.ParentFemale.PassiveSkills);
            //Убираем повторяющиеся
            var parentsSkillsWeighted = parentsSkills.Distinct()
                //Получаем вес каждого скилла
                .Select(skill => (PassiveSkill: skill, Weight: weights.Data.TryGetValue(skill, out var skillWeight) ? skillWeight : 0))
                //Оставляем только полезные скиллы
                .Where(a => a.Weight > 0)
                //Сортируем скиллы по уменьшению веса
                .OrderByDescending(a => a.Item2)
                //Берём не больше возможного максимума скиллов у одного пала
                .Take(PalPassiveSkillsCount)
                .ToList();

            var draftWeight = parentsSkillsWeighted.Sum(a => a.Weight);

            return new(parents, draftWeight, parentsSkillsWeighted.Select(a => a.PassiveSkill).ToList());
        }
    }
}