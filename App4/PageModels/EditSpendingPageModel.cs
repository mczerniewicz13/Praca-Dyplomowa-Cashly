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
        public Spendings selectedSpd { get; set; }
        public string SpendingTitle { get; set; }
        public string SpendingDescription { get; set; }
        public double SpendingValue { get; set; }
        public DateTime SpendingDate { get; set; }

        public Command EditCommand { get; set; }
        public Command CancelCommand { get; set; }

        public Command DeleteCommand { get; set; }
        public string SpendingCategory { get; set; }
        public List<string> Categories { get; set; }
        public EditSpendingPageModel(Spendings SelectedItem)
        { 
            selectedSpd = SelectedItem;
            SpendingTitle = selectedSpd.Title;
            SpendingDescription = selectedSpd.Description;
            SpendingDate = selectedSpd.Date;
            SpendingValue = selectedSpd.Value;
            SpendingCategory = selectedSpd.Category;
            Categories = new List<string>();
            InitializeCategories();
            EditCommand = new Command(() => EditAction());
            CancelCommand = new Command(() => CancelAction());
            DeleteCommand = new Command(() => DeleteAction());
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
        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void EditAction()
        {
            
            
            var spd = (await firebaseClient
                .Child("Spendings").OnceAsync<Spendings>())
                .FirstOrDefault(s => s.Object.Id == selectedSpd.Id);

            var title = SpendingTitle;
            var description = SpendingDescription;
            var value = Convert.ToDouble(SpendingValue.ToString());
            var date = SpendingDate.Date;
            var id = spd.Object.Id;
            var ownerid = spd.Object.OwnerId;
            var cat = SpendingCategory;
            var newSpd = new Spendings
            {
                Id = id,
                OwnerId = ownerid,
                Title = title,
                Description = description,
                Value = value,
                Date = date,
                Category = cat
            };

            var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
            var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
            App.Current.MainPage.Navigation.RemovePage(navStack);
            await firebaseClient.Child("Spendings").Child(spd.Key).PutAsync(newSpd);
            App.Current.MainPage.Navigation.InsertPageBefore(new SummaryPage(), App.Current.MainPage.Navigation.NavigationStack.Last());
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            bool delete = await App.Current.MainPage.DisplayAlert("Warning", 
                "Do you want to delete this item?","Yes", "No");
            if (delete)
            {
                var spd = (await firebaseClient
                .Child("Spendings").OnceAsync<Spendings>())
                .FirstOrDefault(s => s.Object.Id == selectedSpd.Id);
                var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
                var navStack = App.Current.MainPage.Navigation.NavigationStack[pageBefore];
                App.Current.MainPage.Navigation.RemovePage(navStack);
                await firebaseClient.Child("Spendings").Child(spd.Key).DeleteAsync();
                App.Current.MainPage.Navigation.InsertPageBefore(new SummaryPage(), App.Current.MainPage.Navigation.NavigationStack.Last());
                await Application.Current.MainPage.Navigation.PopAsync();
            }


        }
    }
}
