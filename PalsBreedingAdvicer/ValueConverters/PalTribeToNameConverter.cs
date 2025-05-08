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
    internal class PalTribeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Null value";
            if (value is PalTribeId tribeId) {
                var result = PalNames.ResourceManager.GetString(tribeId.ToString());
                return result == null ? "Unknon pal" : result;
            }
            return "Unknown value";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
