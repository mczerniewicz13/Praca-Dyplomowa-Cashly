using App4.Models;
using App4.Pages;
using Firebase.Database;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class CyclicalExpensesPageModel:FreshBasePageModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        public ObservableCollection<CyclicalBudget> CyclicalExpensesItems { get; set; }
        public IDisposable collection { get; set; }
        public Command BackCommand { get; set; }
        public Command GoToAddPageCommand { get; set; }
        public Command SelectItemCommand { get; set; }
        public CyclicalBudget SelectedItem { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Cycle { get; set; }
        public CashlyUser user { get; set; }
        private string uid = Preferences.Get("AuthUserID", "");
        public CyclicalExpensesPageModel(CashlyUser user)
        {
            this.user = user;
            CyclicalExpensesItems = new ObservableCollection<CyclicalBudget>();
            BackCommand = new Command(()=>OnBack());
            GoToAddPageCommand = new Command(()=>OnAdd());
            SelectItemCommand = new Command(()=>OnSelect());
            collection = firebaseClient
                .Child("CyclicBudget")
                .AsObservable<CyclicalBudget>()
                .Subscribe((dbevent) =>
                {

                    if (dbevent.Object != null && dbevent.Object.OwnerId == uid)
                    {
                        CyclicalExpensesItems.Add(dbevent.Object);
                    }
                });

        }

        private void OnBack()
        {
            var count = App.Current.MainPage.Navigation.NavigationStack.Count;
            var navstack = App.Current.MainPage.Navigation.NavigationStack[count - 2];
            App.Current.MainPage.Navigation.RemovePage(navstack);
            App.Current.MainPage.Navigation.InsertPageBefore(new BudgetPage(user), App.Current.MainPage.Navigation.NavigationStack.Last());
            App.Current.MainPage.Navigation.PopAsync();
        }

        private void OnAdd()
        {
            App.Current.MainPage.Navigation.PushAsync(new AddCyclicalExpensesPage(user));
        }

        private void OnSelect()
        {
            App.Current.MainPage.Navigation.PushAsync(new EditCyclicalExpensesPage(SelectedItem, user));
        }

    }
}
