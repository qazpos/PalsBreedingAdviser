using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace PalsBreedingAdvicer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var culture = new CultureInfo(Config.Instance.GetCurrentCultureCode());
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
            //    new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("en-US")));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Config.Instance.Save();
        }
    }
}
