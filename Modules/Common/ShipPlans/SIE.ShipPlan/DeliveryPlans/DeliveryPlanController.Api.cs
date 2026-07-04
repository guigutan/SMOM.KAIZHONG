using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货计划控制器
    /// </summary>
    public partial class DeliveryPlanController
    {
        /// <summary>
        /// 创建发货计划
        /// </summary>
        /// <param name="planParams">发货计划参数</param>
        public virtual void CreateDeliveryPlanData(List<ShipPlanParam> planParams)
        {
            if (planParams == null || !planParams.Any())
            {
                return;
            }

            List<string> itemCodes = planParams.Select(p => p.ItemCode).Distinct().ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes);
            if (!items.Any())
            {
                throw new ValidationException("未找到物料".L10N());
            }

            //部门
            var enterpriseCodes = planParams.Select(p => p.EnterpriseCode).Distinct().ToList();
            var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(enterpriseCodes);

            //客户
            var customerCodes = planParams.Select(p => p.CustomerCode).Distinct().ToList();
            var customers = RT.Service.Resolve<CustomerController>().GetCustomers(customerCodes);

            //供应商
            var supplierCodes = planParams.Select(p => p.SupplierCode).Distinct().ToList();
            var suppliers = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodes);

            //发货仓库
            var warehouseCodes = planParams.Select(p => p.WarehouseCode).Distinct().ToList();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetWarehouseList(warehouseCodes);

            //目标仓库
            var targetWarehouCodes = planParams.Select(p => p.TargetWarehouseCode).Distinct().ToList();
            var targetWarehouse = RT.Service.Resolve<WarehouseController>().GetWarehouseList(targetWarehouCodes);

            //货主编码
            var storerCodes = planParams.Select(p => p.StorerCode).Distinct().ToList();
            var storers = RT.Service.Resolve<CustomerController>().GetCustomerTypeCustomers(storerCodes);

            //物料扩展属性
            ////var itemExtProps = planParams.SelectMany(p => p.ItemExtendProps).ToList();
            ////var itemExtPropList = RT.Service.Resolve<ItemController>().GetItemPropertyList(items.Select(p => p.Id).ToList());
            ////if (itemExtProps.Any())
            ////{
            ////    itemExtProps.ForEach(p =>
            ////    {
            ////        if (!itemExtPropList.Any(t => t.ItemCode == p.ItemCode && t.DefinitionName == p.ItemExtPropName && t.Value == p.ItemExtPropValue))
            ////        {
            ////            throw new ValidationException("物料编码:[{0}]未找到物料扩展属性".L10nFormat(p.ItemCode));
            ////        }
            ////    });
            ////}

            EntityList<DeliveryPlan> plans = new EntityList<DeliveryPlan>();
            foreach (var shipPlan in planParams)
            {
                DeliveryPlan deliveryPlan = new DeliveryPlan();
                deliveryPlan.GenerateId();
                deliveryPlan.OrderType = (OrderType)shipPlan.OrderType;
                deliveryPlan.CreateQty = shipPlan.Qty;
                deliveryPlan.RequireQty = shipPlan.Qty;
                deliveryPlan.NoCreateQty = shipPlan.Qty;               
                deliveryPlan.State = DeliveryState.Created;
                deliveryPlan.No = shipPlan.PlanOrderNo;
                deliveryPlan.LineNo = shipPlan.PlanDtlLineNo;
                var item = items.FirstOrDefault(p => p.Code == shipPlan.ItemCode);
                deliveryPlan.ItemId = item.Id;
                deliveryPlan.Item = item;
                ////var tmpItemExtProps = shipPlan.ItemExtendProps.Where(t => t.ItemCode == shipPlan.ItemCode).ToList();
                ////if (tmpItemExtProps.Any())
                ////{
                ////    string itemExtProp = string.Empty;
                ////    string itemExtPropName = string.Empty;
                ////    SetItemExtPropData(tmpItemExtProps, itemExtPropList, itemExtProp, itemExtPropName);
                ////    deliveryPlan.ItemExtProp = itemExtProp;
                ////    deliveryPlan.ItemExtPropName = itemExtPropName;
                ////}
                deliveryPlan.ItemExtProp = shipPlan.ItemExtProp;
                deliveryPlan.ItemExtPropName = shipPlan.ItemExtPropName;
                deliveryPlan.DeliveryDate = shipPlan.DeliveryDate;
                deliveryPlan.EnterpriseId = enterprises.FirstOrDefault(t => t.Code == shipPlan.EnterpriseCode)?.Id;
                deliveryPlan.CustomerId = customers.FirstOrDefault(t => t.Code == shipPlan.CustomerCode)?.Id;
                deliveryPlan.SupplierId = suppliers.FirstOrDefault(t => t.Code == shipPlan.SupplierCode)?.Id;
                deliveryPlan.WarehouseId = warehouses.FirstOrDefault(t => t.Code == shipPlan.WarehouseCode)?.Id;
                deliveryPlan.TargetWarehouseId = targetWarehouse.FirstOrDefault(t => t.Code == shipPlan.TargetWarehouseCode)?.Id;
                deliveryPlan.OrderNo = shipPlan.OrderNo;
                deliveryPlan.OrderLineNo = shipPlan.OrderLineNo;
                deliveryPlan.ResourceId = shipPlan.ResourceId;
                deliveryPlan.IsMergeIssued=shipPlan.IsMergeIssued;
                if (shipPlan.StorerCode.IsNotEmpty() && !storers.Any(p => p.Code == shipPlan.StorerCode))
                {
                    throw new ValidationException("货主编码不存在".L10nFormat(shipPlan.StorerCode));
                }

                deliveryPlan.StorerCode = shipPlan.StorerCode;
                deliveryPlan.ProjectNo = shipPlan.ProjectNo;
                deliveryPlan.TaskNo = shipPlan.TaskNo;
                deliveryPlan.LotCode = shipPlan.LotCode;
                deliveryPlan.ProductBatch = shipPlan.ProductBatch;
                deliveryPlan.SourceType = DeliverySourceType.External;
                plans.Add(deliveryPlan);
            }
            using (var tran = DB.TransactionScope(ShipPlanEntityDataProvider.ConnectionStringName))
            {
                RF.Save(plans);
                RT.Service.Resolve<DeliveryPlanController>().AuditDeliveryPlans(plans.Select(p=>p.Id).ToList());
                tran.Complete();
            }
        }

        /////// <summary>
        /////// 设置物料扩展属性
        /////// </summary>
        /////// <param name="extPropDatas">扩展属性数据</param>
        /////// <param name="itemExtPropList">物料扩展属性列表</param>
        /////// <param name="itemExtProp">物料扩展属性</param>
        /////// <param name="itemExtPropName">物料扩展属性名称</param>
        ////private void SetItemExtPropData(List<ItemExtPropData> extPropDatas, EntityList<ItemPropertyValue> itemExtPropList, string itemExtProp, string itemExtPropName)
        ////{
        ////    extPropDatas.ForEach(p =>
        ////    {
        ////        var itemExtPropData = itemExtPropList.FirstOrDefault(t => t.ItemCode == p.ItemCode && t.DefinitionName == p.ItemExtPropName && t.Value == p.ItemExtPropValue);

        ////        if (itemExtPropData == null)
        ////        {
        ////            throw new ValidationException("物料编码:[{0}]未找到物料扩展属性".L10nFormat(p.ItemCode));
        ////        }

        ////        itemExtPropName += itemExtPropData.DefinitionName + ":" + itemExtPropData.Value + ";";
        ////        itemExtProp += itemExtPropData.DefinitionId + ":" + itemExtPropData.Value + ";";
        ////    });
        ////}
    }
}