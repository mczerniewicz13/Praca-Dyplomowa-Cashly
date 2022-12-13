using System;
using System.Collections.Generic;
using System.Text;

namespace App4.Models
{
    public class CashlyUser
    {
        public string Id { get; set; }
        public string UID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public double budget { get; set; }
        public string inviteCode { get; set; }
        public int groupId { get; set; }
        public string saldoId { get; set; }

        public int selectedPlan { get; set; }
    }
}
