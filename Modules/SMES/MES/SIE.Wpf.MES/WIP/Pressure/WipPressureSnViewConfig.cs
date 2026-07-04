using SIE.MES.WIP.Pressure;
using SIE.Wpf.MES.WIP.Pressure;

namespace SIE.Web.MES.WIP.Pressure
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipPressureSnViewConfig : WPFViewConfig<WipPressureSn>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-0.65, -0.35);
            View.ClearCommands();
            //View.UseCommands(typeof(RePrintSnCommand));
            using (View.OrderProperties())
            {

                View.Property(p => p.Sn).ShowInList(200).Readonly();
                View.Property(p => p.BatchNo).ShowInList(120).Readonly();
                View.Property(p => p.ResourceName).ShowInList(120).Readonly();
                View.Property(p => p.TestResult).ShowInList(120).Readonly();
                View.Property(p => p.TestTime).ShowInList(150).Readonly();
                View.Property(p => p.RawData).ShowInList(300).Readonly();
                View.Property(p => p.IsOver).ShowInList().Readonly();

                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);

                View.ChildrenProperty(p => p.TestValueList).HasLabel("测试结果").Visible(false);

            }
        }
    }
}