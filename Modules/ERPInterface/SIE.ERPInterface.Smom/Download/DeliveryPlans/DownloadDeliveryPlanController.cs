using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas;
using SIE.ERPInterface.Common.Enums;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.ShipPlan;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 下载发货计划控制器
    /// </summary>
    public class DownloadDeliveryPlanController : DomainController
    {
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="planDatas"></param>
        /// /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadDeliveryPlanToBusiness(List<DeliveryPlanData> planDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<DeliveryPlanData>(
                planDatas,
                p => this.SaveDeliveryPlans(p),
                JobType.Allocate,
                invOrg);
        }

        /// <summary>
        /// 保存库存调拨数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SaveDeliveryPlans(List<DeliveryPlanData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;
            //EntityList<DeliveryPlan> deliveryPlans = new EntityList<DeliveryPlan>();
            var customerCodes = datas.Select(a => a.CustomerCode).Distinct().ToList();
            var cusDics = RT.Service.Resolve<CustomerController>().GetCustomers(customerCodes).ToDictionary(p => p.Code, p => p.Id);

            var enterCodes = datas.Select(a => a.EnterpriseCode).Distinct().ToList();
            var enterDics = RT.Service.Resolve<EnterpriseController>().GetEnterprises(enterCodes).ToDictionary(p => p.Code, p => p.Id);

            //var supplierCodes = datas.Select(a => a.SupplierCode).Distinct().ToList();
            var supDics = RT.Service.Resolve<SupplierController>().GetSupplierList().ToDictionary(p => p.Code, p => p.Id);

            var itemCodes = datas.Select(a => a.ItemCode).Distinct().ToList();
            var itemDics = RT.Service.Resolve<ItemController>().GetItems(itemCodes).ToDictionary(p => p.Code, p => p.Id);


            var whDics = RF.GetAll<Warehouse>().ToDictionary(a => a.Code, a => a.Id);

            datas.ForEach(p =>
            {
                try
                {
                    if (p.No.IsNullOrEmpty())
                        throw new ValidationException("单号不能为空".L10N());
                    if (p.RequireQty <= 0)
                        throw new ValidationException("需求数量必须大于0".L10N());
                    OrderType orderType = (OrderType)p.OrderType;
                    if (orderType != OrderType.SaleOut && orderType != OrderType.WorkFeed
                    && orderType != OrderType.SupplierReturn && orderType != OrderType.OutWorkFeed
                    && orderType != OrderType.DirectAllocate && orderType != OrderType.TwoAllocate
                    )
                        throw new ValidationException("单据类型不正确".L10N());
                    if (!itemDics.ContainsKey(p.ItemCode))
                        throw new ValidationException("物料{0}不存在".L10nFormat(p.ItemCode));

                    DeliveryPlan plan = new DeliveryPlan()
                    {
                        No = p.No,
                        RequireQty = p.RequireQty,
                        LineNo = p.LineNo,
                        OrderNo = p.OrderNo,
                        StorerCode = p.StorerCode,
                        ProjectNo = p.ProjectNo,
                        TaskNo = p.TaskNo,
                        LotCode = p.LotCode,
                        ProductBatch = p.ProductBatch,
                        OrderType = orderType,
                        CustomerId = cusDics.GetValue<double?>(p.CustomerCode),
                        EnterpriseId = enterDics.GetValue<double?>(p.EnterpriseCode),
                        SupplierId = supDics.GetValue<double?>(p.SupplierCode),
                        ItemId = itemDics.GetValue<double>(p.ItemCode),
                        WarehouseId = whDics.GetValue<double?>(p.FromWhCode),
                        TargetWarehouseId = whDics.GetValue<double?>(p.ToWhCode),
                        State = DeliveryState.Created,
                        SourceType = DeliverySourceType.External,
                    };
                    RF.Save(plan);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.No });
                }
            });
            return errors;
        }
    }
}
