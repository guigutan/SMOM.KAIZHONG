using SIE.CSM.ItemInspCharacteristicses;
using SIE.Security;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.PackingQC;
using SIE.Wpf.MES.WIP.SuspectReport.Commands;
using System;
using System.Linq.Expressions;

namespace SIE.Wpf.MES.WIP.SuspectReport
{
    /// <summary>
    /// 可疑品报工 配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class SuspectReportViewModelViewConfig : WPFViewConfig<SuspectReportViewModel>
    {
        /// <summary>
        /// IsSn为ture时只读
        /// </summary>
        public static readonly Expression<Func<SuspectReportViewModel, bool>> IsReadonlySuspectQtyProperty = p => p.IsSn == true;

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SuspectReportViewModel), typeof(PackingQcViewModel));
            if (ViewGroup == CollectionUITemplate.CollectionUIViewGroup)
                ConfigDetailsView();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands().UseCommands(typeof(SubmitSuspectReportCommand), typeof(PrintSuspectLabelCommand));
            View.UseDetail(columnCount: 6);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("报工信息".L10N(), collapsable: true, detailColumnCount: 1))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail(columnSpan: 3, height: 30).Readonly();
                    View.Property(p => p.DispatchTask.No).HasLabel("任务单号").ShowInDetail(columnSpan: 3, height: 30).Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 6, height: 30).Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 6, height: 30).Readonly();
                    View.Property(p => p.WipBatch.BatchNo).HasLabel("工序标签").ShowInDetail(columnSpan: 3, height: 30).Readonly();
                    View.Property(p => p.WipPressureSn.Sn).HasLabel("耐压标签").ShowInDetail(columnSpan: 3, height: 30).Readonly();
                    View.Property(p => p.SuspectQty).ShowInDetail(columnSpan: 6, height: 30).UseSpinEditor(p => { p.MinValue = 0; }).Readonly(IsReadonlySuspectQtyProperty);

                }
                using (View.DeclareGroup("打印信息".L10N(), collapsable: true, detailColumnCount: 1))
                {
                    View.Property(p => p.Template).HasLabel("打印模板").ShowInDetail(columnSpan: 3, height: 30).Readonly();
                    View.Property(p => p.Printer).HasLabel("打印机").ShowInDetail(columnSpan: 3, height: 30).UsePrinterExEditor();

                }
            }
        }

    }
}
