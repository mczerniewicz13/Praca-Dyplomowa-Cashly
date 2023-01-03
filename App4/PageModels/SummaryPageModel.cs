﻿using App4.Models;
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

namespace App4.PageModels
{
    public class SummaryPageModel : FreshBasePageModel, INotifyPropertyChanged
    {

        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");
      

        public IDisposable collection { get; set; }
        public ObservableCollection<Spendings> DatabaseItems { get; set; } /*= new
            ObservableCollection<Spendings>();*/
        public List<Spendings> Spendings { get; set; }
        public Spendings SelectedItem { get; set; }

        public CollectionView ColView { get; set; }
        public double Budget { get; set; } = 1000;

        public bool isBusy { get;set; }

        public string Title { get; set; }
        public string Value { get; set; }

        public string Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Command GoToAddPageCommand { get; set; }
        public Command SelectItemCommand { get; set; }

        private string uid = Preferences.Get("AuthUserID", "");

        public SummaryPageModel(CollectionView colView)
        {
            DatabaseItems = new ObservableCollection<Spendings>();
            GoToAddPageCommand = new Command(() => OpenAddPageAsync());
            SelectItemCommand = new Command(() => OnSelectionAsync());
            ColView = colView;

            /*Refresh();*/
            //DatabaseItems.Clear();
            collection = firebaseClient
                .Child("Spendings")
                .AsObservable<Spendings>()
                .Subscribe((dbevent) =>
                {

                    if (dbevent.Object != null && dbevent.Object.OwnerId == uid)
                    {
                        DatabaseItems.Add(dbevent.Object);//EDIT
                    }
                });

            Spendings = DatabaseItems.ToList();
            ColView = colView;
        }

        public void OnPropertyChanged(string name)=>
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));

    

        public async void OpenAddPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddSpendingPage());
        }

        private void OnSelectionAsync()
        {
            var sel = SelectedItem;
            Application.Current.MainPage.Navigation.PushAsync(new EditSpendingPage(sel));

            
        }    
        

    }
}
