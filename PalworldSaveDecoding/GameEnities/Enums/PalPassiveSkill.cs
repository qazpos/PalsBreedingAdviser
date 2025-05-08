using System.Text.Json.Serialization;

namespace PalworldSaveDecoding
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PalPassiveSkill
    {
        CraftSpeed_down1 = 0,             //Slacker	Work Speed -10%
        CraftSpeed_down2 = 1,             //Clumsy	Work Speed -30%
        CraftSpeed_up1 = 2,               //Serious	Work Speed +20%
        CraftSpeed_up2 = 3,               //Artisan	Work Speed +50%
        Deffence_down1 = 4,               //Downtrodden	Defense -10%
        Deffence_down2 = 5,               //Brittle	Defense -20%
        Deffence_up1 = 6,                 //Hard Skin	Defense +10%
        Deffence_up2 = 7,                 //Burly Body	Defense +20%
        ElementBoost_Aqua_1_PAL = 8,      //Hydromaniac	10% increase to Water attack damage.
        ElementBoost_Aqua_2_PAL = 9,      //Lord of the Sea	20% increase to Water attack damage.
        ElementBoost_Dark_1_PAL = 10,     //Veil of Darkness	10% increase to Dark attack damage.
        ElementBoost_Dark_2_PAL = 11,     //Lord of the Underworld	20% increase to Dark attack damage.
        ElementBoost_Dragon_1_PAL = 12,   //Blood of the Dragon	10% increase to Dragon attack damage
        ElementBoost_Dragon_2_PAL = 13,   //Divine Dragon	20% increase to Dragon attack damage.
        ElementBoost_Earth_1_PAL = 14,    //Power of Gaia	10% increase to Earth attack damage.
        ElementBoost_Earth_2_PAL = 15,    //Earth Emperor	20% increase to Earth attack damage.
        ElementBoost_Fire_1_PAL = 16,     //Pyromaniac	10% increase to Fire attack damage.
        ElementBoost_Fire_2_PAL = 17,     //Flame Emperor	20% increase to Fire attack damage.
        ElementBoost_Ice_1_PAL = 18,      //Coldblooded	10% increase to Ice attack damage.
        ElementBoost_Ice_2_PAL = 19,      //Ice Emperor	20% increase to Ice attack damage.
        ElementBoost_Leaf_1_PAL = 20,     //Fragrant Foliage	10% increase to Grass attack damage
        ElementBoost_Leaf_2_PAL = 21,     //Spirit Emperor	20% increase to Grass attack damage.
        ElementBoost_Normal_1_PAL = 22,   //Zen Mind	10% increase to Neutral attack damage
        ElementBoost_Normal_2_PAL = 23,   //Celestial Emperor	20% increase to Normal attack damage.
        ElementBoost_Thunder_1_PAL = 24,  //Capacitor	10% increase to Lightning attack damage.
        ElementBoost_Thunder_2_PAL = 25,  //Lord of Lightning	20% increase to Lightning attack damage.
        ElementResist_Aqua_1_PAL = 26,    //Waterproof	10% decrease to incoming Water damage.
        ElementResist_Dark_1_PAL = 27,    //Cheery	10% decrease to incoming Dark damage.
        ElementResist_Dragon_1_PAL = 28,  //Dragonkiller	10% decrease to incoming Dragon damage
        ElementResist_Earth_1_PAL = 29,   //Earthquake Resistant	10% decrease to incoming Ground damage
        ElementResist_Fire_1_PAL = 30,    //Suntan Lover	10% decrease to incoming Fire damage.
        ElementResist_Ice_1_PAL = 31,     //Heated Body	10% decrease to incoming Ice damage.
        ElementResist_Leaf_1_PAL = 32,    //Botanical Barrier	10% decrease to incoming Grass damage
        ElementResist_Normal_1_PAL = 33,  //Abnormal	10% decreased to incoming Neutral damage.
        ElementResist_Thunder_1_PAL = 34, //Insulated Body	10% decrease to incoming Lightning damage
        MoveSpeed_up_1 = 35,              //Nimble	10% increase to Movement Speed.
        MoveSpeed_up_2 = 36,              //Runner	20% increase to Movement Speed.
        MoveSpeed_up_3 = 37,              //Swift	30% increase to movement speed.
        Noukin = 38,                      //Musclehead	Attack +30%, Work Speed -50%
        PAL_ALLAttack_down1 = 39,         //Coward	Attack -10%
        PAL_ALLAttack_down2 = 40,         //Pacifist	Attack -20%
        PAL_ALLAttack_up1 = 41,           //Brave	Attack +10%
        PAL_ALLAttack_up2 = 42,           //Ferocious	Attack +20%
        PAL_conceited = 43,               //Conceited	Work Speed +10%, Defense -10%
        PAL_CorporateSlave = 44,          //Work Slave	Work Speed +30%, Attack -30%
        PAL_FullStomach_Down_1 = 45,      //Dainty Eater	Satiety drops +10% slower.
        PAL_FullStomach_Down_2 = 46,      //Diet Lover	Decrease in Hunger is less likely by +15%
        PAL_FullStomach_Up_1 = 47,        //Glutton	Satiety drops +10% faster.
        PAL_FullStomach_Up_2 = 48,        //Bottomless Stomach	Satiety drops +15% faster.
        PAL_masochist = 49,               //Masochist	Defense +15%, Attack -15%
        PAL_oraora = 50,                  //Aggressive	Attack +10%, Defense -10%
        PAL_rude = 51,                    //Hooligan	Attack +15%, Work Speed -10%
        PAL_sadist = 52,                  //Sadist	Attack +15%, Defense -15%
        PAL_Sanity_Down_1 = 53,           //Positive Thinker	SAN drops +10% slower.
        PAL_Sanity_Down_2 = 54,           //Workaholic	SAN drops +15% slower.
        PAL_Sanity_Up_1 = 55,             //Unstable	SAN drops +10% faster.
        PAL_Sanity_Up_2 = 56,             //Destructive	SAN drops +15% faster.
        Rare = 57,                        //Lucky	Work Speed +15%, Attack +15%
        TrainerATK_UP_1 = 58,             //Vanguard	10% increase to Player Attack.
        TrainerDEF_UP_1 = 59,             //Stronghold Strategist	10% increase to Player Defense.
        TrainerLogging_up1 = 60,          //Logging Foreman	25% increase to Player Logging Efficiency.
        TrainerMining_up1 = 61,           //Mine Foreman	25% increase to Player Mining Efficiency.
        TrainerWorkSpeed_UP_1 = 62,       //Motivational Leader	25% increase to Player Work Speed.
        Legend = 63,                      //Legend	Attack +20%, Defense +20%, Movement Speed increases 15%

        Witch = 64,                       //Siren of the Void   Siren of the Void	20% increase to Dark attack damage. 20% increase to Ice attack damage.
        NonKilling = 65,                  //Mercy Hit   Mercy Hit	Pacifist. Will not reduce the target's Health below 1.

        Alien = 66,                       //Otherworldly Cells  Attack +10%. Fire damage reduction 15%. Lightning damage reduction 15%.
        CoolTimeReduction_Down_1 = 67,    //Easygoing   Active Skill Cooldown extension -15%.
        CoolTimeReduction_Up_1 = 68,      //Serenity    Active skill cooldown reduction 30% Attack +10%.
        CoolTimeReduction_Up_2 = 69,      //Impatient   Active skill cooldown reduction 15%.
        EternalFlame = 70,                //Eternal Flame   30% increase to Fire attack damage. 30% increase Lightning attack damage.
        Nocturnal = 71,                   //Nocturnal   Does not sleep and continous to work even at night.
        SalePrice_Down_1 = 72,            //Shabby  Sale price -10%
        SalePrice_Up_1 = 73,              //Noble   Sale price +5%.
        SalePrice_Up_2 = 74,              //Fine Furs   Sale price +3%.
        Stamina_Down_1 = 75,              //Sickly  Max Stamina -25%.
        Stamina_Up_1 = 76,                //Infinite Stamina    Max Stamina +50%.
        Stamina_Up_2 = 77,                //Fit as a Fiddle     Max Stamina +25%.
        Test_PalEgg_HatchingSpeed_Up = 78,//Philantrophist  When assigned to a Breeding Farm, egg production time is reduced by 100%.

        PAL_FullStomach_Down_3 = 79,      //Mastery of Fasting  Hunger decreases +20% slower
        Invader = 80,                     //Invader     30% increase to Dark attack damage. 30% increase to Dragon attack damage
        Deffence_up3 = 81,                //Diamond Body    Defense +30%
        PAL_ALLAttack_up3 = 82,           //Demon God   Attack +30%. Defense +5%
        PAL_Sanity_Down_3 = 83,           //Heart of the Immovable King     SAN drops +20.0% slower.
        CraftSpeed_up3 = 84,              //Remarkable Craftsmanship    Work Speed +75%
        Stamina_Up_3 = 85,                //Eternal Engine  Max stamina +75%
        Vampire = 86,                     //Vampiric    Absorbs a portion of the damage dealt to restore Health. Does not sleep at night and continues to work
    }
}
