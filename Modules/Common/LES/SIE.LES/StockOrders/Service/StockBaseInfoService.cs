using NPOI.HSSF.Record;
using SIE.Common.Configs;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.StockOrders.APIModels;
using SIE.LES.StockOrders.Configs;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using SIE.Web.LES.StockOrders.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace SIE.LES.StockOrders.Service
{
    /// <summary>
    /// API服务
    /// </summary>
    public partial class StockOrderService : DomainService
    {
        /// <summary>
        /// 获取员工权限下的工厂
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<BaseInfo> GetFactoryInfos(double employeeId)
        {
            if (employeeId == 0)
            {
                throw new ValidationException("员工信息异常!".L10N());
            }
            var factoryList = _stockOrderDao.GetEnterprises(employeeId);
            List<BaseInfo> factoryInfos = new List<BaseInfo>();
            factoryList.ForEach(item =>
            {
                BaseInfo factoryInfo = new BaseInfo
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                };
                factoryInfos.Add(factoryInfo);
            });
            return factoryInfos;
        }

        /// <summary>
        /// 获取工厂下的车间
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<BaseInfo> GetWorkShopInfos(double factoryId)
        {
            if (factoryId == 0)
            {
                throw new ValidationException("请先选择工厂!".L10N());
            }
            List<Enterprise> workShopList = new List<Enterprise>();
            _stockOrderDao.GetWorkShops(factoryId, workShopList);
            List<BaseInfo> workShopInfos = new List<BaseInfo>();
            workShopList.ForEach(item =>
            {
                BaseInfo workShopInfo = new BaseInfo
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                };
                workShopInfos.Add(workShopInfo);
            });
            return workShopInfos;
        }

        /// <summary>
        /// 获取工厂下的资源并带出车间
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual List<BaseInfo> GetWipResourceInfos(double factoryId, double? workShopId)
        {
            if (factoryId == 0)
            {
                throw new ValidationException("请先选择工厂!".L10N());
            }
            var wipList = _stockOrderDao.GetWipResources(factoryId, workShopId);
            List<BaseInfo> wipInfos = new List<BaseInfo>();
            wipList.ForEach(item =>
            {
                BaseInfo wipInfo = new BaseInfo
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    WorkShopId = item.WorkShopId,
                    WorkShopCode = item.WorkShopCode,
                    WorkShopName = item.WorkShopName,
                };
                wipInfos.Add(wipInfo);
            });
            return wipInfos;
        }

        /// <summary>
        /// 根据生产资源获取产线线边仓
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual List<BaseInfo> GetLineWarehouse(double? wipId)
        {
            if (wipId == null || wipId == 0)
            {
                throw new ValidationException("生产资源信息有误!".L10N());
            }
            var lineWarehouse = _stockOrderDao.GetLinesideWarehouse(wipId.Value);

            if (!lineWarehouse.Any())
            {
                var wip = RF.GetById<WipResource>(wipId.Value);
                throw new ValidationException("生产资源【{0}】未维护线边仓，请维护!".L10nFormat(wip.Name));
            }

            var lineWareList = new List<BaseInfo>();
            lineWarehouse.ForEach(item =>
            {
                BaseInfo warehouse = new BaseInfo
                {
                    Id = item.WarehouseId,
                    Code = item.WarehouseCode,
                    Name = item.WarehouseName,
                };
                lineWareList.Add(warehouse);
            });

            return lineWareList;
        }

        /// <summary>
        /// 获取生产资源下的产线线边仓库位
        /// </summary>
        /// <param name="wipId"></param>
        /// <param name="wareId"></param>
        /// <returns></returns>
        public virtual List<BaseInfo> GetLineStorage(double? wipId, double? wareId)
        {
            if (wipId == null || wipId == 0)
            {
                throw new ValidationException("生产资源信息有误!".L10N());
            }
            if (wareId == null || wareId == 0)
            {
                throw new ValidationException("仓库信息有误!".L10N());
            }
            var lineWarehouse = _stockOrderDao.GetLinesideWarehouse(wipId.Value);
            var lineWarehouseList = lineWarehouse.Where(p => p.WarehouseId == wareId).ToList();
            var lineStorageList = new List<BaseInfo>();
            lineWarehouseList.ForEach(item =>
            {
                BaseInfo storage = new BaseInfo
                {
                    Id = item.StorageLocationId,
                    Code = item.LocaltionCode,
                    Name = item.LocaltionName,
                };
                lineStorageList.Add(storage);
            });
            return lineStorageList;
        }

        /// <summary>
        /// 获取备料单接收方式
        /// </summary>
        /// <returns></returns>
        public virtual StockReceiveType GetStockReceiveType()
        {
            var config = ConfigService.GetConfig(new StockReceiveTypeConfig(), typeof(StockOrder));
            if (config == null)
            {
                throw new ValidationException("未找到接收方式配置,请检查配置项".L10N());
            }
            return config.ReceiveType;
        }

        /// <summary>
        /// 更新或创建用户备料单app基本信息
        /// </summary>
        /// <param name="stockBaseInfo"></param>
        public virtual void UpdataUserInfo(StockBaseInfo stockBaseInfo)
        {
            if (stockBaseInfo.EmployeeId == 0)
            {
                throw new ValidationException("用户信息异常!".L10N());
            }
            _stockOrderDao.UpdataUserInfo(stockBaseInfo);
        }


        /// <summary>
        /// 获取用户的备料单APP基本信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual StockBaseInfo GetUserInfo(double employeeId)
        {
            if (employeeId == 0)
            {
                throw new ValidationException("用户信息异常!".L10N());
            }
            return _stockOrderDao.GetUserInfo(employeeId);
        }

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="itemCode">物料编码</param>
        /// <param name="warehouseId">仓库id</param>
        /// <returns></returns>
        public virtual BaseInfo GetItemInfo(string itemCode, double? warehouseId)
        {
            if (itemCode.IsNullOrEmpty())
            {
                throw new ValidationException("请输入物料编码或物料标签！".L10N());
            }

            var matchItemCode = itemCode;

            //尝试获取物料标签
            var itemLabe = _stockOrderDao.GetItemLabel(itemCode);
            if (itemLabe != null)
            {
                matchItemCode = itemLabe.ItemCode;
            }
            var itemInfo = _stockOrderDao.GetItemInfo(matchItemCode, warehouseId);
            if (itemInfo != null)//加载其扩展属性
            {
                var itemCtl = RT.Service.Resolve<Items.ItemController>();
                var itemExtProps = itemCtl.GetItemPropertys(itemInfo.Id, string.Empty, null);
                itemExtProps.OrderBy(p => p.DefinitionId).ForEach(p =>
                {
                    itemInfo.ItemExtPropDataInfos.Add(new ItemExtPropDataInfo
                    {
                        DefinitionId = p.DefinitionId,
                        DefinitionName = p.DefinitionName,
                        ItemExtPropValue = p.Value,
                    });
                });
            }
            return itemInfo;
        }

        /// <summary>
        /// 根据物料获取工单
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="workShopId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<BaseInfo> GetWorkOders(double? itemId, double? workShopId, double? resourceId)
        {
            var hasItem = itemId != 0 && itemId != null;
            var hasWorkShopId = workShopId != 0 && workShopId != null;
            var hasResourceId = resourceId != 0 && resourceId != null;
            if (!hasItem)
            {
                throw new ValidationException("请先维护物料！".L10N());
            }
            if (!hasWorkShopId || !hasResourceId)
            {
                throw new ValidationException("请维护基本信息的车间信息和生产资源信息！".L10N());
            }
            return _stockOrderDao.GetWorkOders(itemId, workShopId, resourceId);
        }

        /// <summary>
        /// 创建备料单
        /// </summary>
        /// <param name="baseInfo"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="itemExtPropName"></param>
        /// <param name="isEnableExtProp"></param>
        /// <param name="stockQty"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual string CreateStockOrder(StockBaseInfo baseInfo, double itemId,
            string itemExtProp, string itemExtPropName, bool isEnableExtProp,
            decimal stockQty, double? workOrderId)
        {
            var hasWorkOrderId = workOrderId != 0 && workOrderId != null;
            var hasLineWare = baseInfo.WarehouseId != 0 && baseInfo.StorageId != null;

            if (itemId == 0)
            {
                throw new ValidationException("请先维护物料信息！".L10N());
            }

            if (stockQty <= 0)
            {
                throw new ValidationException("本次备料量不允许为非正数！".L10N());
            }

            if (baseInfo.StockType == PrepareItemType.Push && !hasWorkOrderId)
            {
                throw new ValidationException("备料模式为推式时工单必填！".L10N());
            }

            if (baseInfo.StockType == PrepareItemType.Pull && !hasLineWare)
            {
                throw new ValidationException("备料模式为拉式时【仓库】和【库位】必填！".L10N());
            }
            if (isEnableExtProp && itemExtProp.IsNullOrEmpty())
            {
                throw new ValidationException("启用扩展属性的物料，必须选择扩展属性！".L10N());
            }

            return _stockOrderDao.CreateStockOrder(baseInfo, itemId, stockQty, workOrderId, itemExtProp, itemExtPropName);
        }

        /// <summary>
        /// 获取资源下的线边仓仓库和库位
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual WareAndStorage GetWareAndStorage(double? wipId)
        {
            WareAndStorage wareAndStorage = new WareAndStorage();
            if (wipId == null || wipId == 0)
            {
                throw new ValidationException("生产资源信息有误!".L10N());
            }
            var line = _stockOrderDao.GetLinesideWarehouse(wipId.Value);
            if (line != null && line.Count == 1)
            {
                BaseInfo baseInfo1 = new BaseInfo
                {
                    Id = line[0].WarehouseId,
                    Code = line[0].WarehouseCode,
                    Name = line[0].WarehouseName,
                };
                BaseInfo baseInfo2 = new BaseInfo
                {
                    Id = line[0].StorageLocationId,
                    Code = line[0].LocaltionCode,
                    Name = line[0].LocaltionName,
                };
                wareAndStorage.WareList.Add(baseInfo1);
                wareAndStorage.StorageList.Add(baseInfo2);
            }
            return wareAndStorage;
        }

        /// <summary>
        /// 备料单查询物料
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderItemViewModel> GetStockOrderItemViewModels(StockOrderItemViewModelCriteria criteria)
        {
            return _stockOrderDao.GetStockOrderItemViewModels(criteria);
        }
    }
}
