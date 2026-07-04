using SIE.APS;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Kit.APS.EngineerPlans.Configs;
using SIE.Kit.APS.EngineerPlans.Settings;
using SIE.Kit.APS.FactoryPlanQtys;
using SIE.Kit.EventMessages.EngineerPlans;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans.Handle
{
    /// <summary>
    /// 排计划命令
    /// </summary>
    [Services.Service(FallbackType = typeof(DoScheduleHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class DoScheduleHandle
    {

        /// <summary>
        /// 等级日排产上限配置池
        /// </summary>
        CustLevelPool clpool;
        /// <summary>
        /// 客户等级配置池
        /// </summary>
        CustLevelSettingPool cspool;
        /// <summary>
        /// 工程节假日配置池
        /// </summary>
        HolidaySettingPool hspool;
        /// <summary>
        /// 工程计划配置池
        /// </summary>
        EngineerPlanPool eppool;
        /// <summary>
        /// 工厂计划配置数配置池
        /// </summary>
        FactoryPlanQtyPool fpqpool;


        /// <summary>
        /// 工程节假日维护
        /// </summary>
        List<HolidaySetting> HolidaySettingList;

        /// <summary>
        /// 等级日排产上限列表
        /// </summary>
        List<CustLevel> Capacties;
        /// <summary>
        /// 客户等级设置
        /// </summary>
        List<CustLevelSetting> CustLevelSettingList;


        /// <summary>
        /// 工程计划ALL
        /// </summary>
        List<EngineerPlan> EngineerPlanListAll;

        /// <summary>
        /// 计划工程计划All
        /// </summary>
        EntityList<EngineerPlan> ScheduleEngineerPlanListAll;

        /// <summary>
        /// 当前工厂符合条件的工程计划(已计划未完成的，未计划且指定紧急的,未计划的)
        /// </summary>
        List<EngineerPlan> CurrentEngineerPlanList;

        /// <summary>
        /// 下一个工作日
        /// </summary>
        DateTime NextDay;
        /// <summary>
        /// 日总产能
        /// </summary>
        decimal MaxWorkCapactiy;
        /// <summary>
        /// 工程计划所涉及到的所有工厂集合
        /// </summary>
        List<double> FactoryIdList;

        /// <summary>
        /// 是否占用下一天产能
        /// </summary>
        bool IsOverTimeTakeCapacity;

        decimal ecnUnitPre;


        /// <summary>
        /// 明天已完成的工程计划
        /// </summary>
        public EntityList<EngineerPlan> NextDayFinishPlanList { get; set; }

        /// <summary>
        /// 当天之前(包括当天) 已计划未完成的工程计划
        /// </summary>
        public EntityList<EngineerPlan> NotFinishPlanList { get; set; }

        /// <summary>
        /// (未计划 或 排单日期在明天的已计划)的加急工程计划
        /// </summary>
        public EntityList<EngineerPlan> UrgentPlanList { get; set; }

        /// <summary>
        /// (未计划 或 排单日期在明天的已计划)的没加急工程计划
        /// </summary>
        public EntityList<EngineerPlan> NormalPlanList { get; set; }

        /// <summary>
        /// 日期字符(排序用)
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int SortStart { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DoScheduleHandle()
        {
            clpool = new CustLevelPool();
            cspool = new CustLevelSettingPool();
            hspool = new HolidaySettingPool();
            eppool = new EngineerPlanPool();
            fpqpool = new FactoryPlanQtyPool();

            HolidaySettingList = new List<HolidaySetting>();
            Capacties = new List<CustLevel>();
            CustLevelSettingList = new List<CustLevelSetting>();
            MaxWorkCapactiy = 0;

            FactoryIdList = new List<double>();
            IsOverTimeTakeCapacity = false;

            EngineerPlanListAll = new List<EngineerPlan>();
            CurrentEngineerPlanList = new List<EngineerPlan>();
            ScheduleEngineerPlanListAll = new EntityList<EngineerPlan>();
        }


        /// <summary>
        ///  排计划命令
        /// </summary>
        public virtual void DoSchedule()
        {
            LoadBase();

            ValidateDate();

            InitDate();

            SaveDate();
        }

        /// <summary>
        /// 加载基础数据
        /// </summary>
        public virtual void LoadBase()
        {
            //初始化配置池数据
            clpool.Load();
            cspool.Load();
            hspool.Load();
            fpqpool.Load();

            //如果等级分类数据不存在则初始化等级分类数据
            bool IsExt = RT.Service.Resolve<EngineerPlanController>().ValidateCustLevelDate();
            if (!IsExt)
            {
                RT.Service.Resolve<EngineerPlanController>().Init();
            }

            //不包含已删除的数据
            List<SOMI_PlanState> statelist = new List<SOMI_PlanState>() { SOMI_PlanState.Deleted };

            EngineerPlanListAll = RT.Service.Resolve<EngineerPlanController>().GetEngineerPlanAll(statelist).ToList();

            //工程计划数据涉及到的所有工厂
            FactoryIdList = EngineerPlanListAll.GroupBy(p => p.FactoryId).Select(p => p.Key).Distinct().ToList();

            //初始化等级日排产上限
            Capacties = clpool.GetCustLevelList();
            //配置项【是否占用下一天产能】
            IsOverTimeTakeCapacity = GetConfig_IsOverTimeTakeCapacity();
            //配置项【新单与外部ECM比例】
            ecnUnitPre = GetConfig_NewEcnPre();

        }

        /// <summary>
        /// 验证数据
        /// </summary>
        public virtual void ValidateDate()
        {
            if (!Capacties.Any())
            {
                throw new ValidationException("等级日排产上限还未初始化!".L10N());
            }
            foreach (var factoryId in FactoryIdList)
            {
                //工厂客户等级信息
                CustLevelSettingList = cspool.GetCustLevelSettingList(factoryId);
                if (CustLevelSettingList == null || !CustLevelSettingList.Any())
                {
                    throw new ValidationException("【工厂客户等级】信息未维护!".L10N());
                }
                //工厂日总产能
                MaxWorkCapactiy = fpqpool.GetWorkCeil(factoryId);
                if (MaxWorkCapactiy == 0)
                {
                    throw new ValidationException("【工厂计划数配置】未维护!".L10N());
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void InitDate()
        {
            DateTime ScheduleDayNow = DateTime.Now.Date;
            //循环工厂分厂排计划
            foreach (var factoryId in FactoryIdList)
            {
                //取当前工厂的节假日配置信息
                HolidaySettingList = hspool.GetHolidaySettingList(factoryId);
                //工厂客户等级信息
                CustLevelSettingList = cspool.GetCustLevelSettingList(factoryId);
                //工厂日总产能
                MaxWorkCapactiy = fpqpool.GetWorkCeil(factoryId);
                //取时间信息
                ScheduleDayNow = ScheduleDayNow.Date;

                //取下一个工作日NextDay
                NextDay = GetNextWorkDay(ScheduleDayNow);
                DateStr = NextDay.ToString("yyyyMMdd");

                //已分厂则只取当前工厂的工程计划
                CurrentEngineerPlanList = EngineerPlanListAll.Where(p => p.FactoryId == factoryId).ToList();
                // ---------------

                //明天已完成的工程计划
                NextDayFinishPlanList = CurrentEngineerPlanList.Where(o => o.ScheduleDay == NextDay && o.PlanState == SOMI_PlanState.Finish).AsEntityList();
                //当天之前(包括当天) 已计划未完成的工程计划
                NotFinishPlanList = CurrentEngineerPlanList.Where(o => o.ScheduleDay <= ScheduleDayNow && o.PlanState == SOMI_PlanState.Scheduled).AsEntityList();
                //(未计划 或 排单日期在明天的已计划)的加急工程计划
                UrgentPlanList = CurrentEngineerPlanList.Where(o => o.IsUrgent == true && (o.PlanState == SOMI_PlanState.WaitToPlan || (o.ScheduleDay == NextDay && o.PlanState == SOMI_PlanState.Scheduled))).AsEntityList();
                //(未计划 或 排单日期在明天的已计划)的没加急工程计划
                NormalPlanList = CurrentEngineerPlanList.Where(o => o.IsUrgent == false
                                                    && (o.PlanState == SOMI_PlanState.WaitToPlan || (o.ScheduleDay == NextDay && o.PlanState == SOMI_PlanState.Scheduled))).AsEntityList();

                List<EngineerPlan> needFitMiPlans = new List<EngineerPlan>();
                needFitMiPlans.AddRange(NotFinishPlanList);
                needFitMiPlans.AddRange(UrgentPlanList);
                needFitMiPlans.AddRange(NormalPlanList);

                //客户等级提升
                fitCustLevel(needFitMiPlans);

                //排序序号
                SortStart = RT.Service.Resolve<EngineerPlanController>().GetMaxSortIndex(factoryId, NextDay);

                //    ----------------

                //取需要排的工程计划并标记优先级
                //Tasks = getNeedSchedules();
                //每个工厂都需要重新赋值占用的产能
                Capacties.ForEach(o => o.LessWorkCapaty = o.DayWorkCapacity);
                //明天已完成的工程计划 占用产能
                ScheduleNextDayFinishPlans(factoryId);
                //当天之前(包括当天) 已计划未完成的工程计划 占用产能
                ScheduleNotFinishPlans(factoryId);
                //(未计划 或 排单日期在明天的已计划)的加急工程计划 占用产能
                ScheduleUrgentPlans(factoryId);
                //(未计划 或 排单日期在明天的已计划)的没加急工程计划 占用产能
                ScheduleNormalPlans(factoryId);
                //产能还有剩余的时候 不看客户日上限,把产能占满
                ScheduleRemainCapactiy(factoryId);

                ScheduleEngineerPlanListAll.AddRange(NextDayFinishPlanList);
                ScheduleEngineerPlanListAll.AddRange(NotFinishPlanList);
                ScheduleEngineerPlanListAll.AddRange(UrgentPlanList);
                ScheduleEngineerPlanListAll.AddRange(NormalPlanList);
            }
        }

        /// <summary>
        /// 保存数据d
        /// </summary>
        public virtual void SaveDate()
        {
            using (var tran = DB.TransactionScope(ApsCoreEntityDataProvider.ConnectionStringName))
            {
                if (ScheduleEngineerPlanListAll.Any()) RF.Save(ScheduleEngineerPlanListAll);
                var NeedSavePlans= ScheduleEngineerPlanListAll.Where(p => p.PlanState == SOMI_PlanState.Scheduled).AsEntityList();
                if (NeedSavePlans.Any())
                {
                    SentToMiTaskPlan(NeedSavePlans);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 通过客户编码查询 客户优先级设置,设置优先级与时效性
        /// </summary>
        /// <param name="Plans"></param>
        private void fitCustLevel(List<EngineerPlan> Plans)
        {
            var cusIds = CustLevelSettingList.Select(o => o.CustomerId).Distinct().ToList();

            var custLevelSetList = CustLevelSettingList.Where(m => cusIds.Contains(m.CustomerId)).ToList();

            var allLevels = Capacties;

            foreach (var item in Plans)
            {
                var setting = custLevelSetList.FirstOrDefault(o => o.CustomerId == item.CustomerId);
                if (setting == null)
                {
                    //未定客户等级，当它最低优先级
                    item.CustLevel = allLevels[allLevels.Count - 1];
                    item.Hour = allLevels[allLevels.Count - 1].Hour;
                }
                else
                {
                    item.CustLevel = allLevels.First(o => o.Id == setting.CustLevelId);
                    item.Hour = item.CustLevel.Hour;
                }
            }

            //超时未排 升档
            var waitPlans = Plans.Where(o => o.PlanState == SOMI_PlanState.WaitToPlan).ToList();
            foreach (var item in waitPlans)
            {
                //已经最高了，不能再高了
                if (item.CustLevelId == allLevels[0].Id)
                    continue;

                if (item.RegisterDateTime.HasValue == false)
                    continue;

                var overHours = new TimeSpan(DateTime.Now.Date.Ticks - item.RegisterDateTime.Value.Date.Ticks).TotalHours;
                double calHours = overHours - item.Hour.Value;
                if (calHours > 24) //超过时效性后每24小时升级一次
                {

                    //超过自己本级，确认自己的等级
                    item.CustLevel = GetUpdateLevel(allLevels, item.CustLevel, calHours);
                    item.Hour = item.CustLevel.Hour;
                }
            }
        }

        private CustLevel GetUpdateLevel(List<CustLevel> CustLevels, CustLevel CurrentLevel, double OverHours)
        {
            double upHours = 24;//超过时效性后每24小时升级一次
            int currentSort = CustLevels.Select(o => o.Id).ToList().IndexOf(CurrentLevel.Id);
            for (int i = currentSort; i >= 0; --i)
            {
                var level = CustLevels[i];
                OverHours -= upHours;

                if (OverHours >= 0)
                {
                    //已经到顶了
                    if (i == 0)
                        return level;
                    //再进上一级，再检查 要不要再 
                    var upLevel = CustLevels[i - 1];
                    return GetUpdateLevel(CustLevels, upLevel, OverHours);
                }
                else
                    return level;
            }

            throw new ValidationException("程序:排序出错");
        }

        #region 工程计划配置信息

        private decimal GetConfig_NewEcnPre()
        {
            var config = ConfigService.GetConfig(new EngineerPlan_NewEcnPre_Config(), typeof(EngineerPlan));
            if (config == null || config.WithEcnPrecent <= 0)
                return 1.00m;

            return Math.Round(1.00m / config.WithEcnPrecent, 4);
        }


        private bool GetConfig_IsOverTimeTakeCapacity()
        {
            var config = ConfigService.GetConfig(new EngineerPlan_IsOverTimeTakeCapacity_Config(), typeof(EngineerPlan));
            if (config == null)
                return false;

            return config.IsYes;
        }

        #endregion

        /// <summary>
        /// 取下一工作日，跳过 【工程节假日维护】
        /// </summary>
        /// <returns></returns>
        private DateTime GetNextWorkDay(DateTime Now)
        {
            var rNextDay = Now.AddDays(1);
            //此工厂无配置信息则直取下一天为工作日
            if (HolidaySettingList == null || !HolidaySettingList.Any())
            {
                return rNextDay;
            }
            var holiday = HolidaySettingList.Where(o => o.StartDate <= rNextDay && o.EndDate >= rNextDay).FirstOrDefault();
            if (holiday == null)
                return rNextDay;
            return GetNextWorkDay(holiday.EndDate);
        }

        /// <summary>
        /// 产能 是否排完了
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="date"></param>
        /// <param name="MaxWorkCapactiy"></param>
        /// <returns>已占用产能大于日常产能 则返回true 否则返回false</returns>
        private decimal IsUseOver(double factoryId, DateTime date, decimal MaxWorkCapactiy)
        {
            var LessWorkCapaty = eppool.GetUsedCapacity(factoryId, date);
            return LessWorkCapaty;
        }



        #region MI任务计划 相关 传出与 完成状态回写

        /// <summary>
        /// 同步产生或更新Mi任务
        /// </summary>
        /// <param name="ChangingPlans"></param>
        public virtual void SentToMiTaskPlan(EntityList<EngineerPlan> ChangingPlans)
        {
            var eventDatas = new List<ScheduleAndCreateMITaskData>();
            foreach (var item in ChangingPlans)
            {
                //还未指定
                if (item.SortDate.HasValue == false)
                    continue;

                var newData = new ScheduleAndCreateMITaskData();
                newData.FactoryId = item.FactoryId;
                newData.ItemId = item.ItemId;
                newData.ItemRevision = item.ItemRevision;
                newData.ItemExtPropName = item.ItemExtPropName;
                newData.CustomerId = item.CustomerId;

                newData.SaleOrderDetailId = item.SaleOrderDetailId.Value;
                newData.SaleOrderNo = item.SaleOrderNo;
                newData.RequireDelivery = item.RequireDelivery;
                newData.ScheduleDay = item.SortDate.Value;

                eventDatas.Add(newData);
            }

            var strInputValue = Newtonsoft.Json.JsonConvert.SerializeObject(eventDatas);
            var inputValue = "发送工程计划信息到工程任务:{0}".L10nFormat(strInputValue);
            var log = new SIE.Core.Logs.InterfaceLog()
            {
                Name = "发送工程计划信息到工程任务",
                Method = "SentToMiTaskPlan",
                ControllerName = "EngineerController",
                InputValue = inputValue,
            };
            RF.Save(log);
            RT.Service.Resolve<IScheduleAndCreateMITask>().SyncMiTask(eventDatas);
        }

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

        #endregion



        #region  重构方法

        /// <summary>
        /// 明天已完成的工程计划 占用产能
        /// </summary>
        private void ScheduleNextDayFinishPlans(double factoryId)
        {
            //排序
            var sortedTasks = NextDayFinishPlanList
                .OrderByDescending(o => o.IsUrgent)
                .ThenBy(o => o.RequireDelivery)
                .ThenBy(o => o.CustLevel.GetProperty(SIE.Common.Sort.SortExtension.INDEX_Property)).ToList();
            //占用产能
            foreach (var task in sortedTasks)
            {
                decimal takeNum = task.IsNew ? 1 : ecnUnitPre;
                var cap = Capacties.First(o => o.Id == task.CustLevelId);

                cap.LessWorkCapaty -= takeNum;
                //MaxWorkCapactiy -= takeNum;
                eppool.AddUsedCapacity(factoryId, NextDay, takeNum);

                task.SortIndex = DateStr + SortStart.ToString("D4");
                SortStart += 1;
            }


        }

        /// <summary>
        /// 当天之前(包括当天) 已计划未完成的工程计划 占用产能
        /// </summary>
        private void ScheduleNotFinishPlans(double factoryId)
        {
            //排序
            var sortedTasks = NotFinishPlanList
                .OrderByDescending(o => o.IsUrgent)
                .ThenBy(o => o.RequireDelivery)
                .ThenBy(o => o.CustLevel.GetProperty(SIE.Common.Sort.SortExtension.INDEX_Property)).ToList();
            //占用产能
            foreach (var task in sortedTasks)
            {
                decimal takeNum = task.IsNew ? 1 : ecnUnitPre;
                var cap = Capacties.First(o => o.Id == task.CustLevelId);

                if (IsOverTimeTakeCapacity)
                {
                    //获取已占用工厂总产能
                    var UnMaxWorkCapactiy = eppool.GetUsedCapacity(factoryId, NextDay);
                    //总产能或者分类不够
                    if ((MaxWorkCapactiy - UnMaxWorkCapactiy < takeNum) || (cap.LessWorkCapaty < takeNum))
                    {
                        task.PlanState = SOMI_PlanState.WaitToPlan;
                        task.SortDate = null;
                        task.SortIndex = string.Empty;
                        continue;
                    }
                    //占用产能
                    cap.LessWorkCapaty -= takeNum;
                    //MaxWorkCapactiy -= takeNum;
                    eppool.AddUsedCapacity(factoryId, NextDay, takeNum);
                }

                task.PlanState = SOMI_PlanState.Scheduled;
                task.SortDate = NextDay;
                task.SortIndex = DateStr + SortStart.ToString("D4");
                SortStart += 1;
            }
        }

        /// <summary>
        /// (未计划 或 排单日期在明天的已计划)的加急工程计划 占用产能
        /// </summary>
        private void ScheduleUrgentPlans(double factoryId)
        {
            //排序
            var sortedTasks = UrgentPlanList
                .OrderByDescending(o => o.IsUrgent)
                .ThenBy(o => o.RequireDelivery)
                .ThenBy(o => o.CustLevel.GetProperty(SIE.Common.Sort.SortExtension.INDEX_Property));
            //占用产能
            foreach (var task in sortedTasks)
            {
                decimal takeNum = task.IsNew ? 1 : ecnUnitPre;
                var cap = Capacties.First(o => o.Id == task.CustLevelId);

                //加急的超产能也要排完
                cap.LessWorkCapaty -= takeNum;
                eppool.AddUsedCapacity(factoryId, NextDay, takeNum);
                //MaxWorkCapactiy -= takeNum;
                task.PlanState = SOMI_PlanState.Scheduled;
                task.ScheduleDay = NextDay;
                task.SortDate = NextDay;
                task.SortIndex = DateStr + SortStart.ToString("D4");
                SortStart += 1;
            }
        }

        /// <summary>
        /// (未计划 或 排单日期在明天的已计划)的没加急工程计划 占用产能
        /// </summary>
        private void ScheduleNormalPlans(double factoryId)
        {
            //排序
            var sortedTasks = NormalPlanList
                .OrderByDescending(o => o.IsUrgent)
                .ThenBy(o => o.RequireDelivery)
                .ThenBy(o => o.CustLevel.GetProperty(SIE.Common.Sort.SortExtension.INDEX_Property)).ToList();
            //占用产能
            foreach (var task in sortedTasks)
            {
                decimal takeNum = task.IsNew ? 1 : ecnUnitPre;
                var cap = Capacties.First(o => o.Id == task.CustLevelId);

                //获取已占用工厂总产能
                var UnMaxWorkCapactiy = eppool.GetUsedCapacity(factoryId, NextDay);

                //产能不够
                if ((MaxWorkCapactiy - UnMaxWorkCapactiy < takeNum) || cap.LessWorkCapaty < takeNum)
                {
                    task.PlanState = SOMI_PlanState.WaitToPlan;
                    task.SortDate = null;
                    task.SortIndex = string.Empty;
                    task.ScheduleDay = null;
                    continue;
                }

                //排计划成功
                cap.LessWorkCapaty -= takeNum;
                //MaxWorkCapactiy -= takeNum;
                eppool.AddUsedCapacity(factoryId, NextDay, takeNum);
                task.PlanState = SOMI_PlanState.Scheduled;
                task.ScheduleDay = NextDay;
                task.SortDate = NextDay;
                task.SortIndex = DateStr + SortStart.ToString("D4");
                SortStart += 1;
            }
        }


        /// <summary>
        /// 产能还有剩余的时候 不看客户日上限,把产能占满
        /// </summary>
        private void ScheduleRemainCapactiy(double factoryId)
        {
            var UnMaxWorkCapactiy1 = eppool.GetUsedCapacity(factoryId, NextDay);
            if (MaxWorkCapactiy - UnMaxWorkCapactiy1 > 0)
            {
                //排序
                var notFinishSortedTasks = NotFinishPlanList.Where(p => p.PlanState == SOMI_PlanState.WaitToPlan)
                    .OrderByDescending(o => o.IsUrgent)
                    .ThenBy(o => o.RequireDelivery)
                    .ThenBy(o => o.CustLevel.GetProperty(SIE.Common.Sort.SortExtension.INDEX_Property)).ToList();
                var NormalSortedTasks = NormalPlanList.Where(p => p.PlanState == SOMI_PlanState.WaitToPlan)
                    .OrderByDescending(o => o.IsUrgent)
                    .ThenBy(o => o.RequireDelivery)
                    .ThenBy(o => o.CustLevel.GetProperty(SIE.Common.Sort.SortExtension.INDEX_Property)).ToList();

                List<EngineerPlan> sortedTasks = new List<EngineerPlan>();
                sortedTasks.AddRange(notFinishSortedTasks);
                sortedTasks.AddRange(NormalSortedTasks);
                //占用产能
                foreach (var task in sortedTasks)
                {
                    //获取已占用工厂总产能
                    var UnMaxWorkCapactiy = eppool.GetUsedCapacity(factoryId, NextDay);

                    if ((MaxWorkCapactiy - UnMaxWorkCapactiy) < ecnUnitPre)
                        break;
                    decimal takeNum = task.IsNew ? 1 : ecnUnitPre;
                    if ((MaxWorkCapactiy - UnMaxWorkCapactiy) < takeNum)
                        continue;

                    eppool.AddUsedCapacity(factoryId, NextDay, takeNum);
                    // MaxWorkCapactiy -= takeNum;

                    task.PlanState = SOMI_PlanState.Scheduled;
                    task.ScheduleDay = task.ScheduleDay ?? NextDay;
                    task.SortDate = NextDay;
                    task.SortIndex = DateStr + SortStart.ToString("D4");
                    SortStart += 1;
                }
            }
        }

        #endregion
    }
}
