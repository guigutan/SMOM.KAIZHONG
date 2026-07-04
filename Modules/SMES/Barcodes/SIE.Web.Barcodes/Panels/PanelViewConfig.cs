using SIE.Barcodes.Panels;
using SIE.Domain;
using SIE.Web.Barcodes.Panels.Commands;

namespace SIE.Web.Barcodes.Panels
{
    /// <summary>
    /// 拼板码视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class PanelViewConfig : WebViewConfig<Panel>
    {
        #region 条码范围 SnRange
        /// <summary>
        /// 条码范围
        /// </summary>
        public static readonly Property<string> SnRangeProperty = P<Panel>.RegisterExtensionReadOnly("SnRange", typeof(PanelViewConfig),
            GetSnRangeProperty, Panel.RangeIdProperty);

        /// <summary>
        /// 获取条码范围
        /// </summary>
        /// <param name="me">条码</param>
        /// <returns>条码范围</returns>
        public static string GetSnRangeProperty(Panel me)
        {
            return $"{me.StartSn}-{me.EndSn}";
        }
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseGridSelectionModel();
            View.UseCommands(typeof(ReprintCommand).FullName, typeof(ScarpCommand).FullName, typeof(PendingCommand).FullName,typeof(UnPendingCommand).FullName, typeof(PanelBelongCommand).FullName);
            View.Property(p => p.Code).HasLabel("拼板码").Readonly().ShowInList(width: 150);
            View.Property(SnRangeProperty).HasLabel("条码范围").ShowInList(width: 250);
            View.Property(p => p.PrintDate).Readonly().ShowInList(width: 150);
            View.Property(p => p.PrintQty).Readonly();
            View.Property(p => p.PrintBy).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.IsScrap).Readonly();
            View.Property(p => p.ScrapReason).Readonly();
            View.Property(p => p.IsPending).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置视图
        }
    }
}