using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace App4.Models
{
    public class CashlyUser
    {
        private FirebaseClient firebaseClient = new FirebaseClient("https://cashly-9d2ac-default-rtdb.europe-west1.firebasedatabase.app/");

        public string Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public double budget { get; set; }
        public string inviteCode { get; set; }
        public int groupId { get; set; }
        public string saldoId { get; set; }

        public int selectedPlan { get; set; }

        public async void UpdateBudget()
        {
            var dbId = (await firebaseClient.Child("Users")
                .OnceAsync<CashlyUser>()).FirstOrDefault(u => u.Object.Id == Id);
            await firebaseClient.Child("Users").Child(dbId.Key).PutAsync(this);
        }
    }
}
