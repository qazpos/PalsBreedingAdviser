using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    internal class SkillWeightPair
    {
        public PalPassiveSkill PassiveSkill { get; set; }
        public int Weight { get; set; }




        public SkillWeightPair(PalPassiveSkill passiveSkill, int weight)
        {
            PassiveSkill = passiveSkill;
            Weight = weight;
        }


        public SkillWeightPair() { }
    }
}
