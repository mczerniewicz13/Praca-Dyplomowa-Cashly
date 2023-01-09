using App4.Models;
using App4.Pages;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            Task.Run(async()=> await CheckCyclical()).Wait();
            //GetUserInfo();
            UserName = user.username;
            BudgetValue = user.budget;
            WelcomeMessage = "Welcome " + UserName + "!";
            BudgetClicked = new Command(()=>BudgetAction());
            SpendingsClicked = new Command(() => SpendingsAction());
            ProfileClicked = new Command(() => ProfileAction());
            SettingsClicked = new Command(() => SettingsAction());
            
        }

        private async void SettingsAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SettingsPage(user));
        }

        private async void ProfileAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ProfilePage(user));
        }

        private async void SpendingsAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SummaryPage(user));
        }

        private async void BudgetAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new BudgetPage(user));
        }
        /*private async Task GetUser()
        {
            await GetUserInfo();
            await SetWelcome();
        }*/

        private async Task CheckCyclical()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            var cyclicalExpenses = (await firebaseClient.Child("CyclicBudget")
                .OnceAsync<CyclicalBudget>()).Select(x =>
                new CyclicalBudget
                {
                    Id = x.Object.Id,
                    OwnerId = x.Object.OwnerId,
                    Title = x.Object.Title,
                    Category = x.Object.Category,
                    Description = x.Object.Description,
                    Value = x.Object.Value,
                    Date = x.Object.Date,
                    Direction = x.Object.Direction,
                    Cycle = x.Object.Cycle,
                    BeginDate = x.Object.BeginDate,
                    wasExecuted = x.Object.wasExecuted
                }).ToList();
            
            for (int i = 0; i < cyclicalExpenses.Count; i++)
            {
                var tmp = cyclicalExpenses.ElementAt(i);
                if(tmp != null && tmp.OwnerId == uid)
                {
                    if(tmp.wasExecuted == false)
                    {
                        if(tmp.Cycle=="Daily")
                        {
                            if(tmp.Date.AddDays(1).ToShortDateString() == DateTime.Now.ToShortDateString())
                            {
                                tmp.wasExecuted = true;
                                tmp.Date = DateTime.Now;
                                await AddCyclical(tmp);
                            }
                           
                        }
                        if(tmp.Cycle=="Weekly")
                        {
                            if(tmp.Date.DayOfWeek==DateTime.Now.DayOfWeek)
                            {
                                tmp.Date = DateTime.Now;
                                tmp.wasExecuted = true;
                                await AddCyclical(tmp);
                            }
                            
                        }
                        if(tmp.Cycle=="Monthly")
                        {
                            if(tmp.Date.AddDays(30) == DateTime.Now)
                            {
                                tmp.Date = DateTime.Now;
                                tmp.wasExecuted = true;
                                await AddCyclical(tmp);
                            }
                        }
                    }
                    if(tmp.wasExecuted==true)
                    {
                        if(tmp.Cycle == "Daily")
                        {
                            if(tmp.Date.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                tmp.Date = DateTime.Now;
                                await AddCyclical(tmp);
                            }
                        }
                        if(tmp.Cycle == "Weekly")
                        {
                            if(tmp.Date.DayOfWeek != DateTime.Now.DayOfWeek)
                            {
                                tmp.wasExecuted = false;
                            }
                        }
                        if(tmp.Cycle == "Monthly")
                        {
                            if(tmp.Date.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                tmp.wasExecuted = false;
                            }
                        }
                    }
                }
                var tmpspd = (await firebaseClient
                .Child("CyclicBudget").OnceAsync<CyclicalBudget>())
                .FirstOrDefault(s => s.Object.Id == tmp.Id);
                await firebaseClient.Child("CyclicBudget").Child(tmpspd.Key).PutAsync(tmp);
            }
        }

        private async Task AddCyclical(CyclicalBudget cb)
        {
            var spd = new Budget
            {
                Id = cb.Id,
                OwnerId = cb.OwnerId,
                Title = cb.Title,
                Value = cb.Value,
                Date = cb.Date,
                Description = cb.Description,
                Direction = cb.Direction,
                Category = cb.Category
            };
            BudgetValue += cb.Value;
            await firebaseClient.Child("Budget").PostAsync(spd);
        }
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
                var userData = (await firebaseClient
                    .Child("Users").OnceAsync<CashlyUser>())
                    .FirstOrDefault(u => u.Object.Id == uid);
                user = new CashlyUser();
                user.Id = userData.Object.Id;
                user.username = userData.Object.username;
                user.email = userData.Object.email;
                user.budget = userData.Object.budget;
                user.inviteCode = userData.Object.inviteCode;
                user.groupId = userData.Object.groupId;
                user.saldoId = userData.Object.saldoId;
                user.selectedPlan = userData.Object.selectedPlan;
                UserName = user.username;
                BudgetValue = user.budget;

            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Alert","Token Expired"," OK");
            }
        }

    }
}
