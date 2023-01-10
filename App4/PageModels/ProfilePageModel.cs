using App4.Models;
using App4.PageModels.Base;
using App4.Pages;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class ProfilePageModel : FreshBasePageModel, INotifyPropertyChanged
    {
        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyD8QwWxxXeotah-wMNNQsCwipOnD7DL_3U"));
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        string username;
        string email;
        string password;
        string inviteCode;
        double budget;
        CashlyUser user;

        

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public string InviteCode
        {
            get { return inviteCode; }
            set
            {
                inviteCode = value;
                OnPropertyChanged("InviteCode");
            }
        }

        public double Budget
        {
            get { return budget; }
            set
            {
                budget = value;
                OnPropertyChanged("Budget");
            }
        }

        public CashlyUser User
        {
            get { return user; }
            set 
            {
                user = value;
                OnPropertyChanged("User");
            }
        }


        public Command LogOutCommand { get; set; }
        public Command UsernameClicked { get; set; }
        public Command EmailClicked { get; set; }
        public Command PasswordClicked { get; set; }
        public Command BackCommand { get; set; }
        public Command InviteCodeClicked { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ProfilePageModel(CashlyUser user)
        {
            User = user;
            LogOutCommand = new Command(() =>LogOutAction());
            UsernameClicked = new Command(() => UsernameAction());
            EmailClicked = new Command(() => EmailAction());
            PasswordClicked = new Command(() => PasswordAction());
            InviteCodeClicked = new Command(()=>InviteCodeAction());
            BackCommand = new Command(()=>OnBack());
            Username = User.username;
            Email = User.email;
            Budget = User.budget;
            InviteCode = User.inviteCode;
            Password = "*****";
        }
        private async void InviteCodeAction()
        {
            await Clipboard.SetTextAsync(InviteCode);
            await App.Current.MainPage.DisplayAlert("Invite Code", "Invite Code copied to clipboard!", "OK");
        }
        private void OnBack()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
        private async void UsernameAction()
        {
            string result = await App.Current.MainPage.DisplayPromptAsync("Change your username",
                "","Edit","Cancel",Username,-1,default,Username);
            if(result!="")
            {
                Username = result;
                this.User.username = Username;
                var tmp = (await firebaseClient.Child("Users").OnceAsync<CashlyUser>())
                    .FirstOrDefault(u => u.Object.Id==User.Id);
                await firebaseClient.Child("Users").Child(tmp.Key).PutAsync(this.User);
            }
        }
        private async void EmailAction()
        {
            //not doable due to no support from the library side
        }
        private async void PasswordAction()
        {
            //not doable due to no support from the library side
        }




        private void LogOutAction()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Remove("AuthUserID");
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var navigationPage = new FreshNavigationContainer(page);
            App.Current.MainPage = navigationPage;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
