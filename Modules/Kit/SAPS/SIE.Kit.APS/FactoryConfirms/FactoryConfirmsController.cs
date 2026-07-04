using SIE.Domain;
using SIE.Kit.EventMessages.FactoryConfirms;
using SIE.SO.SaleOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 工厂确认控制器
    /// </summary>
    public partial class FactoryConfirmsController : DomainController
    {
        /// <summary>
        /// 工厂修改
        /// </summary>
        /// <param name="factoryConfirms">厂别确认实体</param>
        /// <param name="IdList">勾选销售订单明细ID列表</param>
        /// <returns></returns>
        public virtual List<FactoryConfirmsViewModel> SaveFactory(List<FactoryConfirmsViewModel> factoryConfirms, List<double> IdList)
        {
            SaleOrderDetailController saleOrderDetailController = new SaleOrderDetailController();
            EntityList<SaleOrderDetail> salesOrderDetailList = saleOrderDetailController.GetSalesOrderDetails(IdList);
            foreach (var salesOrderDetail in salesOrderDetailList)
            {
                double enterpriseId = factoryConfirms.Where(x => x.Id == salesOrderDetail.Id).FirstOrDefault().EnterpriseId;
                salesOrderDetail.LineState = LineState.CONFIRMED;
                salesOrderDetail.EnterpriseId = enterpriseId;
            }
            RF.Save(salesOrderDetailList);
            return factoryConfirms;
        }

        /// <summary>
        /// 获取已确认工厂的 销售单明细
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<FactoryConfirmsViewModel2> GetHistory()
        {
            //因行状态节点无地方变更状态暂时不作为数据筛选条件
            return Query<FactoryConfirmsViewModel2>().Where(x => x.EnterpriseId > 0 ).ToList();
            //return Query<FactoryConfirmsViewModel2>().Where(x => x.EnterpriseId > 0 && x.LineState != LineState.NEW
            //).ToList();
        }

        /// <summary>
        /// 获取已确认工厂的 销售单明细
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<FactoryConfirmsViewModel2> GetConfirmsViewModel2(List<string> years)
        {
            List<int> lstDt = new List<int>();
            foreach (var y in years)
                lstDt.Add(Convert.ToInt32(y));
            DateTime start = new DateTime(lstDt.Min(), 1, 1);
            DateTime end = new DateTime(lstDt.Max(), 12, 31);
            return Query<FactoryConfirmsViewModel2>().Where(x => x.EnterpriseId > 0 &&
            ((x.RequireDelivery >= start && x.RequireDelivery <= end) || (x.PromiseDelivery != null && x.PromiseDelivery >= start && x.PromiseDelivery <= end))
                ).ToList();
        }

        /// <summary>
        /// 按月份统计负载
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public virtual List<OutputCapacityInfo> SumMonthLoad(int year)
        {
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime lastDay = firstDay.AddYears(1).AddTicks(-1);
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SaleOrderDetail.EnterpriseProperty);
            var queryPromiseDelivery = Query<SaleOrderDetail>().Where(p => p.PromiseDelivery != null && p.PromiseDelivery >= firstDay && p.PromiseDelivery <= lastDay && p.EnterpriseId > 0).ToList(null, elo);
            var queryRequireDelivery = Query<SaleOrderDetail>().Where(p => p.PromiseDelivery == null && p.RequireDelivery >= firstDay && p.RequireDelivery <= lastDay && p.EnterpriseId > 0).ToList(null, elo);

            List<OutputCapacityInfo> outputCapacityInfos = new List<OutputCapacityInfo>();
            List<OutputCapacityInfo> groupCapacityInfos = new List<OutputCapacityInfo>();
            foreach (var tmp in queryPromiseDelivery)
            {
                OutputCapacityInfo outputCapacityInfo = new OutputCapacityInfo()
                {
                    LoadArea = tmp.Area,
                    HourDate = Convert.ToDateTime(tmp.PromiseDelivery.Value.Year + "/" + tmp.PromiseDelivery.Value.Month),
                    Month = tmp.PromiseDelivery.Value.Month,
                    FactoryId = tmp.EnterpriseId.Value,
                    FactoryName = tmp.Enterprise.Name
                };
                outputCapacityInfos.Add(outputCapacityInfo);
            }
            foreach (var tmp in queryRequireDelivery)
            {
                OutputCapacityInfo outputCapacityInfo = new OutputCapacityInfo()
                {
                    LoadArea = tmp.Area,
                    HourDate = Convert.ToDateTime(tmp.RequireDelivery.Year + "/" + tmp.RequireDelivery.Month),
                    Month = tmp.RequireDelivery.Month,
                    FactoryId = tmp.EnterpriseId.Value,
                    FactoryName = tmp.Enterprise.Name
                };
                outputCapacityInfos.Add(outputCapacityInfo);
            }
            var factoryList = outputCapacityInfos.GroupBy(p => new { p.FactoryId, p.FactoryName }).Distinct().OrderBy(x => x.Key.FactoryId).ToList();
            var definitionList = outputCapacityInfos.GroupBy(p => new { HourDate = p.HourDate, FactoryId = p.FactoryId, FactoryName = p.FactoryName }).Distinct().ToList();
            definitionList.ForEach(p =>
            {
                var dataGroup = new OutputCapacityInfo();
                dataGroup.HourDate = p.Key.HourDate;
                dataGroup.FactoryId = p.Key.FactoryId;
                dataGroup.FactoryName = p.Key.FactoryName;
                dataGroup.Month = p.Min(y => y.Month);
                dataGroup.LoadArea = p.Sum(y => y.LoadArea);
                groupCapacityInfos.Add(dataGroup);
            });
            foreach (var factory in factoryList)
            {
                for (int i = 1; i <= 12; i++)
                {
                    DateTime newDate = lastDay.AddMonths(-i);
                    DateTime hourDate = Convert.ToDateTime(newDate.Year + "/" + newDate.Month);
                    int month = newDate.Month;
                    var isExist = groupCapacityInfos.Where(x => x.FactoryId == factory.Key.FactoryId && x.Month == month).FirstOrDefault();
                    if (isExist == null)
                    {
                        var dataGroup = new OutputCapacityInfo();
                        dataGroup.HourDate = hourDate;
                        dataGroup.Month = month;
                        dataGroup.FactoryId = factory.Key.FactoryId;
                        dataGroup.FactoryName = factory.Key.FactoryName;
                        dataGroup.LoadArea = 0;
                        groupCapacityInfos.Add(dataGroup);
                    }
                }
            }
            return groupCapacityInfos;
        }

        /// <summary>
        /// 生成工序计划
        /// </summary>
        /// <param name="orderDetailIds">销售订单明细ID列表</param>
        /// <returns>生成结果</returns>
        public virtual string GenerateEngineeringPlan(List<double> orderDetailIds)
        {
            GenerateEngineerPlanHandle handle = new GenerateEngineerPlanHandle();
            var msg = handle.GenerateEngineeringPlan(orderDetailIds);
            return msg;
        }

        
        /// <summary>
        /// 生成订单评审
        /// </summary>
        /// <param name="ids">销售订单明细Ids</param>
        public virtual void GenerateFactoryConfirms(List<double> ids)
        {
            RT.Service.Resolve<ICreateOrderReview>().CreateOrderReview(ids);
        }

        /// <summary>
        ///  根据条件查询返回厂别确认列表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回厂别确认列表</returns>
        public virtual EntityList<FactoryConfirmsViewModel> GetFactoryConfirmList(FactoryConfirmsViewModelCriteria criteria)
        {
            var query = Query<FactoryConfirmsViewModel>();
            //销售订单编码
            if (criteria.SalesOrderCode.IsNotEmpty())
            {
                query.Where(p => p.SaleOrder.Code.Contains(criteria.SalesOrderCode));
            }
            //物料
            if (criteria.ItemCode.IsNotEmpty())
            {
                query.Where(p => p.ItemCode.Contains(criteria.ItemCode));
            }
            //物料名称
            if (criteria.ItemName.IsNotEmpty())
            {
                query.Where(p => p.ItemName.Contains(criteria.ItemName));
            }
            //行业类型
            if (criteria.IndustryType.IsNotEmpty())
            {
                query.Where(p => p.IndustryType == criteria.IndustryType);
            }
            //订单类型
            if (criteria.OrderType.IsNotEmpty())
            {
                query.Where(p => p.OrderType == criteria.OrderType);
            }
            //产品类型
            if (criteria.ProductType.IsNotEmpty())
            {
                query.Where(p => p.ProductType == criteria.ProductType);
            }
            //行状态
            if (criteria.LineState.ToLabel().IsNotEmpty())
            {
                query.Where(p => p.LineState == criteria.LineState);
            }
            //库存组织
            if (criteria.EnterpriseId > 0)
            {
                query.Where(p => p.Enterprise.Id == criteria.EnterpriseId);
            }
            //新单
            if (criteria.IsNew.HasValue)
            {
                query.Where(p => p.IsNew == criteria.IsNew);
            }
            //客户交期
            if (criteria.RequireDelivery.BeginValue.HasValue)
            {
                query.Where(p => p.RequireDelivery >= criteria.RequireDelivery.BeginValue);
            }
            //客户交期
            if (criteria.RequireDelivery.EndValue.HasValue)
            {
                query.Where(p => p.RequireDelivery <= criteria.RequireDelivery.EndValue);
            }
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            var queryList = query.ToList(criteria.PagingInfo, elo);
            foreach (var entity in queryList)
            {
                if (entity.Enterprise != null)
                    entity.ExtValues[FactoryConfirmsViewModelCriteria.EnterpriseIdProperty.Name + "_Display"] = entity.Enterprise.Code;
            }
            return queryList;
        }

    }
}
