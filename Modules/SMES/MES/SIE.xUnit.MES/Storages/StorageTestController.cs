using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.MES.Storages;
using SIE.Tech.Stations;
using System.Linq;

namespace SIE.xUnit.MES.Storages
{
    public class StorageTestController : DomainController
    {
        static StorageController StorageController = RT.Service.Resolve<StorageController>();

        public virtual StationStorageArea GetOrCreateStorageArea(double stationId)
        {
            var stationStorageArea = RT.Service.Resolve<CommonController>().GetData<StationStorageArea>(p => p.StationId == stationId);
            if (stationStorageArea == null)
            {
                var station = RF.GetById<Station>(stationId);
                var storageArea = GetOrCreateStorageArea($"MES-{station?.Name}工位货区");
                stationStorageArea = new StationStorageArea()
                {
                    StorageArea = storageArea,
                    StationId = stationId
                };
                stationStorageArea.GenerateId();
                RF.Save(stationStorageArea);
            }
            return stationStorageArea;
        }

        public virtual StorageArea GetOrCreateStorageArea(string code)
        {
            var storageArea = RT.Service.Resolve<CommonController>().GetData<StorageArea>(p => p.Code == code);
            if (storageArea == null)
            {
                var warehouse = GetOrCreateWarehouse("MES-原材料仓");
                storageArea = new StorageArea()
                {
                    Warehouse = warehouse,
                    Code = code,
                    Name = code,
                    Type = StorageAreaType.Input
                };
                storageArea.GenerateId();
                RF.Save(storageArea);
            }
            return storageArea;
        }

        public virtual Warehouses.Warehouse GetOrCreateWarehouse(string code)
        {
            var warehouse = RT.Service.Resolve<CommonController>().GetData<Warehouses.Warehouse>(p => p.Code == code);
            if (warehouse == null)
            {
                warehouse = new Warehouses.Warehouse()
                {
                    Code = code,
                    Name = code,
                    Category = "MES",
                    IsFrozen = false,
                    LibraryType = Warehouses.LibraryType.Entity,
                    State = State.Enable
                };
                warehouse.GenerateId();
                RF.Save(warehouse);
            }
            return warehouse;
        }
    }
}