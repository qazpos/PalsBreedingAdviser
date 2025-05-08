using System.IO;
using System.Text.RegularExpressions;

namespace PalsBreedingAdvicer
{
    internal class PassiveSkillsWeightSetsEditor
    {
        public List<PassiveSkillsWeightSetEditable> WeightSetsEditable { get; set; }

        public bool HasChanges
        {
            get {
                if (parentWeightSets.Count != WeightSetsEditable.Count)
                    return true;
                foreach (var weightSet in WeightSetsEditable) {
                    if (weightSet.HasChanges)
                        return true;
                }
                return false;
            }
        }

        internal bool IdsFixed { get; set; } = false;


        private List<PassiveSkillsWeightSet> parentWeightSets;

        private bool allIdsUnique => WeightSetsEditable.GroupBy(s => s.Id).Where(g => g.Count() > 1).Count() == 0;

        private static string newWeightSetBaseName = "New passive skills weight set";



        public PassiveSkillsWeightSetsEditor(List<PassiveSkillsWeightSet> passiveSkillsWeightSets)
        {
            WeightSetsEditable = new();
            parentWeightSets = passiveSkillsWeightSets;
            foreach (var weightSet in passiveSkillsWeightSets) {
                WeightSetsEditable.Add(new(weightSet));
            }
            if (!allIdsUnique) {
                FixIdsUniqueness();
            }
        }




        public void CreateNewSet()
        {
            var newWeightSetName = GenerateNewName(newWeightSetBaseName);
            WeightSetsEditable.Add(new PassiveSkillsWeightSetEditable(newWeightSetName, GetFreeId()));
        }




        public void DeleteSet(int id)
        {
            var setIndex = WeightSetsEditable.FindIndex(s => s.Id == id);
            if (setIndex != -1) {
                WeightSetsEditable.RemoveAt(setIndex);
            }
        }




        public void CreateNewSetAsCopy(int id)
        {
            var setToCopy = WeightSetsEditable.Find(s => s.Id == id);
            if (setToCopy == null)
                throw new InvalidDataException($"Can't find weight set with id = {id}.");
            var newWeightSetName = GenerateNewName(setToCopy.Name);
            WeightSetsEditable.Add(new PassiveSkillsWeightSetEditable(setToCopy, GetFreeId(), newWeightSetName));
        }




        public List<PassiveSkillsWeightSet> ToPassiveSkillsWeightSetsList()
        {
            var result = new List<PassiveSkillsWeightSet>();
            foreach (var weightSet in WeightSetsEditable) {
                var newWeightSet = weightSet.ToPassiveSkillsWeightSet();
                result.Add(newWeightSet);
            }
            return result;
        }




        private string GenerateNewName(string baseName)
        {
            if (string.IsNullOrEmpty(baseName))
                throw new ArgumentNullException("baseName can not be null or empty");
            var newName = baseName;

            var findNumberPattern = "( \\(\\d+\\))$";
            var regexFindNumber = new Regex(findNumberPattern);
            newName = regexFindNumber.Replace(newName, "");


            var regexNamePattern = $@"^{newName}(?'nameStr' \((?'numVal'\d+)\))?$";
            var regexFindName = new Regex(regexNamePattern);

            var nameMatches = WeightSetsEditable.Select(s => regexFindName.Match(s.Name)).Where(m => m.Success);
            if (nameMatches.Count() > 0) {
                var nameNumber = nameMatches.Select(m => m.Groups["numVal"].Value)
                    .Select(str => string.IsNullOrEmpty(str) ? "0" : str)
                    .Select(int.Parse)
                    .Max();
                newName += $" ({nameNumber + 1})";
            }

            return newName;
        }


        private void FixIdsUniqueness()
        {
            var id = 0;
            foreach (var weightSet in WeightSetsEditable) {
                weightSet.Id = id++;
            }
            parentWeightSets = ToPassiveSkillsWeightSetsList();
            IdsFixed = true;
        }


        private int GetFreeId()
        {
            var i = 0;
            foreach (var weightSet in WeightSetsEditable.OrderBy(s => s.Id)) {
                if (weightSet.Id != i)
                    return i;
                i++;
            }
            return i;
        }
    }
}
