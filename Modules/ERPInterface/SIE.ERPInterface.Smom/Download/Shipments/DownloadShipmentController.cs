using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.Shipments;
using SIE.Inventory.Commom;
using SIE.Inventory.Strategy;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.Warehouses;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 发运单下载控制器
    /// </summary>
    public class DownloadShipmentController : DomainController
    {
        /// <summary>
        /// 从API下载发运单到业务表
        /// </summary>
        /// <param name="shippingOrderDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadOrderToBusiness(List<ShippingOrderData> shippingOrderDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ShippingOrderData>(
                shippingOrderDatas,
                p => this.SaveShippingOrders(p.OrderByLastUpdateDate()),
                JobType.ShippingOrder,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载发运单到业务表
        /// </summary>
        public virtual ProcessResult DownloadSOInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ShippingOrderInf, ShippingOrderDetailInf>(
                () => ctl.GetUnprocessedDatas<ShippingOrderInf>(),
                p =>
                {
                    //发运单明细中间表数据
                    var nos = p.Select(y => y.No).Distinct().ToList();
                    var whereDtl = nos.CreateContainsExpression<ShippingOrderDetailInf>("x", ShippingOrderDetailInf.ShippingOrderNoProperty.Name);
                    var dtlDatas = ctl.GetUnprocessedDatas(whereDtl);
                    return dtlDatas;
                },
                (x, y) =>
                {
                    //构建明细数据嵌套字典
                    var dtlDataDicts = ctl.GenerateDictionarys<string, ShippingOrderDetailInf>(y, ShippingOrderDetailInf.ShippingOrderNoProperty);

                    //调用业务接口
                    var paras = this.GenerateShippingOrderPara(x, dtlDataDicts);
                    return this.SaveShippingOrders(paras.OrderByLastUpdateDate());
                },
                JobType.ShippingOrder, JobType.ShippingOrderDtl, isManual);
        }

        /// <summary>
        /// 生成发运单实体
        /// </summary>
        /// <param name="orderInfs">中间表实体数据</param>
        /// <param name="orderDtlInfs">中间表明细实体数据</param>
        /// <returns></returns>
        private List<ShippingOrderData> GenerateShippingOrderPara(IEnumerable<ShippingOrderInf> orderInfs, Dictionary<string, List<ShippingOrderDetailInf>> orderDtlInfs)
        {
            var paras = new List<ShippingOrderData>();
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();
            var dtlCtl = RT.Service.Resolve<DownloadShipmentDtlController>();

            orderInfs.ForEach(p =>
            {
                //构建子列表
                List<ShippingOrderDetailInf> details;
                if (orderDtlInfs.TryGetValue(p.No, out details))
                    orderDtlInfs.Remove(p.No);      //由于来源数据集允许重复数据，已取值明细清除，避免重复构建浪费资源
                else
                    details = new List<ShippingOrderDetailInf>();
                ctl.GenerateChildren(p, details);
                var dtlDatas = dtlCtl.GenerateShippingOrderDtlPara(details);

                //构建主数据
                var data = new ShippingOrderData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.No = p.No;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                ////data.OrderState = p.OrderState;
                ////data.OrderType = (int)p.OrderType;
                ////orderData.Contacts = orderInf.Contacts;
                ////orderData.ContactNumber = orderInf.ContactNumber;
                ////orderData.Connecter = orderInf.Connecter;
                data.ShippingWareHouseCode = p.WarehouseCode;
                data.CustomerCode = p.CustomerCode;
                data.EnterpriseCode = p.EnterpriseCode;
                data.SupplierCode = p.SupplierCode;
                data.Address = p.Address;
                data.DeliveryDate = p.DeliveryDate;
                data.ShippingDate = p.ShippingDate;
                data.ErpKey = p.ErpKey;
                data.ErpId = double.Parse(p.ErpKey);                    //注意，ERP表主键不一定是number类型
                data.DetailList = dtlDatas;                            //附加字列表

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapShipmentController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadSOInfToBusiness(true);           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;
        }

        /// <summary>
        /// 用于接口中心下载数据保存到SMOM 发运单表中
        /// </summary>
        /// <returns>错误数据列表</returns>
        public virtual List<ErpErrorData> SaveShippingOrders(List<ShippingOrderData> datas)
        {
            List<ErpErrorData> errors = new List<ErpErrorData>();
           
            //var ctl = RT.Service.Resolve<ShippingOrderService>();

            #region 获取数据
            List<string> noList = datas.Select(p => p.No).Distinct().ToList();
            //var shippingOrderList = ctl.GetShippingOrders(noList);
            //var soDict = shippingOrderList.ToDictionary(p => p.No, p => p);

            //获取明细数据
            //var soDetails = ctl.GetShippingOrderDetailList(shippingOrderList.Select(p => p.Id).ToList());
            //var dicDetails = soDetails.GroupBy(p => p.ShippingOrder.No).ToDictionary(p => p.Key, p => p.ToList());    //<soNo,明细列表>

            //获取供应商数据
            var supplierCodes = datas.Select(p => p.SupplierCode).Distinct().ToList();
            var suppliers = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodes);
            var supplierDict = suppliers.ToDictionary(p => p.Code, p => p);

            //获取部门数据
            var enterpriseCodes = datas.Select(p => p.EnterpriseCode).Distinct().ToList();
            var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(enterpriseCodes);
            var enterpriseDict = enterprises.ToDictionary(p => p.Code, p => p);

            //获取客户数据
            var customerCodes = datas.Select(p => p.CustomerCode).Distinct().ToList();
            var customers = RT.Service.Resolve<CustomerController>().GetCustomers(customerCodes);
            var customerDict = customers.ToDictionary(p => p.Code, p => p);

            //获取仓库数据
            var warehouseCodes = datas.Select(p => p.ShippingWareHouseCode).Distinct().ToList();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetUserWarehouses(warehouseCodes, string.Empty, null);
            var warehouseDict = warehouses.ToDictionary(p => p.Code, p => p);

            //获取物料数据
            var itemCodes = datas.SelectMany(p => p.DetailList).Select(p => p.ItemCode).Distinct().ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes);
            var itemDict = items.ToDictionary(p => p.Code, p => p);

            //获取库位
            var locationCodeList = datas.SelectMany(p => p.DetailList).Select(p => p.AppointStorageLocationCode).Distinct().ToList();
            var locationList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locationCodeList);
            var locDict = locationList.ToDictionary(p => p.Code, p => p);

            // 批次
            var lotCodeList = datas.SelectMany(p => p.DetailList).Select(p => p.AppointLotCode).Distinct().ToList();
            var lotList = RT.Service.Resolve<LotController>().GetLot(lotCodeList);
            var lotDict = lotList.ToDictionary(p => p.Code, p => p);

            //获取PO
            var poNoList = datas.SelectMany(p => p.DetailList).Select(p => p.PoNo).Distinct().ToList();
            //var poList = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrderDatas(poNoList);
            //var poDict = poList.ToDictionary(p => p.No, p => p);

            //获取物料包装规则
            var itemIdList = itemDict.Select(p => p.Value.Id).Distinct().ToList();
            ////var packDetailDic = RT.Service.Resolve<PackageController>().GetItemsMasterUnit(itemIdList);

            //获取物料收发信息
            //var itemIOLimits = RT.Service.Resolve<ItemExtController>().GetItemIOLimits(itemIdList);
            //var dicItemIOLimit = itemIOLimits.ToDictionary(p => "{0}_{1}".FormatArgs(p.ItemId, p.WarehouseId));

            var ruleCtl = RT.Service.Resolve<RuleController>();
            AssignRule defaultAssignRule = ruleCtl.GetAssignRule(AssignRule.Default);          //默认分配规则
            TurnOverRule defaultTurnOverRule = ruleCtl.GetTurnOverRule(TurnOverRule.Default);  //默认周转规则

            //单据小类
            var transactions = RF.GetAll<FunctionTransaction>(null, new EagerLoadOptions().LoadWith(FunctionTransaction.FunctionProperty).LoadWith(FunctionTransaction.TransactionProperty));
            var transactionDict = transactions.GroupBy(p => p.Function.Code).ToDictionary(p => p.Key, p => p.Select(d => d.Transaction).ToList());

            #endregion

            foreach (var data in datas)
            {
                try
                {
                    //SaveShippingOrder(data, soDict, supplierDict, enterpriseDict, customerDict, warehouseDict, transactionDict, dicDetails, itemDict, locDict, lotDict, poDict, dicItemIOLimit, defaultAssignRule, defaultTurnOverRule);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 保存发运单
        /// </summary>
        /// <param name="data">发运单数据</param>
        /// <param name="dicShippingOrder">发运单字典</param>
        /// <param name="dicSupplier">供应商字典</param>
        /// <param name="dicEnterprise">资源字典</param>
        /// <param name="dicCustomer">客户字典</param>
        /// <param name="dicWarehouse">仓库字典</param>
        /// <param name="dicTransactions">单据小类字典</param>
        /// <param name="dicDetails">发运单明细字典</param>
        /// <param name="dicItem">物料字典</param>
        /// <param name="dicLocation">库位字典</param>
        /// <param name="dicLot">批次字典</param>
        /// <param name="dicPo">PO字典</param>
        /// <param name="dicItemIOLimit">仓储字典</param>
        /// <param name="defaultAssignRule">默认分配规则</param>
        /// <param name="defaultTurnOverRule">默认周转规则</param>
        //private void SaveShippingOrder(ShippingOrderData data,
        //    Dictionary<string, ShippingOrder> dic,
        //    Dictionary<string, Supplier> dicSupplier,
        //    Dictionary<string, Enterprise> dicEnterprise,
        //    Dictionary<string, Customer> dicCustomer,
        //    Dictionary<string, Warehouse> dicWarehouse,
        //    Dictionary<string, List<Transaction>> dicTransactions,
        //    Dictionary<string, List<ShippingOrderDetail>> dicDetails,
        //    Dictionary<string, Item> dicItem,
        //    Dictionary<string, StorageLocation> dicLocation,
        //    Dictionary<string, Lot> dicLot,
        //    Dictionary<string, PurchaseOrder> dicPo,
        //    Dictionary<string, ItemIOLimit> dicItemIOLimit,
        //    AssignRule defaultAssignRule,
        //    TurnOverRule defaultTurnOverRule
        //    )
        //{
        //    using (var tran = DB.TransactionScope(ShipmentEntityDataProvider.ConnectionStringName))
        //    {
        //        var ctl = RT.Service.Resolve<DownloadBusBaseController>();
        //        var dtlCtl = RT.Service.Resolve<DownloadShipmentDtlController>();

        //        ShippingOrder shippingOrder;
        //        var key = data.No;
        //        if (key.IsNullOrEmpty())
        //            throw new ValidationException("发运单号为空".L10nFormat(key));
        //        //处理待删除数据
        //        if (dic.ContainsKey(key))
        //        {
        //            if (data.IsDelete)
        //            {
        //                ctl.DeleteEntity(dic, key, dic[key]);
        //            }
        //            return;
        //        }
        //        if (!dic.ContainsKey(key))
        //            dic.Add(key, new ShippingOrder());
        //        shippingOrder = dic[key];

        //        //获取供应商实体
        //        Supplier supplier = null;
        //        if (data.OrderType == (int)OrderType.SupplierReturn && !dicSupplier.TryGetValue(data.SupplierCode, out supplier))
        //        {
        //            throw new ValidationException("供应商编码[{0}]不存在".L10nFormat(data.SupplierCode));
        //        }

        //        Enterprise enterprise = null;
        //        if (data.OrderType == (int)OrderType.WorkFeed && !dicEnterprise.TryGetValue(data.EnterpriseCode, out enterprise))
        //        {
        //            throw new ValidationException("部门编码[{0}]不存在".L10nFormat(data.EnterpriseCode));
        //        }

        //        Customer customer = null;
        //        if (data.OrderType == (int)OrderType.SaleOut && !dicCustomer.TryGetValue(data.CustomerCode, out customer))
        //        {
        //            throw new ValidationException("客户编码[{0}]不存在".L10nFormat(data.CustomerCode));
        //        }

        //        //获取仓库实体
        //        Warehouse warehouse;
        //        if (!dicWarehouse.TryGetValue(data.ShippingWareHouseCode, out warehouse))
        //            throw new ValidationException("仓库编码[{0}]不存在[字段ShippingWareHouseCode]".L10nFormat(data.ShippingWareHouseCode));

        //        shippingOrder.No = data.No;
        //        shippingOrder.OrderState = (ShippingOrderState)data.OrderState;
        //        shippingOrder.OrderType = (OrderType)data.OrderType;
        //        shippingOrder.DeliveryDate = data.DeliveryDate;               
        //        shippingOrder.Connecter = data.Connecter;

        //        if (!dicTransactions.ContainsKey(shippingOrder.OrderType.ToString()))
        //            throw new ValidationException("单据大类[{0}]未维护单据小类[单据大类字段OrderType]。".L10nFormat(shippingOrder.OrderType.ToLabel()));

        //        shippingOrder.Transaction = dicTransactions[shippingOrder.OrderType.ToString()].FirstOrDefault();
        //        shippingOrder.ShippingWareHouse = warehouse;

        //        shippingOrder.Supplier = supplier;
        //        shippingOrder.Enterprise = enterprise;
        //        shippingOrder.Customer = customer;

        //        shippingOrder.Contacts = data.Contacts;
        //        shippingOrder.ContactsNumber = data.ContactNumber;
        //        shippingOrder.SourceKey = data.ErpKey;
        //        shippingOrder.Address = data.Address;
        //        shippingOrder.TransportCompany = data.TransportCompany;
        //        shippingOrder.TransportNo = data.TransportNo;
        //        shippingOrder.PriorityType = (PriorityType)data.PriorityType;
        //        shippingOrder.CancelReason = data.CancelReason;
        //        shippingOrder.Remark = data.Remark;

        //        RF.Save(shippingOrder);

        //        if (data.DetailList.Count > 0)
        //        {
        //            if (!dicDetails.ContainsKey(data.No))
        //                dicDetails.Add(data.No, new List<ShippingOrderDetail>());
        //            var dicDetail = dicDetails[data.No].ToDictionary(p => "{0}_{1}".FormatArgs(p.Item.Code, p.LineNo));   //物料+行号 为主键
        //            foreach (var detail in data.DetailList)
        //            {
        //                dtlCtl.SaveShippingOrderDetail(detail, dicDetail, dicItem, shippingOrder, dicLocation, dicLot, dicPo, dicItemIOLimit, defaultAssignRule, defaultTurnOverRule);
        //            }
        //        }
        //        tran.Complete();
        //    }
        //}
    }
}
