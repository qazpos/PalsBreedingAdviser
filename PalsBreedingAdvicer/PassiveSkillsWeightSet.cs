using PalworldSaveDecoding;

namespace PalsBreedingAdvicer
{
    public class PassiveSkillsWeightSet
    {
        public string Name { get; private set; }
        public Dictionary<PalPassiveSkill, int> Data { get; private set; }
        public int Id { get; private set; }



        public PassiveSkillsWeightSet(string name, Dictionary<PalPassiveSkill, int> data, int id) {
            Name = name;
            Data = data;
            Id = id;
        }




        public void Edit(PalPassiveSkill palPassiveSkill, int newWeight)
        {
            if (newWeight == 0)
                Data.Remove(palPassiveSkill);

            if (Data.ContainsKey(palPassiveSkill))
                Data[palPassiveSkill] = newWeight;
            else
                Data.Add(palPassiveSkill, newWeight);
        }






        internal static readonly PassiveSkillsWeightSet MaximizeCraftSpeed = new("MaximizeCraftSpeed",
            new() {
                { PalPassiveSkill.CraftSpeed_up2, 50 },
                { PalPassiveSkill.CraftSpeed_up1, 20 },
                { PalPassiveSkill.PAL_CorporateSlave, 30 },
                { PalPassiveSkill.Rare, 15 },
                { PalPassiveSkill.PAL_conceited, 10 },
                { PalPassiveSkill.CraftSpeed_down2, -30 },
                { PalPassiveSkill.CraftSpeed_down1, -10 },
                { PalPassiveSkill.Noukin, -50 },
                { PalPassiveSkill.PAL_rude, -10 },
                { PalPassiveSkill.Nocturnal, 25 },
                { PalPassiveSkill.MoveSpeed_up_1, 2 },
                { PalPassiveSkill.MoveSpeed_up_2, 4 },
                { PalPassiveSkill.MoveSpeed_up_3, 6 },
                { PalPassiveSkill.PAL_FullStomach_Down_1, 2 },
                { PalPassiveSkill.PAL_FullStomach_Down_2, 4 },
                { PalPassiveSkill.PAL_FullStomach_Up_1, -1 },
                { PalPassiveSkill.PAL_FullStomach_Up_2, -2 },
                { PalPassiveSkill.PAL_Sanity_Down_1, 2 },
                { PalPassiveSkill.PAL_Sanity_Down_2, 4 },
                { PalPassiveSkill.PAL_Sanity_Up_1, -1 },
                { PalPassiveSkill.PAL_Sanity_Up_2, -2 },
            }, 0);

        internal static readonly PassiveSkillsWeightSet MaximizeMovementSpeed = new("MaximizeMovementSpeed",
            new() {
                { PalPassiveSkill.MoveSpeed_up_3, 30 },
                { PalPassiveSkill.MoveSpeed_up_2, 20 },
                { PalPassiveSkill.MoveSpeed_up_1, 10 },
                { PalPassiveSkill.Legend, 15 },
                { PalPassiveSkill.Stamina_Up_1, 30 },
                { PalPassiveSkill.Stamina_Up_2, 15 },
                { PalPassiveSkill.Stamina_Down_1, -15 },
                { PalPassiveSkill.PAL_FullStomach_Down_2, 4 },
                { PalPassiveSkill.PAL_FullStomach_Down_1, 2 },
                { PalPassiveSkill.PAL_FullStomach_Up_2, -2 },
                { PalPassiveSkill.PAL_FullStomach_Up_1, -1 },
            }, 1);

        internal static readonly PassiveSkillsWeightSet MaximizeAttack = new("MaximizeAttack",
            new() {
                { PalPassiveSkill.PAL_ALLAttack_up2, 20 },
                { PalPassiveSkill.PAL_ALLAttack_up1, 10 },
                { PalPassiveSkill.PAL_ALLAttack_down2, -20 },
                { PalPassiveSkill.PAL_ALLAttack_down1, -10 },
                { PalPassiveSkill.Rare, 15 },
                { PalPassiveSkill.Noukin, 30 },
                { PalPassiveSkill.PAL_rude, 15 },
                { PalPassiveSkill.PAL_sadist, 8 },
                { PalPassiveSkill.PAL_oraora, 5 },
                { PalPassiveSkill.PAL_CorporateSlave, -30 },
                { PalPassiveSkill.PAL_masochist, -7 },
                { PalPassiveSkill.Legend, 20 },
                { PalPassiveSkill.Alien, 10 },
                { PalPassiveSkill.CoolTimeReduction_Up_1, 30 },
                { PalPassiveSkill.CoolTimeReduction_Up_2, 15 },
                { PalPassiveSkill.CoolTimeReduction_Down_1, -15 },
            }, 2);

        internal static readonly PassiveSkillsWeightSet MaximizeDeffense = new("MaximizeDeffense",
            new() {
                { PalPassiveSkill.Deffence_up2, 20 },
                { PalPassiveSkill.Deffence_up1, 10 },
                { PalPassiveSkill.Deffence_down2, -10 },
                { PalPassiveSkill.Deffence_down1, -5 },
                { PalPassiveSkill.PAL_masochist, 15 },
                { PalPassiveSkill.PAL_conceited, -5 },
                { PalPassiveSkill.PAL_oraora, -5 },
                { PalPassiveSkill.PAL_sadist, -7 },
                { PalPassiveSkill.Legend, 20 },
            }, 3);

        internal static readonly PassiveSkillsWeightSet MaximizePlayer = new("MaximizePlayer",
            new()
            {
                { PalPassiveSkill.TrainerATK_UP_1, 10 },
                { PalPassiveSkill.TrainerDEF_UP_1, 10 },
            }, 4);
    }
}