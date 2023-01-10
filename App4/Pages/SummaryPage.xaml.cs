using App4.Models;
using App4.PageModels;
using Firebase.Database;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryPage : ContentPage, INotifyPropertyChanged
    {

        public SummaryPage(CashlyUser user)
        {

            InitializeComponent();
            BindingContext = new SummaryPageModel(user);
        }


    }
}