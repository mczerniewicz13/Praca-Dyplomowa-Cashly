using App4.Models;
using App4.PageModels;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSpendingPage : ContentPage
    {
       public EditSpendingPage(Budget SelectedSpedning, CashlyUser user)
        {
            InitializeComponent();
            if (SelectedSpedning.Direction == 0)
            {
                this.ContentPage1.BackgroundColor = Color.LightPink;
                this.Title.Text = "Edit Spending";
                this.@switch.IsOn = false;
            }
            else if(SelectedSpedning.Direction == 1)
            {
                this.ContentPage1.BackgroundColor = Color.LightGreen;
                this.Title.Text = "Edit Income";
                this.@switch.IsOn = true;
            }
            BindingContext = new EditSpendingPageModel(SelectedSpedning,user);
         }

        private void SfSwitch_StateChanging(object sender, Syncfusion.XForms.Buttons.SwitchStateChangingEventArgs e)
        {
            if (e.NewValue == true)
            {
                this.ContentPage1.BackgroundColor = Color.LightGreen;
                this.Title.Text = "Edit Income";
                this.stateEntry.Text = "1";

            }
            if (e.NewValue == false)
            {
                this.ContentPage1.BackgroundColor = Color.LightPink;
                this.Title.Text = "Edit Spending";
                this.stateEntry.Text = "0";
            }
        }
    }
}