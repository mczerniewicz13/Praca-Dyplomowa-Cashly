using System;
using System.Collections.Generic;
using System.Text;

namespace App4.Models
{
    public class CyclicalBudget
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Cycle { get; set; } //daily, weekly,monthly
        public DateTime BeginDate { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public int Direction { get; set; } //0 is a spending, 1 is an income
        public bool wasExecuted { get; internal set; }
    }
}
