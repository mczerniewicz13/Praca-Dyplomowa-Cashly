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
    public partial class BudgetPage : ContentPage
    {
        public CollectionView selItem { get; set; }
        public BudgetPage(CashlyUser user)
        {
            InitializeComponent();
            BindingContext = new BudgetPageModel(selItem, user);
        }

        
    }
}