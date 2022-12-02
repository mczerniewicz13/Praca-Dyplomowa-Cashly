using App4.Page;
using App4.Services.Account;
using App4.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TinyIoC;
using Xamarin.Forms;

namespace App4.PageModels.Base
{
    public class PageModelLocator
    {
        static TinyIoCContainer container;
        static Dictionary<Type, Type> viewLookup;

        static PageModelLocator()
        {
            viewLookup = new Dictionary<Type, Type>();
            container = new TinyIoCContainer();

            //Register pages and pagemodels
            Register<LoginPageModel, LoginPage>();
            Register<DashboardPageModel,DashboardPage>();
            Register<ProfilePageModel, ProfilePage>();
            Register<SettingsPageModel,SettingsPage>();
            Register<SummaryPageModel, SummaryPage>();
            Register<BudgetPageModel, BudgetPage>();
            Register<AddSpendingPageModel, AddSpendingPage>();

            //Register services (services are registered as singletons by default)
            container.Register<INavigationService, NavigationService>();
            container.Register<IAccountService>(DependencyService.Get<IAccountService>());
            
        }

        public static T Resolve<T>() where T : class
        {
            return container.Resolve<T>();
        }

        public static Xamarin.Forms.Page CreatePageFor(Type pageModelType)
        {
            var pageType = viewLookup[pageModelType];
            var page  = (Xamarin.Forms.Page)Activator.CreateInstance(pageType);
            var pageModel = container.Resolve(pageModelType);
            page.BindingContext = pageModel;
            return page;
        }

        static void Register<TPageModel, TPage>() where TPageModel : PageModelBase where TPage: Xamarin.Forms.Page
        {
            viewLookup.Add(typeof(TPageModel), typeof(TPage));
            container.Register<TPageModel>();

        }
    }
}
