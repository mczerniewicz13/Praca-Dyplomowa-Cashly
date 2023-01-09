using App4.Models;
using App4.Pages;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class AddCyclicalExpensesPageModel:FreshBasePageModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");

        public string state { get; set; }
        public string SpendingTitle { get; set; }
        public string SpendingDescription { get; set; }
        public double SpendingValue { get; set; }
        public bool StartNowCheck { get; set; }
        public DateTime SpendingDate { get; set; }
        public DateTime AddDate { get; set; }
        public string SelectedCycle { get; set; }
        public List<string> Cycles { get; set; }
        public CashlyUser user { get; set; }
        public List<string> Categories { get; set; }
        public string SelectedItem { get; set; }
        public Command AddCommand { get; set; }
        public Command CancelCommand { get; set; }
        public AddCyclicalExpensesPageModel(CashlyUser user)
        {
            this.user = user;
            AddDate = DateTime.Now;
            SpendingDate = DateTime.Now;
            Categories = new List<string>();
            Cycles = new List<string>();
            InitializeCategories();
            InitializeCycles();
            AddCommand = new Command(()=>AddAction());
            CancelCommand = new Command(()=>CancelAction());
            StartNowCheck = false;
        }

        private void InitializeCategories()
        {
            Categories.Add("Groceries");
            Categories.Add("Food");
            Categories.Add("Bills");
            Categories.Add("Entertaiment");
            Categories.Add("Clothes");
            Categories.Add("Health");
            Categories.Add("Transportation");
            Categories.Add("Animals");
            Categories.Add("Plants");
            Categories.Add("Education");
            Categories.Add("Maintenance");
            Categories.Add("Insurance");
            Categories.Add("Kids");
            Categories.Add("Hobby");
            Categories.Add("Travel");
            Categories.Add("Salary");
            Categories.Add("Gift");
            Categories.Add("Other");
        }

        private void InitializeCycles()
        {
            Cycles.Add("Daily");
            Cycles.Add("Weekly");
            Cycles.Add("Monthly");
        }

        private void AddAction()
        {
            var multiplier = -1;
            var dir = 0;
            if (state == "1")
            {
                dir = 1;
                multiplier = 1;
            }


            var uid = Preferences.Get("AuthUserID", "");
            var cyclic = new CyclicalBudget
            {
                Id = FirebaseKeyGenerator.Next().ToString(),
                OwnerId = uid,
                Title = SpendingTitle,
                Description = SpendingDescription,
                Value = SpendingValue * multiplier,
                Date = AddDate,
                Category = SelectedItem,
                Direction = dir,
                BeginDate = SpendingDate,
                Cycle = SelectedCycle,
                wasExecuted = false

            };
            if (StartNowCheck)
            {
                cyclic.wasExecuted = true;
                var spd = new Budget
                {
                    Id=FirebaseKeyGenerator.Next().ToString(),
                    OwnerId=uid,
                    Title = SpendingTitle,
                    Description= SpendingDescription,
                    Value= SpendingValue * multiplier,
                    Date= AddDate,
                    Category= SelectedItem,
                    Direction= dir,
                };
                firebaseClient.Child("Budget").PostAsync(spd);
            }
            firebaseClient.Child("CyclicBudget").PostAsync(cyclic);
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
            
            App.Current.MainPage.Navigation.InsertPageBefore(new CyclicalExpensesPage(user), App.Current.MainPage.Navigation.NavigationStack.Last());
            Application.Current.MainPage.Navigation.PopAsync();

        }

        private void CancelAction()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }

    }
}
