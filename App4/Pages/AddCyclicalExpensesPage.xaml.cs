using App4.Models;
using App4.PageModels;
using Syncfusion.XForms.Buttons;
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
    public partial class AddCyclicalExpensesPage : ContentPage
    {
        public AddCyclicalExpensesPage(CashlyUser user)
        {
            InitializeComponent();
            this.ContentPage1.BackgroundColor = Color.LightPink;
            BindingContext = new AddCyclicalExpensesPageModel(user);
        }
        private void SfSwitch_StateChanging(object sender, SwitchStateChangingEventArgs e)
        {
            if (e.OldValue == false)
            {
                this.ContentPage1.BackgroundColor = Color.LightGreen;
                this.Title.Text = "Add Income";
                this.stateEntry.Text = "1";

            }
            if (e.OldValue == true)
            {
                this.ContentPage1.BackgroundColor = Color.LightPink;
                this.Title.Text = "Add Spending";
                this.stateEntry.Text = "0";
            }
        }
    }
}