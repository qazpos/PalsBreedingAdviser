using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    public class PalInfo
    {
        public Guid Id { get; private set; }
        public PalTribeId TribeId { get; private set; }
        public string? NickName { get; private set; }
        public int Level { get; private set; }
        public PalGenderType Gender { get; private set; }
        public bool IsRarePal { get; private set; }
        public List<PalPassiveSkill> PassiveSkills { get; private set; } = new();




        public PalInfo(PalTribeId tribeId, string? nickName, int level, PalGenderType gender,
            bool isRarePal, List<PalPassiveSkill> passiveSkills)
        {
            TribeId = tribeId;
            NickName = nickName;
            Level = level == 0 ? 1 : level;
            Gender = gender;
            IsRarePal = isRarePal;
            PassiveSkills = passiveSkills;
            Id = new Guid();
        }
    }
}
