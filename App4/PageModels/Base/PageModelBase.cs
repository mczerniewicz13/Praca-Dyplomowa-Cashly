using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4.PageModels.Base
{
    public class PageModelBase : BindableObject
    {

        string title;
        public string Title
        {
            get => title;
            set =>SetProperty(ref title, value);
        }
        bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public virtual Task InitializeAsync(object NavigationDate = null)
        {
            return Task.CompletedTask;
        }


        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        
    }
}
