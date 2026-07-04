using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.WIP;
using System;

namespace SIE.Wpf.MES.PanelBindings
{
    /// <summary>
    /// 工单条码绑定视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class PanelBindingViewModelViewConfig : WPFViewConfig<PanelBindingViewModel>
    {
        #region 板号只读 BoardNoReadOnly
        /// <summary>
        /// 板号只读
        /// </summary>
        [Label("板号只读")]
        public static readonly Property<bool> BoardNoReadOnlyProperty = P<PanelBindingViewModel>.RegisterExtensionReadOnly("BoardNoReadOnly", typeof(PanelBindingViewModelViewConfig),
            GetBoardNoReadOnly, PanelBindingViewModel.ForkPlateQtyProperty);

        /// <summary>
        /// 板号只读
        /// </summary>
        public static bool GetBoardNoReadOnly(PanelBindingViewModel me)
        {
            return me.ForkPlateQty == 0;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(PanelBindingViewModel));
            View.ClearCommands();
            View.UseCommands(typeof(CollectRestartCommand));
            View.UseDetail(columnCount: 5);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 5, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 5, height: 0, hideLabel: true);
                }
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);
                    View.Property(p => p.UnBindingPanel).UseBoolSwitchEditor(e => e.DisplayName = new string[] { "无拼板码绑定", "有拼板码绑定" }).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                }
                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.CanBindQty).ShowInDetail().Readonly();
                    View.Property(p => p.ForkPlateQty).UseForkPlateQtyEditor().ShowInDetail();
                    View.Property(p => p.BoardNo).ShowInDetail().Readonly(BoardNoReadOnlyProperty);
                    View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.UnBindSnQty).HasLabel("未绑定SN").ShowInDetail().Readonly();
                    View.Property(p => p.UnBindPanelQty).HasLabel("未绑定拼板码").ShowInDetail().Readonly();
                }
            }
        }
    }
}