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
using Syncfusion.XForms.Buttons;

namespace App4.Pages
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSpendingPage : ContentPage
    {
        public AddSpendingPage(CashlyUser user)
        {
            InitializeComponent();
            this.ContentPage1.BackgroundColor = Color.LightPink;
            BindingContext = new AddSpendingPageModel(user);
        }

        private void SfSwitch_StateChanging(object sender, SwitchStateChangingEventArgs e)
        {
            if(e.OldValue == false)
            {
                this.ContentPage1.BackgroundColor = Color.LightGreen;
                this.Title.Text = "Add Income";
                this.stateEntry.Text = "1";

            }
            if(e.OldValue==true)
            {
                this.ContentPage1.BackgroundColor = Color.LightPink;
                this.Title.Text = "Add Spending";
                this.stateEntry.Text = "0";
            }
        }
    }
}