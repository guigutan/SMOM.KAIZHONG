using SIE.Domain;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspLogs;
using SIE.Wpf.ProductIntfc.FirstInsps.Commands;

namespace SIE.Wpf.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 报检条码日志明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class FirstInspDetailViewConfig : WPFViewConfig<InspBarcodeLog>
    {
        /// <summary>
        /// 报检单明细视图
        /// </summary>
        public const string FirstInspDetailView = "FirstInspDetailView";

        #region 已报检 Inspection
        /// <summary>
        /// 已报检
        /// 已报检不能选择
        /// </summary>
        [Label("自定义")]
        public static readonly Property<bool> InspectionProperty = P<InspBarcodeLog>.RegisterExtensionReadOnly("Inspection", typeof(FirstInspDetailViewConfig),
            GetInspection, InspBarcodeLog.IdProperty);

        /// <summary>
        /// 按产品分批
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        public static bool GetInspection(InspBarcodeLog me)
        {
            return me.InspState != InspState.UnInspection;
        }
        #endregion 

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(FirstInspDetailView);
            if (ViewGroup == FirstInspDetailView)
                ConfigFirstInspDetailView();
        }

        /// <summary>
        /// 配置首检报检明细视图
        /// </summary>
        private void ConfigFirstInspDetailView()
        {
            View.AssignAuthorize(typeof(SIE.ProductIntfc.FirstInsps.FirstInsp));
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(FirstInspCommand), WPFCommandNames.Export);
                View.AddBehavior(typeof(InspRecords.InspBarcodeViewBehavior));
                View.Property(p => p.Barcode).ShowInList();
                View.Property(p => p.Process).ShowInList();
                View.Property(p => p.CollectionDate).UseListSetting(e => e.ListGridWidth = 180).ShowInList();
                View.Property(p => p.InspState).ShowInList();
                View.Property(p => p.InspectionDate).UseListSetting(e => e.ListGridWidth = 180).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}