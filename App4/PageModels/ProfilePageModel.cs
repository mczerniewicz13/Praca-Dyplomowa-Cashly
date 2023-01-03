using App4.PageModels.Base;
using App4.Pages;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class ProfilePageModel : FreshBasePageModel
    {
        public Command LogOutCommand { get; set; }
        public Command BudgetCommand { get; set; }
        public Command SaldoCommand { get; set; }
        public Command SettingsCommand { get; set; }

        public ProfilePageModel()
        {
            LogOutCommand = new Command(()=>LogOutAction());

            BudgetCommand = new Command(() => BudgetAction());
            SaldoCommand = new Command(() => SaldoAction());
            SettingsCommand = new Command(() => SettingsAction());
        }

        private void LogOutAction()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Remove("AuthUserID");
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var navigationPage = new FreshNavigationContainer(page);
            App.Current.MainPage = navigationPage;
        }

        private async void BudgetAction()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PushModalAsync(new BudgetPage());
        }

        private async void SaldoAction()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PushModalAsync(new SummaryPage());
        }

        private async void SettingsAction()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PushModalAsync(new BudgetPage());
        }
    }
}
