using App4.PageModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace App4.PageModels
{
    public class DashboardPageModel : FreshBasePageModel
    {
        private string WebAPIkey = "AIzaSyD8QwWxxXeotah-wMNNQsCwipOnD7DL_3U";
        //public string EmailUser { get; set; }
        public DashboardPageModel()
        {
            GetUserInfo();
        }
        private async void GetUserInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var savedAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences
                    .Get("MyFirebaseRefreshToken",""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedAuth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                /*EmailUser = savedAuth.User.Email;*/

            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Alert","Token Expired"," OK");
            }
        }

    }
}
