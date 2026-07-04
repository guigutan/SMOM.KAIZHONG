using SIE.MES.Workbench.KeyPerformances;
using SIE.Wpf.Resources;

namespace SIE.Wpf.MES.Workbench.KeyPerformances.TargetSettings
{
    public class ShopTargetViewConfig : WPFViewConfig<ShopTargetSetting>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseDefaultBehaviors();

            using (View.OrderProperties())
            {
                View.Property(p => p.WorkShop).HasLabel("车间编码").UseShopEditor();
                View.Property(p => p.WorkShop.Name).HasLabel("车间名称");
                View.Property(p => p.TargetSettingType);
                View.Property(p => p.TargetValue).UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 1; });
                View.Property(p => p.YellowAlert).UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 1; });
                View.Property(p => p.RedAlert).UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 1; });
            }
        }

        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.WorkShop);
                View.Property(p => p.TargetSettingType);
            }
        }
    }
}
