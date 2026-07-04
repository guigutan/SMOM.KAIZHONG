using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MetaModel.View;
using SIE.Packages.ItemLabels;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Packages.ItemLabels.Commands;

namespace SIE.Web.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签视图配置
    /// </summary>
    internal class ItemLabelViewConfig : WebViewConfig<ItemLabel>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(ConfigCommands.ModuleConfigCommand,  typeof(ItemLabelImportCommand).FullName,typeof(ItemLabelDeleteCommand).FullName);//WebCommandNames.Delete,
            View.UseCommands(typeof(ItemLabelLabelPrintCommand).FullName);
            //View.UseCommand("SIE.Web.Packages.ItemLabels.Commands.ItemLabelExportCommand");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Label).ShowInList(width: 150).Readonly().HasOrderNo(1);
            View.Property(p => p.Exidv).Show().Readonly();
            View.Property(p => p.Exidv2).Show().Readonly();
            View.Property(p => p.ItemLabelState).Show();
            View.Property(p => p.ItemCode).ShowInList(width: 150).Readonly().HasOrderNo(10);
            View.Property(p => p.ItemName).ShowInList(width: 200).Readonly().HasOrderNo(15);
            View.Property(p => p.ShortDescription).ShowInList(width: 150).Readonly().HasOrderNo(16);
            View.Property(p => p.MrpController).ShowInList(width: 150).Readonly().HasOrderNo(17);
            View.Property(p => p.ItemExtPropName).Readonly().HasOrderNo(17);
            View.Property(p => p.Specification).Readonly().HasOrderNo(20);
            View.Property(p => p.ItemType).Readonly().HasOrderNo(25);            
            View.Property(p => p.SourceType).Readonly().HasOrderNo(35);
            View.Property(p => p.InitialQty).Readonly().HasOrderNo(37);
            View.Property(p => p.Qty).Readonly().HasOrderNo(40);            
            View.Property(p => p.WorkOrder).ShowInList(150).Readonly().HasOrderNo(50);
            View.Property(p => p.Lot).ShowInList(width: 150).Readonly().HasOrderNo(52);
            View.Property(p => p.Licha).ShowInList(width: 150).Readonly().HasOrderNo(52);
            View.Property(p => p.ProductBatch).Readonly().HasOrderNo(52);
            View.Property(p => p.ProjectNo).Readonly().HasOrderNo(53);
            View.Property(p => p.Supplier).Readonly().HasOrderNo(54);
            View.Property(p => p.Factory).Readonly().HasOrderNo(55);
            View.Property(p => p.Lgort).Readonly().HasOrderNo(56);
            View.Property(p => p.Warehouse).Readonly().HasOrderNo(56);
            View.Property(p => p.StorageLocation).Readonly().HasOrderNo(57);
            View.Property(p => p.ProductionDate).Readonly().HasOrderNo(58);
            View.Property(p => p.NgQty).Readonly().HasOrderNo(59);
            View.Property(p => p.ReturnQtyInTransit).Readonly().HasOrderNo(59);
            View.Property(p => p.NgReturnQtyInTransit).Readonly().HasOrderNo(59);
            View.Property(p => p.IsSerialNumber).Readonly().HasOrderNo(59);
            View.Property(p => p.RemainLongLived).ShowInList(width: 150).Readonly().HasLabel("剩余可用时长(H)").HasOrderNo(60);
            View.Property(p => p.LongLived).ShowInList(width: 150).Readonly().HasLabel("可用时长寿命(H)").HasOrderNo(61);
            View.Property(p => p.ValidityStart).ShowInList(width: 150).Readonly().HasOrderNo(62);
            View.Property(p => p.ValidityEnd).ShowInList(width: 150).Readonly().HasOrderNo(63);
            View.Property(p=>p.Isuse).ShowInList(width: 150).Readonly().HasOrderNo(64);
            View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.WorkOrderList).Show(ChildShowInWhere.Hide);
            View.Property(p => p.CreateByName).HasOrderNo(80);
            View.Property(p => p.CreateDate).ShowInList(150).HasOrderNo(81);
            View.Property(p => p.UpdateByName).HasOrderNo(82);
            View.Property(p => p.UpdateDate).ShowInList(150).HasOrderNo(83);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Label);
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.Item);
            View.Property(p => p.ItemType).UseEnumEditor(p => p.IsEnumNull = true);            
            View.Property(p => p.SourceType);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Label);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.ItemType);
            View.Property(p => p.Qty);
            View.Property(p => p.InitialQty);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.SourceType);
        }
    }
}