using PalworldSaveDecoding;

namespace PalsBreedingAdvicer.BaseClasses
{
    public class ParentsDraftWeighed
    {
        public ParentsDraft Parents { get; private set; }
        public int DrawtWeight { get; private set; }
        public List<PalPassiveSkill> BestSkills { get; private set; }




        public ParentsDraftWeighed(ParentsDraft parents, int drawtWeight, List<PalPassiveSkill> bestSkills)
        {
            Parents = parents;
            DrawtWeight = drawtWeight;
            BestSkills = bestSkills;
        }
    }
}
