using App4.Models;
using App4.PageModels.Base;
using Firebase.Database;
using Firebase.Database.Query;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App4.PageModels
{

    public class AddSpendingPageModel : FreshBasePageModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
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
            id = Guid.NewGuid();
            var spd = new Spendings
            {
                Id = id,
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
