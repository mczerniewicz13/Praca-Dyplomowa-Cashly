using App4.PageModels.Base;
using App4.Services.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App4.PageModels
{
    public class BudgetPageModel : PageModelBase
    {
        private IAccountService accountService;
        public BudgetPageModel(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            var user = await accountService.GetUserAsync();
            if(user != null)
            {

            }
        }
    }
}
