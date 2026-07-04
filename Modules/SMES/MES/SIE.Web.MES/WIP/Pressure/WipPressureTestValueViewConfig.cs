using SIE.MES.WIP.Pressure;

namespace SIE.Web.MES.WIP.Pressure
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipPressureTestValueViewConfig : WebViewConfig<WipPressureTestValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {

                View.Property(p => p.TestItem).ShowInList(180).Readonly();
                View.Property(p => p.Value).ShowInList(120).Readonly();
                View.Property(p => p.TestResult).ShowInList(120).Readonly();

                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);

            }
        }
    }
}