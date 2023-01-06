using App4.Models;
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
        public CashlyUser user { get; set; }

        public ProfilePageModel(CashlyUser user)
        {
            this.user = user;
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
            await App.Current.MainPage.Navigation.PushModalAsync(new BudgetPage(user));
        }

        private async void SaldoAction()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PushModalAsync(new SummaryPage(user));
        }

        private async void SettingsAction()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PushModalAsync(new BudgetPage(user));
        }
    }
}
