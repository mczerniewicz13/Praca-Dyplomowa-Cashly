using System;
using System.Collections.Generic;
using System.Text;

namespace App4.Models
{
    public class SpendingCategories
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public SpendingCategories(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
