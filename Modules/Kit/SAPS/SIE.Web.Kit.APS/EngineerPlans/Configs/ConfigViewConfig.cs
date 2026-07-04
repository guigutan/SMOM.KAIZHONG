using SIE.Kit.APS.EngineerPlans.Configs;

namespace SIE.Web.Kit.APS.EngineerPlans.Configs
{
    public class MSaleOrderMiPlan_StartSoTime_ConfigValueViewConfig :WebViewConfig<EngineerPlan_SST_ConfigValue>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.StartRegisterDateTime)
                                    .UseDateEditor(p => p.Format = "Y/m/d")
                                    .Show(ShowInWhere.All);
        }
    }

    public class MSaleOrderMiPlan_NewEcnPre_ConfigValueViewConfig : WebViewConfig<EngineerPlan_NewEcnPre_ConfigValue>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.WithEcnPrecent).Show(ShowInWhere.All);
        }
    }

    public class MSaleOrderMiPlan_OldNeedWork_ConfigValueViewConfig : WebViewConfig<EngineerPlan_IsOverTimeTakeCapacity_ConfigValue>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsYes).Show(ShowInWhere.All);
        }
    }
}
