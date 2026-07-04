using SIE.Common.Configs;
using SIE.Common.Sort;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Kit.APS.EngineerPlans.Configs;
using SIE.Kit.APS.EngineerPlans.Handle;
using SIE.Kit.APS.EngineerPlans.HelpClass;
using SIE.Kit.EventMessages.EngineerPlans;
using SIE.Resources.Employees;
using SIE.SO.SaleOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 工程计划MI 控制器
    /// </summary>
    public class EngineerPlanController : DomainController, ITaskCompleteSentBackToMIPlan
    {
        /// <summary>
        /// 查询 计划
        /// </summary>
        /// <param name="Criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EngineerPlan> Fetch(EngineerPlanCriteria Criteria)
        {
            var q = Query<EngineerPlan>();
            if (Criteria.FactoryId > 0)
            {
                q.Where(o => o.FactoryId == Criteria.FactoryId);
            }
            if (Criteria.SaleOrderNo.IsNotEmpty())
                q.Where(o => o.SaleOrderNo.Contains("%" + Criteria.SaleOrderNo + "%"));
            if (Criteria.LineNo.IsNotEmpty())
                q.Where(o => o.LineNo == Criteria.LineNo);

            if (Criteria.ItemId.HasValue)
                q.Where(o => o.ItemId == Criteria.ItemId);
            if (Criteria.ItemName.IsNotEmpty())
                q.Where(o => o.ItemName.Contains("%" + Criteria.ItemName + "%"));
            if (Criteria.CustomerCode.IsNotEmpty())
                q.Where(o => o.CustomerCode.Contains("%" + Criteria.CustomerCode + "%"));
            if (Criteria.CustomerName.IsNotEmpty())
                q.Where(o => o.CustomerName.Contains("%" + Criteria.CustomerName + "%"));
            if (Criteria.OrderType.IsNotEmpty())
                q.Where(o => o.OrderType == Criteria.OrderType);
            if (Criteria.PlanState != null)
                q.Where(o => o.PlanState == Criteria.PlanState.Value);

            if (Criteria.ScheduleDay != null)
            {
                if (Criteria.ScheduleDay.BeginValue != null)
                    q.Where(o => o.ScheduleDay >= Criteria.ScheduleDay.BeginValue.Value);
                if (Criteria.ScheduleDay.EndValue != null)
                    q.Where(o => o.ScheduleDay <= Criteria.ScheduleDay.EndValue.Value);
            }
            if (Criteria.SortDate != null)
            {
                if (Criteria.SortDate.BeginValue != null)
                    q.Where(o => o.SortDate >= Criteria.SortDate.BeginValue.Value);
                if (Criteria.SortDate.EndValue != null)
                    q.Where(o => o.SortDate <= Criteria.SortDate.EndValue.Value);
            }
            if (Criteria.RequireDelivery != null)
            {
                if (Criteria.RequireDelivery.BeginValue != null)
                    q.Where(o => o.RequireDelivery >= Criteria.RequireDelivery.BeginValue.Value);
                if (Criteria.RequireDelivery.EndValue != null)
                    q.Where(o => o.RequireDelivery <= Criteria.RequireDelivery.EndValue.Value);
            }

            return q.OrderBy(Criteria.OrderInfoList).ToList(Criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 初始化数据
        ///    8类等级
        /// </summary>
        public virtual void Init()
        {
            //检查是否有产品类型(销售明细单)
            if (DB.Query<CustLevel>().Count() == 0)
            {
                EntityList<CustLevel> list = new EntityList<CustLevel>();
                var c1 = new CustLevel()
                {
                    LevelName = "一类",
                    Hour = 24
                };
                list.Add(c1);
                var c2 = new CustLevel()
                {
                    LevelName = "二类",
                    Hour = 48
                };
                list.Add(c2);

                var c3 = new CustLevel()
                {
                    LevelName = "三类",
                    Hour = 72
                };
                list.Add(c3);

                var c4 = new CustLevel()
                {
                    LevelName = "四类",
                    Hour = 96
                };
                list.Add(c4);

                var c5 = new CustLevel()
                {
                    LevelName = "五类",
                    Hour = 120
                };
                list.Add(c5);

                var c6 = new CustLevel()
                {
                    LevelName = "六类",
                    Hour = 144
                };
                list.Add(c6);

                var c7 = new CustLevel()
                {
                    LevelName = "七类",
                    Hour = 168
                };
                list.Add(c7);

                var c8 = new CustLevel()
                {
                    LevelName = "八类",
                    Hour = 192
                };
                list.Add(c8);

                c1.SetProperty(SortExtension.INDEX_Property, 1);
                c2.SetProperty(SortExtension.INDEX_Property, 2);
                c3.SetProperty(SortExtension.INDEX_Property, 3);
                c4.SetProperty(SortExtension.INDEX_Property, 4);
                c5.SetProperty(SortExtension.INDEX_Property, 5);
                c6.SetProperty(SortExtension.INDEX_Property, 6);
                c7.SetProperty(SortExtension.INDEX_Property, 7);
                c8.SetProperty(SortExtension.INDEX_Property, 8);
                RF.Save(list);
            }

            if (DB.Query<CustLevelSetting>().Count() == 0)
            {
                var list2 = new EntityList<CustLevelSetting>();
                var levers = Query<CustLevel>().ToList();
                var allCustomer = Query<Customer>().OrderBy(o => o.Code).ToList();
                foreach (var item in allCustomer)
                {
                    list2.Add(new CustLevelSetting()
                    {
                        Customer = item,
                        CustLevel = levers.Last(),
                    });
                }
                RF.Save(list2);
            }
        }

        /// <summary>
        /// 验证等级日排产上限是否有数据 true：有数据 false：无数据
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateCustLevelDate()
        {
            return Query<CustLevel>().Count() > 0;
        }

        

        #region 同步销售订单信息至工程计划命令
        /// <summary>
        /// 同步订单信息
        /// </summary>
        public virtual string GenerateEngineerPlan()
        {
            string Msg = string.Empty;
            GenerateEngineerPlanHandle handle = new GenerateEngineerPlanHandle();
            handle.GenerateEngineerPlan();
            return Msg;
        }

        /// <summary>
        /// 获取可同步的销售订单行
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetSaleOrderDetailList()
        {
            //获取开始登记日期
            var startSoTime = GetConfig_StartSoTime();
            var query = Query<SaleOrderDetail>()
                .Join<EmployeeEnterprise>((x, y) => x.EnterpriseId == y.EnterpriseId)
                .Where<EmployeeEnterprise>((x, y) => y.EmployeeId == RT.IdentityId)
                .Where(o => o.SaleOrder.RegisterDateTime >= startSoTime)
                .Where(o => o.EnterpriseId > 0)
                .Where(o => o.IsNew == true || o.ExternalEcn == true);
            //过滤掉已经转过去 【计划表上的】
            query.NotExists<EngineerPlan>((sLine, Mi) => Mi.Where(p => p.SaleOrderDetailId == sLine.Id));
            query.OrderBy(o => o.RequireDelivery);
            var soLines = query.ToList(null, new EagerLoadOptions()
                                            .LoadWith(SaleOrderDetail.ItemProperty)
                                            .LoadWith(SaleOrderDetail.UnitProperty)
                                            .LoadWith(SaleOrderDetail.SaleOrderProperty)
                                            .LoadWith(SaleOrder.CustomerProperty));
            return soLines;
        }

        private DateTime GetConfig_StartSoTime()
        {
            var config = ConfigService.GetConfig(new EngineerPlan_SST_Config(), typeof(EngineerPlan));
            if (config == null)
                return (new DateTime(2000, 1, 1)).Date;
            return config.StartRegisterDateTime;
        }

        #endregion

        #region 排计划命令
        /// <summary>
        /// 排计划命令
        /// </summary>
        public virtual void DoSchedule()
        {
            DoScheduleHandle handle = new DoScheduleHandle();
            handle.DoSchedule();
        }


        /// <summary>
        /// 根据id获取Mi任务计划
        /// </summary>
        /// <param name="SaleOrderDetailIds"></param>
        /// <returns>Mi任务计划</returns>
        public virtual EntityList<EngineerPlan> GetPlanBy_SaleOrderDetailIds(List<double> SaleOrderDetailIds)
        {
            List<double?> tmpList = SaleOrderDetailIds.Select(o => (double?)o).ToList();
            return tmpList.SplitContains(ids => Query<EngineerPlan>()
                                                    .Where(p => ids.Contains(p.SaleOrderDetailId)).ToList());
        }

        /// <summary>
        /// 获取所有订单行非完成,非已删除的工程计划
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EngineerPlan> GetEngineerPlanAll(List<SOMI_PlanState> statelist)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EngineerPlan.CustLevelProperty);
            return Query<EngineerPlan>().Where(o => !statelist.Contains(o.PlanState)).OrderBy(o => o.SaleOrderDetail.RequireDelivery).ToList(null, elo);
        }


        /// <summary>
        /// 获取所有订单行非完成,非已删除的工程计划(同步)
        /// </summary>
        /// <returns></returns>
        public virtual List<EngineerPlanInfo> GetEngineerPlanAllToInfo()
        {
            return Query<EngineerPlan>()
                   .LeftJoin<SaleOrderDetail>((x, y) => x.SaleOrderDetailId == y.Id)
                   .LeftJoin<SaleOrderDetail, SaleOrder>((x, y) => x.SaleOrderId == y.Id)
                   .Select<SaleOrderDetail, SaleOrder>((plan, detail, sale) => new
                   {
                       Id = plan.Id,
                       OrderDetailId = detail.Id,
                       FactoryId = (double)detail.EnterpriseId,
                       LineNo = detail.LineNo,
                       ItemId = detail.ItemId,
                       UnitId = detail.UnitId,
                       RequireDelivery = detail.RequireDelivery,
                       AllegroType = detail.AllegroType,
                       AppArea = detail.AppArea,
                       Area = detail.Area,
                       ExternalEcn = detail.ExternalEcn,
                       IsNew = detail.IsNew,
                       OrderType = detail.OrderType,
                       ProductType = detail.ProductType,
                       Qty = detail.Qty,
                       ItemRevision = detail.ItemRevision,
                       ItemExtPropName = detail.ItemExtPropName,

                       RegisterDateTime = sale.RegisterDateTime,
                       CustomerId = sale.CustomerId,
                       CustomerPoDate = sale.CustomerPoDate
                   }).Where(p => p.PlanState != SOMI_PlanState.Finish && p.PlanState != SOMI_PlanState.Deleted).ToList<EngineerPlanInfo>().ToList();
        }

        #endregion

        /// <summary>
        /// MI任务完成接口-更新计划状态为已完成
        /// </summary>
        /// <param name="SaleOrderDetailIds"></param>
        public virtual void SentBackToMIPlan(List<double> SaleOrderDetailIds)
        {
            var plans = RT.Service.Resolve<EngineerPlanController>().GetPlanBy_SaleOrderDetailIds(SaleOrderDetailIds);
            foreach (var item in plans)
            {
                if (item.PlanState != SOMI_PlanState.Finish)
                    item.PlanState = SOMI_PlanState.Finish;
            }
            RF.Save(plans);
        }

        /// <summary>
        /// 获取最大排序顺序
        /// </summary>
        /// <param name="factoryId">工厂ID</param>
        /// <param name="scheduleDate">日期</param>
        /// <returns></returns>
        public virtual int GetMaxSortIndex(double factoryId, DateTime scheduleDate)
        {
            var maxSortIndex = Query<EngineerPlan>().Where(p => p.SortDate == scheduleDate && p.FactoryId == factoryId).OrderByDescending(p => p.SortIndex).FirstOrDefault();
            if (maxSortIndex == null)
                return 1;
            return Convert.ToInt32(maxSortIndex.SortIndex.Substring(8, 4)) + 1;
        }
    }
}
