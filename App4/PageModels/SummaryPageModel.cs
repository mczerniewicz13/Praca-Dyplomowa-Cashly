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

namespace App4.PageModels
{
    public class SummaryPageModel : FreshBasePageModel, INotifyPropertyChanged
    {

        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");

        private string uid = Preferences.Get("AuthUserID", "");
        public CashlyUser user { get; set; }

        public ObservableCollection<Budget> ChartSpendings {get;set;}
        public ObservableCollection<Budget> ChartWeekSpendings { get; set; }
        public ObservableCollection<Budget> ChartMonthSpendings { get; set; }
        public bool ChartSpendingsNoDataLabel { get; set; }
        public bool ChartSpendingsVisible { get; set; }
        public bool ChartMonthSpendingsNoDataLabel { get; set; }
        public bool ChartMonthSpendingsVisible { get; set; }
        public bool ChartWeekSpendingsNoDataLabel { get; set; }
        public bool ChartWeekSpendingsVisible { get; set; }

        public double BudgetValue { get; set; }
        public double TotalIncome { get; set; }
        public double TotalSpendings { get; set; }

        public Command BackCommand { get; set; }
        public SummaryPageModel(CashlyUser user)
        {
            ChartSpendingsNoDataLabel = false;
            ChartSpendingsVisible = false;
            ChartMonthSpendingsNoDataLabel = false;
            ChartMonthSpendingsVisible = false;
            ChartWeekSpendingsNoDataLabel = false;
            ChartWeekSpendingsVisible = false;
            BudgetValue = 0;
            TotalIncome = 0;
            TotalIncome = 0;
            this.user = user;
            ChartSpendings = new ObservableCollection<Budget>();
            ChartMonthSpendings = new ObservableCollection<Budget>();
            ChartWeekSpendings = new ObservableCollection<Budget>();
            BackCommand = new Command(()=>OnBack());
            Task.Run(async () => await GetData()).Wait();

            
        }

        private void OnBack()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
        public void SetChartsVisibility()
        {
            if(ChartSpendings.Count>0)
            {
                ChartSpendingsVisible = true;
                ChartSpendingsNoDataLabel = false;
            }
            if(ChartSpendings.Count==0)
            {
                ChartSpendingsVisible = false;
                ChartSpendingsNoDataLabel = true;
            }

            if (ChartMonthSpendings.Count > 0)
            {
                ChartMonthSpendingsVisible = true;
                ChartMonthSpendingsNoDataLabel = false;
            }
            if (ChartMonthSpendings.Count == 0)
            {
                ChartMonthSpendingsVisible = false;
                ChartMonthSpendingsNoDataLabel = true;
            }

            if (ChartWeekSpendings.Count > 0)
            {
                ChartWeekSpendingsVisible = true;
                ChartWeekSpendingsNoDataLabel = false;
            }
            if (ChartWeekSpendings.Count == 0)
            {
                ChartWeekSpendingsVisible = false;
                ChartWeekSpendingsNoDataLabel = true;
            }
        }

        private async Task GetData()
        {
            var list = (await firebaseClient.Child("Budget")
                .OnceAsync<Budget>()).Select(x =>
                new Budget
                {
                    Id = x.Object.Id,
                    OwnerId = x.Object.OwnerId,
                    Title = x.Object.Title,
                    Category = x.Object.Category,
                    Description = x.Object.Description,
                    Value = x.Object.Value,
                    Date = x.Object.Date,
                    Direction = x.Object.Direction
                }).ToList();


            for (int i = 0; i < list.Count(); i++)
            {
                if (list.ElementAt(i).OwnerId == uid)
                {
                    if (list.ElementAt(i).Direction == 0)
                    {
                        TotalSpendings+=list.ElementAt(i).Value;
                        ChartSpendings.Add(list.ElementAt(i));
                        if (DateTime.Now.Month == list.ElementAt(i).Date.Month)
                        {
                            ChartMonthSpendings.Add(list.ElementAt(i));
                            if (DateTime.Now.Day - list.ElementAt(i).Date.Day <= 7)
                            {
                                ChartWeekSpendings.Add(list.ElementAt(i));
                            }
                        }
                    }

                    BudgetValue+=list.ElementAt(i).Value;
                    if(list.ElementAt(i).Direction == 1)
                    {
                        TotalIncome+=list.ElementAt(i).Value;
                    }
                }
            }

            SetChartsVisibility();
            
        }

        /*public void OnPropertyChanged(string name)=>
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));*/

    }
}
