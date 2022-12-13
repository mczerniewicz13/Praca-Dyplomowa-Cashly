using App4.PageModels.Base;
using App4.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using FreshMvvm;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using App4.Pages;

namespace App4.PageModels
{
    public class LoginPageModel : FreshBasePageModel
    {
        public string WebAPIkey = "AIzaSyD8QwWxxXeotah-wMNNQsCwipOnD7DL_3U";
        public string Username { get; set; }
        public string Password { get; set; }

        public Command LoginCommand { get; set; }
        public Command SignUpCommand { get; set; }
        public Command ForgotCommand { get; set; }
        public LoginPageModel()
        {
            LoginCommand = new Command(()=>LoginAction());
            SignUpCommand = new Command(() => SignUpAction());
        }

        public async void LoginAction()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Username, Password);
                var content = await auth.GetFreshAuthAsync();
                var serialized = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serialized);
                Preferences.Set("AuthUserID", auth.User.LocalId);
                await App.Current.MainPage.Navigation.PushAsync(new DashboardPage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid credentials", "OK");
            }
            
        }

        public async void SignUpAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }

  
    }
}
