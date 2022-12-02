using App4.PageModels.Base;
using App4.Services.Account;
using App4.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class LoginPageModel : PageModelBase
    {

        private ICommand signInCommand;
        private INavigationService navServ;
        private IAccountService accServ;
        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get => loginCommand;
            set => SetProperty(ref loginCommand, value);
        }

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ICommand SignInCommand
        {
            get => signInCommand;
            set => SetProperty(ref signInCommand, value);
        }

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
