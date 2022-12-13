using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App4.Models;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4.Droid.OnServiceListeners
{
    public class OnCompleteListener : Java.Lang.Object
    {
        private TaskCompletionSource<CashlyUser> _tcs;

        public OnCompleteListener(TaskCompletionSource <CashlyUser> tcs)
        {
            _tcs = tcs;
        }
        
    }
}