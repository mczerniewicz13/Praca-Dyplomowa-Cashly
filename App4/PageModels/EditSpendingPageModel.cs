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

        public EditSpendingPageModel(Spendings SelectedItem)
        { 
            selectedSpd = SelectedItem;
            SpendingTitle = selectedSpd.Title;
            SpendingDescription = selectedSpd.Description;
            SpendingDate = selectedSpd.Date;
            SpendingValue = selectedSpd.Value;

            EditCommand = new Command(() => EditAction());
            CancelCommand = new Command(() => CancelAction());
            DeleteCommand = new Command(() => DeleteAction());
        }
        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
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
            var newSpd = new Spendings
            {
                Id = id,
                Title = title,
                Description = description,
                Value = value,
                Date = date
            };
            
            await firebaseClient.Child("Spendings").Child(spd.Key).PatchAsync(newSpd);
            await Application.Current.MainPage.Navigation.PopModalAsync();
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

                await firebaseClient.Child("Spendings").Child(spd.Key).DeleteAsync();
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }


        }
    }
}
