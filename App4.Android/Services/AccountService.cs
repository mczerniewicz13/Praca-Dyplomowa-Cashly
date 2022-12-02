using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App4.Models;
using App4.Services.Account;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Firestore;
using App4.Droid.OnServiceListeners;

[assembly:Dependency(typeof(AccountService))]
namespace App4.Droid.Services
{
    public class AccountService : IAccountService
    {
        public Task<AuthenticatedUser> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();
            FirebaseFirestore.Instance.Collection("users")
                .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                .Get().AddOnCompleteListener(new OnCompleteListener(tcs));

            return tcs.Task;
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            var tcs = new TaskCompletionSource<bool>();
            FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));
            return tcs.Task;
        }

        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if(task.IsCanceled || task.IsFaulted)
            {
                tcs.SetResult(false);
                return;
            }
            tcs.SetResult(true);
        }
    }
}