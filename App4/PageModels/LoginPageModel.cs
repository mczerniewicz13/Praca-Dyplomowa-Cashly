using App4.PageModels.Base;
using App4.Services.Account;
using App4.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using FreshMvvm;

namespace App4.PageModels
{
    public class LoginPageModel : FreshBasePageModel
    {

        private ICommand signInCommand;
        private INavigationService navServ;
        private IAccountService accServ;
        private ICommand loginCommand;
        public ICommand LoginCommand { get; set; }
        

        private string username;
        public string Username { get; set; }

        private string password;
        public string Password { get; set; }

        public ICommand SignInCommand { get; set; }

        public LoginPageModel(INavigationService navigationService, IAccountService accountService)
        {
            navServ = navigationService;
            accServ = accountService;
            LoginCommand = new Command(OnLoginAction);
        }

        private async void OnLoginAction(object obj)
        {
            var loginAttempt = await accServ.LoginAsync(Username, Password);
            if(loginAttempt)
            {
                await navServ.NavigateToAsync<DashboardPageModel>();
            }
            else
            {
                //TODO : Display alert for failure
            }
            
        }
    }
}
