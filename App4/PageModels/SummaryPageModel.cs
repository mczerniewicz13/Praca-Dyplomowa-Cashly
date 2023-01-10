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


        CashlyUser user;
        ObservableCollection<Budget> chartMonthSpendings;
        ObservableCollection<Budget> chartSpendings;
        bool chartSpendingsNoDataLabel;
        bool chartSpendingsVisible;
        bool chartMonthSpendingsVisible;
        bool chartMonthSpendingsNoDataLabel;
        string selectedChartMonth;
        string chartTitle;
        int selectedMonthIndex;
        double budgetValue;
        double totalIncome; 
        double totalSpendings;
        List<string> chartMonths;
        

        public CashlyUser User
        {
            get { return user; }
            set
            {
                user = value;
            }
        }
      
        public ObservableCollection<Budget> ChartSpendings
        {
            get { return chartSpendings; }
            set
            {
                chartSpendings = value;
                OnPropertyChanged("ChartSpendings");
            }
        }

        public ObservableCollection<Budget> ChartMonthSpendings 
        { 
            get { return chartMonthSpendings; } 
            set
            {
                ChartMonthSpendings = value;
                OnPropertyChanged("ChartMonthSpendings");
            }
        } 
        
        public bool ChartSpendingsNoDataLabel 
        { 
            get { return chartSpendingsNoDataLabel; }
            set 
            {
                chartSpendingsNoDataLabel = value;
                OnPropertyChanged("ChartSpendingsNoDataLabel");
            } 
        }

        public bool ChartSpendingsVisible 
        { 
            get { return chartSpendingsVisible; } 
            set
            {
                chartSpendingsVisible = value;
                OnPropertyChanged("ChartSpendingsVisible");
            }
        }

        public bool ChartMonthSpendingsVisible 
        { 
            get { return chartMonthSpendingsVisible; }
            set
            {
                chartMonthSpendingsVisible = value;
                OnPropertyChanged("ChartMonthSpendingsVisible");
            }
        }

        public string SelectedChartMonth 
        { 
            get { return selectedChartMonth; }
            set
            {
                selectedChartMonth = value;
                OnPropertyChanged("SelectedChartMonth");
            }
        }

        public List<string> ChartMonths 
        { 
            get { return chartMonths; }
            set
            {
                chartMonths = value;
                OnPropertyChanged("ChartMonths");
            }
        }

        public string ChartTitle 
        { 
            get { return chartTitle; }
            set
            {
                chartTitle = value;
                OnPropertyChanged("ChartTitle");
            }
        }

        public bool ChartMonthSpendingsNoDataLabel 
        { 
            get { return chartMonthSpendingsNoDataLabel; } 
            set 
            {
                chartMonthSpendingsNoDataLabel= value;
                OnPropertyChanged("ChartMonthSpendingsNoDataLabel");
            }
        }

        public int SelectedMonthIndex 
        { 
            get { return selectedMonthIndex; } 
            set
            {
                selectedMonthIndex = value;
                OnPropertyChanged("SelectedMonthIndex");
            }
        }

        public double BudgetValue 
        { 
            get { return budgetValue; }
            set
            {
                budgetValue = value;
                OnPropertyChanged("BudgetValue");
            }
        }

        public double TotalIncome 
        {
            get { return totalIncome; } 
            set
            {
                totalIncome = value;
                OnPropertyChanged("TotalIncome");
            }
        }

        public double TotalSpendings 
        { 
            get { return totalSpendings; }
            set 
            { 
                totalSpendings = value;
                OnPropertyChanged("TotalSpendings");
            } 
        }

        public Command ChartMonthSelectionCommand { get; set; }
        public Command BackCommand { get; set; }



#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        public SummaryPageModel(CashlyUser user)
        {
            ChartSpendingsNoDataLabel = false;
            ChartSpendingsVisible = false;
            ChartMonthSpendingsVisible = false;
            SelectedChartMonth = "";
            SelectedMonthIndex = -1;
            BudgetValue = 0;
            TotalIncome = 0;
            TotalIncome = 0;
            this.user = user;
            chartSpendings = new ObservableCollection<Budget>();
            chartMonthSpendings = new ObservableCollection<Budget>();
            ChartMonthSelectionCommand = new Command(() => ChartMonthSelection());
            BackCommand = new Command(() => OnBack());
            chartMonths = new List<string>();
            ChartMonthsInit();
            Task.Run(async () => await GetData()).Wait();


        }
        private void ChartMonthsInit()
        {
            ChartMonths.Add("January");
            ChartMonths.Add("February");
            ChartMonths.Add("March");
            ChartMonths.Add("April");
            ChartMonths.Add("May");
            ChartMonths.Add("June");
            ChartMonths.Add("July");
            ChartMonths.Add("August");
            ChartMonths.Add("September");
            ChartMonths.Add("October");
            ChartMonths.Add("November");
            ChartMonths.Add("December");
        }
        public async void ChartMonthSelection()
        {
            ChartSpendingsNoDataLabel = false;
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
            
                ChartMonthSpendings.Clear();
                ChartMonthSpendingsVisible = true;
                ChartTitle = "Spendings in " + ChartMonths[SelectedMonthIndex];
                for(int i =0;i<list.Count;i++)
                {
                    var tmp = list.ElementAt(i);
                    if(tmp.OwnerId==uid && tmp.Direction==0)
                    {
                        if(tmp.Date.Month == SelectedMonthIndex+1)
                        {
                            ChartMonthSpendings.Add(tmp);
                        }
                    }
                }
                if(ChartMonthSpendings.Count==0)
                {
                    ChartMonthSpendingsVisible = false;
                    ChartSpendingsNoDataLabel = true;
                }
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

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
