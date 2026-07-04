using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Common.Configs;
using SIE.EMS.Common.Utils;
using SIE.EMS.DataAuth;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Maintains.Configs;
using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Projects;
using SIE.EMS.Maintains.Records;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.Tpms;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.EMS.Repairs;
using SIE.Rbac.Users;
using SIE.Resources.Enterprises;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Maintains.Controller
{

    /// <summary>
    /// 设备保养控制器
    /// </summary>
    public partial class MaintainController : DomainController
    {
        #region 配置项

        /// <summary>
        /// 是否精确生成保养计划
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsMaintainForPrecisePlan()
        {
            var config = ConfigService.GetConfig(new MaintainPrecisePlanConfig(), typeof(MaintainPlanViewModel));
            return config.IsMaintainForPrecisePlan == YesNo.Yes;
        }

        /// <summary>
        /// 是否按部门进行保养
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsDepartmentMaintain()
        {
            var config = ConfigService.GetConfig(new IsDepartmentPlanConfig());
            if (config == null)
                throw new ValidationException("未找到按部门进行保养配置,请检查配置项".L10N());
            return config.IsDepartmentPlan == YesNo.Yes;
        }

        /// <summary>
        /// 保养是否有间隔时间限制
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsIntervalTime()
        {
            var config = ConfigService.GetConfig(new MaintainIntervalTimeConfig(), typeof(MaintainPlanViewModel));
            return config.IsIntervalTime;
        }

        /// <summary>
        /// 保养间隔时间限制天数
        /// </summary>
        /// <returns>bool</returns>
        public virtual int? GetIntervalTime()
        {
            var config = ConfigService.GetConfig(new MaintainIntervalTimeConfig(), typeof(MaintainPlanViewModel));
            return config.IntervalTime;
        }


        /// <summary>
        /// 保养是否必填工时登记
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMaintainWorkTime()
        {
            var config = ConfigService.GetConfig(new MaintainWorkTimeConfig(), typeof(MaintainPlanViewModel));
            return config.IsMaintainForWorkTime == YesNo.Yes;
        }

        /// <summary>
        /// 获取保养提前预警时间(H)
        /// </summary>
        /// <returns>保养提前预警时间(H)</returns>
        public virtual int? GetMaintainAlertTime()
        {
            var config = ConfigService.GetConfig(new MaintainAlertTimeConfig(), typeof(MaintainPlanViewModel));
            if (config == null || config.AlertTime == null)
                throw new ValidationException("未找到点检提前预警时间的配置规则,请检查规则配置".L10N());
            return config.AlertTime;
        }

        /// <summary>
        /// 获取保养超时预警时间(H)
        /// </summary>
        /// <returns>保养超时预警时间(H)</returns>
        public virtual int? GetMaintainExpiredTime()
        {
            var config = ConfigService.GetConfig(new MaintainAlertTimeConfig(), typeof(MaintainPlanViewModel));
            if (config == null || config.ExpiredTime == null)
                throw new ValidationException("未找到点检超时预警时间的配置规则,请检查规则配置".L10N());
            return config.ExpiredTime;
        }
        #endregion

        #region 保养计划
        /// <summary>
        /// 修改保养计划
        /// </summary>
        public virtual void EditMaintainPlans(EntityList<MaintainPlan> plans)
        {
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                plans.ForEach(plan =>
                {
                    DB.Update<MaintainPlan>()
                        .Set(p => p.MaintainType, plan.MaintainType)
                        .Set(p => p.PrecisePlanBeginDate, plan.PrecisePlanBeginDate)
                        .Set(p => p.PrecisePlanEndDate, plan.PrecisePlanEndDate)
                        .Set(p => p.MaintainTime, plan.MaintainTime)
                    .Where(p => p.Id == plan.Id).Execute();
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建保养计划主信息列表（新）
        /// </summary>
        /// <param name="accounts">设备台账列表</param>
        /// <param name="dicMaintainPlans">保养计划字典</param>
        /// <returns>保养计划主信息列表</returns>
        private EntityList<MaintainPlanViewModel> CreateMaintainPlanMainInfos(EntityList<EquipAccountSelect> accounts, Dictionary<double, List<MaintainPlan>> dicMaintainPlans)
        {
            EntityList<MaintainPlanViewModel> records = new EntityList<MaintainPlanViewModel>();

            foreach (var account in accounts)
            {
                var record = new MaintainPlanViewModel();
                record.EquipAccountId = account.Id;
                record.EquipAccountCode = account.Code;
                record.EquipAccountName = account.Name;
                record.EquipModelName = account.ModelName;
                record.EquipTypeCategory = account.EquipTypeCategory;
                record.WorkShopName = account.WorkShopName;
                record.ResourceName = account.ResourceName;
                record.UseState = account.UseState;

                List<MaintainPlan> plans = null;
                List<string> ColumnNames = new List<string>();

                if (dicMaintainPlans.TryGetValue(account.Id, out plans))
                {
                    List<CheckPlanColumn> columns = new List<CheckPlanColumn>();

                    plans.ForEach((Action<MaintainPlan>)(plan =>
                    {
                        var list = plans.Where(p => p.PlanBeginDate == plan.PlanBeginDate && p.PlanEndDate == plan.PlanEndDate).ToList();

                        CheckPlanColumn column = new CheckPlanColumn();

                        if ((plan.PlanEndDate - plan.PlanBeginDate).TotalDays <= 7)
                        {
                            column.DayNum = plan.Cycle;
                            column.BeginTime = DateTimeFormat.ToShortFormat1(plan.PlanBeginDate);
                            column.EndTime = DateTimeFormat.ToShortFormat1(plan.PlanEndDate);
                            column.ShiftName = plan.MaintainType.ToLabel();
                            column.ColumnName = column.BeginTime + "-" + column.EndTime;
                        }
                        else
                        {
                            column.DayNum = plan.Cycle;
                            var weekInfo = GetWeekInfoOfDate(plan.PlanBeginDate);
                            column.BeginTime = DateTimeFormat.ToShortFormat1(weekInfo.Item2);
                            column.EndTime = DateTimeFormat.ToShortFormat1(weekInfo.Item3);
                            column.ShiftName = plan.MaintainType.ToLabel();
                            column.ColumnName = column.BeginTime + "-" + column.EndTime;
                        }

                        if (!ColumnNames.Contains(column.ColumnName))
                        {
                            ColumnNames.Add(column.ColumnName);

                            if (list.All(p => p.ExeResult == ExeResult.Successed) || plan.ExeResult == ExeResult.Successed)
                            {
                                column.ExeResult = "/" + "OK";
                            }
                            else if (list.Any(p => p.ExeResult == ExeResult.Failed) || plan.ExeResult == ExeResult.Failed)
                            {
                                column.ExeResult = "/" + "NG";
                            }
                            else
                            {
                                column.ExeResult = "";
                            }

                            //执行状态
                            if (list.Any(p => p.ExeState == MaintExeState.NotPerformed) || plan.ExeState == MaintExeState.NotPerformed)
                            {
                                column.ExeState = plan.PlanEndDate < DateTime.Now.Date ? 2 : 0;//2：超期，0：未执行
                            }
                            else if (list.Any(p => p.ExeState == MaintExeState.Performing) || plan.ExeState == MaintExeState.Performing)
                            {
                                column.ExeState = plan.PlanEndDate < DateTime.Now.Date ? 2 : 4;//2：超期，4：执行中
                            }
                            else if (list.Any(p => p.ExeState == MaintExeState.NotConfirm) || plan.ExeState == MaintExeState.NotConfirm)
                            {
                                //待确认
                                column.ExeState = 5;
                            }
                            else
                            {
                                //已执行(已评分)
                                column.ExeState = 1;
                            }

                            columns.Add(column);
                        }
                    }));

                    record.DataJsonString = JsonConvert.SerializeObject(columns);
                }
                else // 数据为空也要new一个对象填充一个空单元格
                {
                    List<CheckPlanColumn> columns = new List<CheckPlanColumn>();
                    record.DataJsonString = JsonConvert.SerializeObject(columns);
                }
                records.Add(record);
                ColumnNames.Clear();
            }
            return records;
        }

        /// <summary>
        /// 根据保养计划查询实体和设备台账Id列表获取保养计划列表（新）
        /// </summary>
        /// <param name="criteria">保养计划查询实体</param>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <param name="isShowAllEquipAccount"></param>
        /// <returns>保养计划列表</returns>
        public virtual EntityList<MaintainPlan> GetMaintainPlanListByAccountIds(MaintainPlanCriteria criteria, List<double> accountIds, bool isShowAllEquipAccount = false)
        {
            //获取是否按部门进行保养
            bool isDepartmentMaintain = IsDepartmentMaintain();

            return accountIds.SplitContains((tempIds) =>
            {
                var query = Query<MaintainPlan>().Where(p => tempIds.Contains(p.EquipAccountId));

                if (isDepartmentMaintain)
                {
                    query.Where(p => p.DepartmentId != null && p.DepartmentId != 0);
                    query.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                                             .LeftJoin<DeviceDepa>((a, d) => a.Id == d.DevicePurId)
                                             .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                                             .WhereIf<DeviceDepa>(isDepartmentMaintain, (a, d) => x.DepartmentId == d.EnterpriseId));
                }
                else
                {
                    query.Where(p => p.DepartmentId == null || p.DepartmentId == 0);
                }

                return query.ToList();
            });
        }

        /// <summary>
        /// 获取相关设备的保养计划列表(新)
        /// </summary>
        /// <param name="criteria">保养计划查询实体</param>
        /// <returns>保养计划返回信息</returns>
        public virtual EntityList<MaintainPlanViewModel> GetEquipMaintainPlans(MaintainPlanCriteria criteria)
        {
            var equipCt = RT.Service.Resolve<EquipController>();
            var accounts = equipCt.CriteriaEquipForMaintainPlans(criteria);

            var accountIds = accounts.Select(p => p.Id).ToList();
            var maintainPlans = GetMaintainPlanListByAccountIds(criteria, accountIds);
            var dicMaintainPlans = maintainPlans.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());
            var records = CreateMaintainPlanMainInfos(accounts, dicMaintainPlans);
            records.SetTotalCount(accounts.TotalCount);
            return records;
        }

        /// <summary>
        /// 获取保养计划列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>保养计划列表</returns>
        public virtual EntityList<MaintainPlan> GetMaintainPlansByExpression(Expression<Func<MaintainPlan, bool>> exp)
        {
            var query = Query<MaintainPlan>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取保养计划
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>保养计划</returns>
        public virtual MaintainPlan GetMaintainPlan(Expression<Func<MaintainPlan, bool>> exp)
        {
            var query = Query<MaintainPlan>();
            if (exp != null)
                query.Where(exp);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取保养单号
        /// </summary>
        /// <returns>保养单号</returns>
        public virtual string GetMaintainPlanNo()
        {
            var config = ConfigService.GetConfig(new MaintainPlanNoConfig(), typeof(MaintainPlanViewModel));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到保养单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取多个保养计划单号(优化计算按部门获取个数)
        /// </summary>
        /// <param name="equipList">设备台账</param>
        /// <param name="planList">添加计划</param>
        /// <param name="projectList">设备保养项目</param>
        /// <param name="isDepartmentCheck">是否按部门进行保养</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<string> CalculateMaintainPlanNoList(EntityList<EquipAccount> equipList, EntityList<MaintainPlan> planList, EntityList<EquipAccountMaintainProject> projectList, bool isDepartmentCheck)
        {
            // 不按部门进行保养直接生成设备数量*计划数量个保养计划
            if (!isDepartmentCheck)
            {
                return GetMaintainPlanNoList(equipList.Count * planList.Count);
            }
            else
            {
                // 分别计算出，每个设备的保养项目有多少个保养部门字段
                int noCount = 0;
                foreach (var equip in equipList)
                {
                    var currentProjectsDeptIdsCount = projectList.Where(p => p.EquipAccountId == equip.Id && p.DepartmentId != null && p.DepartmentId != 0)
                              .Select(p => p.DepartmentId).Distinct().Count();
                    noCount += currentProjectsDeptIdsCount * planList.Count;
                }
                return GetMaintainPlanNoList(noCount);
            }
        }

        /// <summary>
        /// 获取多个保养计划单号
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<string> GetMaintainPlanNoList(int number)
        {
            var config = ConfigService.GetConfig(new MaintainPlanNoConfig(), typeof(MaintainPlanViewModel));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到保养单号生成规则,请检查规则配置".L10N());
            if (number <= 0)
            {
                return new List<string>();
            }
            else
            {
                return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, number).ToList();
            }

        }

        /// <summary>
        /// 根据保养计划查询实体和设备台账Id列表获取保养计划列表
        /// </summary>
        /// <param name="criteria">保养计划查询实体</param>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>保养计划列表</returns>
        public virtual EntityList<MaintainPlan> GetMaintainPlanList(MaintainPlanCriteria criteria, List<double> accountIds)
        {
            return accountIds.SplitContains((tempIds) =>
            {
                var query = Query<MaintainPlan>().Where(p => tempIds.Contains(p.EquipAccountId));
                if (criteria.Year.HasValue)
                    query.Where(w => w.PlanBeginDate >= DateTime.Parse(criteria.Year.Value.Year + "/1/1") && w.PlanEndDate < DateTime.Parse(criteria.Year.Value.AddYears(1).Year + "/1/1"));
                return query.ToList();
            });
        }

        /// <summary>
        /// 获取当前登录用户所属部门当天所有未执行的保养单信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="departmentIds">部门ID</param>
        /// <param name="pagingInfo">分页实体</param>
        /// <param name="exeState">单据状态</param>
        /// <returns></returns>
        public virtual EntityList GetNotPerformedMaintainPlans(string keyword, List<double> departmentIds, PagingInfo pagingInfo, List<MaintExeState?> exeState)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();

            var q = Query<MaintainPlan>();

            q.Where(p => p.PlanBeginDate < now.Date.AddDays(1));

            //过滤部门
            var deptIds = RT.Service.Resolve<DevicePurController>().GetDutyDepartmentIds(RT.Identity.UserId);

            if (departmentIds != null && departmentIds.Count >= 1)
            {
                departmentIds = departmentIds.Where(p => deptIds.Contains(p)).ToList();
            }
            else
            {
                departmentIds = deptIds;
            }

            var nullableDeptIds = departmentIds.Cast<double?>();

            q.Where(p => nullableDeptIds.Contains(p.DepartmentId) || p.DepartmentId == null);

            //模糊查询
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.EquipAccount.Code.Contains(keyword) || p.EquipAccount.Name.Contains(keyword));
            }

            //过滤状态
            if (exeState.Any())
            {
                q.Where(p => exeState.Contains(p.ExeState));
            }
            else
            {
                q.Where(p => p.ExeState == MaintExeState.Performing || p.ExeState == MaintExeState.NotPerformed || p.ExeState == MaintExeState.Overdue);
            }
            q.OrderByDescending(p => p.ExeState).OrderByDescending(p => p.PlanBeginDate);
            //当前用户可管理的设备台账
            var query = q.ToQuery();
            query.QueryWithEquipAccountPermissions(MaintainPlan.EquipAccountIdProperty.Name);

            return q.Repository.QueryList(query, pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前登录用户所属部门当天所有未执行的保养单信息
        /// </summary>
        /// <param name="exeStates">单据状态</param>
        /// <returns></returns>
        public virtual int GetNotPerformedMaintainPlansCount(List<MaintExeState> exeStates)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();

            var q = Query<MaintainPlan>();

            q.Where(p => p.PlanBeginDate < now.Date);

            //过滤部门
            var deptIds = RT.Service.Resolve<DevicePurController>()
                .GetDutyDepartments(RT.Identity.UserId)
                .Select(p => p.Id)
                .ToList();

            var nullableDeptIds = deptIds.Cast<double?>();

            q.Where(p => nullableDeptIds.Contains(p.DepartmentId) || p.DepartmentId == null);


            //过滤状态
            if (exeStates.Any())
            {
                q.Where(p => exeStates.Contains(p.ExeState));
            }
            else
            {
                q.Where(p => p.ExeState == MaintExeState.Performing || p.ExeState == MaintExeState.NotPerformed || p.ExeState == MaintExeState.Overdue);
            }
            q.OrderByDescending(p => p.ExeState).OrderByDescending(p => p.PlanBeginDate);
            //当前用户可管理的设备台账
            var query = q.ToQuery();
            query.QueryWithEquipAccountPermissions(MaintainPlan.EquipAccountIdProperty.Name);

            return q.Repository.Count(query);
        }

        /// <summary>
        /// 获取上次保养小结
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public virtual string GetLastMaintainSummary(double accountId, double? departmentId)
        {
            var q = Query<MaintainPlan>();
            q.Where(p => p.EquipAccountId == accountId);
            q.Where(p => p.DepartmentId == departmentId);
            q.Where(p => p.ExeState == MaintExeState.Performed || p.ExeState == MaintExeState.Scored);
            q.OrderByDescending(p => p.PlanEndDate);

            return q.ToList(new PagingInfo(1, 1)).FirstOrDefault()?.MaintainSummary;
        }

        /// <summary>
        /// 保存保养单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual void SaveMaintainPlan(MaintainSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存主单数据
                DB.Update<MaintainPlan>().Where(p => p.Id == info.MaintainPlanId)
                .Set(p => p.MaintainSummary, info.MaintainSummary)
                .Set(p => p.ExeState, MaintExeState.Performing)
                .Set(p => p.ExecuteById, RT.IdentityId)
                .Set(p => p.ActBeginDate, info.BeginTime)
                .Set(p => p.ActEndDate, info.EndTime)
                .Execute();

                //保存保养项目
                SaveMaintainItems(info);

                //保存备件申请单
                SaveSparePartApplyInfo(info);

                //保存备件申请数据
                SaveSparePartApllyDatas(info);

                //保存工时登记数据
                SaveWorkHoursData(info);

                //保存图片
                var hepler = new FileUrlHelper();
                var attachments = new EntityList<MaintainPlanAttachment>();
                //找出保养单已经存在表里的图片
                var MaintainPlanAttachment = Query<MaintainPlanAttachment>().Where(p => p.OwnerId == info.MaintainPlanId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var ExitPhotoIds = MaintainPlanAttachment.Select(p => p.Id).ToList();
                var submitPhotoIds = info.Photoes.Where(p => p.Id.HasValue).Select(p => p.Id).ToList();
                if (ExitPhotoIds.Count > 0)
                {
                    var DeleteIds = ExitPhotoIds.Where(x => !submitPhotoIds.Any(a => x == a)).ToList();
                    if (DeleteIds.Count > 0)
                    {
                        DeleteIds.ForEach(P =>
                        {
                            DB.Delete<MaintainPlanAttachment>().Where(x => x.Id == P).Execute();
                        });
                    }
                }
                info.Photoes.ForEach(p =>
                {
                    if (p.Id == null)
                    {
                        var attachment = hepler.GenerateAttachmentBase64StringContent(new MaintainPlanAttachment(), p.Content, p.FileName) as MaintainPlanAttachment;
                        attachment.OwnerId = info.MaintainPlanId;
                        attachments.Add(attachment);
                    }
                });
                RF.Save(attachments);

                trans.Complete();
            }
        }

        private void SaveWorkHoursData(MaintainSaveSubmitInfo info)
        {
            info.WorkHourDetails.ForEach(x =>
            {
                if (x.MaintainWorkHourId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的工时登记ID为0的数据".L10N());
                }

                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var workHoursRegister = new WorkHoursRegister();
                            workHoursRegister.EmployeeId = x.EmployeeId;
                            workHoursRegister.BeginDay = DateTime.Parse(x.BeginDay);
                            workHoursRegister.EndDay = DateTime.Parse(x.EndDay);
                            workHoursRegister.WorkHours = x.WorkHours;
                            workHoursRegister.MaintainPlanId = info.MaintainPlanId;
                            workHoursRegister.GenerateId();
                            RF.Save(workHoursRegister);
                            break;
                        }
                    case 1:
                        {
                            //修改
                            DB.Update<WorkHoursRegister>().Where(p => p.Id == x.MaintainWorkHourId)
                                .Set(p => p.EmployeeId, x.EmployeeId)
                                .Set(p => p.BeginDay, DateTime.Parse(x.BeginDay))
                                .Set(p => p.EndDay, DateTime.Parse(x.EndDay))
                                .Set(p => p.WorkHours, x.WorkHours)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除
                            DB.Delete<WorkHoursRegister>().Where(p => p.Id == x.MaintainWorkHourId).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        private void SaveSparePartApllyDatas(MaintainSaveSubmitInfo info)
        {
            info.SparePartAplDetails.ForEach(x =>
            {
                if (x.MaintainSparePartId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                }
                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var maintainPlanSparePartApl = new MaintainPlanSparePartApl();
                            maintainPlanSparePartApl.SparePartId = x.SparePartId;
                            maintainPlanSparePartApl.ApplyQty = x.ApplyQty;
                            maintainPlanSparePartApl.OutStockWarehouseId = x.OutStockWarehouseId;
                            maintainPlanSparePartApl.ApplyDetailId = x.AppDtlId;
                            maintainPlanSparePartApl.Remark = x.Remark;
                            maintainPlanSparePartApl.MaintainPlanId = info.MaintainPlanId;
                            maintainPlanSparePartApl.GenerateId();
                            RF.Save(maintainPlanSparePartApl);
                            break;
                        }
                    case 1:
                        {
                            //修改
                            DB.Update<MaintainPlanSparePartApl>().Where(p => p.Id == x.MaintainSparePartId && !p.IsApply)
                                .Set(p => p.SparePartId, x.SparePartId)
                                .Set(p => p.ApplyQty, x.ApplyQty)
                                .Set(p => p.OutStockWarehouseId, x.OutStockWarehouseId)
                                .Set(p => p.ApplyDetailId, x.AppDtlId)
                                .Set(p => p.Remark, x.Remark)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除(已申请的不允许删除)
                            DB.Delete<MaintainPlanSparePartApl>().Where(p => p.Id == x.MaintainSparePartId && !p.IsApply).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        private void SaveSparePartApplyInfo(MaintainSaveSubmitInfo info)
        {
            info.SparePartDetails.ForEach(x =>
            {
                if (x.MaintainSparePartId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                }

                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var maintainPlanSparePart = new MaintainPlanSparePart();
                            maintainPlanSparePart.SparePartId = x.SparePartId;
                            maintainPlanSparePart.PartOutDepotDetailId = x.OutDtlId;
                            maintainPlanSparePart.ChangeQty = x.ChangeQty;
                            maintainPlanSparePart.Remark = x.Remark;
                            maintainPlanSparePart.MaintainPlanId = info.MaintainPlanId;
                            maintainPlanSparePart.GenerateId();
                            RF.Save(maintainPlanSparePart);
                            break;
                        }
                    case 1:
                        {
                            //修改(已更换的不允许删除)
                            DB.Update<MaintainPlanSparePart>().Where(p => p.Id == x.MaintainSparePartId && p.State == ChangeSparePartState.New)
                                .Set(p => p.SparePartId, x.SparePartId)
                                .Set(p => p.PartOutDepotDetailId, x.OutDtlId)
                                .Set(p => p.ChangeQty, x.ChangeQty)
                                .Set(p => p.Remark, x.Remark)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除(已更换的不允许删除)
                            DB.Delete<MaintainPlanSparePart>().Where(p => p.Id == x.MaintainSparePartId && p.State == ChangeSparePartState.New).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        private void SaveMaintainItems(MaintainSaveSubmitInfo info)
        {
            info.ProjectDetails.ForEach(x =>
            {
                if (x.ProjectId <= 0) throw new ValidationException("存在提交的保养项目ID为0的数据".L10N());
                CheckMaintainResult reslut = (CheckMaintainResult)x.Result;

                decimal? valueNull = null;
                var value = x.ActualValue.IsNullOrEmpty() ? valueNull : decimal.Parse(x.ActualValue);
                DB.Update<MaintainProject>().Where(p => p.Id == x.ProjectId)
                    .Set(p => p.ActualValue, value)
                    .Set(p => p.ExeState, MaintExeState.Performing)
                    .Set(p => p.MaintainResult, reslut)
                    .Set(p => p.Defect, x.DefectDesc)
                    .Execute();
            });
        }

        /// <summary>
        ///  根据ID获取保养单
        /// </summary>
        /// <param name="mainTainPlanId">保养单Id</param>
        /// <returns></returns>
        public virtual MaintainPlan GetMaintainPlanById(double mainTainPlanId)
        {
            var maintainPlan = Query<MaintainPlan>().Where(p => p.Id == mainTainPlanId)
                .FirstOrDefault();
            return maintainPlan;
        }

        /// <summary>
        /// 更换保养计划状态
        /// </summary>
        /// <param name="maintainPlanId"></param>
        /// <param name="state"></param>
        public virtual void ChangeMaintainPlanState(double maintainPlanId, MaintExeState state)
        {
            DB.Update<MaintainPlan>().Where(p => p.Id == maintainPlanId).Set(p => p.ExeState, state).Execute();
        }

        /// <summary>
        /// 获取保养TPM管理信息
        /// </summary>
        /// <param name="equip"></param>
        /// <returns></returns>
        public virtual EntityList<TpmViewModel> GetMaintainPlanTpmViewModel(double equip)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();
            var beginDateDay = now.Date;

            //上一次已执行/已评分时间
            var executedTime = Query<MaintainPlan>()
                .Where(p => (p.ExeState == MaintExeState.Performed || p.ExeState == MaintExeState.Scored || p.ExeState == MaintExeState.Confirmed) && p.EquipAccountId == equip)
                .OrderByDescending(p => p.PlanBeginDate)
                .Select(p => p.PlanBeginDate)
                .ToList(new PagingInfo(1, 1)).FirstOrDefault()?.PlanBeginDate;

            //按计划时间
            var toBeExecutPlanTimes = Query<MaintainPlan>()
                .Where(p => (p.ExeState == MaintExeState.NotPerformed) && p.EquipAccountId == equip)
                .Where(p => p.PlanBeginDate >= beginDateDay)
                .OrderBy(p => p.PlanBeginDate)
                .Select(p => p.PlanBeginDate)
                .ToList<DateTime>(new PagingInfo(1, 2));

            //最终结果，下一次待执行时间
            List<DateTime> toBeExecutTimes = new List<DateTime>();

            //构建返回数据
            var vms = new EntityList<TpmViewModel>();

            var vm = new TpmViewModel();
            if (executedTime != null)
            {
                vm.LastExecuteTime = executedTime;
            }
            if (toBeExecutTimes.Count > 0)
            {
                vm.CurrentToBeExecuteTime = toBeExecutTimes.OrderBy(p => p).FirstOrDefault();
                if (toBeExecutTimes.Count > 1)
                {
                    vm.NextToBeExecuteTime = toBeExecutTimes.OrderBy(p => p).LastOrDefault();
                }
            }
            if (vm.LastExecuteTime != null || vm.CurrentToBeExecuteTime != null || vm.NextToBeExecuteTime != null)
            {
                vms.Add(vm);
            }
            return vms;
        }

        #endregion

        #region 添加保养计划

        /// <summary>
        /// 保养类型匹配保养项目的周期类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private CycleType? MaintainTypeChange(MaintainType type)
        {
            switch (type)
            {
                case MaintainType.Week:
                    return CycleType.Week;
                case MaintainType.DbWeek:
                    return CycleType.DoubleWeek;
                case MaintainType.Month:
                    return CycleType.Month;
                case MaintainType.Season:
                    return CycleType.Season;
                case MaintainType.HalfYear:
                    return CycleType.HalfYear;
                case MaintainType.Year:
                    return CycleType.Year;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 按部门添加保养计划
        /// </summary>
        /// <param name="maintainProjectsOfCurrent">当前设备保养计划</param>
        /// <param name="plan">计划</param>
        /// <param name="planNos">保养单号</param>
        /// <param name="noIndex">保养单号当前下标</param>
        /// <param name="equip">当前设备</param>
        /// <param name="maintainPlans">保存列表</param>
        /// <param name="checkSourceType">数据来源</param>
        /// <param name="lockObj">线程锁</param>
        private void GeneratePlanWithDept(List<EquipAccountMaintainProject> maintainProjectsOfCurrent, MaintainPlan plan, List<string> planNos, ref int noIndex, EquipAccount equip, EntityList<MaintainPlan> maintainPlans, CheckSourceType checkSourceType, object lockObj)
        {
            if (maintainProjectsOfCurrent.Any(p => p.DepartmentId != null && p.DepartmentId != 0 && p.CycleType == MaintainTypeChange(plan.MaintainType)))
            {

                List<double?> departmentIds = maintainProjectsOfCurrent
                  .Where(p => p.DepartmentId != null && p.DepartmentId != 0)
                  .Select(p => p.DepartmentId).ToList();
                departmentIds = departmentIds.Distinct().ToList();
                foreach (var departmentId in departmentIds)
                {
                    MaintainPlan departmentPlan = JsonConvert.DeserializeObject<MaintainPlan>(JsonConvert.SerializeObject(plan));
                    if (plan.MaintainTypeInfoId.IsNotEmpty())
                    {
                        int.TryParse(plan.MaintainTypeInfoId, out int type);
                        departmentPlan.MaintainType = (MaintainType)type;
                    }
                    departmentPlan.EquipAccountId = equip.Id;
                    departmentPlan.ExeState = MaintExeState.NotPerformed;
                    departmentPlan.MaintainSourceType = checkSourceType;
                    departmentPlan.DepartmentId = departmentId;
                    departmentPlan.IsAbnormalInfoPush = true;
                    lock (lockObj)
                    {
                        departmentPlan.MaintainNo = planNos[noIndex++];
                        maintainPlans.Add(departmentPlan);
                    }
                }
            }
            else
            {
                throw new ValidationException("设备[{0}]不存在[{1}]保养项目或责任部门,无法生成保养计划！".L10nFormat(equip.Code, plan.MaintainType.ToLabel().L10N()));
            }
        }

        /// <summary>
        /// 添加保养计划
        /// </summary>
        /// <param name="plan">计划</param>
        /// <param name="planNos">保养单号</param>
        /// <param name="noIndex">保养单号当前下标</param>
        /// <param name="equip">当前设备</param>
        /// <param name="maintainPlans">保存列表</param>
        /// <param name="checkSourceType">数据来源</param>
        /// <param name="lockObj">线程锁</param>
        private void GeneratePlanWithNoDept(MaintainPlan plan, List<string> planNos, ref int noIndex, EquipAccount equip, EntityList<MaintainPlan> maintainPlans, CheckSourceType checkSourceType, object lockObj)
        {
            MaintainPlan maintainPlan = JsonConvert.DeserializeObject<MaintainPlan>(JsonConvert.SerializeObject(plan));
            if (plan.MaintainTypeInfoId.IsNotEmpty())
            {
                int.TryParse(plan.MaintainTypeInfoId, out int type);
                maintainPlan.MaintainType = (MaintainType)type;
            }
            maintainPlan.EquipAccountId = equip.Id;
            maintainPlan.ExeState = MaintExeState.NotPerformed;
            maintainPlan.MaintainSourceType = checkSourceType;
            maintainPlan.IsAbnormalInfoPush = true;
            lock (lockObj)
            {
                maintainPlan.MaintainNo = planNos[noIndex++];
                maintainPlans.Add(maintainPlan);
            }
        }

        /// <summary>
        /// 添加保养计划
        /// </summary>
        /// <param name="maintainPlanList">保养计划列表</param>
        /// <param name="EquipAccountIds">设备列表</param>
        /// <param name="checkSourceType">数据来源</param>
        public virtual string AddMaintainPlans(EntityList<MaintainPlan> maintainPlanList, List<double> EquipAccountIds, CheckSourceType checkSourceType = CheckSourceType.NewCreated)
        {
            //是否按部门进行保养
            bool isDepartmentCheck = IsDepartmentMaintain();

            //是否精确计划保养
            bool isMaintainForPrecisePlan = IsMaintainForPrecisePlan();

            //查询出需要保养的设备(待优化)
            EntityList<EquipAccount> equipAccounts = EquipAccountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccount>().Where(p => tempIds.Contains(p.Id))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            //创建最终保存的保养计划集合
            EntityList<MaintainPlan> maintainPlans = new EntityList<MaintainPlan>();

            // 设备ids
            var accountIds = equipAccounts.Select(x => x.Id).Distinct().ToList();

            //查询出已生成的保养计划集合
            var beginDate = maintainPlanList.Min(p => p.PlanBeginDate);
            var endDate = maintainPlanList.Max(p => p.PlanEndDate);
            List<MaintainPlanCreatedInfo> createdPlans = Query<MaintainPlan>()
                .Where(p => p.PlanBeginDate >= beginDate && p.PlanEndDate <= endDate && accountIds.Contains(p.EquipAccountId))
                .Select(p => new
                {
                    Id = p.Id,
                    MachineNo = p.MaintainNo,
                    EquipAccountId = p.EquipAccountId,
                    Cycle = p.Cycle,
                }).ToList<MaintainPlanCreatedInfo>().ToList();


            // 设备的保养项目(待优化)
            var maintainProjectsOfAccounts = RT.Service.Resolve<Equipments.EquipController>()
                .GetMaintainProjectsOfAccounts(accountIds);

            // 提前计算出需要的保养单号
            var planNos = CalculateMaintainPlanNoList(equipAccounts, maintainPlanList, maintainProjectsOfAccounts, isDepartmentCheck);

            ParallelOptions paraOP = new ParallelOptions() { MaxDegreeOfParallelism = 100 };
            object lockObj = new object();//线程锁
            var noIndex = 0;

            // 跳过生成的项目提示
            StringBuilder sb = new StringBuilder();

            ParallelLoopResult result = Parallel.ForEach<EquipAccount>(equipAccounts, paraOP, equip =>
            {
                // 当前设备的保养项目
                var maintainProjectsOfCurrent = maintainProjectsOfAccounts
                    .Where(x => x.EquipAccountId == equip.Id).ToList();


                foreach(var plan in maintainPlanList)
                {
                    if (isMaintainForPrecisePlan && (plan.PrecisePlanBeginDate == null || plan.PrecisePlanEndDate == null))
                    {
                        throw new ValidationException("配置项已配置精确计划保养，指定计划的开始时间和结束时间不能为空，请检查！".L10N());
                    }
                    if (createdPlans.Any(p => p.EquipAccountId == equip.Id && p.Cycle == plan.Cycle))
                    {
                        sb.AppendLine("设备[{0}]在第[{1}]周已生成过保养计划,请检查！".L10nFormat(equip.Code, plan.Cycle));
                        continue;
                    }
                    if (isDepartmentCheck)
                    {
                        GeneratePlanWithDept(maintainProjectsOfCurrent, plan, planNos, ref noIndex, equip, maintainPlans, checkSourceType, lockObj);
                    }
                    else
                    {
                        GeneratePlanWithNoDept(plan, planNos, ref noIndex, equip, maintainPlans, checkSourceType, lockObj);
                    }
                }
            });
            if (result.IsCompleted)
            {
                if (maintainPlans.Any())
                {
                    RF.BatchInsert(maintainPlans);
                    //RF.Save(maintainPlans);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region 生成保养项目、保存保养计划        
        private MaintainProject GenerateMaintainProject(MaintainPlan plan, EquipAccountMaintainProject projectDetail)
        {
            var maintainProject = new MaintainProject();
            maintainProject.MaintainPlan = plan;
            maintainProject.EquipAccountId = projectDetail.EquipAccountId;
            maintainProject.EquipMaintainProjectId = projectDetail.Id;

            //设备点检项目赋值点检计划项
            maintainProject.ProjectName = projectDetail.ProjectName;
            maintainProject.Part = projectDetail.Part;
            maintainProject.ProjectConsumable = projectDetail.Consumable;
            maintainProject.Method = projectDetail.Method;
            maintainProject.Standard = projectDetail.Standard;
            maintainProject.MinValue = projectDetail.MinValue;
            maintainProject.MaxValue = projectDetail.MaxValue;
            maintainProject.Unit = projectDetail.Unit;
            maintainProject.UseTime = projectDetail.UseTime;
            maintainProject.ProjectType = projectDetail.ProjectType;
            maintainProject.CycleType = projectDetail.CycleType;

            return maintainProject;
        }


        #endregion

        #region 保养项目 
        /// <summary>
        /// 获取保养计划列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>保养计划列表</returns>
        public virtual EntityList<MaintainProject> GetMaintainProjects(Expression<Func<MaintainProject, bool>> exp)
        {
            var query = Query<MaintainProject>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据保养单ID获取保养项目列表
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <returns></returns>
        public virtual EntityList<MaintainProject> GetMaintainProjects(double maintainPlanId)
        {
            var query = Query<MaintainProject>();
            query.Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainProject.EquipMaintainProjectProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取保养计划列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="orderby">排序条件</param>
        /// <returns>保养计划列表</returns>
        public virtual MaintainProject GetMaintainProject(Expression<Func<MaintainProject, bool>> exp, Expression<Func<MaintainProject, object>> orderby)
        {
            var query = Query<MaintainProject>();
            if (exp != null)
                query.Where(exp);
            return query.OrderBy(orderby).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据保养计划查询实体和设备台账Id列表获取保养项目视图列表
        /// </summary>
        /// <param name="criteria">保养计划查询实体</param>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>保养项目视图列表</returns>
        public virtual EntityList<MaintainProjectView> GetMaintainPlanProjectByCriteria(MaintainPlanCriteria criteria, List<double> accountIds)
        {
            return accountIds.SplitContains((tempIds) =>
            {
                var q = Query<MaintainProjectView>().Where(w => tempIds.Contains(w.EquipAccountId));
                if (criteria.Year.HasValue)
                    q.Where(w => w.BeginDate >= DateTime.Parse(criteria.Year.Value.Year + "/1/1") && w.BeginDate < DateTime.Parse(criteria.Year.Value.AddYears(1).Year + "/1/1"));
                return q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据台账获取保养计划
        /// </summary>
        /// <param name="equipAccountId">台账ID</param>
        /// <returns>保养计划</returns>
        public virtual EntityList<MaintainPlan> GetMtPlanByAccount(double equipAccountId)
        {
            var now = DateTime.Now;
            var list = Query<MaintainPlan>().Where(w => w.EquipAccountId == equipAccountId && w.PlanBeginDate >= new DateTime(now.Year, now.Month, 1) && w.ExeState != MaintExeState.Performed).OrderBy(o => o.PlanBeginDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取到期的设备保养单
        /// </summary>
        /// <param name="enterpriseId">车间Id</param>
        /// <param name="processId">工序Id</param>
        /// <returns>到期的设备保养单</returns>
        public virtual EntityList<MaintainPlan> GetTimeOnMaintainPlanList(double enterpriseId, double processId)
        {
            var q = Query<MaintainPlan>().Where(p => p.EquipAccount.WorkShopId == enterpriseId && p.EquipAccount.ProcessId == processId && p.PlanBeginDate <= DateTime.Now.Date && p.PlanEndDate >= DateTime.Now.Date && p.ExeState == MaintExeState.NotPerformed);
            return q.ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取超时的设备保养单
        /// </summary>
        /// <param name="enterpriseId">车间Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>超时的设备保养单</returns>
        public virtual EntityList<MaintainPlan> GetTimeOutMaintainPlanList(double enterpriseId, double processId, int timeOut)
        {
            var q = Query<MaintainPlan>().Where(p => p.EquipAccount.WorkShopId == enterpriseId && p.EquipAccount.ProcessId == processId && p.PlanBeginDate <= DateTime.Now.Date && p.PlanEndDate >= DateTime.Now.Date && p.ExeState == MaintExeState.NotPerformed && p.PlanEndDate <= DateTime.Now.AddDays(timeOut).Date);
            return q.ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取超时的保养计划单
        /// </summary>
        /// <returns>超时的保养计划单</returns>
        public virtual EntityList<MaintainPlan> GetTimeOutMaintainPlanList()
        {
            //是否精确计划保养
            bool isPrecisePlan = IsMaintainForPrecisePlan();
            //获取超时预警时间（小时）
            int timeOut = (int)RT.Service.Resolve<MaintainController>().GetMaintainExpiredTime();

            var date = DateTime.Now.AddHours(-timeOut);
            var date1 = date.AddHours(-timeOut);

            if (!isPrecisePlan)
            {
                return Query<MaintainPlan>().Where(p => p.PlanEndDate <= date && p.PlanEndDate > date1 && (p.ExeState == MaintExeState.NotPerformed || p.ExeState == MaintExeState.Performing))
                    .ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return Query<MaintainPlan>().Where(p => p.PrecisePlanEndDate <= date && p.PrecisePlanEndDate > date1 && (p.ExeState == MaintExeState.NotPerformed || p.ExeState == MaintExeState.Performing))
                    .ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 根据保养计划ID列表获取保养项目列表
        /// </summary>
        /// <param name="maintainPlanIds">保养计划ID列表</param>
        /// <returns>保养项目列表</returns>
        public virtual EntityList<MaintainProject> GetMaintainProjectsByPlanIds(List<double> maintainPlanIds)
        {
            return Query<MaintainProject>().Where(p => maintainPlanIds.Contains(p.MaintainPlanId)).ToList();
        }

        /// <summary>
        /// 获取设备台账保养明细,查询30天的未保养未超期
        /// 计划开始跟计划结束时间在当前时间内的
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>保养计划</returns>
        public virtual EntityList<MaintainPlan> GetMaintainPlans(double equipId)
        {
            var now = RF.Find<MaintainPlan>().GetDbTime();
            return Query<MaintainPlan>()
                .Where(w => w.EquipAccountId == equipId && w.ExeState != MaintExeState.Performed && w.PlanBeginDate <= now && w.PlanEndDate >= now).OrderBy(o => o.PlanBeginDate)
                .ToList();
        }

        /// <summary>
        /// 获取当前登陆人的责任部门清单
        /// </summary>
        /// <returns></returns>
        private List<DeviceDepa> GetDevUserDepts()
        {
            var query = Query<DeviceDepa>().As("dd").Join<DevicePur>("dp", (dd, dp) => dd.DevicePurId == dp.Id)
                .LeftJoin<DevicePur, UserInUserGroup>("uug", (dp, uug) => dp.UserGroupId == uug.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((dd, dp, uug) => dp.UserId == RT.Identity.UserId || uug.UserId == RT.Identity.UserId)
                .ToList();
            return query.ToList();
        }

        /// <summary>
        /// 获取当前登录人是否有保养确认权限
        /// </summary>
        /// <returns></returns>
        private List<DevicePur> GetMaintainConfirmUser()
        {
            var query = Query<DevicePur>().As("dp").LeftJoin<UserInUserGroup>("uug", (dp, uug) => dp.UserGroupId == uug.UserGroupId)
                .Where<UserInUserGroup>((dp, uug) => (dp.UserId == RT.Identity.UserId || uug.UserId == RT.Identity.UserId))
                .ToList();
            return query.ToList();
        }

        /// <summary>
        /// 获取时间范围内指定的保养计划
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlan> GetMatintainPlanList(double equipId, DateTime beginDate, DateTime endDate)
        {
            //保养查询器
            var queryer = DB.Query<MaintainPlan>();

            //匹配计划日期
            var nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            queryer.Where(p => p.PlanBeginDate >= beginDate && p.PlanBeginDate <= endDate && p.PlanBeginDate <= nowDate);

            //匹配设备Id
            queryer.Where(p => p.EquipAccountId == equipId);


            //贪懒加载评分项
            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainPlan.MaintainConfirmationListProperty);
            elo.LoadWithViewProperty();

            //保养单列表
            var queryerList = queryer.ToList(null, elo);
            return queryerList;
        }

        /// <summary>
        /// 根据保养计划Ids获取保养确认项
        /// </summary>
        /// <param name="mainIds">保养计划Ids</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanConfirmItem> GetMaintainConfirmations(List<double> mainIds)
        {
            return mainIds.SplitContains(tempIds =>
            {
                return Query<MaintainPlanConfirmItem>().Where(p => tempIds.Contains(p.MaintainPlanId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 生成执行数据
        /// </summary>
        /// <param name="maintainPlanInfos">保养信息</param>
        /// <param name="plan">保养计划</param>
        private void GetExecuteInfo(List<MaintainPlanInfos> maintainPlanInfos, MaintainPlan plan)
        {
            maintainPlanInfos.Add(new MaintainPlanInfos()
            {
                Id = plan.Id,                                      //保养计划id
                PlanBeginDate = plan.PlanBeginDate.ToString(),     //计划执行时间
                PlanEndDate = plan.PlanEndDate.ToString(),         //计划结束时间
                No = plan.MaintainNo,                              //保养单号
                Qty = plan.ProjectList.Count(),                    //项目数量
                EquipId = plan.EquipAccountId,                     //设备ID 
                EquipCode = plan.EquipAccountCode,                //设备编码
                EquipName = plan.EquipAccountName,                //设备名称
                DepartmentId = plan.DepartmentId,                //部门ID
                DepartmentCode = plan.DepartmentCode,            //部门编码
                DepartmentName = plan.DepartmentName,            //部门名称
                State = (int)plan.ExeState,
                StateName = plan.ExeState.ToLabel(),
                Shop = plan.WorkShopName,
                Line = plan.ResourceName,
                MaintainTime = plan.MaintainTime,
                EquipTypeId = plan.EquipTypeId,
                EquipTypeCode = plan.EquipTypeCode,
                EquipTypeName = plan.EquipTypeName,
                EquipModelId = plan.EquipModelId,
                EquipModelCode = plan.EquipModelCode,
                EquipModelName = plan.EquipModelName,
                MaintainSummary = plan.MaintainSummary
            });
        }

        /// <summary>
        /// 生成保养确认数据
        /// </summary>
        /// <param name="maintainPlanInfos">保养信息</param>
        /// <param name="plan">保养计划</param>
        /// <param name="maintainConfirmations">保养确认项</param>
        /// <param name="nowDevpurs">设备与人员权限</param>
        /// <param name="nowDepts">责任部门</param>
        private void GetConfirmInfo(List<MaintainPlanInfos> maintainPlanInfos, MaintainPlan plan, EntityList<MaintainPlanConfirmItem> maintainConfirmations, List<DevicePur> nowDevpurs
            , List<DeviceDepa> nowDepts)
        {
            foreach (var item in maintainConfirmations)
            {
                // 责任部门
                var deptPurIds = nowDepts.Where(p => p.EnterpriseId == item.DepartmentId).Select(p => p.DevicePurId).ToList();
                if (!deptPurIds.Any())
                {
                    continue;
                }
                // 是否保养确认人
                var isConfirm = nowDevpurs.Where(p => deptPurIds.Contains(p.Id)).Any(p => p.MaintainConfirm);
                maintainPlanInfos.Add(new MaintainPlanInfos()
                {
                    Id = plan.Id,                                      //保养计划id
                    PlanBeginDate = plan.PlanBeginDate.ToString(),     //计划执行时间
                    PlanEndDate = plan.PlanEndDate.ToString(),         //计划结束时间
                    No = plan.MaintainNo,                              //保养单号
                    Qty = plan.ProjectList.Count(),                    //项目数量
                    EquipId = plan.EquipAccountId,                     //设备ID 
                    EquipCode = plan.EquipAccountCode,                //设备编码
                    EquipName = plan.EquipAccountName,                //设备名称
                    DepartmentId = item.DepartmentId,                     //部门ID
                    DepartmentCode = item.DeptCode,                 //部门编码
                    DepartmentName = item.DeptName,                 //部门名称
                    State = (int)item.MaintExeState,
                    StateName = item.MaintExeState.ToLabel().L10N(),
                    Shop = plan.WorkShopName,
                    Line = plan.ResourceName,
                    MaintainTime = plan.MaintainTime,
                    EquipTypeId = plan.EquipTypeId,
                    EquipTypeCode = plan.EquipTypeCode,
                    EquipTypeName = plan.EquipTypeName,
                    EquipModelId = plan.EquipModelId,
                    EquipModelCode = plan.EquipModelCode,
                    EquipModelName = plan.EquipModelName,
                    MaintainSummary = plan.MaintainSummary,
                    MaintainConfirm = isConfirm,
                });
            }
        }

        /// <summary>
        /// 获取保养执行信息
        /// </summary>
        /// <param name="maintainPlanId">保养单Id</param>
        /// <returns></returns>
        public virtual List<MaintainPlanInfos> GetMaintainPlansDisplay(double maintainPlanId)
        {
            var list = new List<MaintainPlanInfos>();

            // 保养计划
            var maintainPlan = RF.GetById<MaintainPlan>(maintainPlanId);
            if (maintainPlan.PlanBeginDate > DateTime.Now)
            {
                throw new ValidationException("保养计划未到执行时间".L10N());
            }

            // 当前登陆人权限责任部门
            var nowDepts = GetDevUserDepts();

            // 当前登陆人权限
            var nowDevpurs = GetMaintainConfirmUser();

            // 保养执行数据
            if (maintainPlan.ExeState != MaintExeState.NotConfirm && maintainPlan.ExeState != MaintExeState.Scored && maintainPlan.ExeState != MaintExeState.Confirmed)
            {
                // 当前登陆人要有责任部门权限
                if (maintainPlan.DepartmentId == null || (maintainPlan.DepartmentId != null && nowDepts.FirstOrDefault(p => p.EnterpriseId == maintainPlan.DepartmentId) != null))
                {
                    GetExecuteInfo(list, maintainPlan);
                }
            }
            else // 保养确认数据
            {
                // 保养确认项
                var checkConfirmItems = GetMaintainConfirmations(new List<double> { maintainPlanId });

                GetConfirmInfo(list, maintainPlan, checkConfirmItems, nowDevpurs, nowDepts);
            }

            return list;
        }

        /// <summary>
        /// 根据条件获取保养计划
        /// </summary>
        /// <param name="equipId">设备Id</param>
        /// <param name="beginDate">计划开始时间</param>
        /// <param name="endDate">计划结束时间</param>
        /// <returns></returns>
        public virtual IEnumerable<MaintainPlanInfos> GetMaintainPlans(double equipId, DateTime beginDate, DateTime endDate)
        {
            // 保养信息
            var list = new List<MaintainPlanInfos>();

            // 保养计划
            var maintainList = GetMatintainPlanList(equipId, beginDate, endDate);
            var maintainIds = maintainList.Select(p => p.Id).ToList();
            if (maintainIds.Count <= 0)
            {
                throw new ValidationException("当前日期不存在保养单或保养单没到执行时间".L10N());
            }

            // 当前登陆人权限责任部门
            var nowDepts = GetDevUserDepts();

            // 当前登陆人权限
            var nowDevpurs = GetMaintainConfirmUser();

            // 保养确认信息
            var mainConfirmItems = GetMaintainConfirmations(maintainIds);

            //点检单分类
            foreach (var plan in maintainList)
            {
                if (plan.ExeState != MaintExeState.NotConfirm && plan.ExeState != MaintExeState.Scored && plan.ExeState != MaintExeState.Confirmed) // 保养执行数据
                {
                    if (plan.DepartmentId != null && nowDepts.FirstOrDefault(p => p.EnterpriseId == plan.DepartmentId) == null) continue;
                    GetExecuteInfo(list, plan);
                }
                else // 保养确认数据
                {
                    var planConfirmItems = mainConfirmItems.Where(p => p.MaintainPlanId == plan.Id).AsEntityList();
                    GetConfirmInfo(list, plan, planConfirmItems, nowDevpurs, nowDepts);
                }
            }
            return list;
        }


        /// <summary>
        /// 判断设备是否存在多条保养项目
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <param name="plan">保养计划</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistMaintainPlan(double equipId, MaintainPlan plan)
        {
            var now = RF.Find<MaintainPlan>().GetDbTime();
            return Query<MaintainPlan>().Where(p => p.EquipAccountId == equipId && p.Id != plan.Id && p.ExeState != MaintExeState.Performed && p.PlanEndDate >= now.Date && p.PlanBeginDate < plan.PlanBeginDate).Count() > 0;
        }

        /// <summary>
        /// 获取对应周期的保养项目
        /// </summary>
        /// <param name="type">周期类型</param>
        /// <param name="maintainProjects">保养项目</param>
        /// <returns></returns>
        private List<EquipAccountMaintainProject> GetCycleTypeProject(MaintainType type, EntityList<EquipAccountMaintainProject> maintainProjects)
        {
            //根据保养执行不同保养类型保养项目显示不同列表
            switch (type)
            {
                case MaintainType.Week:
                    return maintainProjects.Where(p => p.CycleType == CycleType.Week).ToList();
                case MaintainType.DbWeek:
                    return maintainProjects.Where(p => p.CycleType == CycleType.Week
                    || p.CycleType == CycleType.DoubleWeek).ToList();
                case MaintainType.Month:
                    return maintainProjects.Where(p => p.CycleType == CycleType.Week
                    || p.CycleType == CycleType.DoubleWeek
                    || p.CycleType == CycleType.Month).ToList();
                case MaintainType.DbMonth:
                    return maintainProjects.Where(p => p.CycleType == CycleType.Week
                    || p.CycleType == CycleType.DoubleWeek
                    || p.CycleType == CycleType.Month
                    || p.CycleType == CycleType.DoubleMonth).ToList();
                case MaintainType.Season:
                    return maintainProjects.Where(p => p.CycleType == CycleType.Week
                    || p.CycleType == CycleType.DoubleWeek
                    || p.CycleType == CycleType.Month
                    || p.CycleType == CycleType.DoubleMonth
                    || p.CycleType == CycleType.Season).ToList();
                case MaintainType.HalfYear:
                    return maintainProjects.Where(p => p.CycleType == CycleType.Week
                    || p.CycleType == CycleType.DoubleWeek
                    || p.CycleType == CycleType.Month
                    || p.CycleType == CycleType.DoubleMonth
                    || p.CycleType == CycleType.Season
                    || p.CycleType == CycleType.HalfYear).ToList();
                case MaintainType.Year:
                    return maintainProjects.ToList();
                default:
                    return new List<EquipAccountMaintainProject>();
            }
        }

        /// <summary>
        /// 生成保养计划保养项目
        /// </summary>
        /// <param name="maintainPlan"></param>
        public virtual void GeneratePlanProject(MaintainPlan maintainPlan)
        {
            if (maintainPlan == null || maintainPlan.ExeState != MaintExeState.NotPerformed)
                return;

            //是否带出设备子项的保养项目
            var config = ConfigService.GetConfig(new MaintainChildProjectConfig(), typeof(MaintainPlanViewModel));
            bool isChildProject = config.IsBringChildMaintainProject == YesNo.Yes;

            //获取设备及子设备ID
            var equipIds = new List<double>();
            if (isChildProject)
            {
                RT.Service.Resolve<Equipments.EquipController>().GetEquipAccountTreeUnderIds(maintainPlan.EquipAccountId, equipIds);
            }
            else
            {
                equipIds.Add(maintainPlan.EquipAccountId);
            }

            if (equipIds.Count <= 0)
            {
                throw new ValidationException("数据异常，设备不能为空".L10N());
            }

            // 获取设备保养项目
            var maintainProjects = equipIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountMaintainProject>()
                        .Where(p => p.ProjectType == ProjectType.Maintain)
                        .Where(p => tempIds.Contains(p.EquipAccountId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 获取对应保养周期的保养项目
            var typePrjects = GetCycleTypeProject(maintainPlan.MaintainType, maintainProjects);
            // 是否按部门生成
            if (IsDepartmentMaintain())
            {
                typePrjects = typePrjects.Where(p => p.DepartmentId == null || p.DepartmentId == maintainPlan.DepartmentId).ToList();
            }
            // 排序
            var equipMaintainProjects = typePrjects.OrderBy(p => p.MinValue).OrderBy(p => p.MaxValue).ToList();

            // 旧保养项目
            EntityList<MaintainProject> oldProjects = Query<MaintainProject>().Where(p => p.MaintainPlanId == maintainPlan.Id).ToList();
            oldProjects.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
            });

            // 新生成保养项目
            EntityList<MaintainProject> newProjects = new EntityList<MaintainProject>();
            //生成保养项目
            foreach (var equipMaintainProject in equipMaintainProjects)
            {
                newProjects.Add(GenerateMaintainProject(maintainPlan, equipMaintainProject));
            }

            if (newProjects.Count <= 0)
            {
                throw new ValidationException("该设备不存在[{0}]保养项目，请联系设备管理员！"
                    .L10nFormat(maintainPlan.MaintainType.ToLabel().L10N()));
            }
            //未生成保养项目时，才生成
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(oldProjects);
                RF.BatchInsert(newProjects);
                trans.Complete();
            }
            

        }

        #endregion

        #region 保养记录
        /// <summary>
        /// 获取保养计划主表列信息
        /// </summary>
        /// <returns>保养计划主表列信息</returns>
        public virtual List<CheckPlanColumn> GetMaintainPlanColumns(DateTime dateTime)
        {
            const int weeks = 52;

            List<CheckPlanColumn> columnList = new List<CheckPlanColumn>();

            int year = dateTime.Year;
            DateTime startDateOfYear = new DateTime(year, 1, 1);


            var weekInfo = GetWeekInfoOfDate(startDateOfYear);
            columnList.Add(new CheckPlanColumn()
            {
                DayNum = weekInfo.Item1,
                BeginTime = DateTimeFormat.ToShortFormat1(weekInfo.Item2),
                EndTime = DateTimeFormat.ToShortFormat1(weekInfo.Item3)
            });

            for (int i = 1; i <= weeks; i++)
            {
                var dateTimeTemp = startDateOfYear.AddDays(7 * i);

                var weekInfoTemp = GetWeekInfoOfDate(dateTimeTemp);

                columnList.Add(new CheckPlanColumn()
                {
                    DayNum = weekInfoTemp.Item1,
                    BeginTime = DateTimeFormat.ToShortFormat1(weekInfoTemp.Item2),
                    EndTime = DateTimeFormat.ToShortFormat1(weekInfoTemp.Item3)
                });
            }

            return columnList;
        }


        /// <summary>
        /// 获取周信息
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static Tuple<int, DateTime, DateTime> GetWeekInfoOfDate(DateTime dateTime)
        {
            int year = dateTime.Year;
            DateTime startDateOfYear = new DateTime(year, 1, 1);
            int dayOfWeek = (int)startDateOfYear.DayOfWeek;

            if (dayOfWeek == 0)
            {
                //星期一算第一天（1），星期天算最后天（7）
                dayOfWeek = 7;
            }

            //2022年1月1日 星期六（6） 当周最后一天 2022年1月1日+（7-6）天
            DateTime endDateOfFirstWeek = startDateOfYear.AddDays(7 - dayOfWeek);
            DateTime firstDayOfSecondWeek = endDateOfFirstWeek.AddDays(1);

            if (dateTime < firstDayOfSecondWeek)
            {
                var firtDayOfWeek = endDateOfFirstWeek.AddDays(-6);
                var lastDayOfWeek = endDateOfFirstWeek;
                return new Tuple<int, DateTime, DateTime>(1, firtDayOfWeek, lastDayOfWeek);
            }
            else
            {
                var dayOfYearOfSecondWeekStart = firstDayOfSecondWeek.DayOfYear;
                var dayOfYear = dateTime.DayOfYear;

                var week = (int)(Math.Floor((decimal)(dayOfYear - dayOfYearOfSecondWeekStart) / 7) + 2);

                var firtDayOfWeek = firstDayOfSecondWeek.AddDays(7 * (week - 2));
                var lastDayOfWeek = firtDayOfWeek.AddDays(6);

                return new Tuple<int, DateTime, DateTime>(week, firtDayOfWeek, lastDayOfWeek);
            }
        }


        /// <summary>
        /// 获取周信息
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns>年，周别，周开始日期，周结束日期</returns>
        public virtual Tuple<int, int, DateTime, DateTime> GetWeekInfoOfDateTime(DateTime dateTime)
        {
            int year = dateTime.Year;
            DateTime startDateOfYear = new DateTime(year, 1, 1);
            int dayOfWeek = (int)startDateOfYear.DayOfWeek;

            if (dayOfWeek == 0)
            {
                //星期一算第一天（1），星期天算最后天（7）
                dayOfWeek = 7;
            }

            DateTime endDateOfFirstWeek = startDateOfYear.AddDays(7 - dayOfWeek);
            DateTime firstDayOfSecondWeek = endDateOfFirstWeek.AddDays(1);

            if (dateTime < firstDayOfSecondWeek)
            {
                var firtDayOfWeek = endDateOfFirstWeek.AddDays(-6);
                var lastDayOfWeek = endDateOfFirstWeek;
                return new Tuple<int, int, DateTime, DateTime>(year, 1, firtDayOfWeek, lastDayOfWeek);
            }
            else
            {
                var dayOfYearOfSecondWeekStart = firstDayOfSecondWeek.DayOfYear;
                var dayOfYear = dateTime.DayOfYear;

                var week = (int)(Math.Floor((decimal)(dayOfYear - dayOfYearOfSecondWeekStart) / 7) + 2);

                var firtDayOfWeek = firstDayOfSecondWeek.AddDays(7 * (week - 2));
                var lastDayOfWeek = firtDayOfWeek.AddDays(6);

                //当年最后一周不满时，算成下一年的第一周
                if (lastDayOfWeek.Year > firtDayOfWeek.Year)
                {
                    return new Tuple<int, int, DateTime, DateTime>(lastDayOfWeek.Year, 1, firtDayOfWeek, lastDayOfWeek);
                }
                else
                {
                    return new Tuple<int, int, DateTime, DateTime>(year, week, firtDayOfWeek, lastDayOfWeek);
                }
            }
        }

        /// <summary>
        /// 获取保养计划信息（添加）
        /// </summary>        
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maintainCycleType"></param>
        /// <returns>保养计划信息</returns>
        public virtual List<MaintainPlanRecord> GetMaintainPlanRecords(DateTime beginDate, DateTime endDate, MaintainCycleType maintainCycleType)
        {
            bool isIntervalTime = IsIntervalTime();
            int? intervalTime = GetIntervalTime();

            List<MaintainPlanRecord> records = new List<MaintainPlanRecord>();
            if (maintainCycleType == MaintainCycleType.Week)
            {
                Dictionary<int, Dictionary<int, bool>> yearDictionary =
                    new Dictionary<int, Dictionary<int, bool>>();

                for (DateTime dateTime = beginDate; dateTime <= endDate; dateTime = dateTime.AddDays(1))
                {
                    //weekInfo的数据按顺序为： 年，第几周，周开始日期，周结束日期
                    var weekInfo = GetWeekInfoOfDateTime(dateTime);
                    var year = weekInfo.Item1;
                    var week = weekInfo.Item2;
                    var firstDayOfWeek = weekInfo.Item3;
                    var lastDayOfWeek = weekInfo.Item4;

                    if (!yearDictionary.ContainsKey(year))
                    {
                        yearDictionary.Add(year, new Dictionary<int, bool>());
                    }

                    var weekDictionary = yearDictionary[year];

                    if (weekDictionary.ContainsKey(week))
                    {
                        continue;
                    }

                    //计算本周已经生成过
                    weekDictionary.Add(week, true);

                    records.Add(new MaintainPlanRecord()
                    {
                        YearAndMonth = new DateTime(lastDayOfWeek.Year, lastDayOfWeek.Month, 1),
                        Cycle = week,
                        PlanBeginDate = firstDayOfWeek,
                        PlanEndDate = lastDayOfWeek,
                        MaintainType = MaintainType.Week,
                        IntervalTime = isIntervalTime ? intervalTime : 0,
                        MaintainCycleType = maintainCycleType,
                        MaintainTypeInfoId = ((int)MaintainType.Week),
                        MaintainTypeInfoValue = MaintainType.Week.ToLabel(),
                        WhetherRepair = YesNo.No,
                    });
                }
            }
            else
            {
                Dictionary<int, Dictionary<int, bool>> yearDictionary =
                    new Dictionary<int, Dictionary<int, bool>>();

                for (DateTime dateTime = beginDate; dateTime <= endDate; dateTime = dateTime.AddDays(1))
                {
                    var year = dateTime.Year;

                    if (!yearDictionary.ContainsKey(year))
                    {
                        yearDictionary.Add(year, new Dictionary<int, bool>());
                    }

                    var monthDictionary = yearDictionary[year];

                    var month = dateTime.Month;

                    if (monthDictionary.ContainsKey(month))
                    {
                        continue;
                    }

                    //计算本月已经生成过
                    monthDictionary.Add(month, true);

                    var firstDateOfMonth = new DateTime(year, month, 1);
                    var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
                    var weekInfo = GetWeekInfoOfDateTime(firstDateOfMonth);

                    records.Add(new MaintainPlanRecord()
                    {
                        Cycle = weekInfo.Item2,
                        YearAndMonth = firstDateOfMonth,
                        PlanBeginDate = firstDateOfMonth,
                        PlanEndDate = lastDateOfMonth,
                        MaintainType = MaintainType.Month,
                        IntervalTime = isIntervalTime ? intervalTime : 0,
                        MaintainCycleType = maintainCycleType,
                        MaintainTypeInfoId = ((int)MaintainType.Month),
                        MaintainTypeInfoValue = MaintainType.Month.ToLabel(),
                        WhetherRepair = YesNo.No,

                    });
                }
            }

            return records;
        }

        /// <summary>
        /// 获取保养计划信息（修改）
        /// </summary>
        /// <returns>保养计划信息</returns>
        public virtual List<MaintainPlanRecord> GetEditMaintainPlanRecords(string equipAccountCode)
        {
            bool isIntervalTime = IsIntervalTime();
            int? intervalTime = GetIntervalTime();
            EquipAccount account = RT.Service.Resolve<Equipments.EquipController>().GetEquipAccountsByCode(equipAccountCode);
            var planList = Query<MaintainPlan>()
                .Where(p => p.EquipAccountId == account.Id)
                .Where(p => p.PlanEndDate >= DateTime.Now)
                .Where(p => p.ExeState == MaintExeState.NotPerformed).ToList();

            int i = 0;
            List<MaintainPlanRecord> records = new List<MaintainPlanRecord>();
            planList.ForEach(plan =>
            {
                MaintainPlanRecord record = new MaintainPlanRecord();
                record.Id = plan.Id;
                record.EquipAccountId = account.Id;
                record.YearAndMonth = plan.YearAndMonth;
                record.Cycle = plan.Cycle;
                record.PlanBeginDate = plan.PlanBeginDate;
                record.PlanEndDate = plan.PlanEndDate;
                record.MaintainType = plan.MaintainType;
                record.PrecisePlanBeginDate = plan.PrecisePlanBeginDate;
                record.PrecisePlanEndDate = plan.PrecisePlanEndDate;
                record.MaintainTime = plan.MaintainTime;
                record.IntervalTime = isIntervalTime ? intervalTime : 0;
                records.Add(record);
                i++;
            });

            return records;
        }
        /// <summary>
        /// 查询设备保养记录
        /// </summary>
        /// <param name="criteria">设备保养记录查询实体</param>
        /// <returns>设备保养记录</returns>
        public virtual EntityList QueryMaintainPlanLog(MaintainRecordCriteria criteria)
        {
            var dateNow = RF.Find<MaintainRecord>().GetDbTime();

            var q = Query<MaintainRecord>();

            if (!criteria.MaintainNo.IsNullOrWhiteSpace())
            {
                q.Where(p => p.MaintainNo.Contains(criteria.MaintainNo));
            }

            if (criteria.EquipAccountId.HasValue)
            {
                q.Where(w => w.EquipAccountId == criteria.EquipAccountId.Value);
            }

            if (criteria.WorkshopId.HasValue)
            {
                q.Where(w => w.EquipAccount.WorkShopId == criteria.WorkshopId);
            }

            if (!criteria.ResourceName.IsNullOrWhiteSpace())
            {
                q.Where(p => p.EquipAccount.Resource.Name.Contains(criteria.ResourceName));
            }

            if (criteria.ProcessId.HasValue)
            {
                q.Where(w => w.EquipAccount.ProcessId == criteria.ProcessId);
            }

            if (criteria.MachineName.IsNotEmpty())
            {
                q.Where(w => w.EquipAccount.Name.Contains(criteria.MachineName));
            }

            if (criteria.ExeState.HasValue)
            {
                if (criteria.ExeState == MaintExeState.Overdue || criteria.ExeState == MaintExeState.NotPerformed)
                {
                    //超期和未执行，在数据库的状态都是未执行
                    q.Where(w => w.ExeState == MaintExeState.NotPerformed);
                    if (criteria.ExeState == MaintExeState.Overdue)
                    {
                        //超期
                        q.Where(w => (w.PrecisePlanBeginDate != null && w.PrecisePlanBeginDate < dateNow)
                            || (w.PrecisePlanBeginDate == null && w.PlanEndDate < dateNow));
                    }
                    else
                    {
                        //未执行（没有超期）
                        q.Where(w => (w.PrecisePlanBeginDate != null && w.PrecisePlanBeginDate > dateNow)
                            || (w.PrecisePlanBeginDate == null && w.PlanEndDate > dateNow));
                    }
                }
                else
                {
                    //其他状态与数据库一致
                    q.Where(w => w.ExeState == criteria.ExeState);
                }
            }

            if (criteria.PlanMaintainDate.BeginValue.HasValue)
            {
                q.Where(p => p.PlanEndDate >= criteria.PlanMaintainDate.BeginValue);
            }

            if (criteria.PlanMaintainDate.EndValue.HasValue)
            {
                q.Where(p => p.PlanBeginDate <= criteria.PlanMaintainDate.EndValue);
            }

            if (criteria.ExeResult.HasValue)
            {
                q.Where(w => w.ExeResult == criteria.ExeResult.Value);
            }

            if (criteria.ConfirmResult.HasValue)
            {
                q.Where(w => w.ConfirmResult == criteria.ConfirmResult.Value);
            }

            q.OrderBy(p => p.PlanBeginDate).OrderBy(criteria.OrderInfoList);

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();


            var iquery = q.ToQuery().QueryWithEquipAccountPermissions(MaintainRecord.EquipAccountIdProperty.Name);
            var maintainRecords = q.Repository.QueryList(iquery, paging: criteria.PagingInfo, eagerLoad: elo);

            foreach (var i in maintainRecords)
            {
                var item = i as MaintainRecord;
                var precisePlanEndDate = item.PrecisePlanEndDate;
                if (!item.PrecisePlanEndDate.HasValue)
                {
                    precisePlanEndDate = item.PlanEndDate;
                }

                if (precisePlanEndDate < dateNow && item.ExeState == MaintExeState.NotPerformed)
                {
                    item.ExeState = MaintExeState.Overdue;
                }

                item.ExeStateName = item.ExeState.ToLabel().L10N();//为了拿到枚举的label
            }


            return maintainRecords;
        }

        /// <summary>
        /// 查询设备保养记录
        /// </summary>
        /// <param name="id">设备保养记录id</param>
        /// <returns>设备保养记录</returns>
        public virtual MaintainRecord QueryMaintainPlanLogById(double id)
        {
            var eagerLoadOptions = new EagerLoadOptions().LoadWithViewProperty();
            eagerLoadOptions.LoadWith(MaintainRecord.EquipAccountProperty);
            eagerLoadOptions.LoadWith(EquipAccount.WorkShopProperty);
            eagerLoadOptions.LoadWith(EquipAccount.ProcessProperty);
            eagerLoadOptions.LoadWith(MaintainRecord.ProjectListProperty);
            eagerLoadOptions.LoadWith(MaintainProject.EquipMaintainProjectProperty);
            return Query<MaintainRecord>().Where(p => p.Id == id).FirstOrDefault(eagerLoadOptions);
        }

        /// <summary>
        /// 根据日期获取保养计划明细
        /// </summary>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <param name="maxDate">最大时间</param>
        /// <param name="minDate">最小时间</param>
        /// <returns>保养计划明细</returns>
        public virtual EntityList<MaintainPlan> GetMaintainPlanByPlanBeginDate(double equipAccountId, DateTime? maxDate, DateTime? minDate)
        {
            return Query<MaintainPlan>().Where(w => w.EquipAccountId == equipAccountId && w.PlanBeginDate <= maxDate && w.PlanEndDate >= minDate).ToList();
        }

        /// <summary>
        /// 设备保养记录-进行评分
        /// </summary>
        /// <param name="maintainScores">保养评分列表</param>
        public virtual void MaintainPlanScore(EntityList<MaintainScore> maintainScores)
        {
            //var plan = RF.GetById<MaintainPlan>(maintainScores[0].MaintainPlanId);
            //plan.ExeState = ExeState.Scored;
            //plan.Score = 0;
            //foreach (var item in maintainScores)
            //{
            //    item.PersistenceStatus = PersistenceStatus.New;
            //    plan.Score += item.Score * double.Parse(item.Rate.ToString()) / 100;
            //}
            //plan.ScoreById = AppRuntime.IdentityId;
            //plan.ScoreDate = DateTime.Now;
            //using (var trans = DB.TransactionScope(EMS.EmsEntityDataProvider.ConnectionStringName))
            //{
            //    RF.Save(plan);
            //    RF.Save(maintainScores);
            //    trans.Complete();
            //}
        }
        #endregion

        #region 备件更换

        /// <summary>
        /// 根据保养单ID和申请标记获取备件申请项目列表
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <param name="isApply">是否申请</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePartApl> GetMaintainPlanSparePartApls(double maintainPlanId, bool isApply)
        {
            var query = Query<MaintainPlanSparePartApl>();
            query.Where(p => p.MaintainPlanId == maintainPlanId);
            query.Where(p => p.IsApply == isApply);

            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainPlanSparePartApl.SparePartProperty);

            return query.ToList(null, elo);
        }


        /// <summary>
        /// 根据保养单ID获取备件申请项目列表
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePartApl> GetMaintainPlanSparePartApls(double maintainPlanId)
        {
            var query = Query<MaintainPlanSparePartApl>();
            query.Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainPlanSparePartApl.SparePartProperty);

            var list = query.ToList(null, elo);

            if (list.Count > 0)
            {
                var partIds = list.Select(p => p.SparePartId).ToList();
                var storeSummaryDepotList = RT.Service.Resolve<SparePartController>().GetStoreSummaryDepots(partIds);
                list.ForEach(p =>
                {
                    //获取库存明细
                    p.StoreQty = storeSummaryDepotList.FirstOrDefault(x => x.WarehouseId == p.OutStockWarehouseId && x.SparePartId == p.SparePartId)?.StoreQty ?? 0;
                });

            }

            return list;
        }

        /// <summary>
        /// 获取保养计划备件更换列表
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <param name="equipId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePartApl> GetMaintainPlanSparePartApls(double maintainPlanId, double equipId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainPlanSparePartApl>();
            q.Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainPlanSparePartApl.ApplyDetailProperty);
            elo.LoadWith(ApplyDetail.SparePartAppProperty);
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值备件库存
            if (list.Count > 0)
            {
                var partIds = list.Select(p => p.SparePartId).ToList();
                var whIds = list.Where(p => p.OutStockWarehouseId != null).Select(p => (double)p.OutStockWarehouseId).ToList();
                var whInfos = RT.Service.Resolve<SparePartController>().GetStoreSummaryDepots(partIds, whIds);
                list.ForEach(p =>
                {
                    //赋值库存
                    var whInfo = whInfos.FirstOrDefault(x => x.SparePartId == p.SparePartId && x.WarehouseId == p.OutStockWarehouseId);
                    p.StoreQty = whInfo?.StoreQty ?? 0;
                });
            }
            list.MarkSaved();
            return list;
        }
        /// <summary>
        /// 根据保养单ID和状态获取备件更换项目列表
        /// </summary>
        /// <param name="maintainPlanId">点检计划单ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePart> GetMaintainPlanSpareParts(double maintainPlanId, ChangeSparePartState state)
        {
            var query = Query<MaintainPlanSparePart>();
            query.Where(p => p.MaintainPlanId == maintainPlanId);
            query.Where(p => p.State == state);

            var elo = new EagerLoadOptions();

            elo.LoadWith(MaintainPlanSparePart.SparePartProperty);
            elo.LoadWith(MaintainPlanSparePart.SparePartProperty);
            elo.LoadWith(MaintainPlanSparePart.MaintainPlanProperty);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据保养单ID获取备件更换项目列表
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePart> GetMaintainPlanSpareParts(double maintainPlanId)
        {
            var query = Query<MaintainPlanSparePart>();
            query.Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainPlanSparePart.SparePartProperty);

            var list = query.ToList(null, elo);

            return list;
        }

        /// <summary>
        /// 获取保养计划备件更换列表
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <param name="maintainNo">保养计划单号</param>
        /// <param name="equipId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePart> GetMaintainPlanSpareParts(double maintainPlanId, string maintainNo, double equipId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainPlanSparePart>();
            q.Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(MaintainPlanSparePart.PartOutDepotDetailProperty);
            elo.LoadWith(ApplyDetail.SparePartAppProperty);
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值申请单明细
            if (list.Count > 0)
            {
                var outDtls = RT.Service.Resolve<OutDepotController>().GetPartOutDepotDetailDtl(maintainNo, SpareParts.OutDepots.Enums.OutDepotType.Maintain);
                list.ForEach(p =>
                {
                    if (p.State == ChangeSparePartState.New)
                    {
                        //赋值UI属性
                        var outDtl = outDtls.FirstOrDefault(x => x.SparePartId == p.SparePartId);
                        if (outDtl != null)
                        {
                            p.PartOutDepotDetailId = outDtl.Id;
                            p.OutDepotNoView = outDtl.OutDepot.No;
                            p.SeriaNoView = outDtl.SeriaNoRef?.OrderNumberCode;
                            p.BatchNoView = outDtl.BatchNoRef?.BatchNumber;
                            p.RemainingQty = outDtl.OutDepotCount - outDtl.UseCount;
                        }
                    }
                    else
                    {
                        p.RemainingQty = p.PartOutDepotDetail.OutDepotCount - p.PartOutDepotDetail.UseCount;
                    }
                });
            }

            return list;
        }

        /// <summary>
        /// UI执行备件更换
        /// </summary>
        /// <param name="uiMaintainPlanSpareParts"></param>
        public virtual void UIChangeMaintainPlanSparePart(List<MaintainPlanSparePart> uiMaintainPlanSpareParts)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiMaintainPlanSpareParts.ForEach(p => RF.Save(p));

                //TODO校验申请单状态

                var maintainPlan = uiMaintainPlanSpareParts.FirstOrDefault().MaintainPlan;
                ChangeMaintainPlanSparePart(maintainPlan.Id);

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存保养更换数据
        /// </summary>
        /// <param name="info"></param>
        public virtual void SaveMaintainChangeInfo(MaintainSaveSubmitInfo info)
        {
            // 未更换的记录
            var changeRecord  = RT.Service.Resolve<MaintainController>().GetMaintainPlanSpareParts(info.MaintainPlanId, EMS.Enums.ChangeSparePartState.New);

            // pda更换数据
            var infoChangeList = info.SparePartDetails;
            
            foreach (var change in changeRecord)
            {
                var changeInfo = infoChangeList.FirstOrDefault(p => p.SparePartId == change.SparePartId);
                if (changeInfo == null)
                {
                    continue;
                }

                change.ChangeQty = changeInfo.ChangeQty;
                change.PartOutDepotDetailId = changeInfo.OutDtlId;
            }
            RF.Save(changeRecord);
        }

        /// <summary>
        /// 保存保养申请数据
        /// </summary>
        /// <param name="info"></param>
        public virtual void SaveMaintainApplyInfo(MaintainSaveSubmitInfo info)
        {
            // 未申请的记录
            var applyRecord = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartApls(info.MaintainPlanId, false);

            // pda申请数据
            var infoApplyList = info.SparePartAplDetails;

            foreach (var apply in applyRecord)
            {
                var applyInfo = infoApplyList.FirstOrDefault(p => p.SparePartId == apply.SparePartId);
                if (applyInfo == null)
                {
                    continue;
                }

                apply.ApplyQty = applyInfo.ApplyQty;
                apply.OutStockWarehouseId = applyInfo.OutStockWarehouseId;
            }
            RF.Save(applyRecord);
        }

        /// <summary>
        /// 执行备件更换逻辑
        /// </summary>
        /// <param name="maintainPlanId"></param>
        public virtual void ChangeMaintainPlanSparePart(double maintainPlanId)
        {
            try
            {
                var datas = RT.Service.Resolve<MaintainController>().GetMaintainPlanSpareParts(maintainPlanId, EMS.Enums.ChangeSparePartState.New);
                if (datas.Count <= 0) throw new ValidationException("没有备件更换数据".L10N());
                var list = datas.Where(p => p.PartOutDepotDetail != null).ToList();
                if (list.Count <= 0) throw new ValidationException("存在备件更换数据没有选择备件出库单".L10N());
                if (list.Any(p => p.ChangeQty <= 0)) throw new ValidationException("存在备件更换数据更换数量为0".L10N());

                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    //回写备件申请单使用数量
                    list.ForEach(p =>
                    {
                        if (p.PartOutDepotDetail.UseCount + p.ChangeQty > p.PartOutDepotDetail.OutDepotCount)
                            throw new ValidationException("备件[{0}]更换数量不能大于剩余数量".L10nFormat(p.SparePart.SparePartCode));

                        //回写申请单
                        DB.Update<PartOutDepotDetail>().Where(x => x.Id == p.PartOutDepotDetailId).Set(x => x.UseCount, x => x.UseCount + p.ChangeQty).Execute();
                        //修改备件更换状态
                        DB.Update<MaintainPlanSparePart>().Where(x => x.Id == p.Id).Set(x => x.State, Enums.ChangeSparePartState.Finished).Execute();
                        //修改序列号状态
                        DB.Update<StoreSummaryDetail>().Where(x => x.Id == p.PartOutDepotDetail.SeriaNoRefId).Set(x => x.StoreStatus, OrdNumStoreStatus.Using).Execute();
                        //插入备件履历
                        var record = new SparePartChangedRecord()
                        {
                            EquipAccountId = p.MaintainPlan.EquipAccountId,
                            Qty = p.ChangeQty,
                            OldSerialNumber = p.OldSequence,
                            BatchNumber = p.PartOutDepotDetail?.BatchNo,
                            SerialNumber = p.PartOutDepotDetail?.SeriaNo,
                            Source = FromType.Maintain,
                            SourceNo = p.MaintainPlan.MaintainNo,
                            SourceId = p.MaintainPlanId,
                            SparePartId = p.SparePartId
                        };
                        RF.Save(record);
                        trans.Complete();
                    });
                }
            }
            catch (Exception ex)
            {
                //清空未完成的更换单的出库单
                double? value = null;
                DB.Update<MaintainPlanSparePart>().Where(p => p.MaintainPlanId == maintainPlanId && p.State == ChangeSparePartState.New).Set(p => p.PartOutDepotDetailId, value).Execute();
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// 根据设备台账ID获取保养工时和备件成本
        /// </summary>
        /// <param name="equipAccountIds">设备台账ID集合</param>
        /// <returns>设备保养工时和备件成本</returns>
        public virtual List<WorkHourAndCostInfo> GetMaintainWorkHourAndCost(IList<double> equipAccountIds)
        {
            var nowDate = RF.Find<MaintainPlan>().GetDbTime();

            var mainPlanList = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<MaintainPlan>().Where(p => tempIds.Contains(p.EquipAccountId) && p.CreateDate >= nowDate.AddYears(-1)).ToList(null, new EagerLoadOptions().LoadWith(MaintainPlan.MaintainPlanSparePartListProperty));
            });

            var infolist = mainPlanList.GroupBy(p => p.EquipAccountId).Select(mainPlans => new WorkHourAndCostInfo
            {
                EquipAccountId = mainPlans.Key,
                MaintenanceHours = mainPlans.Sum(p => decimal.Parse(p.SumWorkHours.ToString()))
            }).ToList();

            var outDepotDetailList = mainPlanList.SelectMany(p => p.MaintainPlanSparePartList).Select(p => p.PartOutDepotDetailId).Distinct().SplitContains(tempIds =>
            {
                return Query<PartOutDepotDetail>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            foreach (var info in infolist)
            {
                var mainPlans = mainPlanList.Where(p => p.EquipAccountId == info.EquipAccountId);
                var sparePartChgList = mainPlans.SelectMany(p => p.MaintainPlanSparePartList).ToList();

                foreach (var sparePartChg in sparePartChgList)
                {
                    var outDepotDetail = outDepotDetailList.FirstOrDefault(p => p.Id == sparePartChg.PartOutDepotDetailId);

                    if (outDepotDetail != null)
                    {
                        info.SparePartCost += sparePartChg.ChangeQty * decimal.Parse(outDepotDetail.UnitPrice.ToString());
                    }
                }
            }

            return infolist;
        }


        #endregion

        #region 工时登记
        /// <summary>
        /// 添加一条工时登记
        /// </summary>
        /// <param name="entity">工时登记实体</param>
        public virtual void AddWorkHoursRegister(WorkHoursRegister entity)
        {
            if (IsMaintainWorkTime())
            {
                DB.Delete<WorkHoursRegister>().Where(p => p.MaintainPlanId == entity.MaintainPlanId).Execute();
                RF.Save(entity);
            }
            else
            {
                DB.Delete<WorkHoursRegister>().Where(p => p.MaintainPlanId == entity.MaintainPlanId).Execute();
            }
        }

        /// <summary>
        /// 获取保养工时登记
        /// </summary>
        /// <param name="maintainPlanId"></param>
        /// <returns></returns>
        public virtual EntityList<WorkHoursRegister> GetMaintainWorkHoursRegisters(double maintainPlanId)
        {
            var query = Query<WorkHoursRegister>();
            query.Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(WorkHoursRegister.ExecuteByProperty);

            var list = query.ToList(null, elo);
            return list;
        }

        #endregion

        #region 预警
        /// <summary>
        /// 获取提前预警的保养计划单
        /// </summary>
        /// <returns>提前预警的保养计划单</returns>
        public virtual EntityList<MaintainPlan> GetAlertTimeOutMaintainPlanList()
        {
            //是否精确计划保养
            bool isPrecisePlan = IsMaintainForPrecisePlan();
            //获取提前预警时间（小时）
            int alertTimeOut = (int)RT.Service.Resolve<MaintainController>().GetMaintainAlertTime();
            var date = DateTime.Now.AddHours(alertTimeOut);
            var date1 = DateTime.Now;
            if (!isPrecisePlan)
            {
                return Query<MaintainPlan>().Where(p => p.PlanBeginDate <= date && p.PlanBeginDate > date1 && p.ExeState == MaintExeState.NotPerformed).ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return Query<MaintainPlan>().Where(p => p.PrecisePlanBeginDate <= date && p.PrecisePlanBeginDate > date1 && p.ExeState == MaintExeState.NotPerformed).ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        #endregion


        #region 保养计划

        #endregion

        #region 保养执行

        /// <summary>
        /// 保养提交验证
        /// </summary>
        /// <param name="maintainPlan"></param>

        public virtual void CheckSubmitMaintainPlan(MaintainPlan maintainPlan)
        {
            //校验
            if (!ChackIsExeState(maintainPlan.Id))
            {
                throw new ValidationException("保养项目已在其他端操作，不允许提交。".L10N());
            }
            if (maintainPlan.ActBeginDate == null || maintainPlan.ActEndDate == null)
                throw new ValidationException("保养开始时间和结束时间不能为空，不允许提交。".L10N());
            if (maintainPlan.ActBeginDate > maintainPlan.ActEndDate)
                throw new ValidationException("保养结束时间不能比开始时间早，不允许提交。".L10N());
            if (maintainPlan.ProjectList.Count == 0)
                throw new ValidationException("不存在保养项目，不允许提交。".L10N());
            if (maintainPlan.ProjectList.Any(p => p.MaintainResult == null))
                throw new ValidationException("存在未完成的保养项目，不允许提交。".L10N());
            if (maintainPlan.ProjectList.Any(p => p.MaintainResult == CheckMaintainResult.NG && p.Defect.IsNullOrEmpty()))
                throw new ValidationException("不合格保养项目没有填写缺陷描述，不允许提交。".L10N());
            if (maintainPlan.MaintainPlanSparePartList.Count > 0 &&
                maintainPlan.MaintainPlanSparePartList.Any(p => p.State != ChangeSparePartState.Finished))
                throw new ValidationException("当前设备存在未更换完成的备件，不允许提交。".L10N());
            if (maintainPlan.WorkHoursRegisterList.Count <= 0 && this.IsMaintainWorkTime())
                throw new ValidationException("至少需要填写一笔工时登记数据，不允许提交。".L10N());
            if (maintainPlan.WorkHoursRegisterList.Any(p => p.EmployeeId == null))
                throw new ValidationException("工时登记执行人不能为空，不允许提交。".L10N());
        }

        /// <summary>
        /// 计算总工时
        /// </summary>
        /// <param name="maintainPlan">保养计划</param>
        /// <param name="isMaintainForWorkTime">是否</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private double CalculateWorkHours(MaintainPlan maintainPlan, YesNo isMaintainForWorkTime)
        {
            double workHours = 0;
            if (isMaintainForWorkTime == YesNo.Yes)
            {
                foreach (WorkHoursRegister workHoursRegister in maintainPlan.WorkHoursRegisterList)
                {
                    if (workHoursRegister.BeginDay > workHoursRegister.EndDay)
                        throw new ValidationException("工时登记保养结束时间不能比开始时间早，不允许提交。".L10N());

                    DateTime dt1 = Convert.ToDateTime(workHoursRegister.EndDay);
                    DateTime dt2 = Convert.ToDateTime(workHoursRegister.BeginDay);
                    TimeSpan ts1 = dt1.Subtract(dt2);
                    workHoursRegister.WorkHours = Math.Round(ts1.TotalHours, 2);
                    workHours += workHoursRegister.WorkHours;
                }
            }
            else
            {
                if (maintainPlan.ActEndDate != null && maintainPlan.ActBeginDate != null)
                {
                    DateTime dt1 = Convert.ToDateTime(maintainPlan.ActEndDate);
                    DateTime dt2 = Convert.ToDateTime(maintainPlan.ActBeginDate);
                    TimeSpan ts1 = dt1.Subtract(dt2);
                    workHours = Math.Round(ts1.TotalHours, 2);
                }
            }
            return workHours;
        }

        /// <summary>
        /// 提交保养单
        /// </summary>
        /// <param name="maintainPlan"></param>
        /// <returns></returns>
        public virtual MaintainPlan SubmitMaintainPlan(MaintainPlan maintainPlan)
        {
            // 保养提交验证
            CheckSubmitMaintainPlan(maintainPlan);

            //判断工时登记是否可为空
            var config2 = ConfigService.GetConfig(new MaintainWorkTimeConfig(), typeof(MaintainPlanViewModel));
            if (config2 == null)
                throw new ValidationException("未找到工时登记配置,请检查配置项".L10N());
            if (config2.IsMaintainForWorkTime == YesNo.Yes)
            {
                if (maintainPlan.WorkHoursRegisterList.Count == 0)
                {
                    throw new ValidationException("工时登记不能为空，请填写".L10N());
                }
            }

            //通过【保养确认部门】配置项得到部门列表，列表为空则不进行确认，部门不空则进行确认
            List<double> departmentIdList = new List<double>();
            List<double> scoreProjectIds = new List<double>();
            bool IsMarkScore = false;
            var config = ConfigService.GetConfig(new MaintainConfirmDepartConfig(), typeof(MaintainPlanViewModel));
            if (config != null && !config.DepartmentIds.IsNullOrEmpty())
            {
                departmentIdList.AddRange(config.DepartmentIds.Split(',').Select(x => Convert.ToDouble(x)));
                IsMarkScore = config.IsMarkScore;
            }
            departmentIdList = departmentIdList.Distinct().ToList();

            //判断提交后状态是待确认还是已执行
            maintainPlan.ExeState = departmentIdList.Any() ? MaintExeState.NotConfirm : MaintExeState.Performed;
            // 是否进行评分
            if (maintainPlan.ExeState == MaintExeState.NotConfirm && IsMarkScore)
            {
                scoreProjectIds = GetTpmWeekInspectScores(ScoreType.Maintain);
                if (!scoreProjectIds.Any())
                {
                    throw new ValidationException("未维护类型为{0}的TPM评分项！".L10nFormat(ScoreType.Maintain.ToLabel().L10N()));
                }
            }

            //判断保养结果
            maintainPlan.ExeResult = maintainPlan.ProjectList
                .All(p => p.MaintainResult == CheckMaintainResult.OK || p.MaintainResult == CheckMaintainResult.Unright) ? ExeResult.Successed : ExeResult.Failed;

            // 判断是否已报修
            maintainPlan.WhetherRepair = RT.Service.Resolve<IEquipRepairBill>().CheckPlanWithRepairBill(maintainPlan.EquipAccountId, maintainPlan.MaintainNo, 1) ? YesNo.Yes : YesNo.No;

            // 更新保养单项目状态
            maintainPlan.ProjectList.ForEach(p => p.ExeState = maintainPlan.ExeState);

            // 更新保养执行人
            maintainPlan.ExecuteById = RT.IdentityId;

            // 计算总工时
            maintainPlan.SumWorkHours = CalculateWorkHours(maintainPlan, config2.IsMaintainForWorkTime);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存检验单提交结果
                RF.Save(maintainPlan);

                //更新备件更换记录标记
                if (maintainPlan.ExeState == MaintExeState.Performed)
                    RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.Maintain, maintainPlan.Id);

                //保存设备履历
                using (SIE.DataAuth.DataAuths.LoadAll())
                {
                    var equip = Query<EquipAccount>().Where(p => p.Id == maintainPlan.EquipAccountId).Select(p => new
                    {
                        Id = p.Id,
                        State = p.State,
                    }).FirstOrDefault<EquipInfo>();
                    if (equip != null && equip.Id != 0)
                    {
                        RT.Service.Resolve<EquipController>().GenerateEquipAccountResume(equip.Id, ResumeType.Maintain, equip.State, maintainPlan.MaintainNo);
                    }
                }

                // 如果要确认，则生成保养确认单（评分项）
                if (maintainPlan.ExeState == MaintExeState.NotConfirm)
                {
                    RT.Service.Resolve<MaintainController>().GenerateMaintainConfirmation(maintainPlan.Id, departmentIdList, scoreProjectIds, IsMarkScore);
                }

                //需要推送异常信息
                if (maintainPlan.IsAbnormalInfoPush && maintainPlan.ExeResult == ExeResult.Failed)
                    GenerateEquipMaintainAbnormalInfo(maintainPlan);

                trans.Complete();
            }

            return maintainPlan;
        }

        /// <summary>
        /// 生成设备点检异常信息
        /// </summary>
        /// <param name="maintainPlan"></param>
        private void GenerateEquipMaintainAbnormalInfo(MaintainPlan maintainPlan)
        {
            //获取设备保养类型的异常信息定义
            var define = RT.Service.Resolve<AbnormalInfoController>().GetAbnormalDefinition(AbnormalSource.EquipMaintain);
            if (define == null)
                throw new ValidationException("不存在[设备保养]的异常信息定义，请现在[异常信息定义]界面维护。".L10N());

            //收集异常保养项目
            var abnormalProjects = maintainPlan.ProjectList.Where(p => p.MaintainResult == CheckMaintainResult.NG);
            //收集异常点检名称
            var abnormalProjectNames = abnormalProjects.Select(p => p.ProjectName);
            var abnormalProjectNamesStr = string.Join(";", abnormalProjectNames);
            //收集异常点检缺陷描述
            var defectDescs = abnormalProjects.Select(p => p.Defect);
            var defectDescsStr = string.Join(";", defectDescs);

            var abnormal = new AbnormalInfor()
            {
                No = RT.Service.Resolve<AbnormalInfoController>().GetNewAbnormalInfoNo(),
                AbnormalStatus = AbnormalStatus.ToProcess,
                IsSendPdca = false,
                IsRectificationTask = false,
                AbnormalInfoDefinitionId = define.Id,
                InspectionNo = maintainPlan.MaintainNo,
                EquipmentId = maintainPlan.EquipAccountId,
                ProjectDesc = defectDescsStr,
                ProjectNg = abnormalProjectNamesStr
            };

            RF.Save(abnormal);

        }

        #endregion

        #region 保养确认
        /// <summary>
        /// 为保养确认提供指定保养计划Id所对应的保养计划详情
        /// </summary>
        /// <param name="MaintainPlanId">保养计划Id</param>
        /// <param name="confirmDeptId">确认部门Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainConfirmation> GetMaintainPlanConfirmations(double MaintainPlanId, double confirmDeptId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainConfirmation>().Where(p => p.MaintainPlanId == MaintainPlanId && p.ConfirmDeptId == confirmDeptId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(null, elo);

            return list;
        }

        /// <summary>
        /// 获取当前登录用户所属部门全部需要保养确认的检验单信息
        /// </summary>
        /// <param name="keyword">设备的编码或者名称的关键字</param>
        /// <param name="departmentId">部门ID</param>
        /// <param name="pagingInfo">分页实体</param>
        /// <returns></returns>
        public virtual IEnumerable<MaintainPlanInfos> GetNotConfirmedMaintainPlans(string keyword, double? departmentId, PagingInfo pagingInfo)
        {
            var list = new List<MaintainPlanInfos>();

            //用户是否有保养确认的权限，没有的话返回空
            var query = Query<DevicePur>()
                 .LeftJoin<UserInUserGroup>((x, y) => x.UserGroupId == y.UserGroupId)
                 .Where<UserInUserGroup>((x, y) => (x.UserId == RT.Identity.UserId || y.UserId == RT.Identity.UserId) && x.MaintainConfirm);

            var hasMaintainConfirmCount = query.Count();

            if (hasMaintainConfirmCount <= 0)
            {
                return list;
            }

            //过滤 用户有权限点检确认的部门ID
            var deviceInfo = RT.Service.Resolve<DevicePurController>().GetDepartmentsForConfirmCheck(RT.Identity.UserId);

            var deptIds = deviceInfo.Select(x => x.DeptId).Cast<double?>();

            //过滤部门            
            if (departmentId != null)
            {
                deptIds = deptIds.Where(p => p == departmentId).ToList();
            }

            var queryer = Query<MaintainPlan>()
                .Where(p => p.ExeState == MaintExeState.NotConfirm)
                .WhereIf(keyword.IsNotEmpty(), p => p.EquipAccount.Code.Contains(keyword) || p.EquipAccount.Name.Contains(keyword))
                .OrderBy(p => p.ActEndDate);

            var iquery = queryer.ToQuery();
            iquery.QueryWithEquipAccountPermissions(MaintainPlan.EquipAccountIdProperty.Name);
            var queryerList = queryer.Repository.QueryList(iquery, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());

            // 找出这些单的确认项
            var maintainPlanIds = new List<double>();
            var maintainPlanList = new List<MaintainPlan>();
            foreach(var q in queryerList)
            {
                var plan = q as MaintainPlan;
                maintainPlanIds.Add(plan.Id);
                maintainPlanList.Add(plan);
            }
            var maintainConfirmItems = maintainPlanIds.SplitContains(tempIds =>
            {
                return Query<MaintainPlanConfirmItem>().Where(p => p.MaintExeState == MaintExeState.NotConfirm && tempIds.Contains(p.MaintainPlanId) && deptIds.Contains(p.DepartmentId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            // //当前用户可管理的设备台账 过滤设备 有权限的设备
            var departmentList = RT.Service.Resolve<EnterpriseController>().GetEnterpriseByIds(deviceInfo.Select(p => p.DeptId).Distinct().ToList());

            foreach (var item in maintainConfirmItems)
            {
                // 部门权限信息
                var infos = deviceInfo.Where(x => x.DeptId == item.DepartmentId).ToList();

                // 保养单
                MaintainPlan plan = maintainPlanList.FirstOrDefault(x => x.Id == item.MaintainPlanId);
                var department = departmentList.FirstOrDefault(p => p.Id == item.DepartmentId);
                list.Add(new MaintainPlanInfos()
                {
                    Id = plan.Id,                                      //保养计划id
                    PlanBeginDate = plan.PlanBeginDate.ToString(),     //计划执行时间
                    PlanEndDate = plan.PlanEndDate.ToString(),         //计划结束时间
                    No = plan.MaintainNo,                              //保养单号
                    Qty = plan.ProjectList.Count(),                    //项目数量
                    EquipId = plan.EquipAccountId,                     //设备ID 
                    EquipCode = plan.EquipAccountCode,                //设备编码
                    EquipName = plan.EquipAccountName,                //设备名称
                    DepartmentId = department?.Id,                     //部门ID
                    DepartmentCode = department?.Code,                 //部门编码
                    DepartmentName = department?.Name,                 //部门名称
                    State = (int)item.MaintExeState,
                    StateName = item.MaintExeState.ToLabel().L10N(),
                    Shop = plan.WorkShopName,
                    Line = plan.ResourceName,
                    MaintainTime = plan.MaintainTime,
                    EquipTypeId = plan.EquipTypeId,
                    EquipTypeCode = plan.EquipTypeCode,
                    EquipTypeName = plan.EquipTypeName,
                    EquipModelId = plan.EquipModelId,
                    EquipModelCode = plan.EquipModelCode,
                    EquipModelName = plan.EquipModelName,
                    MaintainSummary = plan.MaintainSummary,
                    WhetherRepair = (int)plan.WhetherRepair,
                    MaintainEmployee = plan.ExecuteByName,
                    ActBeginDate = plan.ActBeginDate.HasValue ? plan.ActBeginDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                    ActEndDate = plan.ActEndDate.HasValue ? plan.ActEndDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                });
            }

            return list;
        }

        /// <summary>
        /// 依据指定保养计划单及某个保养确认部门生成保养确认单（评分项）
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <param name="confirmDeptIds">保养确认部门IDS</param>
        /// <param name="scoreProjectIds">评分项Ids</param>
        /// <param name="isMarkScore">是否评分</param>
        public virtual void GenerateMaintainConfirmation(double maintainPlanId, List<double> confirmDeptIds, List<double> scoreProjectIds, bool isMarkScore)
        {
            // 确认项
            EntityList<MaintainPlanConfirmItem> maintainPlanConfirmItems = new EntityList<MaintainPlanConfirmItem>();
            // 评分项
            EntityList<MaintainConfirmation> maintainConfirmations = new EntityList<MaintainConfirmation>();

            foreach (var confirmDeptId in confirmDeptIds)
            {
                // 生成保养确认项
                MaintainPlanConfirmItem maintainPlanConfirmItem = new MaintainPlanConfirmItem
                {
                    MaintainPlanId = maintainPlanId,
                    DepartmentId = confirmDeptId,
                    MaintExeState = MaintExeState.NotConfirm,
                };
                maintainPlanConfirmItems.Add(maintainPlanConfirmItem);
                if (isMarkScore)
                {
                    //生成保养评分项
                    foreach (var projectId in scoreProjectIds)
                    {
                        var maintainConfirmation = new MaintainConfirmation();
                        maintainConfirmation.MaintainPlanId = maintainPlanId;
                        maintainConfirmation.TpmScoreProjectId = projectId;
                        maintainConfirmation.ConfirmDeptId = confirmDeptId;
                        maintainConfirmation.FileName = "_";//平台规定不能为空，先用“_”占位，然后再置空。
                        maintainConfirmations.Add(maintainConfirmation);
                    }
                }
            }
            RF.BatchInsert(maintainPlanConfirmItems);
            RF.BatchInsert(maintainConfirmations);
        }

        /// <summary>
        /// 获取TPM评分项
        /// </summary>
        /// <returns></returns>
        private List<double> GetTpmWeekInspectScores(ScoreType scoreType)
        {
            //获取TPM评分项
            var query = Query<TpmWeekInspectScore>()
                .Where(p => p.ScoreType == scoreType)
                .Select(p => new { p.Id })
                .ToList<double>().ToList();
            return query;
        }

        /// <summary>
        /// 获取某个保养计划的保养项目
        /// </summary>
        /// <param name="maintainPlanId">保养计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainProject> GetMaintainProjectList(double maintainPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainProject>().Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }

        /// <summary>
        /// 获取某个保养计划的备件更换
        /// </summary>
        /// <param name="maintainPlanId">保养计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePart> GetMaintainPlanSparePartList(double maintainPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainPlanSparePart>().Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值申请单明细
            list.Where(x => x.PartOutDepotDetailId.HasValue).ForEach(p =>
              {
                  p.RemainingQty = p.OutDepotCount - p.UseCount;
              });

            return list;
        }

        /// <summary>
        /// 获取某个保养计划的备件申请
        /// </summary>
        /// <param name="maintainPlanId">保养计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanSparePartApl> GetMaintainPlanSparePartAplList(double maintainPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainPlanSparePartApl>().Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            if (list.Count > 0)
            {
                var partIds = list.Select(p => p.SparePartId).ToList();
                var storeSummaryDepotList = RT.Service.Resolve<SparePartController>().GetStoreSummaryDepots(partIds);
                list.ForEach(p =>
                {
                    //获取库存明细
                    p.StoreQty = storeSummaryDepotList.FirstOrDefault(x => x.WarehouseId == p.OutStockWarehouseId && x.SparePartId == p.SparePartId)?.StoreQty ?? 0;
                });

            }

            return list;
        }

        /// <summary>
        /// 获取某个保养计划的执行图片
        /// </summary>
        /// <param name="maintainPlanId">保养计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainPlanAttachment> GetMaintainPlanAttachmentList(double maintainPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainPlanAttachment>().Where(p => p.OwnerId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }

        /// <summary>
        /// 获取某个保养计划的工时
        /// </summary>
        /// <param name="maintainPlanId">保养计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<WorkHoursRegister> GetWorkHoursRegisterList(double maintainPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<WorkHoursRegister>().Where(p => p.MaintainPlanId == maintainPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }

        /// <summary>
        /// 获取某个保养计划指定的对应确认部门的保养确认单（评分项）,保养确认部门ID为null时表示拿全部的评分项。
        /// </summary>
        /// <param name="maintainPlanId">保养计划Id</param>
        /// <param name="confirmDeptId">保养确认部门ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<MaintainConfirmation> GetMaintainConfirmationList(double maintainPlanId, double? confirmDeptId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<MaintainConfirmation>().Where(p => p.MaintainPlanId == maintainPlanId);

            if (confirmDeptId != null)
            {
                q.Where(p => p.ConfirmDeptId == confirmDeptId);
            }

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }


        /// <summary>
        /// 是否
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNeedMarkScore()
        {
            var config = ConfigService.GetConfig<MaintainConfirmDepartConfigValue>(new MaintainConfirmDepartConfig(), typeof(MaintainPlanViewModel));
            bool isMarkScore = false;
            if (config != null)
            {
                isMarkScore = config.IsMarkScore;
            }
            return isMarkScore;
        }

        /// <summary>
        /// 是否具有保养确认权限
        /// </summary>
        /// <param name="maintainPlanId">保养计划ID</param>
        /// <param name="confirmDeptId">确认部门ID</param>
        /// <returns></returns>
        public virtual bool CanSubmitMaintainConfirmation(double maintainPlanId, double confirmDeptId)
        {
            //保养计划
            var maintainPlan = RF.GetById<MaintainPlan>(maintainPlanId);

            //判断是否按部门进行保养
            bool isNeedDepartment = RT.Service.Resolve<MaintainController>().IsDepartmentMaintain();

            //匹配人员权限 
            var devicePurQueryer = Query<DevicePur>();
            if (isNeedDepartment)
            {
                devicePurQueryer.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId && a.MaintainConfirm)
                    .LeftJoin<DeviceDepa>((a, d) => a.Id == d.DevicePurId)
                    .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                    .Where<DeviceDepa>((a, d) => d.EnterpriseId == confirmDeptId);
            }
            else if (maintainPlan.DepartmentId == null || maintainPlan.DepartmentId == 0)
            {
                devicePurQueryer.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId && a.MaintainConfirm)
                    .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId);
            }

            var list = devicePurQueryer.ToList();
            return !(list.Count == 0);
        }

        /// <summary>
        /// 保养确认提交验证
        /// </summary>
        /// <param name="info">提交信息</param>
        /// <param name="isNeedScore">是否评分</param>
        private void MaintainConfirmSubmitVali(MaintainConfirmationSubmitInfo[] info, bool isNeedScore)
        {
            // 保养校验
            foreach (var item in info)
            {
                if (item.MaintainPlanId == null) throw new ValidationException("保养单ID不能为空！".L10N());
                if (item.ConfirmResult == null) throw new ValidationException("确认结果不能为空！".L10N());
                if (item.MaintainPlanId != info[0].MaintainPlanId) throw new ValidationException("保养单ID不一致！".L10N());
                if (item.ConfirmResult != info[0].ConfirmResult) throw new ValidationException("同一保养单的确认结果应当一致！".L10N());
                if (item.ConfirmNote != info[0].ConfirmNote) throw new ValidationException("同一保养单的确认备注应当一致！".L10N());
                if (item.ConfirmDeptId != info[0].ConfirmDeptId) throw new ValidationException("同一保养单的确认部门应当一致！".L10N());
                if (item.ConfirmResult == 2 && string.IsNullOrWhiteSpace(item.ConfirmNote)) throw new ValidationException("确认结果不合格时备注不能为空！".L10N());
                if (isNeedScore) // 配置项启用评分
                {
                    if (item.TpmScoreProjectId == null) throw new ValidationException("评分项ID不能为空！".L10N());
                    if (item.Score == null) throw new ValidationException("评分不能为空！".L10N());
                    if (item.Score <= 0 || item.Score >= 6) throw new ValidationException("评分只能是1到5分！".L10N());

                }
            }
        }

        /// <summary>
        /// 点检提交评分项
        /// </summary>
        /// <param name="info">提交信息</param>
        /// <param name="planInfo">保养单信息</param>
        /// <param name="maintainConfirmations">评分项</param>
        /// <param name="dbDate">数据库时间</param>
        private void CreateMaintainMarks(MaintainConfirmationSubmitInfo[] info, MaintainPlanInfo planInfo, EntityList<MaintainConfirmation> maintainConfirmations, DateTime dbDate)
        {
            var tpmScoreProjectIds = info.Select(x => x.TpmScoreProjectId).Distinct().ToList();
            var maintainConfirmationList = tpmScoreProjectIds.SplitContains(tempIds =>
            {
                var query = DB.Query<MaintainConfirmation>()
                     .Where(p => p.MaintainPlanId == planInfo.Id && tempIds.Contains(p.TpmScoreProjectId))
                     .WhereIf(planInfo.ConfirmDeptId != null && planInfo.ConfirmDeptId != 0, p => p.ConfirmDeptId == planInfo.ConfirmDeptId);

                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            //保存确认结果                

            info.ForEach(i =>
            {
                var maintainConfirmation = maintainConfirmationList
                                    .FirstOrDefault(p => p.TpmScoreProjectId == i.TpmScoreProjectId);

                if (maintainConfirmation == null)
                {
                    string msg = $"ID为{i.MaintainPlanId}的保养单找不到";
                    if (planInfo.ConfirmDeptId != null) { msg += $"部门ID为{planInfo.ConfirmDeptId}、"; }
                    msg += $"评分项ID为{i.TpmScoreProjectId}的项！";
                    throw new ValidationException(msg);
                }

                var maintainConfirmationNew = new MaintainConfirmation();

                maintainConfirmationNew.Clone(maintainConfirmation, SIE.Domain.CloneOptions.NewSingleComposition());

                if (!i.Content.IsNullOrEmpty())
                {
                    UploadFile(i, maintainConfirmationNew);
                }
                else
                {

                }

                maintainConfirmationNew.ConfirmResult = (ConfirmResult)i.ConfirmResult;
                maintainConfirmationNew.ConfirmNote = i.ConfirmNote;
                maintainConfirmationNew.Score = (Score)i.Score;
                maintainConfirmationNew.ConfirmorId = RT.IdentityId;
                maintainConfirmationNew.ConfirmDate = RF.Find<MaintainConfirmation>().GetDbTime();
                maintainConfirmationNew.PersistenceStatus = PersistenceStatus.New;

                maintainConfirmations.Add(maintainConfirmationNew);
            });
        }

        /// <summary>
        /// 更新保养单确认结果、备注
        /// </summary>
        /// <param name="plan">保养计划</param>
        /// <param name="planInfo">提交信息</param>
        /// <param name="dbDate">时间</param>
        private void UpdatePlanResult(MaintainPlan plan, MaintainPlanInfo planInfo, DateTime dbDate)
        {
            if (plan == null)
            {
                throw new ValidationException("保养单不存在，[ID:{0}]".L10nFormat(planInfo.Id));
            }

            if (plan.ExeState != MaintExeState.NotConfirm)
            {
                throw new ValidationException("保养单[{0}]是[{1}]状态，不允许进行保养确认".L10nFormat(plan.MaintainNo, plan.ExeState.ToLabel()));
            }

            if (plan.ConfirmNote.IsNullOrEmpty())
            {
                plan.ConfirmNote = planInfo.ConfirmNote;
            }
            else
            {
                plan.ConfirmNote += ";" + planInfo.ConfirmNote;
            }
            // 时间
        }

        /// <summary>
        /// 更新保养单状态
        /// </summary>
        /// <param name="isNeedScore">是否需要评分</param>
        /// <param name="plan">保养单</param>
        /// <param name="planInfo">提交信息</param>
        /// <param name="confirmItems">保养单确认项</param>
        private void UpdatePlanState(bool isNeedScore, MaintainPlan plan, MaintainPlanInfo planInfo, EntityList<MaintainPlanConfirmItem> confirmItems)
        {
            // 更新确认项目
            var confirmItemList = Query<MaintainPlanConfirmItem>().Where(p => p.MaintainPlanId == plan.Id).ToList();
            var confirmItem = confirmItemList.FirstOrDefault(p => p.DepartmentId == planInfo.ConfirmDeptId);
            if (confirmItem == null)
            {
                throw new ValidationException("不存在部门Id[{0}]的保养确认项".L10nFormat(planInfo.ConfirmDeptId));
            }
            else
            {
                confirmItem.MaintExeState = MaintExeState.Confirmed;
                confirmItem.ConfirmResult = planInfo.ConfirmResult == 1 ? ConfirmResult.OK : ConfirmResult.NG;
                confirmItem.ConfirmNote = planInfo.ConfirmNote;
                confirmItems.Add(confirmItem);
            }
            // 确认项是否都已确认
            var noConfirmCount = confirmItemList.Count(p => p.MaintExeState == MaintExeState.NotConfirm && p.Id != confirmItem.Id);
            if (noConfirmCount <= 0)
            {
                plan.ExeState = MaintExeState.Confirmed;
            }
            // 点检确认项任一不合格则整体不合格
            if (confirmItem.ConfirmResult == ConfirmResult.NG || confirmItemList.Any(p => p.ConfirmResult == ConfirmResult.NG))
            {
                plan.ConfirmResult = ConfirmResult.NG;
            }
            else
            {
                plan.ConfirmResult = ConfirmResult.OK;
            }


            // 评分项是否都已打分
            if (isNeedScore)
            {
                // 点检确认项需要更新状态为已评分
                var noMarkList = Query<MaintainConfirmation>().Where(p => p.MaintainPlanId == plan.Id).ToList();
                if (noMarkList.Count > 0)
                {
                    confirmItem.MaintExeState = MaintExeState.Scored;
                    if (noMarkList.Count(p => p.ConfirmResult == null) == 0)
                    {
                        plan.ExeState = MaintExeState.Scored;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// 根据分类获取分类值
        /// </summary>
        /// <param name="maintainCycleType">保养周期类型</param>
        /// <param name="page"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<MaintainTypeInfo> GetMaintainTypeInfoList(MaintainCycleType maintainCycleType, PagingInfo page, string keyword = null)
        {
            EntityList<MaintainTypeInfo> sourceList = new EntityList<MaintainTypeInfo>();

            List<EnumViewModel> list = EnumViewModel.GetByEnumType(typeof(MaintainType));


            if (maintainCycleType == MaintainCycleType.Month)
            {
                list = new List<EnumViewModel>();
                list.Add(new EnumViewModel(MaintainType.Month));
                list.Add(new EnumViewModel(MaintainType.DbMonth));
                list.Add(new EnumViewModel(MaintainType.Season));
                list.Add(new EnumViewModel(MaintainType.HalfYear));
                list.Add(new EnumViewModel(MaintainType.Year));
            }

            if (page != null)
            {
                List<MaintainTypeInfo> spList = null;

                //分页加载枚举类型数据并有值查询
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    spList = list.Where(p => p.Label.Contains(keyword) || p.EnumValue.ToString().Contains(keyword))
                       .Select(p => new MaintainTypeInfo() { Id = Convert.ToInt32(p.EnumValue).ToString(), Value = p.Label })
                       .ToList();
                }
                else
                {
                    spList = list.Select(p => new MaintainTypeInfo() { Id = Convert.ToInt32(p.EnumValue).ToString(), Value = p.Label })
                      .ToList();
                }

                var tmpList = spList.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();

                sourceList.AddRange(tmpList);
                sourceList.SetTotalCount(spList.Count);
            }
            else
            {
                List<MaintainTypeInfo> spList = null;

                //正常加载枚举类型数据
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    spList = list.Where(p => p.Label.Contains(keyword) || p.EnumValue.ToString().Contains(keyword))
                       .Select(p => new MaintainTypeInfo() { Id = Convert.ToInt32(p.EnumValue).ToString(), Value = p.Label })
                       .ToList();
                }
                else
                {
                    spList = list.Select(p => new MaintainTypeInfo() { Id = Convert.ToInt32(p.EnumValue).ToString(), Value = p.Label })
                      .ToList();
                }

                sourceList.AddRange(spList);
                sourceList.SetTotalCount(spList.Count);
            }

            return sourceList;
        }

        /// <summary>
        /// 获取保养计划
        /// </summary>
        /// <param name="maintainPlanId">保养记录ID</param>
        /// <param name="accountId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual MaintainSummary GetMaintainPlanForExcute(double maintainPlanId, double accountId, double? departmentId)
        {
            var maintainPlan = GetMaintainPlanById(maintainPlanId);

            ValidationMaintainDate(maintainPlan);

            //生成保养执行保养项目
            GeneratePlanProject(maintainPlan);

            //获取上次保养小结
            var upMaintainSummary = GetLastMaintainSummary(accountId, departmentId);

            return new MaintainSummary()
            {
                UpMaintainSummary = upMaintainSummary,
                PrecisePlanBeginDate = maintainPlan.PrecisePlanBeginDate,
                PrecisePlanEndDate = maintainPlan.PrecisePlanEndDate
            };
        }

        /// <summary>
        /// 验证保养时间
        /// </summary>
        /// <param name="maintainPlan"></param>
        /// <exception cref="ValidationException"></exception>
        private void ValidationMaintainDate(MaintainPlan maintainPlan)
        {
            if (maintainPlan.PrecisePlanBeginDate != null)
            {
                if (maintainPlan.PrecisePlanBeginDate > DateTime.Now)
                {
                    throw new ValidationException("保养时间为[{0}]，当前时间不在范围内!".L10nFormat(maintainPlan.PrecisePlanBeginDate));
                }
            }
            else
            {
                if ((maintainPlan.PlanEndDate - maintainPlan.PlanBeginDate).TotalDays > 7)
                {
                    var weekInfo = GetWeekInfoOfDate(maintainPlan.PlanBeginDate);
                    if (weekInfo.Item2 > DateTime.Now)
                    {
                        throw new ValidationException("保养时间为[{0}]，当前时间不在范围内!".L10nFormat(maintainPlan.PlanBeginDate));
                    }

                }
                else
                {
                    if (maintainPlan.PlanBeginDate > DateTime.Now)
                    {
                        throw new ValidationException("保养时间为[{0}]，当前时间不在范围内!".L10nFormat(maintainPlan.PlanBeginDate));
                    }
                }
            }
        }

        /// <summary>
        /// 判断保养单状态是否为未执行或执行中
        /// </summary>
        /// <param name="maintainPlanId"></param>
        /// <returns></returns>
        public virtual bool ChackIsExeState(double maintainPlanId)
        {
            var maintainPlan = Query<MaintainPlan>().Where(p => p.Id == maintainPlanId && p.ExeState != MaintExeState.NotPerformed && p.ExeState != MaintExeState.Performing).FirstOrDefault();
            return maintainPlan == null;
        }

        /// <summary>
        /// 保养记录后端删除验证（状态为已执行和已评分的数据不能删除附件图片）
        /// </summary>
        /// <param name="maintainRecordId"></param>
        /// <returns></returns>
        public virtual bool CheckCanExecute(double maintainRecordId)
        {
            var maintainRecord = RF.GetById<MaintainRecord>(maintainRecordId);
            if (maintainRecord == null)
            {
                return false;
            }
            else
            {
                if (maintainRecord.ExeState == MaintExeState.Performed || maintainRecord.ExeState == MaintExeState.Scored)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 根据Ids获取保养计划
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual List<MaintExeState> GetMaintainPlanExeStates(List<double> ids)
        {
            List<MaintExeState> stateList = new List<MaintExeState>();
            ids.SplitDataExecute(tempIds =>
            {
                var list = Query<MaintainPlan>().Where(p => tempIds.Contains(p.Id)).Select(p => new {p.ExeState}).ToList<MaintExeState>().ToList();
            });
            return stateList;
        }

        /// <summary>
        /// 删除未执行的保养计划
        /// </summary>
        /// <param name="ids"></param>
        public virtual void DeleteMaintainByIds(List<double> ids)
        {
            var maintainList = GetMaintainPlanExeStates(ids);
            if (maintainList.Any(p => p == MaintExeState.NotPerformed))
            {
                throw new ValidationException("只有保养状态为【未执行】的数据才能操作".L10N());
            }
            ids.SplitDataExecute(tempIds =>
            {
                DB.Delete<MaintainPlan>().Where(p => tempIds.Contains(p.Id)).Execute();
            });
        }

        /// <summary>
        /// 保养开始
        /// </summary>
        /// <param name="Id">保养单id</param>
        public virtual void BegingMaintain(double Id)
        {
            // 刷新已开始，状态更新为执行中
            DB.Update<MaintainPlan>().Where(p => p.Id == Id).Set(p => p.WhetherBegin, true).Set(p => p.ExeState, MaintExeState.Performing).Set(p => p.ActBeginDate, DateTime.Now).Execute();
        }
        
        /// <summary>
        /// 获取设备的保养计划基础信息
        /// </summary>
        /// <param name="equipIds">设备Ids</param>
        /// <returns></returns>
        public virtual List<MaintainPlanInfos> GetMaintainPlanInfos(List<double> equipIds)
        {
            List<MaintainPlanInfos> maintainPlanInfos = new List<MaintainPlanInfos>();

            equipIds.SplitDataExecute(temps =>
            {
                var list = Query<MaintainPlan>().Where(p => temps.Contains(p.EquipAccountId)).Select(p => new
                {
                    Id = p.Id,
                    EquipId = p.EquipAccountId,
                    PlanBeginDateValue = p.PlanBeginDate,
                    PlanEndDateValue = p.PlanEndDate,
                }).ToList<MaintainPlanInfos>();
                maintainPlanInfos.AddRange(list);
            });

            return maintainPlanInfos;
        }
    }

}