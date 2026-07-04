using SIE.APS;
using SIE.Common.InvOrg;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Units;
//using SIE.Kit.APS.EngineeringPlans;
using SIE.Resources.Enterprises;
using SIE.SO.SaleOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 工程计划处理类
    /// </summary>
    public class GenerateEngineerPlanHandle
    {
        /// <summary>
        ///工厂 key:工厂编码，value：工厂信息
        /// </summary>
        protected Dictionary<string, List<Enterprise>> EnterpriseDic { get; set; }
        /// <summary>
        /// 销售订单 key:销售订单编码，value：销售订单信息
        /// </summary>
        protected Dictionary<string, List<SaleOrder>> SaleOrderDic { get; set; }

        /// <summary>
        /// 销售订单 key:销售订单ID，value：销售订单信息
        /// </summary>
        protected Dictionary<double, List<SaleOrder>> SaleOrderIdDic { get; set; }

        /// <summary>
        /// 客户 key:客户编码，value：客户信息
        /// </summary>
        protected Dictionary<string, List<Customer>> CustomerDic { get; set; }

        /// <summary>
        /// 物料 key:物料编码，value：物料信息
        /// </summary>
        protected Dictionary<string, List<Item>> ItemDic { get; set; }

        /// <summary>
        /// 单位 key:单位编码，value：单位信息
        /// </summary>
        protected Dictionary<string, List<Unit>> UnitDic { get; set; }

        /// <summary>
        /// 工程计划
        /// </summary>
       // protected EntityList<EngineeringPlan> EngineeringPlans { get; set; }

        /// <summary>
        /// 有效销售订单明细
        /// </summary>
        protected List<SaleOrderDetail> ValidateSaleOrderDetailList { get; set; }

        /// <summary>
        /// 无效销售订单明细
        /// </summary>
        protected EntityList<SaleOrderDetail> ValidateNoSaleOrderDetailList { get; set; }

        /// <summary>
        /// 销售订单明细列表
        /// </summary>
        protected EntityList<SaleOrderDetail> SaleOrderDetailList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateEngineerPlanHandle()
        {
            ValidateSaleOrderDetailList = new List<SaleOrderDetail>();
            ValidateNoSaleOrderDetailList = new EntityList<SaleOrderDetail>();
            //EngineeringPlans = new EntityList<EngineeringPlan>();
        }

        /// <summary>
        /// 生成工程计划
        /// </summary>
        /// <param name="orderDetailIds">订单明细列表</param>
        /// <returns>返回处理消息</returns>
        public virtual string GenerateEngineeringPlan(List<double> orderDetailIds)
        {
            // 加载数据
            LoadBaseData(orderDetailIds);
            if (SaleOrderDetailList.Count == 0) return string.Empty;

            // 验证数据
            string msg = ValidateTechOrder();
            if (ValidateSaleOrderDetailList.Count <= 0)
            {
                return msg;
            }
            //// 生成工程计划
            //EntityList<EngineeringPlan> engineeringPlanList = GenerateEngineeringPlan();
            //if (engineeringPlanList.Count <= 0)
            //{
            //    return msg;
            //}
            //// 处理相关数据，并保存
            //DoRelevantData(engineeringPlanList);
            return msg;
        }

        /// <summary>
        /// 获取基础所需数据
        /// </summary>
        /// <param name="orderDetailIds">销售订单明细Id列表</param>
        private void LoadBaseData(List<double> orderDetailIds)
        {
            EnterpriseController enterpriseController = RT.Service.Resolve<EnterpriseController>();
            SaleOrderController saleOrderController = RT.Service.Resolve<SaleOrderController>();
            CustomerController customerController = RT.Service.Resolve<CustomerController>();
            ItemController itemController = RT.Service.Resolve<ItemController>();
            UnitsController unitsController = RT.Service.Resolve<UnitsController>();
            //EngineeringPlanController engineeringPlanController = RT.Service.Resolve<EngineeringPlanController>();
            List<string> doucenterpriseCodes = new List<string>();
            List<double> orderIds = new List<double>();
            List<string> customeCodes = new List<string>();
            List<string> itemCodes = new List<string>();
            List<string> unitCodes = new List<string>();
            SaleOrderDetailList = RT.Service.Resolve<SaleOrderDetailController>().GetSalesOrderDetails(orderDetailIds);
            SaleOrderDetailList.ForEach(item =>
            {
                if (item.Enterprise != null && !doucenterpriseCodes.Contains(item.Enterprise.Code))
                    doucenterpriseCodes.Add(item.Enterprise.Code);
                if (!orderIds.Contains(item.SaleOrderId))
                    orderIds.Add(item.SaleOrderId);
                if (!itemCodes.Contains(item.Item.Code))
                    itemCodes.Add(item.Item.Code);
                if (item.Unit != null && !unitCodes.Contains(item.Unit.Code))
                    unitCodes.Add(item.Unit.Code);

            });
            EntityList<SaleOrder> saleOrderList = saleOrderController.GetSalesOrders(orderIds); //获取销售订单
            SaleOrderDic = saleOrderList.GroupBy(p => p.Code).ToDictionary(p => p.Key, p => p.ToList());
            SaleOrderIdDic = saleOrderList.GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.ToList());
            saleOrderList.ForEach(item =>
            {
                if (!customeCodes.Contains(item.Customer.Code))
                    customeCodes.Add(item.Customer.Code);
            });
            EntityList<Enterprise> enterpriseList = enterpriseController.GetAssistantInvOrg(doucenterpriseCodes); //工厂
            if (enterpriseList.Count > 0)
                EnterpriseDic = enterpriseList.GroupBy(p => p.Code).ToDictionary(p => p.Key, p => p.ToList());
            else
                EnterpriseDic = new Dictionary<string, List<Enterprise>>();
            EntityList<Customer> customerList = customerController.GetCustomerInvOrgList(customeCodes); //获取客户
            CustomerDic = customerList.GroupBy(p => p.Code).ToDictionary(p => p.Key, p => p.ToList());
            EntityList<Item> itemList = itemController.GetItemInvOrgList(itemCodes);//获取物料
            ItemDic = itemList.GroupBy(p => p.Code).ToDictionary(p => p.Key, p => p.ToList());
            EntityList<Unit> unitList = unitsController.GetUnitList(unitCodes);//获取单位
            UnitDic = unitList.GroupBy(p => p.Code).ToDictionary(p => p.Key, p => p.ToList());
           // EngineeringPlans = engineeringPlanController.GetItemLastVersion(itemCodes); //版本号
        }

        /// <summary>
        /// 验证厂别确认是否可以生成工程计划
        /// </summary>
        /// <returns>返回验证不通过的消息</returns>
        private string ValidateTechOrder()
        {
            // 返回结果信息
            var errMsg = string.Empty;
            foreach (var saleOrderDetail in SaleOrderDetailList)
            {
                errMsg = CheckGenerateOrder(errMsg, saleOrderDetail);
                if (errMsg.Length <= 0)
                {
                    ValidateSaleOrderDetailList.Add(saleOrderDetail);
                    saleOrderDetail.LineState = LineState.CONFIRMED;
                }
                else
                {
                    saleOrderDetail.EngineeringPlanState = false;
                    saleOrderDetail.EngineeringPlanRemark = errMsg;
                }
            }
            return errMsg;
        }

        /// <summary>
        /// 检验生成制程单的合法性
        /// </summary>
        /// <param name="saleOrderDetail">销售订单明细</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private string CheckGenerateOrder(string errMsg, SaleOrderDetail saleOrderDetail)
        {
            if (saleOrderDetail.Enterprise == null)
            {
                errMsg += "请先提交分配库存组织,再进行生成制程单！\n".L10N();
                return errMsg;
            }

            if (EnterpriseId(saleOrderDetail) == 0)
            {
                errMsg += "销售订单[{0}],行号[{1}],找不到库存组织[{2}],请通知对应库存组织相关人员进行添加！\n".L10N().FormatArgs(saleOrderDetail.SaleOrder.Code, saleOrderDetail.LineNo, saleOrderDetail.Enterprise.Name);
                return errMsg;
            }
            if (CustomerId(saleOrderDetail) == 0)
            {
                errMsg += "没有创建客户[{0}]，请通知相关人员进行添加！\n".L10N().FormatArgs(saleOrderDetail.SaleOrder.Customer.Name);
                return errMsg;
            }
            if (ItemId(saleOrderDetail) == 0)
            {
                errMsg += "没有创建物料[{0}]，请通知相关人员进行添加！\n".L10N().FormatArgs(saleOrderDetail.Item.Name);
                return errMsg;
            }
            if (UnitId(saleOrderDetail) == 0)
            {
                errMsg += "没有创建单位[{0}]，请通知对应库存组织相关人员进行添加！\n".L10N().FormatArgs(saleOrderDetail.Unit.Name);
                return errMsg;
            }
            return errMsg;
        }

        /// <summary>
        /// 生成工程计划
        /// </summary>
        //private EntityList<EngineeringPlan> GenerateEngineeringPlan()
        //{
        //    EntityList<EngineeringPlan> engineeringPlanList = new EntityList<EngineeringPlan>();
        //    foreach (var data in ValidateSaleOrderDetailList)
        //    {
        //        EngineeringPlan engineeringPlan = new EngineeringPlan();
        //        engineeringPlan.Code = SaleOrderDic[data.SaleOrder.Code].FirstOrDefault().Code; //销售订单编码
        //        engineeringPlan.LineNo = data.LineNo;//行号
        //        engineeringPlan.ItemId = ItemId(data);//物料
        //        //engineeringPlan.Version = data.Version;//版本号
        //        engineeringPlan.ItemRevision = data.ItemRevision;//版本号
        //        engineeringPlan.ItemExtPropName = data.ItemExtPropName;//版本号显示
        //        engineeringPlan.IndustryType = data.IndustryType;//行业类型
        //        engineeringPlan.OrderType = data.OrderType;//订单类型
        //        engineeringPlan.ProductType = data.ProductType;//产品类型
        //        engineeringPlan.SpecialProcessStr = data.SpecialProcessStr;//特殊工艺
        //        engineeringPlan.IsNew = data.IsNew;//是否新单
        //        engineeringPlan.LineState = data.LineState;//行状态
        //        engineeringPlan.Qty = data.Qty;//数量
        //        engineeringPlan.UnitId = UnitId(data);//单位
        //        engineeringPlan.TargetOrderCode = data.TargetOrderCode;//生产订单编号     
        //        engineeringPlan.EnterpriseId = EnterpriseId(data); //工厂   
        //        engineeringPlan.MiDateTime = data.MiDateTime;//MI完成时间
        //        engineeringPlan.Area = data.Area;// 面积M2
        //        engineeringPlan.PlateSize = data.PlateSize;// 大板尺寸
        //        engineeringPlan.MaterialPnl = data.MaterialPnl;// 开料PNL数
        //        engineeringPlan.SetPnl = data.SetPnl;// SET/PNL数
        //        engineeringPlan.PcsPnl = data.PcsPnl;// PCS/PNL数
        //        engineeringPlan.RequireDelivery = data.RequireDelivery;// 客户交期
        //        if (data.PromiseDelivery != null)
        //            engineeringPlan.PromiseDelivery = data.PromiseDelivery.Value;// 承诺交期
        //        engineeringPlan.CustomerId = CustomerId(data);// 客户
        //        engineeringPlan.EmployeeId = SaleOrderDic[data.SaleOrder.Code].FirstOrDefault().EmployeeId;// 员工
        //        engineeringPlan.Remark = SaleOrderDic[data.SaleOrder.Code].FirstOrDefault().Remark;// 备注
        //        engineeringPlan.IsHangUp = data.IsHangUp;// 订单行挂起
        //        engineeringPlan.ProductLevel = data.ProductLevel;// 产品等级
        //        engineeringPlan.EngineeringPlanState = EngineerPlanState(data.IsNew, ItemId(data), data.ItemRevision);
        //        engineeringPlanList.Add(engineeringPlan);
        //    }
        //    return engineeringPlanList;
        //}

        /// <summary>
        /// 处理相关事情
        /// </summary>
        //public virtual void DoRelevantData(EntityList<EngineeringPlan> engineeringPlanList)
        //{
        //    var invOrgId = AppRuntime.InvOrg;
        //    try
        //    {
        //        using (var tran = DB.TransactionScope(ApsCoreEntityDataProvider.ConnectionStringName))
        //        {
        //            var invGroups = engineeringPlanList.GroupBy(p => InvOrgIdExtension.GetInvOrgId(p)).ToList();
        //            foreach (var invGroup in invGroups)
        //            {
        //                AppRuntime.InvOrg = invGroup.Key;
        //                foreach (var data in invGroup)
        //                {
        //                    RF.Save(data);
        //                }
        //            }
        //            RF.Save(SaleOrderDetailList);
        //            tran.Complete();
        //        }
        //    }

        //    finally
        //    {
        //        AppRuntime.InvOrg = invOrgId;
        //    }
        //}
        /// <summary>
        /// 通过主库企业编码获得从库的企业编码
        /// </summary>
        /// <param name="saleOrderDetail">销售订单明细</param>
        /// <returns></returns>
        public double EnterpriseId(SaleOrderDetail saleOrderDetail)
        {
            if (EnterpriseDic.Count > 0 && EnterpriseDic.Keys.Contains(saleOrderDetail.Enterprise.Code))
            {
                var enterpriseModel = EnterpriseDic[saleOrderDetail.Enterprise.Code].FirstOrDefault();
                if (enterpriseModel != null)
                    return enterpriseModel.Id;
            }
            return 0;
        }

        /// <summary>
        /// 返回从库客户ID
        /// </summary>
        /// <param name="saleOrderDetail">销售订单明细</param>
        /// <returns></returns>
        public double CustomerId(SaleOrderDetail saleOrderDetail)
        {
            var customerIdModel = SaleOrderIdDic[saleOrderDetail.SaleOrder.Id].FirstOrDefault();
            if (customerIdModel != null)
            {
                var customerModel = CustomerDic[customerIdModel.Customer.Code];
                var customer = customerModel.Where(x => x.Id != customerIdModel.Customer.Id).FirstOrDefault();
                if (customer != null)
                    return customer.Id;
            }
            return 0;
        }

        /// <summary>
        /// 返回从库物料ID
        /// </summary>
        /// <param name="saleOrderDetail">销售订单明细</param>
        /// <returns></returns>
        public double ItemId(SaleOrderDetail saleOrderDetail)
        {
            var itemModel = ItemDic[saleOrderDetail.Item.Code];
            var item = itemModel.Where(x => x.Id != saleOrderDetail.ItemId).FirstOrDefault();
            if (item != null)
                return item.Id;
            return 0;
        }

        /// <summary>
        /// 返回从库单位ID
        /// </summary>
        /// <param name="saleOrderDetail">销售订单明细</param>

        /// <returns></returns>
        public double UnitId(SaleOrderDetail saleOrderDetail)
        {
            var unitModel = UnitDic[saleOrderDetail.Unit.Code];
            var unit = unitModel.Where(x => x.Id != saleOrderDetail.UnitId).FirstOrDefault();
            if (unit != null)
                return unit.Id;
            return 0;
        }

        /// <summary>
        ///  工程计划状态
        /// </summary>
        /// <param name="isNewOrder">是否新单</param>
        /// <param name="itemId">物料</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        //public EngineeringPlanState EngineerPlanState(bool isNewOrder, double itemId, string itemRevision)
        //{
        //    if (isNewOrder)
        //    {
        //        return EngineeringPlanState.NotPlan;
        //    }
        //    else
        //    {
        //        var engineeringPlan = EngineeringPlans.Where(x => x.ItemId == itemId && x.ItemRevision == itemRevision).OrderByDescending(x => x.ActualCompleteDateTime).FirstOrDefault();
        //        if (engineeringPlan != null)
        //            return EngineeringPlanState.Completed;
        //    }
        //    return EngineeringPlanState.NotPlan;
        //}
    }
}
