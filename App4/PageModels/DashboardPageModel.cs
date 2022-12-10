using App4.PageModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
namespace App4.PageModels
{
    public class DashboardPageModel : FreshBasePageModel
    {
        
        /*private ProfilePageModel profilePM;
        public ProfilePageModel ProfilePageModel { get; set; }

        private SettingsPageModel settingsPM;
        public SettingsPageModel SettingsPageModel { get; set; }

        private SummaryPageModel summaryPM;
        public SummaryPageModel SummaryPageModel { get; set; }

        private BudgetPageModel budgetPM;
        public BudgetPageModel BudgetPageModel { get; set; }
*/
        public DashboardPageModel(/*ProfilePageModel profile, SettingsPageModel settings, SummaryPageModel summary, BudgetPageModel budget*/)
        {
            /*ProfilePageModel = profile;
            SettingsPageModel = settings;
            SummaryPageModel = summary; 
            BudgetPageModel = budget;*/


        }



        /*public override Task InitializeAsync(object NavigationDate = null)
        {

            return Task.WhenAny(base.InitializeAsync(NavigationDate),
                ProfilePageModel.InitializeAsync(null),
                SettingsPageModel.InitializeAsync(null),
                SummaryPageModel.InitializeAsync(null),
                BudgetPageModel.InitializeAsync(null));
        }*/

    }
}
