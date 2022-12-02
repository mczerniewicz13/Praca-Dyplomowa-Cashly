using App4.PageModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace App4.PageModels
{
    public class AddSpendingPageModel :PageModelBase
    {
        private ICommand addCommand;
        public ICommand AddCommand
        {
            get => addCommand;
            set => SetProperty(ref addCommand, value);
        }


    }

}
