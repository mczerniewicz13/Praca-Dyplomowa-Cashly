using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App4.Models;
using App4.Pages;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class EditSpendingPageModel : FreshBasePageModel
    {
        
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        
        public Budget selectedSpd { get; set; }
        public string SpendingTitle { get; set; }
        public string SpendingDescription { get; set; }
        public double SpendingValue { get; set; }
        public DateTime SpendingDate { get; set; }

        public Command EditCommand { get; set; }
        public Command CancelCommand { get; set; }

        public Command DeleteCommand { get; set; }
        public string SpendingCategory { get; set; }
        public List<string> Categories { get; set; }

        public string state { get; set; }
        public CashlyUser user { get; set; }

        private int multiplier { get; set; }
        private int dir { get; set; }
        public EditSpendingPageModel(Budget SelectedItem,CashlyUser user)
        {
            this.user = user;
            selectedSpd = SelectedItem;
            SpendingTitle = selectedSpd.Title;
            SpendingDescription = selectedSpd.Description;
            SpendingDate = selectedSpd.Date;
            SpendingValue = Math.Abs(selectedSpd.Value);
            SpendingCategory = selectedSpd.Category;
            Categories = new List<string>();
            InitializeCategories();
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

        private void InitializeCategories()
        {
            Categories.Add("Food");
            Categories.Add("Bills");
            Categories.Add("Entertaiment");
            Categories.Add("Clothes");
            Categories.Add("Health");
            Categories.Add("Transportation");
            Categories.Add("Salary");
            Categories.Add("Other Income");
            Categories.Add("Other Spending");
        }
        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void EditAction()
        {

            if (state == "1")
            {
                dir = 1;
                multiplier = 1;
            }
            if(state =="0")
            {
                multiplier = -1;
                dir = 0;
            }
            
            var spd = (await firebaseClient
                .Child("Budget").OnceAsync<Budget>())
                .FirstOrDefault(s => s.Object.Id == selectedSpd.Id);

            var title = SpendingTitle;
            var description = SpendingDescription;
            
            var value = Convert.ToDouble(SpendingValue.ToString());
            if (multiplier > 0 && selectedSpd.Value <= 0)
            {
                    value = Math.Abs(value);
            }
            else if(multiplier < 0 && selectedSpd.Value >= 0)
            {
                value = value * multiplier;
            }
            else if(multiplier < 0 && selectedSpd.Value<=0)
            {
                value = -1*SpendingValue;
            }


            var date = SpendingDate.Date;
            var id = spd.Object.Id;
            var ownerid = spd.Object.OwnerId;
            var cat = SpendingCategory;
            var newSpd = new Budget
            {
                Id = id,
                OwnerId = ownerid,
                Title = title,
                Description = description,
                Value = value,
                Date = date,
                Category = cat,
                Direction = dir
            };

            var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
            var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
            App.Current.MainPage.Navigation.RemovePage(navStack);
            await firebaseClient.Child("Budget").Child(spd.Key).PutAsync(newSpd);
            App.Current.MainPage.Navigation.InsertPageBefore(new BudgetPage(user), App.Current.MainPage.Navigation.NavigationStack.Last());
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            bool delete = await App.Current.MainPage.DisplayAlert("Warning", 
                "Do you want to delete this item?","Yes", "No");
            if (delete)
            {
                var spd = (await firebaseClient
                .Child("Budget").OnceAsync<Budget>())
                .FirstOrDefault(s => s.Object.Id == selectedSpd.Id);
                var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
                var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
                App.Current.MainPage.Navigation.RemovePage(navStack);
                await firebaseClient.Child("Budget").Child(spd.Key).DeleteAsync();
                App.Current.MainPage.Navigation.InsertPageBefore(new BudgetPage(user), App.Current.MainPage.Navigation.NavigationStack.Last());
                await Application.Current.MainPage.Navigation.PopAsync();
            }


        }
    }
}
