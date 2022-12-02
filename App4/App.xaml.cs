using App4.PageModels;
using App4.PageModels.Base;
using App4.Services.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4
{
    public partial class App : Application
    {
        public App()
        {
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
