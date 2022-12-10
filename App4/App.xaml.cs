using App4.PageModels;
using App4.PageModels.Base;
using App4.Services.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.Licensing;
using FreshMvvm;

namespace App4
{
    public partial class App : Application
    {
        
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("NzcyNjU3QDMyMzAyZTMzMmUzMG1UVkVNV2xKekVpTlNGTTVzV0ZXSlVEdEE0cDBJZGNIZWZSY3lKRzlHQzQ9");
            InitializeComponent();
            var page = FreshPageModelResolver.ResolvePageModel<DashboardPageModel>();
            var navigationPage = new FreshNavigationContainer(page);
            /*var navigationPage = new FreshTabbedNavigationContainer();
            navigationPage.AddTab<BudgetPageModel>("Budget", "dollar.png");
            navigationPage.AddTab<SummaryPageModel>("Summary", "payment.png");
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;*/
            MainPage = navigationPage;
            
        }

        Task InitNavigation()
        {
            var navService = PageModelLocator.Resolve<INavigationService>();
            return navService.NavigateToAsync<LoginPageModel>();
        }

        protected override void OnStart()
        {
            //await InitNavigation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
