using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.NewPackingQC;
using SIE.Wpf.MES.NewPackingQC.Commands;
using SIE.Wpf.MES.NewPackingQcC.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.NewPackingQcC
{
    public class NewPackingQcCViewModelViewConfig : WPFViewConfig<NewPackingQcCViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseCommands(typeof(NewCollectCRestartCommand), typeof(NewNormalCCommand), typeof(NewUnboxingCCommand), typeof(NewBoxExCChangeCommand), typeof(NewCSubmitCommand));//, typeof(Commands.NewDeleteCommand)
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
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.blueInt).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.blueZInt).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.XtBlue).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.YXtBlue).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p=>p.WorkOrder.Product.ShortDescription).HasLabel("旧物料号").ShowInDetail(columnSpan: 2).Readonly();
                    //UseMemoEditor
                }
            }
        }
    }
}
