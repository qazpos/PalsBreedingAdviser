namespace PalsBreedingAdvicer
{
    public record PalBreedingInfo
    {
        public int BreedingPower { get; private set; }
        public int TieBreakOrder { get; private set; }
        public bool FollowBreedingRule { get; private set; }


        public PalBreedingInfo(int BreedingPower, int TieBreakOrder, bool FollowBreedingRule)
        {
            this.BreedingPower = BreedingPower;
            this.TieBreakOrder = TieBreakOrder;
            this.FollowBreedingRule = FollowBreedingRule;
        }
    }
}
