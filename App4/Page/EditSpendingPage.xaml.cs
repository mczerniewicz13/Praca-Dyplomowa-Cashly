using App4.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSpendingPage : ContentPage
    {
        Spendings selectedSpd;
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        public EditSpendingPage(Spendings SelectedItem)
        {
            InitializeComponent();
            selectedSpd = SelectedItem;
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            
            var spd = (await firebaseClient
                .Child("Spendings").OnceAsync<Spendings>())
                .FirstOrDefault( s => s.Object.Id == selectedSpd.Id);

            var title = SpendingTitle.Text;
            var description = SpendingDescription.Text;
            var value = Convert.ToDouble(SpendingValue.Value.ToString());
            var date = SpendingDate.Date;
            var id =spd.Object.Id;
            var newSpd = new Spendings
            {
                Id = id,
                Title = title,
                Description = description,
                Value = value,
                Date = date
            };
            await firebaseClient.Child("Spendings").Child(spd.Key).PutAsync(newSpd);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}