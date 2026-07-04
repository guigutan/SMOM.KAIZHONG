using System;
using System.Collections.Generic;
using System.Linq;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.Items;
using SIE.Resources.Enterprises;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 生产订单控制器
    /// </summary>
    public partial class SaveProductOrderHandle
    {
        #region 属性
        /// <summary>
        /// 错误信息
        /// </summary>
        private List<ErpErrorData> errorDatas;

        /// <summary>
        /// 生产订单字典 key：生产订单编号 value:生产订单
        /// </summary>
        //private Dictionary<string, ProductOrder> ProductOrderDic { get; set; }

        /// <summary>
        /// 物料字典 key:物料编号 value:物料
        /// </summary>
        private Dictionary<string, Item> ItemDic { get; set; }

        /// <summary>
        /// 制程路线字典 key:制程路线编号 value：制程路线
        /// </summary>
        //private Dictionary<string, ProcessTechRoute> RouteDic { get; set; }

        /// <summary>
        /// 企业模型字典 key:工厂编号 value:企业模型
        /// </summary>
        private Dictionary<string, Enterprise> EnterpriseDic { get; set; }

        /// <summary>
        /// 客户字典 key:客户编号 value:客户
        /// </summary>
        private Dictionary<string, Customer> CustomerDic { get; set; }

        /// <summary>
        /// 生产订单不允许删除状态
        /// </summary>
        //private List<ProductOrderState> StateList { get; set; } = new List<ProductOrderState>() { ProductOrderState.UNGENERATE, ProductOrderState.INVALID };

        #region 控制器
        /// <summary>
        /// 物料控制器
        /// </summary>
        private readonly ItemController _itemController = RT.Service.Resolve<ItemController>();

        /// <summary>
        /// 生产订单控制器
        /// </summary>
        //private readonly ProductOrderController _productOrderController = RT.Service.Resolve<ProductOrderController>();

        /// <summary>
        /// 制程路线控制器
        /// </summary>
        //private readonly TechRouteController _routeController = RT.Service.Resolve<TechRouteController>();

        /// <summary>
        /// 企业模型控制器
        /// </summary>
        private readonly EnterpriseController _enterpriseController = RT.Service.Resolve<EnterpriseController>();

        /// <summary>
        /// 客户控制器
        /// </summary>
        private readonly CustomerController _customController = RT.Service.Resolve<CustomerController>();
        #endregion
        #endregion

        #region 接口平台
        /// <summary>
        /// 用于接口中心下载数据保存到SMOM 生产订单表中
        /// </summary>
        /// <param name="poData">生产订单表</param>
        /// <returns>错误数据列表</returns>
        public virtual List<ErpErrorData> SaveProductOrders(List<ProductOrderData> poData)
        {
            errorDatas = new List<ErpErrorData>();
            List<ProductOrderData> tmpPoDatas = new List<ProductOrderData>();

            if (poData == null || poData.Count == 0) return errorDatas;
            tmpPoDatas.AddRange(poData);
            try
            {
                LoadData(tmpPoDatas);
                errorDatas.AddRange(ValidateData(tmpPoDatas));

                foreach (var tmpPoData in tmpPoDatas)
                {
                    ErpErrorData tmpError = SaveProductOrder(tmpPoData);
                    if (tmpError != null) errorDatas.Add(tmpError);
                }
            }
            catch (Exception ex)
            {
                errorDatas.Clear();
                poData.ForEach(p => errorDatas.Add(new ErpErrorData() { ErrMsg = "代码数据级别错误:" + ex.Message, Infkey = p.Infkey, IsChild = false }));
            }

            return errorDatas;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 保存生产订单信息
        /// </summary>
        /// <param name="poData">生产订单信息</param>
        /// <returns>返回错误信息</returns>
        private ErpErrorData SaveProductOrder(ProductOrderData poData)
        {
            ErpErrorData errorData = null;
            if (poData == null) return errorData;

            try
            {
                //ProductOrder po = null;
                //if (ProductOrderDic.TryGetValue(poData.Code, out po))
                //{
                //    if (!StateList.Contains(po.OrderState) || !string.IsNullOrEmpty(po.RelevantCode))
                //    {
                //        errorData = new ErpErrorData() { Infkey = poData.Infkey, ErrMsg = string.Format("生产订单[{0}]已经被拆分或合并或审核", po.Code), IsChild = false };
                //        return errorData;
                //    }

                //    // 如果已经存在生产订单数据
                //    if (poData.IsDelete)
                //        po.PersistenceStatus = Domain.PersistenceStatus.Deleted;
                //    else SetProductOrder(po, poData);
                //}
                //else
                //{
                //    if (!poData.IsDelete)
                //    {
                //        // 如果不存在生产订单BOM数据,并且不需要删除数据
                //        po = new ProductOrder();
                //        po.GenerateId();
                //        SetProductOrder(po, poData);
                //    }
                //    else
                //    {
                //        // 如果不存在生产订单BOM数据,并且需要删除数据
                //        return errorData;
                //    }
                //}

                //RF.Save(po);
            }
            catch (Exception ex)
            {
                errorData = new ErpErrorData() { Infkey = poData.Infkey, ErrMsg = "代码数据级别错误:" + ex.Message, IsChild = false };
            }

            return errorData;
        }

        /// <summary>
        /// 设置生产订单
        /// </summary>
        /// <param name="po">生产订单实体</param>
        /// <param name="poData">生产订单(接口数据)</param>
        //private void SetProductOrder(ProductOrder po, ProductOrderData poData)
        //{
        //    Item item = null;
        //    ProcessTechRoute route = null;
        //    Enterprise enterprise = null;
        //    Customer customer = null;
        //    if (!string.IsNullOrEmpty(poData.ItemCode)) item = ItemDic[poData.ItemCode];
        //    if (!string.IsNullOrEmpty(poData.RouteCode)) route = RouteDic[poData.RouteCode];
        //    if (!string.IsNullOrEmpty(poData.FactoryCode)) enterprise = EnterpriseDic[poData.FactoryCode];
        //    if (!string.IsNullOrEmpty(poData.CustomerCode)) customer = CustomerDic[poData.CustomerCode];

        //    po.Code = poData.Code;
        //    if (item != null) po.ItemId = item.Id;
        //    po.Qty = poData.Qty;
        //    po.Priority = poData.Priority;
        //    po.ProcessTechRouteId = route?.Id;
        //    po.OrderType = (ProductOrderType)poData.OrderType;
        //    po.FactoryId = enterprise?.Id;
        //    po.SaleNo = poData.SaleNo;
        //    po.CustomerId = customer?.Id;
        //    po.RequireDelivery = poData.RequireDelivery;
        //    po.PromiseDelivery = poData.PromiseDelivery;
        //    po.RawMaterialDate = poData.RawMaterialDate;
        //    po.SuggestStart = poData.SuggestStart;
        //    po.SuggestEnd = poData.SuggestEnd;
        //}

        /// <summary>
        /// 加载所需数据
        /// </summary>
        /// <param name="poDatas">生产订单数据</param>
        private void LoadData(List<ProductOrderData> poDatas)
        {
            // 根据生产订单编号获取生产订单数据
            var poCodes = poDatas.Select(m => m.Code).Distinct().ToList();
            //ProductOrderDic = _productOrderController.GetProductOrderByCodes(poCodes).ToDictionary(p => p.Code);

            // 根据物料编号获取物料数据
            List<string> itemCodes = poDatas.Select(p => p.ItemCode).Distinct().ToList();
            ItemDic = _itemController.GetItems(itemCodes).ToDictionary(p => p.Code);

            // 根据制程路线编号获取制程路线数据
            var routeCodes = poDatas.Select(n => n.RouteCode).Distinct().ToList();
            //RouteDic = _routeController.GetTechRouteByCodeList(routeCodes).ToDictionary(p => p.Code);

            // 根据工厂编号获取工厂信息
            var factoryCodes = poDatas.Select(n => n.FactoryCode).Distinct().ToList();
            EnterpriseDic = _enterpriseController.GetEnterprises(factoryCodes).ToDictionary(p => p.Code);

            // 根据客户编号获取客户信息
            var customerCodes = poDatas.Select(n => n.CustomerCode).Distinct().ToList();
            CustomerDic = _customController.GetCustomers(customerCodes, null).ToDictionary(p => p.Code);
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="poDatas">生产订单数据</param>
        private List<ErpErrorData> ValidateData(List<ProductOrderData> poDatas)
        {
            List<ErpErrorData> errors = new List<ErpErrorData>();
            for (int i = 0; i < poDatas.Count;)
            {
                var poData = poDatas[i];
                string errMsg = string.Empty;
                if (string.IsNullOrEmpty(poData.Code))
                    errMsg = "生产订单编号不允许为空！";

                if (string.IsNullOrEmpty(poData.ItemCode))
                    errMsg = "物料编号不允许为空！";
                else if (!ItemDic.ContainsKey(poData.ItemCode))
                    errMsg = string.Format("匹配不到物料编号[{0}]！", poData.ItemCode);

                //if (!string.IsNullOrEmpty(poData.RouteCode) && !RouteDic.ContainsKey(poData.RouteCode))
                //    errMsg = string.Format("匹配不到制程路线编号[{0}]！", poData.RouteCode);

                if (!string.IsNullOrEmpty(poData.FactoryCode) && !EnterpriseDic.ContainsKey(poData.FactoryCode))
                    errMsg = string.Format("匹配不到工厂编号[{0}]！", poData.FactoryCode);

                if (!string.IsNullOrEmpty(poData.CustomerCode) && !CustomerDic.ContainsKey(poData.CustomerCode))
                    errMsg = string.Format("匹配不到客户编号[{0}]！", poData.CustomerCode);

                if (!string.IsNullOrEmpty(errMsg))
                {
                    errors.Add(new ErpErrorData() { Infkey = poData.Infkey, ErrMsg = errMsg, IsChild = false });
                    poDatas.Remove(poData);
                }
                else
                {
                    i++;
                }
            }

            return errors;
        }
        #endregion
    }
}