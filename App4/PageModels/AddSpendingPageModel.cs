using App4.Models;
using App4.PageModels.Base;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{

    public class AddSpendingPageModel : FreshBasePageModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        FirebaseAuthProvider authProvider { get; set; }
        private string WebAPIkey = "AIzaSyD8QwWxxXeotah-wMNNQsCwipOnD7DL_3U";

        public string SpendingTitle { get; set; }
        public string SpendingDescription { get; set; }
        public double SpendingValue { get; set; }
        public DateTime SpedningDate { get; set; }
        public Guid id { get; set; }

        public Command AddCommand { get; set; }
        public Command CancelCommand { get; set; }

        public AddSpendingPageModel()
        {
            AddCommand = new Command(() => AddAction());
            CancelCommand = new Command(() => CancelAction());
            authProvider  = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
        }


        private void CancelAction()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void AddAction()
        {

            /*var title = SpendingTitle.Text;
            var description = SpendingDescription.Text;
            var value = Convert.ToDouble(SpendingValue.Value.ToString());
            var date = SpendingDate.Date;*/
            var uid = Preferences.Get("AuthUserID", "");
            var spd = new Spendings
            {
                Id = uid,
                Title = SpendingTitle,
                Description = SpendingDescription,
                Value = SpendingValue,
                Date = SpedningDate
            };
            await firebaseClient.Child("Spendings").PostAsync(spd);
            SpendingTitle = "";
            SpendingDescription = "";
            SpendingValue = 0;
            SpedningDate = DateTime.Now;
            /*SpendingTitle.Text = "";
            SpendingDescription.Text = "";
            SpendingDate.Date = DateTime.Now;
            SpendingValue.Value = 0;*/
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

}
