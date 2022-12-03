using App4.Models;
using Firebase.Database;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        IDisposable collection;
        public ObservableCollection<Spendings> DatabaseItems { get; set; } = new 
            ObservableCollection<Spendings>();
        public Spendings SelectedItem { get; set; }
        public SummaryPage()
        {
            DatabaseItems.Clear();
            InitializeComponent();
            BindingContext = this;
            collection = firebaseClient
                .Child("Spendings")
                .AsObservable<Spendings>()
                .Subscribe((dbevent) =>
                {
                    if (dbevent.Object != null)
                    {
                        
                        DatabaseItems.Add(dbevent.Object);
                    }
                });
        
        
        }
        public async void NavigateAddBttn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddSpendingPage());
        }

        private void OnSelection(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = (Spendings)e.CurrentSelection.FirstOrDefault();
            Edit_Clicked(sender,e);
            RefreshCollection();
        }

        private void RefreshCollection()
        {
            DatabaseItems.Clear();
            collection = firebaseClient.Child("Spendings")
                .AsObservable<Spendings>()
                .Subscribe((dbevent) =>
                {
                    if (dbevent.Object != null)
                    {
                        DatabaseItems.Add(dbevent.Object);
                    }
                });
        }
        private async void Edit_Clicked(object sender, EventArgs e)
        {         
            await Navigation.PushAsync(new EditSpendingPage(SelectedItem));
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            RefreshCollection();
        }
    }
}