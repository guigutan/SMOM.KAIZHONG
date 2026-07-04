using SIE.Domain;
using SIE.MES.Workbench.KeyPerformances;
using SIE.Resources.Enterprises;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Workbench.KeyPerformances.TargetSettings
{
    public class LineTargetViewConfig : WPFViewConfig<LineTargetSetting>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().RemoveCommands(typeof(ListSaveCommand));
            View.UseDefaultBehaviors();

            using (View.OrderProperties())
            {
                View.Property(p => p.Line).HasLabel("产线编码").UseDataSource((e, c, r) =>
                {
                    var setting = e as LineTargetSetting;
                    if (setting?.ShopPlanRPSetting?.WorkShop != null)
                        return RT.Service.Resolve<EnterpriseController>().GetResources(c, r, setting.ShopPlanRPSetting.WorkShopId);
                    return new EntityList<Resource>();
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; p.DisplayMember = Resource.CodeProperty.Name; }); 
                View.Property(p => p.Line.Name).HasLabel("产线名称");
                View.Property(p => p.TargetSettingType);
                View.Property(p => p.TargetValue).UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 1; });
            }
        }
    }
}
