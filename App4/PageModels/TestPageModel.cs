using System;
using System.Collections.Generic;
using System.Text;

namespace App4.PageModels
{
    public class TestPageModel: FreshMvvm.FreshBasePageModel
    {
        public string Text { get; set; } = "Test123";
        public TestPageModel()
        {

        }
    }
}
