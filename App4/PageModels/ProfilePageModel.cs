using App4.PageModels.Base;
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
        public ProfilePageModel()
        {
            LogOutCommand = new Command(()=>LogOutAction());
        }

        private void LogOutAction()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Remove("AuthUserID");
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var navigationPage = new FreshNavigationContainer(page);
            App.Current.MainPage = navigationPage;
        }
    }
}
