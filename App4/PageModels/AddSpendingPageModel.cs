using App4.Models;
using App4.PageModels.Base;
using App4.Pages;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<string> Categories { get; set; }
        public string SpendingTitle { get; set; }
        public string SpendingDescription { get; set; }
        public double SpendingValue { get; set; }
        public DateTime SpendingDate { get; set; }
        public Guid id { get; set; }

        public string SelectedItem { get; set; }

        public Command AddCommand { get; set; }
        public Command CancelCommand { get; set; }
       

        public AddSpendingPageModel()
        {
            
            Categories = new List<string>();
            InitializeCategories();
            AddCommand = new Command(() => AddAction());
            CancelCommand = new Command(() => CancelAction());
            //CategorySelected = new Command(() => CatSelAction());
            SpendingDate = DateTime.Now;
            authProvider  = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
        }



        private void InitializeCategories()
        {
            Categories.Add("Food");
            Categories.Add("Bills");
            Categories.Add("Entertaiment");
            Categories.Add("Clothes");
            Categories.Add("Health");
            Categories.Add("Transportation");
            Categories.Add("Other");
        }

        private void CancelAction()
        {
            Application.Current.MainPage.Navigation.PopAsync();
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
                Id = FirebaseKeyGenerator.Next().ToString(),
                OwnerId = uid,
                Title = SpendingTitle,
                Description = SpendingDescription,
                Value = SpendingValue,
                Date = SpendingDate,
                Category = SelectedItem
            };
            await firebaseClient.Child("Spendings").PostAsync(spd);
            SpendingTitle = "";
            SpendingDescription = "";
            SpendingValue = 0;
            SpendingDate = DateTime.Now;
            /*SpendingTitle.Text = "";
            SpendingDescription.Text = "";
            SpendingDate.Date = DateTime.Now;
            SpendingValue.Value = 0;*/
            var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
            var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
            App.Current.MainPage.Navigation.RemovePage(navStack);
            App.Current.MainPage.Navigation.InsertPageBefore(new SummaryPage(), App.Current.MainPage.Navigation.NavigationStack.Last());
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }

}
