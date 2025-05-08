using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    internal class PassiveSkillsWeightSetEditable
    {
        public string Name { get; set; }
        public List<SkillWeightPair> Data { get; set; } = new();
        public int Id { get; set; }

        public bool HasChanges
        {
            get {
                if (ParentWeightSet == null) return true;
                if (ParentWeightSet.Name != Name) return true;

                var hasChanges = false;
                foreach (var weightPair in Data) {
                    if (ParentWeightSet.Data.ContainsKey(weightPair.PassiveSkill))
                        hasChanges = weightPair.Weight != ParentWeightSet.Data[weightPair.PassiveSkill];
                    else
                        hasChanges = weightPair.Weight != 0;

                    if (hasChanges)
                        return true;
                }
                return false;
            }
        }

        private PassiveSkillsWeightSet? ParentWeightSet { get; init; }



        public PassiveSkillsWeightSetEditable(PassiveSkillsWeightSetEditable passiveSkillsWeightSetEditable, int id, string? newName = null)
        {
            Name = newName ?? passiveSkillsWeightSetEditable.Name;
            Id = id;
            foreach(var skillWeightPair in passiveSkillsWeightSetEditable.Data) {
                if (skillWeightPair != null)
                    Data.Add(new SkillWeightPair(skillWeightPair.PassiveSkill, skillWeightPair.Weight));
            }
        }


        public PassiveSkillsWeightSetEditable(PassiveSkillsWeightSet passiveSkillsWeightSet)
        {
            Name = passiveSkillsWeightSet.Name;
            foreach (var passiveSkill in Enum.GetValues<PalPassiveSkill>()) {
                if (passiveSkillsWeightSet.Data.ContainsKey(passiveSkill))
                    Data.Add(new(passiveSkill, passiveSkillsWeightSet.Data[passiveSkill]));
                else
                    Data.Add(new(passiveSkill, 0));
            }

            ParentWeightSet = passiveSkillsWeightSet;
            Id = passiveSkillsWeightSet.Id;
        }


        public PassiveSkillsWeightSetEditable(string name, int id)
        {
            Name = name;
            Id = id;
            foreach (var passiveSkill in Enum.GetValues<PalPassiveSkill>())
                Data.Add(new(passiveSkill, 0));
        }




        public PassiveSkillsWeightSet ToPassiveSkillsWeightSet()
        {
            var result = new PassiveSkillsWeightSet(Name, new Dictionary<PalPassiveSkill, int>(), Id);
            foreach (var weightPair in Data) {
                if (weightPair.Weight != 0)
                    result.Data.Add(weightPair.PassiveSkill, weightPair.Weight);
            }

            return result;
        }
    }
}