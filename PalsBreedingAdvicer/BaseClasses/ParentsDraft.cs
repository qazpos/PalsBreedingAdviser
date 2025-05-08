using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    public class ParentsDraft
    {
        public PalInfo ParentMale { get; private set; }
        public PalInfo ParentFemale { get; private set; }


        public ParentsDraft(PalInfo ParentMale, PalInfo ParentFemale)
        {
            if (ParentMale.Gender == PalGenderType.Male && ParentFemale.Gender == PalGenderType.Female) {
                this.ParentMale = ParentMale;
                this.ParentFemale = ParentFemale;
            } else if (ParentMale.Gender == PalGenderType.Female && ParentFemale.Gender == PalGenderType.Male) {
                this.ParentMale = ParentFemale;
                this.ParentFemale = ParentMale;
            } else {
                throw new ArgumentException("Both parents are male or female");
            }
        }

        public static bool operator ==(ParentsDraft left, ParentsDraft right)
        {
            return left.ParentMale.Id == right.ParentMale.Id && left.ParentFemale.Id == right.ParentFemale.Id;
        }

        public static bool operator !=(ParentsDraft left, ParentsDraft right)
        {
            return (left.ParentMale.Id != right.ParentMale.Id || left.ParentFemale.Id != right.ParentFemale.Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ParentsDraft || obj == null)
                return false;

            return this == (ParentsDraft)obj;
        }

        public override int GetHashCode() => ParentMale.GetHashCode() ^ ParentFemale.GetHashCode();
    }
}
