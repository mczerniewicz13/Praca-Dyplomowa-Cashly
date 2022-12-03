﻿using App4.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App4.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Page
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSpendingPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        public AddSpendingPage()
        {
            InitializeComponent();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            var title = SpendingTitle.Text;
            var description = SpendingDescription.Text;
            var value = Convert.ToDouble(SpendingValue.Value.ToString());
            var date = SpendingDate.Date;
            var id = Guid.NewGuid();
            var spd = new Spendings
            {
                Id = id,
                Title = title,
                Description = description,
                Value = value,
                Date = date
            };
            await firebaseClient.Child("Spendings").PostAsync(spd);
            SpendingTitle.Text = "";
            SpendingDescription.Text = "";
            SpendingDate.Date = DateTime.Now;
            SpendingValue.Value = 0;
            //Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}