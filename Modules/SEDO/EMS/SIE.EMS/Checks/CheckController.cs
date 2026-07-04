using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Configs;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Checks.Records;
using SIE.EMS.Common.Configs;
using SIE.EMS.Common.Utils;
using SIE.EMS.DataAuth;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Rbac.Users;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EMS.Checks
{
    /// <summary>
    /// 点检控制器 API
    /// </summary>
    public partial class CheckController : DomainController
    {
        private const string YEAR_AND_MONTH_FORMAT = "yyyy/MM";
        #region 点检计划 
        /// <summary>
        /// 获取点检单号
        /// </summary>
        /// <returns>点检单号</returns>
        public virtual string GetCheckPlanNo()
        {
            var config = ConfigService.GetConfig(new CheckPlanNoConfig(), typeof(CheckPlanViewModel));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到点检单号生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取多个点检单号
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>

        public virtual List<string> GetCheckPlanNoList(int number)
        {
            var config = ConfigService.GetConfig(new CheckPlanNoConfig(), typeof(CheckPlanViewModel));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到点检单号生成规则,请检查规则配置".L10N());
            }
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
        /// 获取日历方案
        /// </summary>
        /// <returns>日历方案</returns>
        public virtual CalendarScheme GetCalendarScheme()
        {
            var config = ConfigService.GetConfig(new CheckPlanTypeConfig(), typeof(CheckPlanViewModel));
            if (config == null || config.CalendarScheme == null)
            {
                throw new ValidationException("未找到日历方案的配置规则,请检查规则配置".L10N());
            }
            return config.CalendarScheme;
        }

        /// <summary>
        /// 获取点检类型
        /// </summary>
        /// <returns>日历方案</returns>
        public virtual CheckPlanType GetCheckPlanType()
        {
            var config = ConfigService.GetConfig(new CheckPlanTypeConfig(), typeof(CheckPlanViewModel));

            if (config == null)
            {
                throw new ValidationException("未找到点检类型的配置规则,请检查规则配置".L10N());
            }

            return config.CheckPlanType;
        }

        /// <summary>
        /// 获取点检频次/小时
        /// </summary>
        /// <returns>日历方案</returns>
        public virtual int GetCheckFrequency()
        {
            var config = ConfigService.GetConfig(new CheckPlanTypeConfig(), typeof(CheckPlanViewModel));

            if (config == null || config.Frequency == null)
            {
                throw new ValidationException("未找到点检频次的配置规则,请检查规则配置".L10N());
            }

            return config.Frequency.Value;
        }

        /// <summary>
        /// 获取点检提前预警时间(H)
        /// </summary>
        /// <returns>点检提前预警时间(H)</returns>
        public virtual int? GetCheckAlertTime()
        {
            var config = ConfigService.GetConfig(new CheckAlertTimeConfig(), typeof(CheckPlanViewModel));
            if (config == null || config.AlertTime == null)
            {
                throw new ValidationException("未找到点检提前预警时间的配置规则,请检查规则配置".L10N());
            }
            return config.AlertTime;
        }

        /// <summary>
        /// 获取点检超时预警时间(H)
        /// </summary>
        /// <returns>点检超时预警时间(H)</returns>
        public virtual int? GetCheckExpiredTime()
        {
            var config = ConfigService.GetConfig(new CheckAlertTimeConfig(), typeof(CheckPlanViewModel));
            if (config == null || config.ExpiredTime == null)
            {
                throw new ValidationException("未找到点检超时预警时间的配置规则,请检查规则配置".L10N());
            }
            return config.ExpiredTime;
        }

        /// <summary>
        /// 是否按部门进行点检
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsDepartmentCheck()
        {
            var config = ConfigService.GetConfig(new IsDepartmentPlanConfig());
            return config.IsDepartmentPlan == YesNo.Yes;
        }

        /// <summary>
        /// 是否带出设备子项的检验项目
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsBringChildCheckProject()
        {
            var config = ConfigService.GetConfig(new CheckChildProjectConfig(), typeof(CheckPlanViewModel));
            return config.IsBringChildCheckProject == YesNo.Yes;
        }

        /// <summary>
        /// 获取超时的设备点检单
        /// </summary>
        /// <param name="enterpriseId">车间Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="timeOut">时间</param>
        /// <returns>点检计划列表</returns>
        public virtual EntityList<CheckPlan> GetTimeOutCheckPlanList(double enterpriseId, double processId, int timeOut)
        {
            var q = Query<CheckPlan>().Where(p => p.EquipAccount.WorkShopId == enterpriseId && p.EquipAccount.ProcessId == processId && p.CheckBeginDate <= DateTime.Now && p.CheckEndDate >= DateTime.Now && p.ExeState == CheckExeState.NotPerformed && p.CheckEndDate <= DateTime.Now.AddHours(timeOut));
            return q.ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备台账点检，查询(当天和昨天跨日)未超期未点检任务
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>点检计划</returns>
        public virtual EntityList<CheckPlan> GetCheckPlans(double equipId)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();

            return Query<CheckPlan>()
                .Where(w => w.EquipAccountId == equipId && w.ExeState != CheckExeState.Performed
                    && w.CheckBeginDate <= now && w.CheckEndDate > now)
                .OrderBy(o => o.CheckBeginDate)
                .ToList();
        }

        /// <summary>
        /// 获取当前登录用户所属部门当天所有可点检的检验单信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="departmentIds">部门ID集合</param>
        /// <param name="pagingInfo">分页实体</param>
        /// <param name="exeState">单据状态</param>
        /// <returns></returns>
        public virtual EntityList GetNotPerformedCheckPlans(string keyword, List<double> departmentIds,
            PagingInfo pagingInfo, List<CheckExeState?> exeState)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();

            var q = Query<CheckPlan>();

            //对应状态于今天的数据
            //修改为PDA允许查询出超期的点检单
            q.Where(p => p.CheckBeginDate <= now);
           
            //模糊查询
            if (!string.IsNullOrWhiteSpace(keyword))
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
                q.Where(p => p.ExeState == CheckExeState.Performing || p.ExeState == CheckExeState.NotPerformed || p.ExeState == CheckExeState.Overdue);
            }

            //过滤责任部门
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
            q.OrderByDescending(p => p.ExeState).OrderByDescending(p => p.CheckBeginDate);
            //过滤设备台账权限
            var query = q.ToQuery();
            query.QueryWithEquipAccountPermissions(CheckPlan.EquipAccountIdProperty.Name);
            //贪懒加载            
            var list = q.Repository.QueryList(query, pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取当前登录用户所属部门当天所有可点检的检验单信息
        /// </summary>
        /// <param name="exeStates">单据状态</param>
        /// <param name="equipAccountId">设备台账ID</param>
        /// <returns></returns>
        public virtual int GetNotPerformedCheckPlansCount(List<CheckExeState> exeStates, double? equipAccountId = null)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();

            var q = Query<CheckPlan>();

            //对应状态于今天的数据
            //修改为PDA允许查询出超期的点检单
            q.Where(p => p.CheckBeginDate <= now);

            //过滤状态
            if (exeStates.Any())
            {
                q.Where(p => exeStates.Contains(p.ExeState));
            }
            else
            {
                q.Where(p => p.ExeState == CheckExeState.Performing || p.ExeState == CheckExeState.NotPerformed || p.ExeState == CheckExeState.Overdue);
            }

            //过滤责任部门
            var deptIds = RT.Service.Resolve<DevicePurController>()
                .GetDutyDepartments(RT.Identity.UserId)
                .Select(p => p.Id)
                .ToList<double>();

            var nullableDeptIds = deptIds.Cast<double?>();

            q.Where(p => nullableDeptIds.Contains(p.DepartmentId) || p.DepartmentId == null);
            q.OrderByDescending(p => p.ExeState).OrderByDescending(p => p.CheckBeginDate);
            //过滤设备台账权限
            var query = q.ToQuery();
            query.QueryWithEquipAccountPermissions(CheckPlan.EquipAccountIdProperty.Name);
            //贪懒加载            
            var count = q.Repository.QueryCount(query);
            return count;
        }


        /// <summary>
        /// 获取点检计划
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>点检计划</returns>
        public virtual CheckPlan GetCheckPlan(Expression<Func<CheckPlan, bool>> exp)
        {
            var query = Query<CheckPlan>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.FirstOrDefault(new EagerLoadOptions().LoadWith(CheckPlan.CheckProjectListProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 根据点检计划查询实体和设备台账Id列表获取点检计划列表（新）
        /// </summary>
        /// <param name="criteria">点检计划查询实体</param>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <param name="isShowAllEquipAccount"></param>
        /// <returns>点检计划列表</returns>
        public virtual EntityList<CheckPlan> GetCheckPlanListByAccountIds(CheckPlanCriteria criteria, List<double> accountIds, bool isShowAllEquipAccount = false)
        {
            return accountIds.SplitContains((tempIds) =>
            {
                //获取点检类型(日、班、频次)
                CheckPlanType checkType = RT.Service.Resolve<CheckController>().GetCheckPlanType();
                //判断是否按部门进行点检
                bool isDepartmentCheck = RT.Service.Resolve<CheckController>().IsDepartmentCheck();

                var query = Query<CheckPlan>().Where(p => p.CheckPlanType == checkType).Where(p => tempIds.Contains(p.EquipAccountId));

                if (criteria.Month.HasValue)
                {
                    query.Where(p => p.CheckBeginDate >= DateTime.Parse(criteria.Month.Value.ToString(YEAR_AND_MONTH_FORMAT) + "/1")
                        && p.CheckBeginDate < DateTime.Parse(criteria.Month.Value.AddMonths(1).ToString(YEAR_AND_MONTH_FORMAT) + "/1"));
                }

                if (isDepartmentCheck)
                {
                    query.Where(p => p.DepartmentId != null);
                    query.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                             .LeftJoin<DeviceDepa>((a, d) => a.Id == d.DevicePurId)
                             .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                             .Where<DeviceDepa>((a, d) => x.DepartmentId == d.EnterpriseId));
                }
                else
                {
                    query.Where(p => p.DepartmentId == null || p.DepartmentId == 0);
                }

                return query.ToList();
            });
        }

        /// <summary>
        /// 根据点检计划查询实体和设备台账Id列表获取点检计划列表
        /// </summary>
        /// <param name="criteria">点检计划查询实体</param>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>点检计划列表</returns>
        public virtual EntityList<CheckPlan> GetCheckPlanList(CheckPlanCriteria criteria, List<double> accountIds)
        {
            return accountIds.SplitContains((tempIds) =>
            {
                var query = Query<CheckPlan>().Where(p => tempIds.Contains(p.EquipAccountId));
                if (criteria.Month.HasValue)
                {
                    query.Where(p => p.CheckBeginDate >= DateTime.Parse(criteria.Month.Value.ToString(YEAR_AND_MONTH_FORMAT) + "/1")
                        && p.CheckBeginDate < DateTime.Parse(criteria.Month.Value.AddMonths(1).ToString(YEAR_AND_MONTH_FORMAT) + "/1"));
                }
                if (criteria.CheckCycleType.HasValue)
                {
                    query.Where(p => p.CheckCycleType == criteria.CheckCycleType);
                }
                return query.ToList();
            });
        }

        /// <summary>
        /// 获取部门层级的企业模型
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprises(PagingInfo pagingInfo, string keyword)
        {
            EntityList<Enterprise> list = Query<Enterprise>().Where(x => x.Level.Type == EnterpriseType.Department && x.InvOrgId == AppRuntime.InvOrg).WhereIf(keyword.IsNotEmpty(), x => x.Code.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            list.ForEach(data => { data.TreePId = null; });
            return list;
        }
        #endregion

        #region 点检项目                 

        /// <summary>
        /// 获取点检项目列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">Expression</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>点检项目列表</returns>
        public virtual EntityList<CheckProject> GetCheckProjects(Expression<Func<CheckProject, bool>> exp, PagingInfo pagingInfo = null)
        {
            var query = Query<CheckProject>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据点检单ID获取点检项目列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <returns></returns>
        public virtual EntityList<CheckProject> GetCheckProjects(double checkPlanId)
        {
            var query = Query<CheckProject>();
            query.Where(p => p.CheckPlanId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckProject.EquipCheckProjectProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取点检计划备件更换列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <param name="checkNo">点检计划单号</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanSparePart> GetCheckPlanSpareParts(double checkPlanId, string checkNo = null)
        {
            var q = Query<CheckPlanSparePart>();
            q.Where(p => p.CheckPlanId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckPlanSparePart.PartOutDepotDetailProperty);
            elo.LoadWith(PartOutDepotDetail.OutDepotProperty);
            elo.LoadWithViewProperty();

            var list = q.ToList(null, elo);

            //TODO,PDA逻辑注释自动带出申请单逻辑
            ////查询、赋值申请单明细
            //if (list.Count > 0)
            //{
            //    var appDtls = RT.Service.Resolve<SparePartAppController>().GetSparePartAppDtl(checkNo, SpareParts.Applys.Enums.FromType.SpotCheck);
            //    list.ForEach(p =>
            //    {
            //        if (p.State == ChangeSparePartState.New)
            //        {
            //            var appDtl = appDtls.FirstOrDefault(x => x.SparePartId == p.SparePartId);
            //            p.ApplyDetailId = appDtl?.Id;
            //        }
            //    });
            //}

            return list;
        }


        /// <summary>
        /// 根据点检单ID和状态获取备件更换项目列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanSparePart> GetCheckPlanSpareParts(double checkPlanId, ChangeSparePartState state)
        {
            var query = Query<CheckPlanSparePart>();
            query.Where(p => p.CheckPlanId == checkPlanId);
            query.Where(p => p.State == state);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckPlanSparePart.SparePartProperty);
            elo.LoadWith(CheckPlanSparePart.CheckPlanProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据点检单ID获取备件申请项目列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanSparePartApl> GetCheckPlanSparePartApls(double checkPlanId)
        {
            var query = Query<CheckPlanSparePartApl>();
            query.Where(p => p.CheckPlanId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckPlanSparePartApl.SparePartProperty);

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
        /// 根据点检单ID和申请标记获取备件申请项目列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <param name="isApply">是否申请</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanSparePartApl> GetCheckPlanSparePartApls(double checkPlanId, bool isApply)
        {
            var query = Query<CheckPlanSparePartApl>();
            query.Where(p => p.CheckPlanId == checkPlanId);
            query.Where(p => p.IsApply == isApply);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckPlanSparePartApl.SparePartProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据点检计划Id获取设备台账点检项目列表
        /// </summary>
        /// <param name="checkPlanId">点检计划Id</param>
        /// <returns>设备台账点检项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetEquipAccountCheckProjects(double checkPlanId)
        {
            return Query<EquipAccountCheckProject>().Exists<CheckPlan>((a, b) => b.Where(c => a.EquipAccountId == c.EquipAccountId && c.Id == checkPlanId)).ToList();
        }

        /// <summary>
        /// 创建备件申请单
        /// </summary>
        /// <param name="checkPlanId">点检单Id</param>
        /// <param name="sparePartIds">备件Ids</param>
        public virtual void CreateSelSpareApplyList(double checkPlanId, List<double> sparePartIds)
        {
            var dbApplyList = Query<CheckPlanSparePartApl>().Where(p => p.CheckPlanId == checkPlanId && sparePartIds.Contains(p.SparePartId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var spareId in sparePartIds)
            {
                var record = dbApplyList.FirstOrDefault(p => p.SparePartId == spareId);
                if (record != null)
                {
                    throw new ValidationException("已存在备件[{0}]的申请记录".L10nFormat(record.SparePartNameView));
                }
            }
            EntityList<CheckPlanSparePartApl> checkPlanSparePartApls = new EntityList<CheckPlanSparePartApl>();
            foreach (var sparePartId in sparePartIds)
            {
                CheckPlanSparePartApl apply = new CheckPlanSparePartApl
                {
                    CheckPlanId = checkPlanId,
                    SparePartId = sparePartId,
                    ApplyQty = 1,
                };
                checkPlanSparePartApls.Add(apply);
            }
            RF.BatchInsert(checkPlanSparePartApls);
        }

        /// <summary>
        /// 删除备件申请单
        /// </summary>
        /// <param name="Id"></param>
        public virtual void DeleteSelSpareApply(double Id)
        {
            DB.Delete<CheckPlanSparePartApl>().Where(p => p.Id ==  Id).Execute();
        }

        /// <summary>
        /// 创建备件更换单
        /// </summary>
        /// <param name="checkPlanId">点检单Id</param>
        /// <param name="sparePartIds">备件Ids</param>
        public virtual void CreateSelSpareChangeList(double checkPlanId, List<double> sparePartIds)
        {
            // 盘点是否已存在备件更换
            var dbChangeList = Query<CheckPlanSparePart>().Where(p => p.CheckPlanId == checkPlanId && sparePartIds.Contains(p.SparePartId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var spareId in sparePartIds)
            {
                var record = dbChangeList.FirstOrDefault(p => p.SparePartId == spareId);
                if (record != null)
                {
                    throw new ValidationException("已存在备件[{0}]的更换记录".L10nFormat(record.SparePartNameView));
                }
            }

            EntityList<CheckPlanSparePart> checkPlanSpareParts = new EntityList<CheckPlanSparePart>();
            foreach (var sparePartId in sparePartIds)
            {
                CheckPlanSparePart change = new CheckPlanSparePart
                {
                    CheckPlanId = checkPlanId,
                    SparePartId = sparePartId,
                    ChangeQty = 1,
                };
                checkPlanSpareParts.Add(change);
            }
            RF.BatchInsert(checkPlanSpareParts);
        }

        /// <summary>
        /// 删除备件更换单
        /// </summary>
        /// <param name="Id"></param>
        public virtual void DeleteSelSpareChange(double Id)
        {
            DB.Delete<CheckPlanSparePart>().Where(p => p.Id == Id).Execute();
        }
        #endregion

        #region 点检记录 
        /// <summary>
        /// 获取设备点检记录
        /// </summary>
        /// <param name="id">设备点检记录Id</param>
        /// <returns>设备点检记录</returns>
        public virtual CheckRecord GetCheckRecordById(double id)
        {
            return Query<CheckRecord>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询设备点检记录列表
        /// </summary>
        /// <param name="criteria">设备点检记录查询体</param>
        /// <returns>备点检记录列表</returns>
        public virtual EntityList QueryCheckPlanLog(CheckRecordCriteria criteria)
        {
            var q = Query<CheckRecord>();

            if (criteria.CheckPlanNo.IsNotEmpty())
            {
                q.Where(w => w.CheckPlanNo.Contains(criteria.CheckPlanNo));
            }

            if (criteria.EquipAccountId.HasValue)
            {
                q.Where(w => w.EquipAccountId == criteria.EquipAccountId.Value);
            }

            if (criteria.WorkshopId.HasValue)
            {
                q.Where(w => w.EquipAccount.WorkShopId == criteria.WorkshopId);
            }

            if (criteria.LineId.HasValue)
            {
                q.Where(w => w.EquipAccount.ResourceId == criteria.LineId.Value);
            }

            if (criteria.MachineNo.IsNotEmpty())
            {
                q.Where(w => w.EquipAccount.Name.Contains(criteria.MachineNo));
            }

            if (criteria.ExeState != null)
            {
                if (criteria.ExeState == CheckExeState.Overdue)
                {
                    q.Where(w => w.ExeState == CheckExeState.NotPerformed && w.CheckEndDate < DateTime.Now);
                }
                else if (criteria.ExeState == CheckExeState.NotPerformed)
                {
                    q.Where(w => w.ExeState == CheckExeState.NotPerformed && w.CheckEndDate >= DateTime.Now);
                }
                else
                {
                    q.Where(w => w.ExeState == criteria.ExeState);
                }

            }

            if (criteria.DepartmentId.HasValue)
            {
                q.Where(w => w.DepartmentId == criteria.DepartmentId.Value);
            }

            if (criteria.PlanCheckDate.BeginValue.HasValue)
            {
                q.Where(p => p.CheckBeginDate >= criteria.PlanCheckDate.BeginValue);
            }

            if (criteria.PlanCheckDate.EndValue.HasValue)
            {
                q.Where(p => p.CheckEndDate < criteria.PlanCheckDate.EndValue);
            }

            if (criteria.CheckEmployeeId != null && criteria.CheckEmployeeId != 0)
            {
                q.Where(p => p.CheckEmployeeId == criteria.CheckEmployeeId);
            }

            if (criteria.ExeResult.HasValue) {
                q.Where(p => p.ExeResult == criteria.ExeResult.Value);
            }

            if (criteria.ConfirmResult.HasValue)
            {
                q.Where(p => p.ConfirmResult == criteria.ConfirmResult.Value);
            }
            q.OrderBy(criteria.OrderInfoList);

            // 懒加载
            var elo = new EagerLoadOptions().LoadWithViewProperty().LoadWith(CheckRecord.EquipAccountProperty);

            // 权限过滤
            var iquery = q.ToQuery().QueryWithEquipAccountPermissions(CheckPlan.EquipAccountIdProperty.Name);
            var list = q.Repository.QueryList(iquery, paging : criteria.PagingInfo, eagerLoad: elo);

            foreach (var i in list)
            {
                var item = i as CheckRecord;
                if (item.CheckEndDate < DateTime.Now && item.ExeState == CheckExeState.NotPerformed)
                {
                    item.ExeState = CheckExeState.Overdue;
                }

                item.ExeStateName = item.ExeState.ToLabel();//为了拿到枚举的label
            }

            return list;
        }

        /// <summary>
        /// 是否启用评分
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNeedMark()
        {
            // 是否启用评分
            var needScoreConfig = ConfigService.GetConfig<CheckConfirmDepartConfigValue>(new CheckConfirmDepartConfig(), typeof(CheckPlanViewModel));
            bool isNeedScore = false;
            if (needScoreConfig != null)
            {
                isNeedScore = needScoreConfig.IsMarkScore;
            }
            return isNeedScore;
        }

        /// <summary>
        /// 更新确认后的执行状态
        /// </summary>
        /// <param name="isNeedScore">是否评分</param>
        /// <param name="plan">点检计划</param>
        /// <param name="planInfo">计划信息</param>
        /// <param name="checkPlanConfirmItems">用于保存点检确认项</param>
        private void CheckConfirmUpdateState(bool isNeedScore, CheckPlan plan, CheckPlanInfo planInfo, EntityList<CheckPlanConfirmItem> checkPlanConfirmItems)
        {
            // 更新确认项目
            var confirmItemList = Query<CheckPlanConfirmItem>().Where(p => p.CheckPlanId == plan.Id).ToList();
            var confirmItem = confirmItemList.FirstOrDefault(p => p.CheckPlanId == plan.Id && p.DepartmentId == planInfo.ConfirmDeptId);
            if (confirmItem == null)
            {
                throw new ValidationException("不存在部门Id[{0}]的点检确认项".L10nFormat(planInfo.ConfirmDeptId));
            }
            else
            {
                confirmItem.CheckExeState = CheckExeState.Confirmed;
                confirmItem.ConfirmResult = planInfo.ConfirmResult == 1 ? ConfirmResult.OK : ConfirmResult.NG;
                confirmItem.ConfirmNote = planInfo.ConfirmNote;
                checkPlanConfirmItems.Add(confirmItem);
            }
            // 确认项是否都已确认
            var noConfirmCount = confirmItemList.Count(p => p.CheckExeState == CheckExeState.NotConfirm && p.Id != confirmItem.Id);
            if (noConfirmCount <= 0)
            {
                plan.ExeState = CheckExeState.Confirmed;
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
                var noMarkList = Query<CheckConfirmation>().Where(p => p.OwnerId == plan.Id).ToList();
                if (noMarkList.Count > 0)
                {
                    confirmItem.CheckExeState = CheckExeState.Scored;
                    if (noMarkList.Count(p => p.ConfirmResult == null) == 0)
                    {
                        plan.ExeState = CheckExeState.Scored;
                    }
                }
            }
        }

        /// <summary>
        /// 点检确认提交计划信息、更新确认项
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="planInfo"></param>
        /// <param name="dbDate"></param>
        /// <exception cref="ValidationException"></exception>
        private void CheckConfirmSubmitCheckItem(CheckPlan plan, CheckPlanInfo planInfo, DateTime dbDate)
        {
            if (plan == null) { throw new ValidationException("点检单不存在，[ID:{0}]".L10nFormat(planInfo.Id)); }
            if (plan.ExeState != CheckExeState.NotConfirm) { throw new ValidationException("点检单[{0}]是[{1}]状态，不允许进行点检确认".L10nFormat(plan.CheckPlanNo, plan.ExeState.ToLabel())); }

            //修改点检计划的确认结果和确认备注
            if (plan.ConfirmNote.IsNullOrEmpty())
            {
                plan.ConfirmNote = planInfo.ConfirmNote;
            }
            else
            {
                plan.ConfirmNote += ";" + planInfo.ConfirmNote;
            }
            plan.ConfirmDate = dbDate;

        }

        /// <summary>
        /// 点检确认提交评分项
        /// </summary>
        /// <param name="info">确认信息</param>
        /// <param name="planInfo">计划信息</param>
        /// <param name="checkConfirmations">评分项</param>
        /// <param name="dbDate">数据库时间</param>
        /// <exception cref="ValidationException"></exception>
        private void CheckConfirmSubmitMark(CheckConfirmationSubmitInfo[] info, CheckPlanInfo planInfo, EntityList<CheckConfirmation> checkConfirmations, DateTime dbDate)
        {
            EntityList<CheckConfirmation> scoreList = new EntityList<CheckConfirmation>();

            //保存确认结果
            var hepler = new FileUrlHelper();

            // 评分id
            var tpmIds = info.Select(p => p.TpmScoreProjectId).Distinct().ToList();
            tpmIds.SplitDataExecute(tempIds =>
            {
                var list = Query<CheckConfirmation>()
                .Where(p => p.OwnerId == planInfo.Id && tempIds.Contains(p.TpmScoreProjectId))
                .WhereIf(planInfo.ConfirmDeptId != null && planInfo.ConfirmDeptId != 0, p => p.ConfirmDeptId == planInfo.ConfirmDeptId).ToList();
                scoreList.AddRange(list);
            });
            // 点检确认评分项
            info.ForEach(i =>
            {
                var checkConfirmation = scoreList.FirstOrDefault(p => p.OwnerId == i.CheckPlanId && p.TpmScoreProjectId == i.TpmScoreProjectId);
                if (checkConfirmation == null)
                {
                    string msg = $"ID为{i.CheckPlanId}的点检单找不到".L10N();
                    if (planInfo.ConfirmDeptId != null && planInfo.ConfirmDeptId != 0) { msg += $"部门ID为{planInfo.ConfirmDeptId}、"; }
                    msg += $"评分项ID为{i.TpmScoreProjectId}的项！".L10N();
                    throw new ValidationException(msg);
                }
                var checkConfirmationNew = new CheckConfirmation();
                checkConfirmationNew.Clone(checkConfirmation, SIE.Domain.CloneOptions.NewSingleComposition());
                if (i.Content.IsNotEmpty())
                {
                    var content = i.Content.Substring(i.Content.IndexOf(",") + 1);
                    checkConfirmationNew.Content = Convert.FromBase64String(content);
                    checkConfirmationNew.FileExtesion = i.FileExtesion;
                    checkConfirmationNew.FileName = i.FileName;
                    checkConfirmationNew.FilePath = i.FilePath;
                    checkConfirmationNew.FileSize = i.FileSize;
                }
                checkConfirmationNew.ConfirmResult = (ConfirmResult)i.ConfirmResult;
                checkConfirmationNew.ConfirmNote = i.ConfirmNote;
                checkConfirmationNew.Score = (Score)i.Score;
                checkConfirmationNew.ConfirmorId = RT.IdentityId;
                checkConfirmationNew.ConfirmDate = dbDate;
                checkConfirmationNew.PersistenceStatus = PersistenceStatus.New;

                checkConfirmations.Add(checkConfirmationNew);
            });
        }

        /// <summary>
        /// 验证点检确认信息
        /// </summary>
        /// <param name="info">点检确认信息</param>
        /// <param name="isNeedScore">是否启用评分</param>
        private void CheckConfirmSubmitValidate(CheckConfirmationSubmitInfo[] info, bool isNeedScore)
        {
            // 验证
            foreach (var item in info)
            {
                if (item.CheckPlanId == null) throw new ValidationException("点检单ID不能为空！".L10N());
                if (item.ConfirmResult == null) throw new ValidationException("确认结果不能为空！".L10N());
                if (item.CheckPlanId != info[0].CheckPlanId) throw new ValidationException("点检单ID不一致！".L10N());
                if (item.ConfirmResult != info[0].ConfirmResult) throw new ValidationException("同一点检单的确认结果应当一致！".L10N());
                if (item.ConfirmNote != info[0].ConfirmNote) throw new ValidationException("同一点检单的确认备注应当一致！".L10N());
                if (item.ConfirmDeptId != info[0].ConfirmDeptId) throw new ValidationException("同一点检单的确认部门应当一致！".L10N());
                if (item.ConfirmResult == 2 && string.IsNullOrWhiteSpace(item.ConfirmNote)) throw new ValidationException("确认结果不合格时备注不能为空！".L10N());
                if (isNeedScore) // 配置项启用评分
                {
                    if (item.TpmScoreProjectId == null) throw new ValidationException("评分项ID不能为空！".L10N());
                    if (item.Score == null) throw new ValidationException("评分不能为空！".L10N());
                    if (item.Score <= 0 || item.Score >= 6) throw new ValidationException("评分只能是1到5分！".L10N());

                }
            }
        }
        #endregion


    }
}