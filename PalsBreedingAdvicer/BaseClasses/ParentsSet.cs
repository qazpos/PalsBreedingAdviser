using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    public class ParentsSet
    {
        public PalTribeId MaleParent { get; private set; }
        public PalTribeId FemaleParent { get; private set; }


        public ParentsSet(PalTribeId MaleParent, PalTribeId FemaleParent)
        {
            this.MaleParent = MaleParent;
            this.FemaleParent = FemaleParent;
        }

        public static bool operator ==(ParentsSet left, ParentsSet right)
        {
            return left.MaleParent == right.MaleParent && left.FemaleParent == right.FemaleParent;
        }

        public static bool operator !=(ParentsSet left, ParentsSet right)
        {
            return (left.MaleParent != right.MaleParent || left.FemaleParent != right.FemaleParent);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ParentsSet || obj == null)
                return false;

            return this == (ParentsSet)obj;
        }

        public override int GetHashCode() => MaleParent.GetHashCode() ^ FemaleParent.GetHashCode();

        public override string ToString() => $"{MaleParent}, {FemaleParent}";
    }
}
