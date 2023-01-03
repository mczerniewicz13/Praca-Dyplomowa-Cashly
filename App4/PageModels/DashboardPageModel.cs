using App4.Models;
using App4.Pages;
using Firebase.Auth;
using Firebase.Database;
using FreshMvvm;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class DashboardPageModel : FreshBasePageModel, INotifyPropertyChanged
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        public string WelcomeMessage { get; set; }
        private string WebAPIkey = "AIzaSyD8QwWxxXeotah-wMNNQsCwipOnD7DL_3U";
        public double BudgetValue { get; set; }
        public string UserName { get; set; }
        public Command BudgetClicked { get; set; }
        public Command SpendingsClicked { get; set; }
        public Command ProfileClicked { get; set; }
        public Command SettingsClicked { get; set; }
        private string uid = Preferences.Get("AuthUserID", "");
        public CashlyUser user { get; set; }
        public DashboardPageModel()
        {
            Task.Run(async()=> await GetUserInfo()).Wait();
            //GetUserInfo();
             
            WelcomeMessage = "Welcome " + UserName + "!";
            BudgetClicked = new Command(()=>BudgetAction());
            SpendingsClicked = new Command(() => SpendingsAction());
            ProfileClicked = new Command(() => ProfileAction());
            SettingsClicked = new Command(() => SettingsAction());
            
        }

        private async void SettingsAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }

        private async void ProfileAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ProfilePage());
        }

        private async void SpendingsAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SummaryPage());
        }

        private async void BudgetAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new BudgetPage());
        }
        /*private async Task GetUser()
        {
            await GetUserInfo();
            await SetWelcome();
        }*/
        private async Task GetUserInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            
            try
            {
                var savedAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences
                    .Get("MyFirebaseRefreshToken",""));
               
                var refreshedContent = await authProvider.RefreshAuthAsync(savedAuth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));

                /*EmailUser = savedAuth.User.Email;*/

                /*var uid = savedAuth.User.LocalId;*/
                var user = (await firebaseClient
                    .Child("Users").OnceAsync<CashlyUser>())
                    .FirstOrDefault(u => u.Object.Id == uid);
                UserName = user.Object.username;
                BudgetValue = user.Object.budget;
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Alert","Token Expired"," OK");
            }
        }

    }
}
