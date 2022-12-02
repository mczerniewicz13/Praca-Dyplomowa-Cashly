using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App4.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigation method to push onto the NAvigation Stack
        /// </summary>
        /// <typeparam name="TPageModelBase"></typeparam>
        /// <param name="navigationData"></param>
        /// <param name="setRoot"></param>
        /// <returns></returns>
        Task NavigateToAsync<TPageModelBase>(object navigationData = null, bool setRoot = false);

        /// <summary>
        /// Navigation method to pop off of the Navigation Stack
        /// </summary>
        /// <returns></returns>
        Task GoBackAsync();

    }
}
