using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Items._Extentions_;
using SIE.Web.Warehouses.Commands;
using System.Collections.Generic;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 收发控制视图
    /// </summary>
    internal class BaseItemIOLimitViewConfig : WebViewConfig<BaseItemIoLimit>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(AddBaseItemIOLimitCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseId).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.DefaultArea).UseDataSource((o, e, r) =>
                {
                    //获取默认库区选项
                    var itemIOLimit = o as BaseItemIoLimit;
                    if (itemIOLimit == null) 
                        return new EntityList<StorageArea>();
                    return RT.Service.Resolve<WarehouseController>().GetStorageAreas(itemIOLimit.WarehouseId, r, e);
                }).UseListSetting(e => { e.HelpInfo = "显示当前仓库下的默认上架库区"; });
                View.Property(p => p.DefaultLocation).UseDataSource((o, e, r) =>
                {
                    var itemIOLimit = o as BaseItemIoLimit;
                    if (itemIOLimit == null) 
                        return new EntityList<StorageLocation>();
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(itemIOLimit.WarehouseId, r, e);
                }).UseListSetting(e => { e.HelpInfo = "显示当前库区下的默认上架库位"; });
                View.Property(p => p.InUpLimitMultiple);
                View.Property(p => p.MaxInUpLimit);
                View.Property(p => p.OutUpLimitMultiple);
                View.Property(p => p.MaxOutUpLimit);
                //View.Property(p => p.InUpLimitMultiple).DefaultValue(0);
                //View.Property(p => p.MaxInUpLimit).DefaultValue(0);
                //View.Property(p => p.OutUpLimitMultiple).DefaultValue(0);
                //View.Property(p => p.MaxOutUpLimit).DefaultValue(0);
                View.Property(p => p.PoAdvanceUpLimit);
                View.Property(p => p.PoPostponeUpLimit);
                View.Property(p => p.ReSurplusLowerLimit);
                View.Property(p => p.ShSurplusLowerLimit);
                View.Property(p => p.MinStockQty);
                View.Property(p => p.MaxStockQty);
                View.Property(p => p.SafetyStockQty);
                View.Property(p => p.Remarks);
                View.Property(p => p.EmployeeId).UseDataSource((o, e, r) =>
                {
                    var itemIOLimit = o as BaseItemIoLimit;
                    if (itemIOLimit == null) 
                        return new EntityList<Employee>();
                    if (itemIOLimit.EmployeeId == null)
                    {
                        //获取在职员工列表
                        var lists = RT.Service.Resolve<EmployeeController>().GetEmployeeListOnJob();
                        return lists;
                    }
                    var list = RT.Service.Resolve<EmployeeController>().GetEmployeeListOnJob(new List<double> { (double)itemIOLimit.EmployeeId });
                    return list;
                }).UsePagingLookUpEditor().HasLabel("默认操作人");
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {//使用扩展属性编辑器
                    p.DbField = "ItemExtProp";
                }).ShowInList(width: 180)
                        .Readonly(p => p.ItemId <= 0 || !p.ItemEnableExtendProperty)
                        .UseListSetting(e => { e.HelpInfo = "选择的物料启用扩展属性才可编辑"; });
                View.Property(p => p.IsAlotInvInOperation);
            }
        }
    }
}
