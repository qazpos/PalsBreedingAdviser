using PalsBreedingAdvicer.Properties.PalsData;
using PalworldSaveDecoding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PalsBreedingAdvicer
{
    public class PalPassiveSkillToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Null value";
            if (value is PalPassiveSkill palPassiveSkill) {
                var result = PalPassiveSkillDescriptions.ResourceManager.GetString(palPassiveSkill.ToString());
                return result == null ? "Unknon skill" : result;
            }
            return "Unknown value";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
