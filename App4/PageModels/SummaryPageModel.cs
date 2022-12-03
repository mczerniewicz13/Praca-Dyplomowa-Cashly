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

namespace App4.PageModels
{
    public class SummaryPageModel : PageModelBase
    {
        private ICommand moveToAdd;
        private INavigationService navServ;
        FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");

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
