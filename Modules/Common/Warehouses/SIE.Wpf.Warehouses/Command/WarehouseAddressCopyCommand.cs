using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Warehouses.Command
{

    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加数据", Gestures = "Ctrl+Shift+C", GroupType = CommandGroupType.Edit)]
    class WarehouseAddressCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 复制新增
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnItemCreated(Entity entity)
        {
            //base.OnItemCreated(entity);
            var entityObj = View.Current as WarehouseAddress;
            WarehouseAddress curWarehouseAddress = entity as WarehouseAddress;
            curWarehouseAddress.AddressType = entityObj.AddressType;
            curWarehouseAddress.Name = entityObj.Name+"-复制".L10N();
            curWarehouseAddress.Country = entityObj.Country;
            curWarehouseAddress.Province = entityObj.Province;
            curWarehouseAddress.City = entityObj.City;
            curWarehouseAddress.Area = entityObj.Area;
            curWarehouseAddress.Address = entityObj.Address;
            curWarehouseAddress.Phone = entityObj.Phone;
            curWarehouseAddress.Fax = entityObj.Fax;
            curWarehouseAddress.Email = entityObj.Email;
            curWarehouseAddress.ZipCode = entityObj.ZipCode;
            curWarehouseAddress.Remark = entityObj.Remark;
            curWarehouseAddress.Employee = entityObj.Employee;
        }
    }
}

