using App4.PageModels.Base;
using App4.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class SummaryPageModel : PageModelBase
    {
        private ICommand moveToAdd;
        private INavigationService navServ;

        public ICommand MoveToAdd
        {
            get=> moveToAdd;
            set =>SetProperty(ref moveToAdd, value);
        }
        public SummaryPageModel(INavigationService navigationService)
        {
            navServ = navigationService;
/*            MoveToAdd = new Command(OnAddBttnClicked);*/

        }
    }
}
