using App4.Models;
using App4.Pages;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class EditCyclicalExpensesPageModel:FreshBasePageModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");

        public CyclicalBudget selectedSpd { get; set; }
        public string SpendingTitle { get; set; }
        public string SpendingDescription { get; set; }
        public double SpendingValue { get; set; }
        public DateTime SpendingDate { get; set; }

        public Command EditCommand { get; set; }
        public Command CancelCommand { get; set; }

        public Command DeleteCommand { get; set; }
        public string SpendingCategory { get; set; }
        public List<string> Categories { get; set; }
        public bool StartNowCheck { get; set; }
        public List<string> Cycles { get; set; }
        public string state { get; set; }
        public string SelectedCycle { get; set; }
        public CashlyUser user { get; set; }
        public FirebaseObject<CyclicalBudget> spd { get; set; }
        private int multiplier { get; set; }
        private int dir { get; set; }
        public EditCyclicalExpensesPageModel(CyclicalBudget SelectedItem, CashlyUser user)
        {
            this.user = user;
            StartNowCheck = false;
            selectedSpd = SelectedItem;
            SpendingTitle = selectedSpd.Title;
            SpendingDescription = selectedSpd.Description;
            SpendingDate = selectedSpd.Date;
            SpendingValue = Math.Abs(selectedSpd.Value);
            SpendingCategory = selectedSpd.Category;
            SelectedCycle = selectedSpd.Cycle;
            Categories = new List<string>();
            Cycles = new List<string>();
            InitializeCategories();
            InitializeCycles();
            Task.Run(async () => await GetItem()).Wait();
            EditCommand = new Command(() => EditAction());
            CancelCommand = new Command(() => CancelAction());
            DeleteCommand = new Command(() => DeleteAction());
            if (selectedSpd.Value > 0)
            {
                dir = 1;
                multiplier = 1;
            }
            if (selectedSpd.Value <= 0)
            {
                dir = 0;
                multiplier = -1;
            }
        }

        private void EditAction()
        {
            if (state == "1")
            {
                dir = 1;
                multiplier = 1;
            }
            if (state == "0")
            {
                multiplier = -1;
                dir = 0;
            }


            var title = SpendingTitle;
            var description = SpendingDescription;

            var value = Convert.ToDouble(SpendingValue.ToString());
            if (multiplier > 0 && selectedSpd.Value <= 0)
            {
                value = Math.Abs(value);
            }
            else if (multiplier < 0 && selectedSpd.Value >= 0)
            {
                value = value * multiplier;
            }
            else if (multiplier < 0 && selectedSpd.Value <= 0)
            {
                value = -1 * SpendingValue;
            }


            var date = SpendingDate.Date;
            var id = spd.Object.Id;
            var ownerid = spd.Object.OwnerId;
            var cat = SpendingCategory;
            var cyclic = new CyclicalBudget
            {
                Id = id,
                OwnerId = ownerid,
                Title = title,
                Description = description,
                Value = value,
                Date = date,
                Category = cat,
                Direction = dir,
                BeginDate = DateTime.Now,
                Cycle = SelectedCycle,
                wasExecuted=false
            };
            var uid = Preferences.Get("AuthUserID", "");
            if (StartNowCheck)
            {
                cyclic.wasExecuted = true;
                var spd = new Budget
                {
                    Id = FirebaseKeyGenerator.Next().ToString(),
                    OwnerId = uid,
                    Title = SpendingTitle,
                    Description = SpendingDescription,
                    Value = SpendingValue * multiplier,
                    Date = date,
                    Category = SpendingCategory,
                    Direction = dir,
                };
                firebaseClient.Child("Budget").PostAsync(spd);
            }


            var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
            var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
            App.Current.MainPage.Navigation.RemovePage(navStack);
            firebaseClient.Child("CyclicBudget").Child(spd.Key).PutAsync(cyclic);
            App.Current.MainPage.Navigation.InsertPageBefore(new CyclicalExpensesPage(user), App.Current.MainPage.Navigation.NavigationStack.Last());
            Application.Current.MainPage.Navigation.PopAsync();
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
        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async Task GetItem()
        {

            var tmpspd = (await firebaseClient
                 .Child("CyclicBudget").OnceAsync<CyclicalBudget>())
                 .FirstOrDefault(s => s.Object.Id == selectedSpd.Id);
            spd = tmpspd;

        }
        private async void DeleteAction()
        {
            var delete = await App.Current.MainPage.DisplayAlert("Warning",
                "Do you want to delete this item?", "Yes", "No");
            if (delete)
            {
                var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
                var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
                App.Current.MainPage.Navigation.RemovePage(navStack);
                firebaseClient.Child("CyclicBudget").Child(spd.Key).DeleteAsync().Wait();
                App.Current.MainPage.Navigation.InsertPageBefore(new CyclicalExpensesPage(user), App.Current.MainPage.Navigation.NavigationStack.Last());
                Application.Current.MainPage.Navigation.PopAsync();
            }


        }
    }
}
