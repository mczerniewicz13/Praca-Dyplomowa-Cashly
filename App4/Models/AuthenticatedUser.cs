using System;
using System.Collections.Generic;
using System.Text;

namespace App4.Models
{
    public class AuthenticatedUser
    {
        public int Id { get; set; }
        public string nickname { get; set; }

        public string email { get; set; }
        public double budget { get; set; }
        public string inviteCode { get; set; }
        public int groupId { get; set; }
        public int saldoId { get; set; }

        public int selectedPlan { get; set; }
    }
}
