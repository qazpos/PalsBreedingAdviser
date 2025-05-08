using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    public class BreedingSet
    {
        public ParentsSet Parents { get; private set; }
        public PalTribeId Child { get; private set; }


        public BreedingSet(ParentsSet Parents, PalTribeId Child)
        {
            this.Parents = Parents;
            this.Child = Child;
        }

        public override string ToString() => $"{Parents.MaleParent} + {Parents.FemaleParent} = {Child}";
    }
}
