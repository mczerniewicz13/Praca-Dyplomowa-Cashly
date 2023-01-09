using App4.Models;
using App4.PageModels.Base;
using App4.Services.Navigation;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FreshMvvm;
using System.Collections.ObjectModel;
using System.Linq;
using App4.Pages;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Reactive.Linq;
using Syncfusion.DataSource.Extensions;
using Firebase.Database.Streaming;

namespace App4.PageModels
{
    public class BudgetPageModel : FreshBasePageModel, INotifyPropertyChanged
    {

        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
        FirebaseClient firebaseClient1 = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");


        public IDisposable collection { get; set; }
        public ObservableCollection<Budget> DatabaseItems { get; set; } /*= new
            ObservableCollection<Spendings>();*/
        public List<Budget> Spendings { get; set; }
        public Budget SelectedItem { get; set; }
        public double BudgetValue { get; set; }

        public bool isBusy { get; set; }

        public string Title { get; set; }
        public string Value { get; set; }

        public string Category { get; set; }
        public CashlyUser user { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Command GoToAddPageCommand { get; set; }
        public Command SelectItemCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command CyclicalExpensesCommand { get; set; }
        public Color BudgetColor { get; set; }
        private string uid = Preferences.Get("AuthUserID", "");

        public BudgetPageModel(CollectionView colView, CashlyUser user)
        {
            this.user = user;
            DatabaseItems = new ObservableCollection<Budget>();
            GoToAddPageCommand = new Command(() => OpenAddPageAsync());
            SelectItemCommand = new Command(() => OnSelectionAsync());
            BackCommand = new Command(() => OnBack());
            CyclicalExpensesCommand = new Command(()=> CyclicalExpensesAction());
            Spendings = new List<Budget>();
            Task.Run(async () => await GetBudgetList()).Wait();
            Task.Run(async () => await SetBudget()).Wait();
                
            collection = firebaseClient
                .Child("Budget")
                .AsObservable<Budget>()
                .Subscribe((dbevent) =>
                {

                    if (dbevent.Object != null && dbevent.Object.OwnerId == uid)
                    {
                        DatabaseItems.Add(dbevent.Object);
                    }
                });
            user.budget = BudgetValue;
            user.UpdateBudget();
            if (BudgetValue>0)
            {
                BudgetColor = Color.LightGreen;
            }
            if(BudgetValue==0)
            {
                BudgetColor = Color.LightGray;
            }
            if(BudgetValue<0)
            {
                BudgetColor = Color.LightPink;
            }

        }
        
        private void CyclicalExpensesAction()
        {
            App.Current.MainPage.Navigation.PushAsync(new CyclicalExpensesPage(user));
        }


        private void OnBack()
        {
            var navstack = App.Current.MainPage.Navigation.NavigationStack[0];
            App.Current.MainPage.Navigation.RemovePage(navstack);
            App.Current.MainPage.Navigation.InsertPageBefore(new DashboardPage(), App.Current.MainPage.Navigation.NavigationStack.Last());
            App.Current.MainPage.Navigation.PopAsync();

        }
        private async Task SetBudget()
        {
            BudgetValue = 0;
            for (int i = 0; i < Spendings.Count; i++)
            {
                BudgetValue += Spendings[i].Value;
            }
        }
        private async Task GetBudgetList()
        {
            var list = (await firebaseClient1.Child("Budget")
                .OnceAsync<Budget>()).Select(x =>
                new Budget
                {
                    Id = x.Object.Id,
                    OwnerId = x.Object.OwnerId,
                    Title = x.Object.Title,
                    Category = x.Object.Category,
                    Description=x.Object.Description,
                    Value=x.Object.Value,
                    Date= x.Object.Date,
                    Direction=x.Object.Direction
                }).ToList(); 
            for(int i = 0;i<list.Count();i++)
            {
                if(list.ElementAt(i).OwnerId == uid)
                {
                    
                    Spendings.Add(list.ElementAt(i));
                }    
            }


        }
        public void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        public async void OpenAddPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddSpendingPage(user));
            //var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
            //var navStack = App.Current.MainPage.Navigation.NavigationStack[0];
           // App.Current.MainPage.Navigation.RemovePage(navStack);
            //App.Current.MainPage.Navigation
               // .InsertPageBefore(new DashboardPage(),
                //App.Current.MainPage.Navigation.NavigationStack.Last());

        }

        private void OnSelectionAsync()
        {
            var sel = SelectedItem;
            Application.Current.MainPage.Navigation.PushAsync(new EditSpendingPage(sel,user));
            //var pageBefore = App.Current.MainPage.Navigation.NavigationStack.Count - 2;
            var navStack = App.Current.MainPage.Navigation.NavigationStack[0];
            App.Current.MainPage.Navigation.RemovePage(navStack);
            App.Current.MainPage.Navigation
                .InsertPageBefore(new DashboardPage(),
                App.Current.MainPage.Navigation.NavigationStack.Last());



        }


    }
}
