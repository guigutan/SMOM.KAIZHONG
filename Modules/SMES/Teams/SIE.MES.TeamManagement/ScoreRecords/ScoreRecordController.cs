using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Statistics.Entities;
using SIE.MES.TeamManagement.ApiInterfaces;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.RatedItems;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录控制器类
    /// </summary>
    public partial class ScoreRecordController : DomainController
    {
        private const string DATE_FORMAT = "yyyy-MM-01";

        /// <summary>
        /// 获取评分记录集合
        /// </summary>
        /// <param name="criteria">评分记录查询实体</param>
        /// <returns>评分记录集合</returns>
        public virtual EntityList<ScoreRecord> GetScoreRecords(ScoreRecordCriteria criteria)
        {
            EntityList<ScoreRecord> scoreRecords = null;
            var querys = Query<ScoreRecord>();
            if (criteria.InitiateDate.HasValue)
            {
                querys.Where(p => p.InitiateDate >= criteria.InitiateDate.Value.Date && p.InitiateDate < criteria.InitiateDate.Value.AddDays(1));
            }
            if (criteria.OccurDate.HasValue)
            {
                querys.Where(p => p.OccurDate >= criteria.OccurDate.Value.Date && p.OccurDate <= criteria.OccurDate.Value.AddDays(1));
            }
            if (!criteria.EmployeeCode.IsNullOrEmpty())
            {
                querys.Where(p => p.Employee.Code == criteria.EmployeeCode);
            }
            if (!criteria.InitiaorCode.IsNullOrWhiteSpace())
            {
                querys.Where(p => p.Initiator.Code == criteria.InitiaorCode);
            }
            if (!criteria.RatedItemCode.IsNullOrWhiteSpace())
            {
                querys.Where(p => p.RatedItem.Code == criteria.RatedItemCode);
            }
            if (criteria.ScoreState.HasValue)
            {
                querys.Where(p => p.ScoreState == criteria.ScoreState);
            }

            scoreRecords = querys.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return scoreRecords;
        }

        /// <summary>
        /// 获取评分记录集合
        /// </summary>
        /// <param name="dr">日期范围(含时分秒)</param>
        /// <param name="empId">员工Id</param>
        /// <param name="isEffect">是否有效(true:有效; false:无效)</param>
        /// <returns>评分记录集合</returns>
        public virtual EntityList<ScoreRecord> GetScoreRecords(DateRange dr, double empId, bool isEffect = true)
        {
            var scoreRecords = Query<ScoreRecord>().Where(x => x.OccurDate >= dr.BeginValue.Value && x.OccurDate <= dr.EndValue.Value
                              && x.EmployeeId == empId && x.IsEffective == isEffect).ToList();
            return scoreRecords;
        }

        /// <summary>
        /// 看板获取时间范围内，状态集合的记录
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <param name="empIds">员工Ids</param>
        /// <returns>数据集合</returns>
        public virtual EntityList<ScoreRecord> GetScoreRecords(DateRange dr, List<double> empIds)
        {
            return Query<ScoreRecord>().Where(p => p.OccurDate >= dr.BeginValue.Value && p.OccurDate < dr.EndValue.Value.AddDays(1) && empIds.Contains(p.EmployeeId)
            && p.IsEffective).ToList();
        }

        /// <summary>
        /// 获取绩效等级配置集合
        /// </summary>
        /// <returns>绩效等级配置集合</returns>
        public virtual EntityList<AchieveLevelSetting> GetAchieveLevelSettings()
        {
            var querys = Query<AchieveLevelSetting>().OrderBy(x => x.RowIndex).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return querys;
        }

        /// <summary>
        /// 评分绩效等级配置初始化
        /// </summary>
        /// <returns>绩效等级配置集合</returns>
        public virtual EntityList<AchieveLevelSetting> AchieveLevelSetIni()
        {
            using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
            {
                DB.Delete<AchieveLevelSetting>().Execute();
                var newSettings = CreateIniAchieveLevelSettings();
                RF.Save(newSettings);
                tran.Complete();

                return newSettings;
            }
        }

        /// <summary>
        /// 评分绩效等级配置初始化
        /// </summary>
        /// <returns>初始化的实体集合</returns>
        private EntityList<AchieveLevelSetting> CreateIniAchieveLevelSettings()
        {
            var iniAchievwLevelSettings = new EntityList<AchieveLevelSetting>();
            var rowIndex = 1;
            var greatAchieve = CreateAchieveLevelSetting(rowIndex++, 90, null, AchieveLevel.Great, Operator.GreaterEqual);
            iniAchievwLevelSettings.Add(greatAchieve);
            var goodAchieve = CreateAchieveLevelSetting(rowIndex++, 80, 89, AchieveLevel.Good, Operator.Between);
            iniAchievwLevelSettings.Add(goodAchieve);
            var passAchieve = CreateAchieveLevelSetting(rowIndex++, 60, 79, AchieveLevel.Pass, Operator.Between);
            iniAchievwLevelSettings.Add(passAchieve);
            var badAchieve = CreateAchieveLevelSetting(rowIndex, null, 59, AchieveLevel.Bad, Operator.LessEqual);
            iniAchievwLevelSettings.Add(badAchieve);

            return iniAchievwLevelSettings;
        }

        /// <summary>
        /// 创建绩效等级配置实体
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="level">绩效等级</param>
        /// <param name="oprt">运算符</param>
        /// <returns>绩效等级配置实体</returns>
        private AchieveLevelSetting CreateAchieveLevelSetting(int rowIndex, decimal? minValue, decimal? maxValue, AchieveLevel level, Operator oprt)
        {
            var curLevelSetting = new AchieveLevelSetting();
            curLevelSetting.RowIndex = rowIndex;
            curLevelSetting.MinValue = minValue;
            curLevelSetting.MaxValue = maxValue;
            curLevelSetting.AchiLevel = level;
            curLevelSetting.Operator = oprt;
            return curLevelSetting;
        }

        /// <summary>
        /// 看板明细获取有效正评分记录
        /// </summary>
        /// <param name="dr">日期范围</param>
        /// <param name="empIds">员工Id集合</param>         
        /// <returns>评分记录集合</returns>
        public virtual EntityList<ScoreRecord> GetScoreRecordsForDashBoardDetail(DateRange dr, List<double> empIds)
        {
            return Query<ScoreRecord>().Where(p => p.OccurDate >= dr.BeginValue.Value && p.OccurDate < dr.EndValue.Value.AddDays(1) && empIds.Contains(p.EmployeeId)
             && p.IsEffective && p.Score > 0).ToList();
        }

        /// <summary>
        /// 获取评分记录By项目
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <param name="empIds">员工集合</param>
        /// <param name="rateItemId">项目</param>
        /// <returns>评分集合</returns>
        public virtual EntityList<ScoreRecord> GetScoreRecordsByRateItem(DateRange dr, List<double> empIds, double rateItemId)
        {
            return Query<ScoreRecord>().Where(p => p.OccurDate >= dr.BeginValue.Value
            && p.OccurDate < dr.EndValue.Value.AddDays(1) && empIds.Contains(p.EmployeeId)
            && p.RatedItemId == rateItemId).ToList();
        }

        #region 调度处理评分
        /// <summary>
        /// 删除调度生产的待申诉的扣分记录
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="empIds">员工Id</param>
        /// <param name="rateItemId">项目Id</param>
        private void DeleteScore(DateTime dt, List<double> empIds, double rateItemId)
        {
            DB.Delete<ScoreRecord>().Where(p => p.OccurDate >= dt && p.OccurDate < dt.AddDays(1).Date && p.RatedItemId == rateItemId && p.ScoreSource == ScoreSource.Schedule
            && empIds.Contains(p.EmployeeId) && p.ScoreState == ScoreState.State).Execute();
        }

        /// <summary>
        /// 删除调度生产的所有产量未达标扣分记录（当产量达标时）
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="empIds">员工Id</param>
        /// <param name="rateItemId">项目Id</param>
        private void DeleteAllScore(DateTime dt, List<double> empIds, double rateItemId)
        {
            DB.Delete<ScoreRecord>().Where(p => p.OccurDate >= dt && p.OccurDate < dt.AddDays(1).Date && p.RatedItemId == rateItemId && p.ScoreSource == ScoreSource.Schedule
            && empIds.Contains(p.EmployeeId)).Execute();
        }

        /// <summary>
        /// 调度执行考勤及产能评分
        /// </summary>
        /// <param name="days">天数</param>
        /// <param name="inculeToday">是否包含今天</param>
        /// <returns>msg</returns>
        public virtual string ExcScore(int days, bool inculeToday)
        {
            var datetime = RF.Find<ScoreRecord>().GetDbTime();
            var date = datetime.Date;
            DateRange dr = new DateRange() { BeginValue = date.AddDays(-days), EndValue = date };
            if (!inculeToday)
            {
                dr.EndValue = date.AddDays(-1);
            }
            var ratedItem = RT.Service.Resolve<RatedItemController>().GetRatedItems(true);
            if (ratedItem.Count == 0)
            {
                return "没有设置系统评分项目";
            }
            else
            {
                string msg = string.Empty;
                var attent = ratedItem.FirstOrDefault(p => p.Name == "考勤异常");
                if (attent != null)
                {
                    msg = ExecAttentScore(attent, dr, datetime);
                }
                var product = ratedItem.FirstOrDefault(p => p.Name == "标准产能未达标");
                if (product != null)
                {
                    msg += ExecProductScore(product, dr, datetime);
                }
                return msg;
            }
        }

        /// <summary>
        /// 处理考勤异常评分
        /// </summary>
        /// <param name="attent">考勤评分项目</param>
        /// <param name="dr">日期范围</param>
        /// <param name="datetime">发生时间</param>
        /// <returns>msg</returns>
        private string ExecAttentScore(RatedItem attent, DateRange dr, DateTime datetime)
        {
            string msg = string.Empty;
            ////处理考勤异常评分
            try
            {
                var noAttent = RT.Service.Resolve<ClockInController>().GetEmployeeClockInByState(dr, OnDutyState.Absence);
                if (noAttent.Count > 0)
                {
                    noAttent.GroupBy(e => e.ClockInDate).ForEach(f =>
                    {
                        var sdate = f.Key;
                        var empIds = f.Select(p => p.EmployeeId).ToList();
                        DelAndSaveNewScore(sdate, empIds, attent, datetime, false);
                    });
                }
                else
                {
                    msg = dr.BeginValue.Value + " - " + dr.EndValue.Value + "没有考勤异常记录";
                }
            }
            catch (Exception ex)
            {
                msg = "考勤评分错误：" + ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// 处理产能未达标评分
        /// </summary>
        /// <param name="product">产能评分项目</param>
        /// <param name="dr">日期范围</param>
        /// <param name="datetime">发生时间</param>
        /// <returns>msg</returns>
        private string ExecProductScore(RatedItem product, DateRange dr, DateTime datetime)
        {
            string msg = string.Empty;
            try
            {
                var shiftList = RT.Service.Resolve<ShiftScheduleController>().GetShiftSchedules(dr);
                if (shiftList.Count == 0)
                {
                    return msg + dr.BeginValue.Value + " - " + dr.EndValue.Value + "没有排班数据";
                }
                var staticCtl = RT.Service.Resolve<StatisticsController>();
                shiftList.GroupBy(e => e.ScheduleDate.Date).ForEach(f =>
                {
                    var sdate = f.Key;

                    f.AsEntityList().ForEach(p =>
                    {
                        //遍历每个排班，找出排班对应生产资源生产时间段没达标的数据
                        var beginTime = sdate + p.Shift.BeginTime.TimeOfDay;
                        DateTime endTime;
                        if (p.Shift.IsOverDay && p.Shift.EndTime < p.Shift.BeginTime)
                        {
                            endTime = sdate.AddDays(1) + p.Shift.EndTime.TimeOfDay;
                        }
                        else
                        {
                            endTime = sdate + p.Shift.EndTime.TimeOfDay;
                        }
                        var staticList = staticCtl.GetResourceStatisticsList(p.WipResourceId, beginTime, endTime);
                        var wg = RT.Service.Resolve<EmployeeController>().GetEmployeeByWorkGroupId(p.WorkGroupId);
                        if (wg.Count > 0)
                        {
                            var empIds = wg.Select(e => e.Id).ToList();
                            if (staticList.Count == 0)
                            {
                                DelAndSaveNewScore(sdate, empIds, product, datetime, true);
                            }
                            else
                            {
                                if (p.WipResource.TaktTime.HasValue)
                                {
                                    //有生产数据，比对标产（班次时间(s)/节拍（s/个））与实际生产数量
                                    double restSecond = 0;
                                    p.Shift.ShiftRestList.ForEach(e =>
                                    {
                                        var restBegin = sdate + e.BeginTime.TimeOfDay;
                                        var restEnd = sdate + e.EndTime.TimeOfDay;
                                        if (p.Shift.IsOverDay && e.EndTime < e.BeginTime)
                                        {
                                            restEnd.AddDays(1);
                                        }

                                        restSecond += (restEnd - restBegin).TotalSeconds;
                                    });
                                    var shiftSecond = (endTime - beginTime).TotalSeconds - restSecond;
                                    var standQty = int.Parse((shiftSecond / p.WipResource.TaktTime.Value).ToString("F0"));
                                    var realQty = staticList.Where(e => e.OfflineQty.HasValue).Sum(e => e.OfflineQty);
                                    if (standQty > realQty)
                                    {
                                        //未达标
                                        DelAndSaveNewScore(sdate, empIds, product, datetime, true);
                                    }
                                    else
                                    {
                                        //已达标
                                        DeleteAllScore(sdate, empIds, product.Id);
                                    }
                                }
                            }
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                msg += "；标准产能未达标评分错误：" + ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// 删除原有的未达标产量评分记录并保存新的
        /// </summary>
        /// <param name="sdate">发生日期</param>
        /// <param name="empIds">员工Id集合</param>
        /// <param name="rateItem">评分项目</param>
        /// <param name="initiateDate">发起时间</param>
        /// <param name="needDel">删除调度生产的待申诉的扣分记录</param>
        private void DelAndSaveNewScore(DateTime sdate, List<double> empIds, RatedItem rateItem, DateTime initiateDate, bool needDel)
        {
            EntityList<ScoreRecord> list = new EntityList<ScoreRecord>();
            DateRange drkey = new DateRange() { BeginValue = sdate, EndValue = sdate };
            ////没有生产数据
            if (needDel)
            {
                DeleteScore(sdate, empIds, rateItem.Id);
            }
            ////把调度生成的待申诉数据删除
            var hasRecordList = GetScoreRecordsByRateItem(drkey, empIds, rateItem.Id);
            var hasIds = new List<double>();
            if (hasRecordList.Count > 0)
            {
                hasIds = hasRecordList.Select(e => e.EmployeeId).ToList();
            }
            empIds.Where(e => !hasIds.Contains(e)).ForEach(s =>
            {
                ScoreRecord item = new ScoreRecord()
                {
                    InitiateDate = initiateDate,
                    OccurDate = sdate,
                    InitiatorId = RT.IdentityId,
                    Score = rateItem.MinScore,
                    ScoreSource = ScoreSource.Schedule,
                    EmployeeId = s,
                    RatedItemId = rateItem.Id,
                    ScoreState = ScoreState.State,
                    IsEffective = true
                };
                list.Add(item);
            });
            RF.Save(list);
        }
        #endregion

        #region API调用的方法
        /// <summary>
        /// 创建分页信息对象
        /// </summary>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <param name="isNeedCount">是否需要TotalCount</param>
        /// <returns>分页信息对象</returns>
        private PagingInfo CreatePagingInfo(int pageNumber, int pageSize, bool isNeedCount = true)
        {
            PagingInfo pagingInfo = null;
            if (pageNumber > 0 && pageSize > 0)
            {
                pagingInfo = new PagingInfo(pageNumber, pageSize, isNeedCount);
            }
            return pagingInfo;
        }

        /// <summary>
        /// Check输入参数班组Id是否合法
        /// </summary>
        /// <param name="workGourpId">班组Id</param>
        private void CheckWorkGroupId(double workGourpId)
        {
            if (workGourpId <= 0)
            {
                throw new ValidationException("班组Id[{0}]不能<=0!".L10nFormat(workGourpId));
            }
            var workGroup = RF.GetById<WorkGroup>(workGourpId);
            if (workGroup == null)
            {
                throw new ValidationException("班组Id[{0}]未找到对应的班组信息!".L10nFormat(workGourpId));
            }
        }

        /// <summary>
        /// 获取班组的在职员工集合
        /// </summary>
        /// <param name="workGourpId">班组Id</param>
        /// <returns>在职员工集合</returns>
        private List<Employee> GetJobEmployees(double workGourpId)
        {
            var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeByWorkGroupId(workGourpId, null);
            var jobEmployees = employees?.Where(x => x.EmployeeStatus == EmployeeStatus.Job).ToList();
            if (jobEmployees == null || jobEmployees.Count <= 0)
            {
                throw new ValidationException("班组Id[{0}]未找到班组成员信息".L10nFormat(workGourpId));
            }
            return jobEmployees;
        }

        /// <summary>
        /// 获取所有的评分项目分类信息
        /// 如果未获取分类信息, 则抛出异常
        /// </summary>
        /// <returns>评分项目分类集合</returns>
        private EntityList<RatedItemCategory> GetRatedItemCategories()
        {
            var ratedItemCategorys = RT.Service.Resolve<RatedItemController>().GetRatedItemCategories();
            if (ratedItemCategorys == null || ratedItemCategorys.Count <= 0)
            {
                throw new ValidationException("评分项目分类信息未维护!".L10N());
            }
            return ratedItemCategorys;
        }

        /// <summary>
        ///  获取所有评分项目信息
        /// </summary>
        /// <returns>评分项目集合</returns>
        private EntityList<RatedItem> GetRatedItems()
        {
            var ratedItems = RT.Service.Resolve<RatedItemController>().GetRatedItems();
            if (ratedItems == null || ratedItems.Count <= 0)
                throw new ValidationException("评分项目信息未维护!".L10N());
            return ratedItems;
        }

        /// <summary>
        /// 创建评分项目分类API信息
        /// </summary>
        /// <param name="curRatedItemCategory">评分项目分类对象</param>
        /// <param name="ratedItemInfos">评分项目API集合</param>
        /// <returns>评分项目分类API信息</returns>
        private RatedItemCategoryInfo CreateRatedItemCategoryInfo(RatedItemCategory curRatedItemCategory, List<RatedItemInfo> ratedItemInfos)
        {
            var curRatedCategoryInfo = new RatedItemCategoryInfo();
            curRatedCategoryInfo.Id = curRatedItemCategory.Id;
            curRatedCategoryInfo.Code = curRatedItemCategory.Code;
            curRatedCategoryInfo.Name = curRatedItemCategory.Name;

            if (ratedItemInfos != null && ratedItemInfos.Count > 0)
                curRatedCategoryInfo.ItemList.AddRange(ratedItemInfos);
            return curRatedCategoryInfo;
        }

        /// <summary>
        /// 创建评分项目API信息集合
        /// </summary>
        /// <param name="ratedItems">评分项目对象</param>
        /// <param name="ratedItemCategoryId">评分项目分类Id</param>
        /// <returns>评分项目API信息集合</returns>
        private List<RatedItemInfo> CreateRatedItemInfos(EntityList<RatedItem> ratedItems, double ratedItemCategoryId)
        {
            List<RatedItemInfo> ratedItemInfos = null;
            if (ratedItems != null && ratedItems.Count > 0)
            {
                var curRatedItems = ratedItems.Where(x => x.CategoryId == ratedItemCategoryId).ToList();
                if (curRatedItems != null && curRatedItems.Count > 0)
                {
                    ratedItemInfos = new List<RatedItemInfo>();
                    foreach (var ratedItem in curRatedItems)
                    {
                        var curRatedItemInfo = CreateRatedItemInfo(ratedItem);
                        ratedItemInfos.Add(curRatedItemInfo);
                    }
                }
            }

            return ratedItemInfos;
        }

        /// <summary>
        /// 创建评分项目API信息
        /// </summary>
        /// <param name="ratedItem">评分项目对象</param>
        /// <returns>评分项目API信息</returns>
        private RatedItemInfo CreateRatedItemInfo(RatedItem ratedItem)
        {
            var curRatedItemInfo = new RatedItemInfo();
            curRatedItemInfo.Id = ratedItem.Id;
            curRatedItemInfo.Code = ratedItem.Code;
            curRatedItemInfo.Name = ratedItem.Name;
            curRatedItemInfo.MinScore = ratedItem.MinScore;
            curRatedItemInfo.MaxScore = ratedItem.MaxScore;
            curRatedItemInfo.IsSystem = ratedItem.IsSystem;
            return curRatedItemInfo;
        }

        /// <summary>
        /// Check评分填录API信息的合法性
        /// </summary>
        /// <param name="scoreApplyInfo">评分填写API信息</param>
        private void CheckScoreApplyInfoValid(ScoreApplyInfo scoreApplyInfo)
        {
            if (scoreApplyInfo.EmployeeIdList == null || scoreApplyInfo.EmployeeIdList.Count <= 0)
                throw new ValidationException("员工Id列表不能为空!".L10N());
            foreach (var empId in scoreApplyInfo.EmployeeIdList)
            {
                var curEmp = RF.GetById<Employee>(empId);
                if (curEmp == null)
                {
                    throw new ValidationException("员工Id[{0}]对应的员工信息不存在!".L10nFormat(empId));
                }
            }

            if (scoreApplyInfo.RatedItemId <= 0)
                throw new ValidationException("评分项目Id不能<=0!".L10N());
            var curRatedItem = RF.GetById<RatedItem>(scoreApplyInfo.RatedItemId);
            if (curRatedItem == null)
                throw new ValidationException("评分项目Id[{0}]对应的评分项目不存在!".L10nFormat(scoreApplyInfo.RatedItemId));
            if (scoreApplyInfo.Score == 0)
                throw new ValidationException("评分分值为[{0}]无意义!".L10N());
            /////*if (scoreApplyInfo.InitiatorId <= 0)
            ////    throw new ValidationException("发起人Id不能<=0!".L10N());
            ////var curInitiator = RF.GetById<Employee>(scoreApplyInfo.InitiatorId);
            ////if (curInitiator == null)
            ////    throw new ValidationException("发起人Id[{0}]对应的员工信息不存在!".L10nFormat(scoreApplyInfo.InitiatorId));*/
            /////* //备注可以为空--已经确认
            ////if (scoreApplyInfo.Remark.IsNullOrWhiteSpace())
            ////    throw new ValidationException("评分填录的[备注]不能为空!".L10N());*/
            var currentDate = RF.Find<ScoreRecord>().GetDbTime();
            var curMonthFirstDate = DateTime.Parse(currentDate.Date.ToString(DATE_FORMAT));
            if (scoreApplyInfo.OccurDate < curMonthFirstDate || scoreApplyInfo.OccurDate > currentDate)
                throw new ValidationException("评分记录的发生时间[{0}]应该在本月1号[{1}]和当前时间[{2}]之间!".L10nFormat(scoreApplyInfo.OccurDate, curMonthFirstDate, currentDate));
        }

        /// <summary>
        /// 创建评分记录实体集合
        /// </summary>
        /// <param name="scoreApplyInfo">评分填录API信息</param>
        /// <returns>评分记录实体集合</returns>
        private EntityList<ScoreRecord> CreateScoreRecords(ScoreApplyInfo scoreApplyInfo)
        {
            var scoreRecords = new EntityList<ScoreRecord>();
            foreach (var curEmpId in scoreApplyInfo.EmployeeIdList)
            {
                scoreRecords.Add(CreateEntityScoreRecord(scoreApplyInfo, curEmpId));
            }

            return scoreRecords;
        }

        /// <summary>
        /// 创建评分记录实体
        /// </summary>
        /// <param name="scoreApplyInfo">评分填录API信息</param>
        /// <param name="employeeId">员工Id</param>
        /// <returns>评分记录实体</returns>
        private ScoreRecord CreateEntityScoreRecord(ScoreApplyInfo scoreApplyInfo, double employeeId)
        {
            ScoreRecord curScoreRecord = new ScoreRecord();
            var currentDateTime = RF.Find<ScoreRecord>().GetDbTime();

            curScoreRecord.GenerateId();
            curScoreRecord.InitiateDate = currentDateTime;
            curScoreRecord.OccurDate = scoreApplyInfo.OccurDate;
            curScoreRecord.Score = scoreApplyInfo.Score;
            curScoreRecord.Remark = scoreApplyInfo.Remark;
            curScoreRecord.EmployeeId = employeeId;
            curScoreRecord.InitiatorId = RT.IdentityId; ////.scoreApplyInfo.InitiatorId;
            curScoreRecord.RatedItemId = scoreApplyInfo.RatedItemId;
            curScoreRecord.ScoreState = ScoreState.State;
            curScoreRecord.ScoreSource = ScoreSource.App;
            curScoreRecord.IsEffective = true;

            return curScoreRecord;
        }

        /// <summary>
        /// 创建评分填录附件集合
        /// </summary>
        /// <param name="attachmentList">附件API信息集合</param>
        /// <param name="scoreRecords">评分记录实体集合</param>
        /// <returns>评分附件集合</returns>
        private EntityList<ScoreAttachment> CreateScoreAttachmentList(List<AttachmentInfo> attachmentList, EntityList<ScoreRecord> scoreRecords)
        {
            EntityList<ScoreAttachment> scoreAttachments = null;
            if (attachmentList != null && attachmentList.Count > 0)
            {
                scoreAttachments = new EntityList<ScoreAttachment>();
                foreach (var curScoreRecord in scoreRecords)
                {
                    foreach (var attachment in attachmentList)
                    {
                        var curScoreAttachment = new ScoreAttachment();
                        curScoreAttachment.SocreRecordId = curScoreRecord.Id;
                        if (!attachment.ContentBase64.IsNullOrWhiteSpace())
                        {
                            curScoreAttachment.FileContent = Convert.FromBase64String(attachment.ContentBase64);
                        }
                        else
                        {
                            curScoreAttachment.FileContent = null;
                        }
                        curScoreAttachment.FileExtesion = attachment.FileExtesion;
                        curScoreAttachment.FileName = attachment.FileName;
                        scoreAttachments.Add(curScoreAttachment);
                    }
                }
            }

            return scoreAttachments;
        }

        /// <summary>
        /// Check评分记录Id合法性
        /// 及获取对应的评分记录实体
        /// </summary>
        /// <param name="scoreRecordId">评分记录Id</param>
        /// <returns>评分记录实体</returns>
        private ScoreRecord CheckScoreRecordId(double scoreRecordId)
        {
            if (scoreRecordId <= 0)
                throw new ValidationException("评分记录Id不能<=0".L10N());
            var curScoreRecord = RF.GetById<ScoreRecord>(scoreRecordId);
            if (curScoreRecord == null)
                throw new ValidationException("评分记录Id[{0}]对应的评分记录不存在".L10nFormat(scoreRecordId));
            if (curScoreRecord.ScoreState == ScoreState.Deleted)
                throw new ValidationException("评分记录Id[{0}]对应的评分记录已被班组长删除".L10nFormat(scoreRecordId));
            return curScoreRecord;
        }

        /// <summary>
        /// Check员工评分申诉处理的输入参数
        /// </summary>
        /// <param name="processRecordInfo">评分申诉处理记录API信息</param>
        /// <returns>评分记录实体</returns>
        private ScoreRecord CheckProcessRecordInfo(ProcessRecordInfo processRecordInfo)
        {
            if (processRecordInfo.ScoreRecordId <= 0)
                throw new ValidationException("评分记录Id不能<=0".L10N());
            var curScoreRecord = RF.GetById<ScoreRecord>(processRecordInfo.ScoreRecordId);
            if (curScoreRecord == null)
                throw new ValidationException("评分记录Id[{0}]对应的评分记录不存在".L10nFormat(processRecordInfo.ScoreRecordId));
            var curPetitionRecords = curScoreRecord.PetitionList;
            if (curPetitionRecords == null || !curPetitionRecords.Any())
                throw new ValidationException("未找到申诉记录".L10N());
            if (curScoreRecord.ScoreState != ScoreState.Stating)
                throw new ValidationException("评分记录状态为[{0}], 不能处理该评分记录的申诉".L10nFormat(curScoreRecord.ScoreState.ToLabel()));

            /*if (processRecordInfo.HandlerId <= 0)
                throw new ValidationException("处理人Id不能<=0".L10N());
            var curHandler = RF.GetById<Employee>(processRecordInfo.HandlerId);
            if (curHandler == null)
                throw new ValidationException("处理人Id[{0}]对应的员工不存在".L10nFormat(processRecordInfo.HandlerId));*/

            if (processRecordInfo.ProcessMode < 0 || processRecordInfo.ProcessMode > 2)
                throw new ValidationException("处理方式的值只能是0或1或2!".L10N());
            if (processRecordInfo.ProcessMode == 0)
            {
                if (processRecordInfo.RateItemId == curScoreRecord.RatedItemId &&
                    processRecordInfo.Score == curScoreRecord.Score)
                {
                    throw new ValidationException("处理方式为[调整评判], 但是没有修改[评分项目或分值]!".L10N());
                }
            }

            if (processRecordInfo.RateItemId <= 0)
                throw new ValidationException("评分项目Id不能<=0".L10N());
            var curRateItem = RF.GetById<RatedItem>(processRecordInfo.RateItemId);
            if (curRateItem == null)
                throw new ValidationException("评分项目Id[{0}]对应的评分项目不存在".L10nFormat(processRecordInfo.RateItemId));
            if (processRecordInfo.Score == 0)
                throw new ValidationException("项目分值不能等于0".L10N());
            return curScoreRecord;
        }

        /// <summary>
        /// 创建评分申诉的处理记录
        /// 前评分项目Id、后评分项目Id、前分值、后分值
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <param name="processRecordInfo">处理记录API信息</param>
        /// <returns>处理记录实体</returns>
        private ProcessRecord CreateProcessRecord(ScoreRecord scoreRecord, ProcessRecordInfo processRecordInfo)
        {
            var processRecord = new ProcessRecord();
            processRecord.OldRatedItemId = scoreRecord.RatedItemId;
            processRecord.NewRatedItemId = processRecordInfo.RateItemId;
            processRecord.OldValue = scoreRecord.Score;
            processRecord.NewValue = processRecordInfo.Score;
            processRecord.PetitionRecordId = scoreRecord.PetitionList.FirstOrDefault().Id;

            return processRecord;
        }

        /// <summary>
        /// 申诉记录属性修改:处理时间、处理结果、处理方式
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <param name="processRecordInfo">处理记录API信息</param>
        /// <returns>申诉记录</returns>
        private PetitionRecord UpdatePetitionRecord(ScoreRecord scoreRecord, ProcessRecordInfo processRecordInfo)
        {
            var currentDateTime = RF.Find<PetitionRecord>().GetDbTime();
            var curPetitionRecord = scoreRecord.PetitionList.FirstOrDefault();
            curPetitionRecord.ProcessDate = currentDateTime;
            curPetitionRecord.ProcessResult = processRecordInfo.ProcessResult;
            curPetitionRecord.ProcessMode = (StateProcessMode)processRecordInfo.ProcessMode;
            curPetitionRecord.HandlerId = RT.IdentityId; //processRecordInfo.HandlerId;
            return curPetitionRecord;
        }

        /// <summary>
        /// Check班组长评分删除的合法性
        /// </summary>
        /// <param name="scoreRecordId">评分记录Id</param>
        /// <returns>评分记录实体</returns>
        private ScoreRecord CheckDeleteScoreRecord(double scoreRecordId)
        {
            if (scoreRecordId <= 0)
                throw new ValidationException("评分记录Id不能<=0".L10N());
            var curScoreRecord = RF.GetById<ScoreRecord>(scoreRecordId);
            if (curScoreRecord == null)
                throw new ValidationException("评分记录Id[{0}]对应的实体不存在".L10nFormat(scoreRecordId));
            if (curScoreRecord.ScoreState != ScoreState.State)
                throw new ValidationException("评分记录的状态为[{0}], 不能删除".L10nFormat(curScoreRecord.ScoreState.ToLabel()));
            var currentDate = RF.Find<ScoreRecord>().GetDbTime();
            var curMonthFirstDate = DateTime.Parse(currentDate.Date.ToString(DATE_FORMAT));
            if (curScoreRecord.OccurDate < curMonthFirstDate || curScoreRecord.OccurDate > currentDate)
                throw new ValidationException("评分记录的发生时间[{0}]应该在本月1号[{1}]和当前时间[{2}]之间才能进行删除!".L10nFormat(curScoreRecord.OccurDate, curMonthFirstDate, currentDate));
            return curScoreRecord;
        }

        /// <summary>
        /// 评分记录删除更新
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        private void DeleteUpdateScoreRecord(ScoreRecord scoreRecord)
        {
            string updateMsg = string.Empty; ////" ,评分删除!";
            const ScoreState scoreState = ScoreState.Deleted;
            scoreRecord.IsEffective = false;
            UpdateScoreRecord(scoreRecord, updateMsg, scoreState);
        }

        /// <summary>
        /// 获取员工评分记录集合
        /// </summary>
        /// <param name="queryInfo">评分查询条件</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>员工评分记录集合</returns>
        private EntityList<ScoreRecord> GetScoreRecords(ScoreQueryInfo queryInfo, PagingInfo pagingInfo = null)
        {
            EntityList<ScoreRecord> scoreRecords = null;

            var currentDate = RF.Find<ScoreRecord>().GetDbTime();
            var preMonthDate = DateTime.Parse(currentDate.Date.ToString(DATE_FORMAT)).AddMonths(-1);
            var querys = Query<ScoreRecord>().Where(x => x.OccurDate >= preMonthDate
                         && x.OccurDate <= currentDate && x.ScoreState != ScoreState.Deleted);
            if (queryInfo.EmployeeId.HasValue)
            {
                querys.Where(x => x.EmployeeId == queryInfo.EmployeeId);
            }
            if (queryInfo.WorkGroupId.HasValue)
            {
                querys.Where(x => x.Employee.WorkGroupId == queryInfo.WorkGroupId);
            }
            if (queryInfo.CategoryId.HasValue)
            {
                querys.Where(x => x.RatedItem.CategoryId == queryInfo.CategoryId);
            }
            if (queryInfo?.Performance == 0)
            {
                querys.Where(x => x.Score > 0);
            }
            else if (queryInfo?.Performance == 1)
            {
                querys.Where(x => x.Score < 0);
            }
            else
            {
                //
            }
            if (queryInfo.PetitionState.HasValue)
            {
                querys.Where(x => x.ScoreState == (ScoreState)queryInfo.PetitionState);
            }
            if (queryInfo?.QueryDate == 0)
            {
                var preThreeDate = currentDate.AddDays(-2); //最近3天
                querys.Where(x => x.OccurDate >= preThreeDate);
            }
            else if (queryInfo?.QueryDate == 1)
            {
                var preWeekDate = currentDate.AddDays(-6); //最近1周
                querys.Where(x => x.OccurDate >= preWeekDate);
            }
            else if (queryInfo?.QueryDate == 2)
            {
                var latelyMonthDate = currentDate.AddDays(-29); //最近1个月
                querys.Where(x => x.OccurDate >= latelyMonthDate);
            }
            else
            {
                //
            }

            scoreRecords = querys.ToList(pagingInfo);
            return scoreRecords;
        }

        /// <summary>
        /// Check评分记录查询条件
        /// </summary>
        /// <param name="queryInfo">评分记录查询条件</param>
        private void CheckScoreQueryInfo(ScoreQueryInfo queryInfo)
        {
            if (queryInfo.WorkGroupId == null)
            {
                throw new ValidationException("筛选条件中的班组Id为Null".L10N());
            }
            if (queryInfo?.EmployeeId <= 0)
            {
                throw new ValidationException("筛选条件中的员工Id[{0}]不合法".L10nFormat(queryInfo?.EmployeeId));
            }
            if (queryInfo?.WorkGroupId <= 0)
            {
                throw new ValidationException("筛选条件中的班组Id[{0}]不合法".L10nFormat(queryInfo?.WorkGroupId));
            }
            if (queryInfo?.CategoryId <= 0)
            {
                throw new ValidationException("筛选条件中的项目分类Id[{0}]不合法".L10nFormat(queryInfo?.CategoryId));
            }
            if (queryInfo?.Performance < 0 || queryInfo?.Performance > 1)
            {
                throw new ValidationException("筛选条件中的正负绩效[{0}]不合法".L10nFormat(queryInfo?.Performance));
            }
            if (queryInfo?.PetitionState < 0 || queryInfo?.PetitionState > 2)
            {
                throw new ValidationException("筛选条件中的申诉状态[{0}]不合法".L10nFormat(queryInfo?.PetitionState));
            }
            if (queryInfo?.QueryDate < 0 || queryInfo?.QueryDate > 2)
            {
                throw new ValidationException("筛选条件中的申诉状态[{0}]不合法".L10nFormat(queryInfo?.QueryDate));
            }
            if (queryInfo.PageNumber <= 0 || queryInfo.PageSize <= 0)
            {
                throw new ValidationException("筛选条件中的页号及页码不能<=0".L10N());
            }
        }

        /// <summary>
        /// Check员工评分记录查询条件
        /// </summary>
        /// <param name="queryInfo">评分记录查询条件</param>
        private void CheckPersonScoreQueryInfo(ScoreQueryInfo queryInfo)
        {
            if (queryInfo.EmployeeId == null)
            {
                throw new ValidationException("筛选条件中的员工Id为Null".L10N());
            }
            CheckScoreQueryInfo(queryInfo);
        }

        /// <summary>
        /// 创建评分记录API信息集合
        /// </summary>
        /// <param name="scoreRecords">评分记录集合</param>
        /// <param name="isContainPetitions">是否包含申诉记录集合</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>评分记录API信息集合</returns>
        private ScoreRecordInfos CreateScoreRecordInfos(EntityList<ScoreRecord> scoreRecords, bool isContainPetitions, PagingInfo pagingInfo)
        {
            ////List<ScoreRecordInfo> scoreRecordInfos = new List<ScoreRecordInfo>();
            var scoreRecordInfos = new ScoreRecordInfos();
            scoreRecordInfos.TotalCount = pagingInfo.TotalCount;
            foreach (var curScoreRecord in scoreRecords)
            {
                var curScoreRecordInfo = CreateScoreRecordInfo(curScoreRecord, isContainPetitions);
                scoreRecordInfos.ScoreRecordInfoList.Add(curScoreRecordInfo);
            }

            return scoreRecordInfos;
        }

        /// <summary>
        /// 创建员工个人评分记录API信息集合
        /// </summary>
        /// <param name="scoreRecordInfos">评分记录API信息集合</param>
        /// <returns>员工个人评分记录API信息集合</returns>
        private PersonalScoreInfo CreatePersonalScoreInfo(ScoreRecordInfos scoreRecordInfos)
        {
            List<ScoreRecordInfo> scoreRecordInfoList = scoreRecordInfos.ScoreRecordInfoList;
            var achiLevelSettings = GetAchieveLevelSettings();
            var currentDate = RF.Find<ScoreRecord>().GetDbTime().Date;
            var personalScoreRecordInfo = new PersonalScoreInfo();
            var defaultScoreRecordInfo = scoreRecordInfoList.FirstOrDefault();
            personalScoreRecordInfo.EmployeeId = defaultScoreRecordInfo.EmployeeId;
            personalScoreRecordInfo.EmployeeName = defaultScoreRecordInfo.EmployeeName;
            personalScoreRecordInfo.Photo = defaultScoreRecordInfo.Photo;
            personalScoreRecordInfo.Score = GetEmployeeScoreSum(personalScoreRecordInfo.EmployeeId); ////100 + scoreRecordInfos.Sum(x => x.Score);
            personalScoreRecordInfo.EndDate = currentDate;
            personalScoreRecordInfo.BeginDate = DateTime.Parse(currentDate.ToString(DATE_FORMAT));
            personalScoreRecordInfo.ScoreGrade = GetScoreGrade(personalScoreRecordInfo.Score, achiLevelSettings);
            personalScoreRecordInfo.ScoreRecordInfoList = scoreRecordInfoList;
            personalScoreRecordInfo.TotalCount = scoreRecordInfos.TotalCount;
            SetAchieveLevelSetInfos(personalScoreRecordInfo.AchieveLevelSetInfoList, achiLevelSettings);

            return personalScoreRecordInfo;
        }

        /// <summary>
        /// 获取员工本月评分总分
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns>员工本月评分总分</returns>
        private decimal GetEmployeeScoreSum(double empId)
        {
            const bool isEffect = true;
            decimal? scoreSum = 100;
            var currentDate = RF.Find<ScoreRecord>().GetDbTime();
            var monthFirstDate = DateTime.Parse(currentDate.Date.ToString(DATE_FORMAT));
            DateRange dateRange = new DateRange() { BeginValue = monthFirstDate, EndValue = currentDate };
            var sorceRecords = GetScoreRecords(dateRange, empId, isEffect);
            scoreSum += sorceRecords.Sum(x => x.Score);

            return scoreSum.Value;
        }

        /// <summary>
        /// 获取评分等级
        /// 根据"绩效等级配置项"来获取"评分等级"
        /// </summary>
        /// <param name="score">评分分值</param>
        /// <param name="achiLevelSettings">绩效配置等级配置集合</param>
        /// <returns>评分等级</returns>
        private string GetScoreGrade(decimal score, EntityList<AchieveLevelSetting> achiLevelSettings)
        {
            string scoreGrade = string.Empty;
            if (score <= 0)
            {
                return scoreGrade;
            }
            foreach (var settingItem in achiLevelSettings)
            {
                switch (settingItem.Operator)
                {
                    case Operator.Less:
                        if (score < settingItem.MinValue)
                        {
                            scoreGrade = settingItem.AchiLevel.ToLabel();
                            return scoreGrade;
                        }

                        break;
                    case Operator.Between:
                        if (score >= settingItem.MinValue && score <= settingItem.MaxValue)
                        {
                            scoreGrade = settingItem.AchiLevel.ToLabel();
                            return scoreGrade;
                        }

                        break;
                    case Operator.Greater:
                        if (score > settingItem.MinValue)
                        {
                            scoreGrade = settingItem.AchiLevel.ToLabel();
                            return scoreGrade;
                        }

                        break;
                    default:
                        break;
                }
            }

            return scoreGrade;
        }

        /// <summary>
        /// 设置个人评分绩效等级配置API信息集合
        /// </summary>
        /// <param name="achieveLevelSetInfos">个人评分绩效等级配置集合</param>
        /// <param name="achiLevelSettings">绩效配置等级配置集合</param>
        private void SetAchieveLevelSetInfos(List<AchieveLevelSetInfo> achieveLevelSetInfos, EntityList<AchieveLevelSetting> achiLevelSettings)
        {
            if (achiLevelSettings != null && achiLevelSettings.Any())
            {
                foreach (var curAchiLevel in achiLevelSettings)
                {
                    var curLevelInfo = new AchieveLevelSetInfo(curAchiLevel.RowIndex, curAchiLevel.MinValue, curAchiLevel.MaxValue, (int)curAchiLevel.AchiLevel, (int)curAchiLevel.Operator);
                    achieveLevelSetInfos.Add(curLevelInfo);
                }
            }
        }

        /// <summary>
        /// 创建评分记录API信息
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <param name="isContainPetitions">是否包含申诉记录集合</param>
        /// <returns>评分记录API信息</returns>
        private ScoreRecordInfo CreateScoreRecordInfo(ScoreRecord scoreRecord, bool isContainPetitions)
        {
            var scoreRecordInfo = new ScoreRecordInfo();
            scoreRecordInfo.ScoreRecordId = scoreRecord.Id;
            scoreRecordInfo.RateItemId = scoreRecord.RatedItemId;
            scoreRecordInfo.RatedItemName = scoreRecord.RatedItem?.Name;
            scoreRecordInfo.MinScore = scoreRecord.RatedItem?.MinScore;
            scoreRecordInfo.MaxScore = scoreRecord.RatedItem?.MaxScore;
            scoreRecordInfo.ScoreState = scoreRecord.ScoreState.ToLabel(); 
            scoreRecordInfo.ScoreStateValue = scoreRecord.ScoreState;
            scoreRecordInfo.Score = scoreRecord.Score;
            scoreRecordInfo.OccurDate = scoreRecord.OccurDate;
            scoreRecordInfo.EmployeeId = scoreRecord.EmployeeId;
            scoreRecordInfo.EmployeeName = scoreRecord.Employee.Name;

            if (scoreRecord.Employee.Photo != null)
            {
                scoreRecordInfo.Photo = System.Text.Encoding.Default.GetString(scoreRecord.Employee.Photo);
            }

            scoreRecordInfo.Remark = scoreRecord.Remark;
            SetScoreRecordInfoOldScore(scoreRecordInfo, scoreRecord);
            SetScoreRecordInfoProcessMode(scoreRecordInfo, scoreRecord);

            scoreRecordInfo.AttachmentList = isContainPetitions ? CreateAttachmentInfos(scoreRecord.AttachmentList as IEnumerable<ScoreAttachBase>) : null;
            scoreRecordInfo.PetitionList = isContainPetitions && scoreRecord.ScoreState != ScoreState.Canceled ? CreatePetitionRecordInfos(scoreRecord) : null;

            return scoreRecordInfo;
        }

        /// <summary>
        /// 创建附件API信息集合
        /// </summary>
        /// <param name="attachmentList">附件集合</param>
        /// <returns>附件API信息集合</returns>
        private List<AttachmentInfo> CreateAttachmentInfos(IEnumerable<ScoreAttachBase> attachmentList)
        {
            List<AttachmentInfo> attachmentInfos = null;
            if (attachmentList != null && attachmentList.Any())
            {
                attachmentInfos = new List<AttachmentInfo>();
                foreach (var curAttachment in attachmentList)
                {
                    if (curAttachment.FileContent != null)
                    {
                        var curAttachmentInfo = CreateAttachmentInfo(curAttachment);
                        attachmentInfos.Add(curAttachmentInfo);
                    }
                }
            }

            return attachmentInfos;
        }

        /// <summary>
        /// 创建附件API信息
        /// </summary>
        /// <param name="attachment">附件实体</param>
        /// <returns>附件API信息</returns>
        private AttachmentInfo CreateAttachmentInfo(ScoreAttachBase attachment)
        {
            var curAttachmentInfo = new AttachmentInfo();
            curAttachmentInfo.ContentBase64 = Convert.ToBase64String(attachment.FileContent);
            curAttachmentInfo.FileExtesion = attachment.FileExtesion;
            curAttachmentInfo.FileName = attachment.FileName;
            curAttachmentInfo.FileSize = string.Empty;
            return curAttachmentInfo;
        }

        /// <summary>
        /// 创建申诉记录API信息集合
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <returns>申诉记录API信息类集合</returns>
        private List<PetitionRecordInfo> CreatePetitionRecordInfos(ScoreRecord scoreRecord)
        {
            List<PetitionRecordInfo> petitionRecordInfos = null;
            if (scoreRecord.PetitionList != null && scoreRecord.PetitionList.Any())
            {
                petitionRecordInfos = new List<PetitionRecordInfo>();
                var petitionRecords = scoreRecord.PetitionList.OrderBy(x => x.RowIndex).ToList();
                foreach (var curPetitionRecord in petitionRecords)
                {
                    var curPetitionRecordInfo = CreatePetitionRecordInfo(curPetitionRecord, scoreRecord);
                    petitionRecordInfos.Add(curPetitionRecordInfo);
                }
            }

            return petitionRecordInfos;
        }

        /// <summary>
        /// 创建申诉记录API信息
        /// </summary>
        /// <param name="petitionRecord">申诉记录实体</param>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <returns>申诉记录API信息类</returns>
        private PetitionRecordInfo CreatePetitionRecordInfo(PetitionRecord petitionRecord, ScoreRecord scoreRecord)
        {
            var petitionRecordInfo = new PetitionRecordInfo();
            petitionRecordInfo.RowIndex = petitionRecord.RowIndex;
            petitionRecordInfo.ScoreRecordId = petitionRecord.ScoreRecordId;
            petitionRecordInfo.PetitionerId = petitionRecord.PetitionerId;
            petitionRecordInfo.PetitionerName = petitionRecord.Petitioner.Name;
            petitionRecordInfo.PetitionerDate = petitionRecord.PetitionDate;
            petitionRecordInfo.PetitionerRemark = petitionRecord.PetitionRemark;
            if (petitionRecord.HandlerId != null && petitionRecord.HandlerId > 0)
            {
                petitionRecordInfo.HandlerId = petitionRecord.HandlerId;
                petitionRecordInfo.HandlerName = petitionRecord.Handler.Name;
                petitionRecordInfo.ProcessDate = petitionRecord.ProcessDate;
                petitionRecordInfo.ProcessMode = petitionRecord.ProcessMode.ToLabel();
                petitionRecordInfo.ProcessModeValue = petitionRecord.ProcessMode;
                petitionRecordInfo.ProcessResult = petitionRecord.ProcessResult;
            }

            petitionRecordInfo.RateItemId = scoreRecord.RatedItemId;
            petitionRecordInfo.RatedItemName = scoreRecord.RatedItem?.Name;
            petitionRecordInfo.Score = scoreRecord.Score;
            petitionRecordInfo.AttachmentList = CreateAttachmentInfos(petitionRecord.AttachmentList as IEnumerable<ScoreAttachBase>);
            return petitionRecordInfo;
        }

        /// <summary>
        /// 设置评分记录API信息的原分值、原评分项目
        /// </summary>
        /// <param name="scoreRecordInfo">评分记录API信息类</param>
        /// <param name="scoreRecord">评分记录</param>
        private void SetScoreRecordInfoOldScore(ScoreRecordInfo scoreRecordInfo, ScoreRecord scoreRecord)
        {
            var defaultProcessRecord = GetDefaultProcessRecord(scoreRecord);
            if (defaultProcessRecord != null)
            {
                scoreRecordInfo.OldRatedItemId = defaultProcessRecord.OldRatedItemId;
                scoreRecordInfo.OldRatedItemName = defaultProcessRecord.OldRatedItem?.Name;
                scoreRecordInfo.OldScore = defaultProcessRecord.OldValue;
            }
            else
            {
                scoreRecordInfo.OldRatedItemId = scoreRecordInfo.RateItemId;
                scoreRecordInfo.OldRatedItemName = scoreRecordInfo.RatedItemName;
                scoreRecordInfo.OldScore = scoreRecordInfo.Score;
            }
        }

        /// <summary>
        /// 设置评分记录API信息的申诉处理方式
        /// </summary>
        /// <param name="scoreRecordInfo">评分记录API信息类</param>
        /// <param name="scoreRecord">评分记录</param>
        private void SetScoreRecordInfoProcessMode(ScoreRecordInfo scoreRecordInfo, ScoreRecord scoreRecord)
        {
            scoreRecordInfo.PetitionProcessMode = string.Empty;
            scoreRecordInfo.PetitionProcessModeValue = null;
            if (scoreRecord.ScoreState == ScoreState.Processed)
            {
                var defaultPetition = scoreRecord.PetitionList?.FirstOrDefault();
                if (defaultPetition != null)
                {
                    scoreRecordInfo.PetitionProcessMode = defaultPetition.ProcessMode.ToLabel();
                    scoreRecordInfo.PetitionProcessModeValue = defaultPetition.ProcessMode;
                }
            }
        }

        /// <summary>
        /// 获取评分记录的默认处理记录
        /// </summary>
        /// <param name="scoreRecord">评分记录</param>
        /// <returns>默认处理记录</returns>
        private ProcessRecord GetDefaultProcessRecord(ScoreRecord scoreRecord)
        {
            var defaultProcessRecord = scoreRecord.PetitionList.SelectMany(x => x.ProcessList).FirstOrDefault();
            return defaultProcessRecord;
        }

        /// <summary>
        /// Check参数合法--员工评分申诉
        /// </summary>
        /// <param name="petitionInfo">申诉记录API信息</param>
        /// <returns>评分记录实体</returns>
        private ScoreRecord CheckSubmitPetitionValid(PetitionInfo petitionInfo)
        {
            if (petitionInfo.PetitionerRemark.IsNullOrWhiteSpace())
                throw new ValidationException("申诉说明不能为空!".L10nFormat());

            if (petitionInfo.ScoreRecordId <= 0)
                throw new ValidationException("评分记录Id不能<=0!".L10N());
            var scoreRecord = RF.GetById<ScoreRecord>(petitionInfo.ScoreRecordId);
            if (scoreRecord == null)
                throw new ValidationException("评分记录Id[{0}]未找到对应的评分记录信息!".L10nFormat(petitionInfo.ScoreRecordId));

            if (scoreRecord.Score >= 0)
                throw new ValidationException("当前记录分值为[{0}], 只有负绩效评分记录可以进行一次申诉操作!".L10nFormat(scoreRecord.Score));
            if (scoreRecord.ScoreState != ScoreState.State)
                throw new ValidationException("评分记录状态是[{0}], 不能进行申诉!".L10nFormat(scoreRecord.ScoreState.ToLabel()));
            return scoreRecord;
        }

        /// <summary>
        /// 获取当前评分记录的申诉记录的RowIndex
        /// </summary>
        /// <param name="scoreRecord">当前评分记录实体</param>
        /// <returns>申诉记录的RowIndex</returns>
        private int GetRowIndex(ScoreRecord scoreRecord)
        {
            var rowIndex = 0;
            if (scoreRecord.PetitionList != null && scoreRecord.PetitionList.Count > 0)
            {
                rowIndex = scoreRecord.PetitionList.Count + 1;
            }
            else
            {
                rowIndex = 1;
            }
            return rowIndex;
        }

        /// <summary>
        /// 创建申诉记录实体
        /// </summary>
        /// <param name="curPetitionInfo">申诉记录API信息</param>
        /// <param name="rowIndex">行号</param>
        /// <returns>申诉记录实体</returns>
        private PetitionRecord CreatePetitionRecord(PetitionInfo curPetitionInfo, int rowIndex)
        {
            var currentDateTime = RF.Find<PetitionRecord>().GetDbTime();
            var curPetitionRecord = new PetitionRecord();
            curPetitionRecord.GenerateId();
            curPetitionRecord.RowIndex = rowIndex;
            curPetitionRecord.PetitionDate = currentDateTime;
            curPetitionRecord.PetitionRemark = curPetitionInfo.PetitionerRemark;
            curPetitionRecord.ProcessDate = null;
            curPetitionRecord.ProcessResult = null;
            curPetitionRecord.ProcessMode = null;
            curPetitionRecord.HandlerId = null;
            curPetitionRecord.PetitionerId = RT.IdentityId; ////curPetitionInfo.PetitionerId;
            curPetitionRecord.ScoreRecordId = curPetitionInfo.ScoreRecordId;

            return curPetitionRecord;
        }

        /// <summary>
        /// 创建申诉附件列表实体
        /// </summary>
        /// <param name="attachmentList">附件API信息集合</param>
        /// <param name="curPetitionRecord">申诉记录</param>
        /// <returns>申诉附件集合</returns>
        private EntityList<PetitionAttachment> CreatePetitionAttachmentList(List<AttachmentInfo> attachmentList, PetitionRecord curPetitionRecord)
        {
            EntityList<PetitionAttachment> petitionAttachments = null;
            if (attachmentList != null && attachmentList.Any())
            {
                petitionAttachments = new EntityList<PetitionAttachment>();
                foreach (var attachment in attachmentList)
                {
                    var curPetitionAttachment = new PetitionAttachment();
                    curPetitionAttachment.PetitionRecordId = curPetitionRecord.Id;
                    if (!attachment.ContentBase64.IsNullOrWhiteSpace())
                    {
                        curPetitionAttachment.FileContent = Convert.FromBase64String(attachment.ContentBase64);
                    }
                    else
                    {
                        curPetitionAttachment.FileContent = null;
                    }
                    curPetitionAttachment.FileExtesion = attachment.FileExtesion;
                    curPetitionAttachment.FileName = attachment.FileName;
                    petitionAttachments.Add(curPetitionAttachment);
                }
            }

            return petitionAttachments;
        }

        /// <summary>
        /// 验证是否能取消评分记录的申诉
        /// </summary>
        /// <param name="scoreRecordId">评分记录Id</param>
        /// <returns>评分记录实体</returns>
        private ScoreRecord CheckCancelPetitionValid(double scoreRecordId)
        {
            if (scoreRecordId <= 0)
            {
                throw new ValidationException("当前评分记录Id不能<=0!".L10N());
            }
            var scoreRecord = RF.GetById<ScoreRecord>(scoreRecordId);
            if (scoreRecord == null)
            {
                throw new ValidationException("当前评分记录Id[{0}]未找到对应的评分记录信息!".L10nFormat(scoreRecordId));
            }
            if (scoreRecord.ScoreState != ScoreState.Stating)
            {
                throw new ValidationException("当前评分记录的状态为[{0}], 不能撤销申诉!".L10nFormat(scoreRecord.ScoreState.ToLabel()));
            }
            var petitionRecord = scoreRecord.PetitionList;
            if (petitionRecord == null && !petitionRecord.Any())
            {
                throw new ValidationException("当前评分记录没有申诉记录,不能撤销申诉!".L10N());
            }
            return scoreRecord;
        }

        /// <summary>
        /// 取消申诉修改评分记录实体的属性
        /// 评分状态ScoreState由"申诉中"-->"已处理或取消申诉"
        /// 备注Remark添加取消申诉信息
        /// </summary>
        /// <param name="scoreRecord">评分记录</param>
        private void CancelUpdateScoreRecord(ScoreRecord scoreRecord)
        {
            string updateMsg = string.Empty; ////" ,撤销申诉!";
            const ScoreState scoreState = ScoreState.Canceled;
            UpdateScoreRecord(scoreRecord, updateMsg, scoreState);
        }

        /// <summary>
        /// 评分记录属性修改: 评分状态、评分项目、评分分值
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <param name="scoreState">评分状态</param>
        /// <param name="processRecordInfo">处理记录API信息</param>
        private void ProcessPetitionUpdateScoreRecord(ScoreRecord scoreRecord, ScoreState scoreState, ProcessRecordInfo processRecordInfo)
        {
            string updateMsg = string.Empty; //",申诉处理!"
            if (processRecordInfo.ProcessMode == (int)StateProcessMode.Adjust)
            {
                scoreRecord.RatedItemId = processRecordInfo.RateItemId;
                scoreRecord.Score = processRecordInfo.Score;
            }
            else if (processRecordInfo.ProcessMode == (int)StateProcessMode.Repeal)
            {
                scoreRecord.IsEffective = false;
            }
            else
            {
                //
            }

            UpdateScoreRecord(scoreRecord, updateMsg, scoreState);
        }

        /// <summary>
        /// 更新评分记录的属性--评分状态、备注
        /// </summary>
        /// <param name="scoreRecord">评分记录实体</param>
        /// <param name="updateMsg">备注信息</param>
        /// <param name="scoreState">评分状态</param>
        private void UpdateScoreRecord(ScoreRecord scoreRecord, string updateMsg, ScoreState scoreState)
        {
            var curDateTime = RF.Find<ScoreRecord>().GetDbTime();
            var employee = RF.GetById<Employee>(RT.IdentityId);
            scoreRecord.ScoreState = scoreState;
            if (!updateMsg.IsNullOrWhiteSpace())
            {
                updateMsg = curDateTime.ToString() + employee.Name + updateMsg.L10N();
                scoreRecord.Remark += updateMsg;
            }

            RF.Save(scoreRecord);
        }

        #endregion API调用的方法
    }
}
