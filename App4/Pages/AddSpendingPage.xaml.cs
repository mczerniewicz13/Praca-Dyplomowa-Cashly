using App4.Models;
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

namespace App4.Pages
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSpendingPage : ContentPage
    {
        public AddSpendingPage()
        {
            InitializeComponent();
            BindingContext = new AddSpendingPageModel();
        }

    }
}