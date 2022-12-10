using App4.PageModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App4.PageModels
{
    public class ProfilePageModel : FreshMvvm.FreshBasePageModel
    {
        public Command TestCommand { get; set; }
        public ProfilePageModel()
        {
            TestCommand = new Command(()=>
            {
                CoreMethods.PushPageModel<TestPageModel>();
            });
        }
    }
}
