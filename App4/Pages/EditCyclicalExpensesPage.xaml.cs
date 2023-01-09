using App4.Models;
using App4.PageModels;
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
    public partial class EditCyclicalExpensesPage : ContentPage
    {
        public EditCyclicalExpensesPage(CyclicalBudget SelectedSpending, CashlyUser user)
        {
            InitializeComponent();
            BindingContext = new EditCyclicalExpensesPageModel(SelectedSpending, user);
            if (SelectedSpending.Direction == 0)
            {
                this.ContentPage1.BackgroundColor = Color.LightPink;
                this.Title.Text = "Edit Spending";
                this.@switch.IsOn = false;
            }
            else if (SelectedSpending.Direction == 1)
            {
                this.ContentPage1.BackgroundColor = Color.LightGreen;
                this.Title.Text = "Edit Income";
                this.@switch.IsOn = true;
            }
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