using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SIE.xUnit.Warehouses
{
    public class WhTestController : DomainController
    {
        /// <summary>
        /// 创建仓库
        /// </summary>
        /// <returns></returns>
        public virtual Warehouse CreateWarehouse()
        {
            var warehouse = new Warehouse();
            warehouse.GenerateId();
            warehouse.Code = "WareCode" + warehouse.Id.ToString();
            warehouse.Name = "WareName" + warehouse.Id.ToString();
            warehouse.Category = "01";

            var address = new WarehouseAddress();
            address.AddressType = "01";
            address.Name = warehouse.Code;
            address.Address = "测试";
            address.EmployeeId = RT.IdentityId;
            warehouse.WarehouseAddressList.Add(address);
            RF.Save(warehouse);
            var warehouseEmp = new WarehouseEmployee();
            warehouseEmp.WarehouseId = warehouse.Id;
            warehouseEmp.EmployeeId = RT.IdentityId;
            RF.Save(warehouseEmp);
            return warehouse;
        }

        /// <summary>
        /// 创建库区库位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public virtual StorageLocation CreateLocation(double warehouseId)
        {
            var area = new StorageArea();
            var loc = new StorageLocation();

            area.GenerateId();
            area.Code = "AreaCode" + area.Id;
            area.Name = "AreaName" + area.Id;
            area.LibraryType = LibraryType.Fictitious;
            area.WarehouseId = warehouseId;
            RF.Save(area);

            loc.Code = "LocCode" + area.Id;
            loc.Name = "LocCode" + area.Id;
            loc.LibraryType = LibraryType.Fictitious;
            loc.AreaId = area.Id;
            loc.WarehouseId = warehouseId;
            RF.Save(loc);

            return loc;
        }
    }
}
