using App4.PageModels;
using App4.PageModels.Base;
using App4.Services.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.Licensing;

namespace App4
{
    public partial class App : Application
    {
        
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("NzcyNjU3QDMyMzAyZTMzMmUzMG1UVkVNV2xKekVpTlNGTTVzV0ZXSlVEdEE0cDBJZGNIZWZSY3lKRzlHQzQ9");
            InitializeComponent();

        }

        Task InitNavigation()
        {
            var navService = PageModelLocator.Resolve<INavigationService>();
            return navService.NavigateToAsync<LoginPageModel>();
        }

        protected override async void OnStart()
        {
            await InitNavigation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
