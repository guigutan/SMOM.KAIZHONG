using SIE.MES.WIP.Packings;
using SIE.Packages.Packages;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.Common.ViewBehaviors;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP.NewPackages;
using SIE.Wpf.MES.WIP.Packings.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 直接包装采集视图配置
    /// </summary>
    public class DirectPackingViewModelViewConfig : WPFViewConfig<DirectPackingViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(DirectPackingViewModel));
            View.UseCommands(typeof(DirectPackingResetCommand));
            View.UseDetail(columnCount: 6);
            using (View.DeclareGroup(string.Empty))
            {
                View.Property(p => p.Tips).UseEditor(TipsEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                View.Property(p => p.Error).UseEditor(ErrorEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
            }
            using (View.DeclareGroup("扫描信息".L10N(), detailColumnCount: 8))
            {
                View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 8, height: 0, hideLabel: true);
            }
            using (View.DeclareGroup("工单信息".L10N(), detailColumnCount: 5, collapsable: true))
            {
                View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail();
                View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail();
                View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail();
                View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail();
                View.Property(p => p.PackingUnit).UsePagingLookUpEditor(e =>
                {
                    e.DisplayMember = PackingUnit.NameProperty.Name;
                }).HasLabel("本次包装层级").ShowInDetail();
                //View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly(true);
                View.Property(p => p.Printer).HasLabel("指定打印机").UseEditor(PrinterEditor.EditorName)//.Show(ShowInWhere.All)
                    .Visibility(NewPackingViewModel.IsAutoPrintLabelProperty).ShowInDetail(columnSpan: 2);
            }
        }
    }

    /// <summary>
    /// 包装单位wpf下拉引用
    /// </summary>
    public class PackingUnitWPFViewConfig : WPFViewConfig<PackingUnit>
    {
        /// <summary>
        /// 下拉
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.IsMasterUnit).Show(ShowInWhere.All);
                View.Property(p => p.PackageUnitType).Show(ShowInWhere.All);
            }
        }
    }

    /// <summary>
    /// 直接包装条码明细
    /// </summary>
    public class DirectPackageSnRecordViewConfig : WPFViewConfig<DirectPackageSnRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(DirectPackingViewModel));
            //View.AddBehavior(typeof(MultipleRowViewBehavior));
            View.ClearCommands();
            View.UseCommands(typeof(DirectPackingCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).Show();
                View.Property(p => p.PackageUnit.Name).HasLabel("包装单位").Show();
                View.Property(p => p.WorkOrder.No).HasLabel("工单号").Show();
                View.Property(p => p.Product.Code).HasLabel("产品").Show();
                View.Property(p => p.WoSn).ShowInList(gridWidth: 800);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

            }
        }
    }
}
