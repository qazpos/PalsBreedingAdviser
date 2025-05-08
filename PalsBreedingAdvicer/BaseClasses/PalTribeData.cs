using PalsBreedingAdvicer.Properties.PalsData;
using PalworldSaveDecoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalsBreedingAdvicer
{
    public class PalTribeData
    {
        public PalTribeId TribeId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string? NumberSuffix { get; set; }



        public PalTribeData(PalTribeId tribeId)
        {
            TribeId = tribeId;
            var name = PalNames.ResourceManager.GetString(tribeId.ToString());
            Name = name ?? "Unknown pal";
        }


        public PalTribeData(PalTribeId tribeId, string name)
        {
            TribeId = tribeId;
            Name = name;
        }
    }
}
