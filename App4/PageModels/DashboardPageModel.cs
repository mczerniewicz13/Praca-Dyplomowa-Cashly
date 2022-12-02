using App4.PageModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App4.PageModels
{
    public class DashboardPageModel : PageModelBase
    {
        
        private ProfilePageModel profilePM;
        public ProfilePageModel ProfilePageModel
        {
            get => profilePM;
            set => SetProperty(ref profilePM, value);
        }

        private SettingsPageModel settingsPM;
        public SettingsPageModel SettingsPageModel
        {
            get => settingsPM;
            set => SetProperty(ref settingsPM, value);
        }

        private SummaryPageModel summaryPM;
        public SummaryPageModel SummaryPageModel
        {
            get => summaryPM;
            set => SetProperty(ref summaryPM, value);
        }

        private BudgetPageModel budgetPM;
        public BudgetPageModel BudgetPageModel
        {
            get => budgetPM;
            set => SetProperty(ref budgetPM, value);
        }

        public DashboardPageModel(ProfilePageModel profile, SettingsPageModel settings, SummaryPageModel summary, BudgetPageModel budget)
        {
            ProfilePageModel = profile;
            SettingsPageModel = settings;
            SummaryPageModel = summary; 
            BudgetPageModel = budget;
        }

        public override Task InitializeAsync(object NavigationDate = null)
        {

            return Task.WhenAny(base.InitializeAsync(NavigationDate),
                ProfilePageModel.InitializeAsync(null),
                SettingsPageModel.InitializeAsync(null),
                SummaryPageModel.InitializeAsync(null),
                BudgetPageModel.InitializeAsync(null));
        }

    }
}
