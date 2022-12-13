using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using App4.Models;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class SignUpPageModel:FreshBasePageModel
    {
        private string WebAPIkey = "AIzaSyD8QwWxxXeotah-wMNNQsCwipOnD7DL_3U";

        public string Username { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
        public Command SignUpCommand { get; set; }
        public Command LoginCommand { get; set; }

        public SignUpPageModel()
        {
            SignUpCommand = new Command(()=>SignUpAction());
            LoginCommand = new Command(() => LoginAction());
        }

        private async void SignUpAction()
        {
            if(!CheckCredentials())
            {
                ClearPasswdFields();
            }
            else
            {
                try
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(UserEmail, Password);
                    CreateUser(auth.User.LocalId);
                    
                    await App.Current.MainPage.DisplayAlert("Success",
                        "Singed up successfully. You can log in to your account now","OK");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert",ex.Message, "OK");
                }
            }
            
        }
        private async void LoginAction()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private void ClearPasswdFields()
        {
            Password = "";
            RepeatedPassword = "";
        }
        private bool CheckCredentials()
        {
            if(string.IsNullOrEmpty(Username))
            {
                App.Current.MainPage.DisplayAlert("Alert", "Username cannot be empty", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(UserEmail))
            {
                App.Current.MainPage.DisplayAlert("Alert", "Email cannot be empty", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                App.Current.MainPage.DisplayAlert("Alert", "Password cannot be empty", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(RepeatedPassword))
            {
                App.Current.MainPage.DisplayAlert("Alert", "Repeated password cannot be empty", "OK");
                return false;
            }
            if (Password.Length < 6)
            {
                App.Current.MainPage.DisplayAlert("Alert", "Pasword must be 6 characters long", "OK");
                return false;
            }
            if(!IsValidEmail())
            {
                App.Current.MainPage.DisplayAlert("Alert", "Invalid Email", "OK");
                return false;
            }
            if(Password!=RepeatedPassword)
            {
                App.Current.MainPage.DisplayAlert("Alert", "Given passwords are not identical", "OK");
                return false;
            }
            return true;
        }

        private bool IsValidEmail()
        {
            var valid = true;
            try
            {
                var emailAddres = new MailAddress(UserEmail);
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        private void CreateUser(string uid)
        {
            var id = Guid.NewGuid();
            FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
            var user = new CashlyUser
            {
                Id = uid,
                username = Username,
                email = UserEmail,
                budget = 0,
                inviteCode = Guid.NewGuid().ToString().Substring(5, 6),
                groupId = -1,
                saldoId = uid,
                selectedPlan = -1

            };
            firebaseClient.Child("Users").PostAsync(user);
            var teskt = "sdaad";

        }
    }
}
