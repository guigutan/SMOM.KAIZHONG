using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.ConnectorPacking.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking
{
    public class ConnectorSnPackingReplaceVMViewConfig : WPFViewConfig<ConnectorSnPackingReplaceViewModel>
    {
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ConnectorReplaceReStartCommand));
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

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    //View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail(columnSpan: 6).Readonly();
                    View.Property(p => p.OldEngraveLabel.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 6).Readonly();
                    View.Property(p => p.OldEngraveLabel.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 6).Readonly();
                    View.Property(p => p.OldEngraveLabel.BatchNo).HasLabel("工序标签").ShowInDetail(columnSpan: 6).Readonly();

                    View.Property(p => p.OldBatchNo).HasLabel("原刻码标签").ShowInDetail(columnSpan: 6).Readonly();
                    View.Property(p => p.NewBatchNo).HasLabel("新刻码标签").ShowInDetail(columnSpan: 6).Readonly();
                }
            }
        }
    }
}
