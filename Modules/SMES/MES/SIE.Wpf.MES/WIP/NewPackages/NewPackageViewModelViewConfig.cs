using SIE.Wpf.Common;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.WIP.NewPackages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.NewPackages
{
    public class NewPackageViewModelViewConfig : WPFViewConfig<NewPackageViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(NewPackageViewModel));
            View.UseCommands(typeof(NewPackageRestartCommand));
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
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 4, height: 0, hideLabel: true);

                    View.Property(p => p.Printer).UsePrinterEditor().ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                }


                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 2).Readonly();
                    //View.Property(p => p.WorkOrder.OrderQty).HasLabel("ERP工单数").ShowInDetail(columnSpan: 2).Readonly();
                    //View.Property(p => p.WoMoveQty).HasLabel("工单采集数").ShowInDetail(columnSpan: 2).Readonly();
                    //View.Property(p => p.CollectQty).HasLabel("当班采集数").ShowInDetail(columnSpan: 2).Readonly();
                }
            }
        }
    }
}
