using MimeKit;
using SIE.AbnormalInfo.AbnormalMonitors.Configs;
using SIE.AbnormalInfo.AbnormalMonitors.Pushers;
using SIE.AbnormalInfo.AbnormalMonitors.ViewModels;
using SIE.AbnormalInfo.Common;
using SIE.Common;
using SIE.Common.Alert;
using SIE.Common.Configs;
using SIE.Common.Organizations;
using SIE.Common.Sender;
using SIE.Core.Common.Controllers;
using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 
    /// </summary>
    public class PushTargetController : DomainController
    {
        #region 推送对象获取数据
        /// <summary>
        /// 推送对象获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<PushTargetViewModel> GetPushTargetData(PushTargetEnum type, string keyword, PagingInfo pagingInfo)
        {
            if (type == PushTargetEnum.Staff)
            {
                var query = Query<Employee>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<PushTargetViewModel> pushObjectList = new EntityList<PushTargetViewModel>();
                query.ForEach(item =>
                {
                    var pushTarget = new PushTargetViewModel();
                    pushTarget.TargetId = item.Id;
                    pushTarget.TargetCode = item.Code;
                    pushTarget.TargetName = item.Name;
                    pushObjectList.Add(pushTarget);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else if (type == PushTargetEnum.Role)
            {
                var query = Query<Role>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<PushTargetViewModel> pushObjectList = new EntityList<PushTargetViewModel>();
                query.ForEach(item =>
                {
                    var pushTarget = new PushTargetViewModel();
                    pushTarget.TargetId = item.Id;
                    pushTarget.TargetCode = item.Code;
                    pushTarget.TargetName = item.Name;
                    pushObjectList.Add(pushTarget);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else if (type == PushTargetEnum.UserGroup)
            {
                var query = base.Query<UserGroup>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<PushTargetViewModel> pushObjectList = new EntityList<PushTargetViewModel>();
                query.ForEach(item =>
                {
                    var pushTarget = new PushTargetViewModel();
                    pushTarget.TargetId = item.Id;
                    pushTarget.TargetCode = item.Code;
                    pushTarget.TargetName = item.Name;
                    pushObjectList.Add(pushTarget);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else if (type == PushTargetEnum.Department)
            {
                var query = base.Query<Organization>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).Where(p => p.Level.Type == OrganizationType.Department && p.InvOrgId == RT.InvOrg).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<PushTargetViewModel> pushObjectList = new EntityList<PushTargetViewModel>();
                query.ForEach(item =>
                {
                    var pushTarget = new PushTargetViewModel();
                    pushTarget.TargetId = item.Id;
                    pushTarget.TargetCode = item.Code;
                    pushTarget.TargetName = item.Name;
                    pushObjectList.Add(pushTarget);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else
            {
                return new EntityList<PushTargetViewModel>();
            }
        }
        #endregion

        #region 调度推送预警

        #region 1.入口

        /// <summary>
        /// 调度推送预警
        /// </summary>
        /// <param name="curTime"></param>
        public virtual void PushMessageJob(DateTime curTime)
        {
            //1.集中获取数据（在使用中要注意不要使用引用属性触发懒加载）
            EntityList<AbnormalMonitorTask> workingTasks = new EntityList<AbnormalMonitorTask>();
            EntityList<AbnormalDefine> defines = new EntityList<AbnormalDefine>();
            EntityList<AbnormalWarnDefine> warnDefines = new EntityList<AbnormalWarnDefine>();
            EntityList<PushUpgradeRule> upgradeRules = new EntityList<PushUpgradeRule>();
            EntityList<PushTarget> pushTargets = new EntityList<PushTarget>();
            EntityList<AbnormalMonitorTaskLog> taskLogs = new EntityList<AbnormalMonitorTaskLog>();
            EntityList<AbnormalMonitorSendLog> sendLogs = new EntityList<AbnormalMonitorSendLog>();
            PushUpgradeRuleTimeConfigValue pushUpgradeRuleTimeConfigValue = new PushUpgradeRuleTimeConfigValue();
            GetDatas(ref workingTasks, ref defines, ref warnDefines, ref upgradeRules, ref pushTargets, ref taskLogs, ref sendLogs, ref pushUpgradeRuleTimeConfigValue);

            //2.业务计算（不涉及数据库查询）
            var pushTargeSends = GetPushMessages(curTime, workingTasks, defines, warnDefines, upgradeRules, pushTargets, taskLogs, sendLogs, pushUpgradeRuleTimeConfigValue);
            //3.【推送升级机制】的【推送对象】统一转化为【员工】（涉及数据库查询）
            PushSendClassGenerateEmployees(pushTargeSends);

            if (pushTargeSends.IsNullOrEmpty()) return;

            //4.推送消息（包括数据保存）
            PushMessages(pushTargeSends);


        }

        #endregion

        #region 2.集中获取数据

        private void GetDatas(ref EntityList<AbnormalMonitorTask> workingTasks, ref EntityList<AbnormalDefine> defines, ref EntityList<AbnormalWarnDefine> warnDefines, ref EntityList<PushUpgradeRule> upgradeRules, ref EntityList<PushTarget> pushTargets, ref EntityList<AbnormalMonitorTaskLog> taskLogs, ref EntityList<AbnormalMonitorSendLog> sendLogs, ref PushUpgradeRuleTimeConfigValue pushUpgradeRuleTimeConfigValue)
        {
            //1.获取正在工作的【异常任务】
            workingTasks = GetTasks();
            var taskIds = workingTasks.Select(c => c.Id).ToList();
            //2.集中获取【异常定义】
            var defineIds = workingTasks.Where(c => c.AbnormalDefineId.HasValue).Select(x => (double)x.AbnormalDefineId).Distinct().ToList();
            defines = GetDefines(defineIds);
            //3.集中获取【预警定义】
            var warnDefineIds = defines.Where(c => c.AbnormalWarnDefineId.HasValue).Select(c => (double)c.AbnormalWarnDefineId).Distinct().ToList();
           var warnDefineIdsByTask= workingTasks.Where(c=>c.AbnormalWarnDefineId.HasValue).Select(x => (double)x.AbnormalWarnDefineId).Distinct().ToList();
            if (warnDefineIdsByTask.IsNotEmpty())
                warnDefineIds.AddRange(warnDefineIdsByTask);
            warnDefines = GetWarnDefines(warnDefineIds);
            //4.集中获取【推送升级机制】
            upgradeRules = GetPushUpgradeRules(warnDefineIds);
            var upgradeRuleIds = upgradeRules.Select(c => c.Id).ToList();
            //5.集中获取【推送对象】
            pushTargets = GetPushTargets(upgradeRuleIds);
            //6.集中获取【异常任务处理日志】
            taskLogs = GetTaskLogs(taskIds);
            //7.集中获取【推送日志】
            sendLogs = GetSendLogs(taskIds);
            //8.【推送升级机制】循环次数配置项
            pushUpgradeRuleTimeConfigValue = GetPushUpgradeRuleTimeConfig();

        }

        /// <summary>
        ///获取【异常任务】
        /// </summary>
        /// <returns></returns>
        private EntityList<AbnormalMonitorTask> GetTasks()
        {
            //有关联异常预警定义的任务
            var query = Query<AbnormalMonitorTask>()
                .Where(c => c.AbnormalWarnDefineId != null && !(c.WarnTimes>0 && (c.TaskState== TaskStateEnum.Cancel || c.TaskState == TaskStateEnum.Done)));
            ////测试
            //var query = Query<AbnormalMonitorTask>().Where(c => c.Code == "YCRW20230721014");
            return query.ToList();
        }

        /// <summary>
        /// 集中获取异常定义
        /// </summary>
        /// <param name="defineIds"></param>
        /// <returns></returns>
        private EntityList<AbnormalDefine> GetDefines(List<double> defineIds)
        {
            var defines = defineIds.SplitContains(tempIds =>
            {
                return Query<AbnormalDefine>().Where(c => defineIds.Contains(c.Id)).ToList();
            });
            return defines;
        }

        /// <summary>
        ///集中获取预警定义
        /// </summary>
        /// <param name="warnDefineIds">预警定义Id</param>
        /// <returns></returns>
        private EntityList<AbnormalWarnDefine> GetWarnDefines(List<double> warnDefineIds)
        {
            var warnDefines = warnDefineIds.SplitContains(tempIds =>
            {
                return Query<AbnormalWarnDefine>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });
            return warnDefines;
        }

        /// <summary>
        /// 集中获取推送升级机制
        /// </summary>
        /// <param name="warnDefineIds"></param>
        /// <returns></returns>
        private EntityList<PushUpgradeRule> GetPushUpgradeRules(List<double> warnDefineIds)
        {
            var pushUpgradeRules = warnDefineIds.SplitContains(tempIds =>
            {
                return Query<PushUpgradeRule>()
                    .Where(x => tempIds.Contains(x.AbnormalWarnDefineId))
                    .ToList();
            });
            return pushUpgradeRules;
        }

        /// <summary>
        /// 集中获取推送对象
        /// </summary>
        /// <param name="upgradeRuleIds"></param>
        /// <returns></returns>
        private EntityList<PushTarget> GetPushTargets(List<double> upgradeRuleIds)
        {
            var pushTargets = upgradeRuleIds.SplitContains(tempIds =>
            {
                return Query<PushTarget>()
                    .Where(x => tempIds.Contains(x.PushUpgradeRuleId))
                    .ToList();
            });
            return pushTargets;
        }

        /// <summary>
        /// 集中获取推送日志
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        private EntityList<AbnormalMonitorSendLog> GetSendLogs(List<double> taskIds)
        {
            var sendLogs = taskIds.SplitContains(tempIds =>
            {
                return Query<AbnormalMonitorSendLog>()
                    .Where(x => tempIds.Contains(x.AbnormalMonitorTaskId))
                    .ToList();
            });
            return sendLogs;
        }

        /// <summary>
        /// 集中获取任务处理日志
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        private EntityList<AbnormalMonitorTaskLog> GetTaskLogs(List<double> taskIds)
        {
            var sendLogs = taskIds.SplitContains(tempIds =>
            {
                return Query<AbnormalMonitorTaskLog>()
                    .Where(x => tempIds.Contains(x.AbnormalMonitorTaskId))
                    .ToList();
            });
            return sendLogs;
        }

        /// <summary>
        /// 集中获取任务自定义推送对象
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        private EntityList<TaskCustomPushTarget> GetTaskCustomPushTargets(List<double> taskIds)
        {
            var sendLogs = taskIds.SplitContains(tempIds =>
            {
                return Query<TaskCustomPushTarget>()
                    .Where(x => tempIds.Contains(x.AbnormalMonitorTaskId))
                    .ToList();
            });
            return sendLogs;
        }

        /// <summary>
        /// 获取推送方式
        /// </summary>
        /// <param name="pusherIds">推送方式Id</param>
        private EntityList<Pusher> GetPushers(List<double> pusherIds)
        {
            if (pusherIds.IsNullOrEmpty()) return new EntityList<Pusher>();
            var pushers = pusherIds.SplitContains(tempIds =>
            {
                return Query<Pusher>().Where(c => tempIds.Contains(c.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return pushers;
        }

        /// <summary>
        /// 获取推送方式插件
        /// </summary>
        /// <param name="pushPlugIds">推送方式插件Id</param>
        private EntityList<PushPlug> GetPushPlugs(List<double> pushPlugIds)
        {
            if (pushPlugIds.IsNullOrEmpty()) return new EntityList<PushPlug>();
            var pushers = pushPlugIds.SplitContains(tempIds =>
            {
                return Query<PushPlug>().Where(c => tempIds.Contains(c.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return pushers;
        }

        private PushUpgradeRuleTimeConfigValue GetPushUpgradeRuleTimeConfig()
        {
            var config = ConfigService.GetConfig(new PushUpgradeRuleTimeConfig(), typeof(AbnormalWarnDefine));//循环次数
            return config;
        }
        #endregion

        #region 3.推算获取推送类集合

        private List<PushSend> GetPushMessages(DateTime curTime, EntityList<AbnormalMonitorTask> tasks, EntityList<AbnormalDefine> defines, EntityList<AbnormalWarnDefine> warnDefines, EntityList<PushUpgradeRule> upgradeRules, EntityList<PushTarget> pushTargets, EntityList<AbnormalMonitorTaskLog> taskLogs, EntityList<AbnormalMonitorSendLog> sendLogs, PushUpgradeRuleTimeConfigValue pushUpgradeRuleTimeConfigValue)
        {
            List<PushSend> pushSends = new List<PushSend>();
            //1.循环【异常任务】构建推送类（一条推送类对应一条任务）
            foreach (var task in tasks)
            {
                //1.1.推送类
                PushSend pushSend = new PushSend();
                pushSend.AbnormalMonitorTask = task;
                pushSend.PushSendChildrens = new List<PushSendChildren>();

                //1.2.获取相关联的数据集（包括定义、预警定义、等等）
                //注释：
                //避免操作中使用引用对象导致懒加载，应根据Id在提前准备的数据源中寻找
                var singleDefine = defines.FirstOrDefault(c => c.Id == task.AbnormalDefineId);
                var singleWarnDefine = warnDefines.FirstOrDefault(c => c.Id == task.AbnormalWarnDefineId);//第二版，使用task中的预警定义
                if (singleWarnDefine == null) continue;
                //获取任务单状态相同的【升级机制】，并按间隔时间升序排序
                var belongUpgradeRules = upgradeRules.Where(c => c.AbnormalWarnDefineId == singleWarnDefine.Id && c.AbnormalNode == task.TaskState).OrderBy(c => c.IntervalTime);
                pushSend.AbnormalDefine = singleDefine;
                pushSend.AbnormalWarnDefine = singleWarnDefine;


                //1.3.寻找符合条件的【推送升级机制】，并构建推送子类（一条推送子类对应一条【推送升级机制】）
                //注释：
                //第一步：获取【异常任务】最后的【处理日志】，以其创建时间作为【最后处理时间】，无【处理日志】则以【异常任务】的创建时间为【最后处理时间】
                //第二步：单条【推送升级机制】进行业务推算
                var lastNodeLog = taskLogs.Where(c => c.AbnormalMonitorTaskId == task.Id).OrderByDescending(c => c.CreateDate).FirstOrDefault();
                DateTime lastProcessTime;
                if (lastNodeLog == null)
                    lastProcessTime = task.CreateDate;
                else
                    lastProcessTime = lastNodeLog.CreateDate;

                foreach (var upgradRule in belongUpgradeRules)
                {
                    //注释：
                    //【推送升级机制】节点=【异常任务】状态
                    //【推送升级机制】历史推送日志次数n
                    //理论推送时间=（最后推送时间||最后处理时间）+1*IntervalTime
                    //构建推送子类：
                    //节点=完成、取消时： 理论推送时间<=当前时间 and n==0
                    //节点!=完成、取消 时： 理论推送时间<=当前时间 and n<循环次数配置值

                    var historySendLogs = sendLogs.Where(c => c.AbnormalMonitorTaskId == task.Id && c.PushUpgradeRuleId == upgradRule.Id).OrderBy(c => c.CreateDate).ToList();
                    int n = 0;
                    DateTime? lastSendTime = null;
                    if (historySendLogs.IsNotEmpty())
                    {
                        n = historySendLogs.Count;
                        lastSendTime = historySendLogs.Last().CreateDate;
                    }
                    DateTime? shouldSendTime = null;
                    if (lastSendTime != null)
                        shouldSendTime = lastSendTime.Value.AddMinutes(upgradRule.IntervalTime);
                    else
                        shouldSendTime = lastProcessTime.AddMinutes(upgradRule.IntervalTime);

                    var flag = false;
                    var cyclicTimes = pushUpgradeRuleTimeConfigValue.CyclicTimes.HasValue ? pushUpgradeRuleTimeConfigValue.CyclicTimes.Value : 3;//默认3次
                    if (upgradRule.AbnormalNode == TaskStateEnum.Done || upgradRule.AbnormalNode == TaskStateEnum.Cancel)
                    {
                        if (shouldSendTime <= curTime && n == 0)
                            flag = true;
                    }
                    else
                    {
                        if (shouldSendTime <= curTime && n < cyclicTimes)
                            flag = true;

                    }
                    if (flag)
                    {
                        PushSendChildren pushSendChildren = new PushSendChildren();
                        pushSendChildren.PushUpgradeRule = upgradRule;
                        pushSendChildren.PushTargets = pushTargets.Where(c => c.PushUpgradeRuleId == upgradRule.Id).ToList();//推送对象转员工在外层再统一转化
                        pushSend.PushSendChildrens.Add(pushSendChildren);                       
                    }
                }
                if (pushSend.PushSendChildrens.IsNotEmpty())
                    pushSends.Add(pushSend);
            }

            return pushSends;
        }

        #endregion

        #region 4.推送类构建员工

        private void PushSendClassGenerateEmployees(List<PushSend> pushSends)
        {
            if (pushSends.IsNullOrEmpty()) return;
            var employeeIds = new List<double>();
            var userIds = new List<double>();

            var pushTargets = pushSends.SelectMany(c => c.PushSendChildrens.SelectMany(p => p.PushTargets)).ToList();

            List<EmployeeInPushUpgradeRule> employeeInPushUpgradeRules = new List<EmployeeInPushUpgradeRule>();
            List<UserInPushUpgradeRule> userInPushUpgradeRules = new List<UserInPushUpgradeRule>();
            //1.推送对象的直接员工
            employeeIds.AddRange(GetEmployeeIdsDirect(pushTargets, employeeInPushUpgradeRules));
            //2.角色转化用户
            userIds.AddRange(GetUserIdsFromRoles(pushTargets, userInPushUpgradeRules));
            //3.用户组转化用户
            userIds.AddRange(GetUserIdsFromUserGroups(pushTargets, userInPushUpgradeRules));
            //4.部门转化员工
            employeeIds.AddRange(GetEmployeeIdsFromOrganizations(pushTargets, employeeInPushUpgradeRules));
            //5.自定义推送员工
            if (pushTargets.Exists(c => c.TargetType == PushTargetEnum.Custom))
            {
                employeeIds.AddRange(GetEmployeeIdsFromCustom(pushSends, employeeInPushUpgradeRules));
            }
            //6.集中查询用户
            var users = userIds.Distinct().SplitContains(tempIds =>
            {
                return Query<SIE.Common.Users.User>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            employeeIds.AddRange(users.Where(c => c.EmployeeId.HasValue).Select(c => c.EmployeeId.Value));
            employeeIds = employeeIds.Distinct().ToList();

            //7.集中查询员工
            var employees = employeeIds.Distinct().SplitContains(tempIds =>
            {
                return Query<SIE.Common.Employees.Employee>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });
            //7.推送类根据EmployeeIds或UserIds匹配Employees
            foreach (var pushSend in pushSends)
            {
                foreach (var pushSendChildren in pushSend.PushSendChildrens)
                {
                    var belongEmployeeIds = new List<double>();

                    belongEmployeeIds.AddRange(employeeInPushUpgradeRules.Where(c => c.PushUpgradeRuleId == pushSendChildren.PushUpgradeRule.Id).SelectMany(c => c.EmployeeIds).ToList());
                    var belongUserIds = userInPushUpgradeRules.Where(c => c.PushUpgradeRuleId == pushSendChildren.PushUpgradeRule.Id).SelectMany(c => c.UserIds);
                    var belongUsers = users.Where(c => belongUserIds.Contains(c.Id));
                    var employeeIdsFromUsers = belongUsers.Where(c => c.EmployeeId.HasValue).Select(c => c.EmployeeId.Value);
                    belongEmployeeIds.AddRange(employeeIdsFromUsers);
                    var belongEmployees = employees.Where(c => belongEmployeeIds.Contains(c.Id)).ToList();
                    pushSendChildren.Employees = belongEmployees;
                }

            }
        }

        private List<double> GetEmployeeIdsFromCustom(List<PushSend> pushSends, List<EmployeeInPushUpgradeRule> employeeInPushUpgradeRules)
        {
            var result = new List<double>();
            var taskIds = pushSends.Select(c => c.AbnormalMonitorTask.Id).ToList();
            var customPushTargets = GetTaskCustomPushTargets(taskIds);
            pushSends.ForEach(pushSend =>
            {
                bool isCustomSend = false;
                pushSend.PushSendChildrens.ForEach(pushSendChildren =>
                {
                    isCustomSend = pushSendChildren.PushTargets.Exists(c => c.TargetType == PushTargetEnum.Custom);
                    if (isCustomSend)
                    {
                        var employeeInPushUpgradeRule = employeeInPushUpgradeRules.FirstOrDefault(c => c.PushUpgradeRuleId == pushSendChildren.PushUpgradeRule.Id);
                        if (employeeInPushUpgradeRule == null)
                        {
                            employeeInPushUpgradeRule = new EmployeeInPushUpgradeRule() { PushUpgradeRuleId = pushSendChildren.PushUpgradeRule.Id, EmployeeIds = new List<double>() };
                            employeeInPushUpgradeRules.Add(employeeInPushUpgradeRule);
                        }
                        var employeeIds = customPushTargets.Where(c => c.AbnormalMonitorTaskId == pushSend.AbnormalMonitorTask.Id && c.TaskState == pushSendChildren.PushUpgradeRule.AbnormalNode).Select(c => c.EmployeeId).ToList();
                        result.AddRange(employeeIds);
                        employeeInPushUpgradeRule.EmployeeIds.AddRange(employeeIds);
                    }

                });

            });
            return result;
        }

        private List<double> GetEmployeeIdsFromOrganizations(List<PushTarget> pushTargets, List<EmployeeInPushUpgradeRule> employeeInPushUpgradeRules)
        {
            var pushTargetOrganizations = pushTargets.Where(c => c.TargetType == PushTargetEnum.Department).Select(c => new { c.TargetId, c.PushUpgradeRuleId });
            var pushTargetOrganizationsGroup = pushTargetOrganizations.GroupBy(c => c.PushUpgradeRuleId);
            var organizationIds = pushTargetOrganizations.Select(c => c.TargetId).Distinct().ToList();

            var org2Employees = organizationIds.SplitContains(tempIds =>
            {
                return Query<Org2Employee>().Where(x => tempIds.Contains(x.OrganizationId))
                    .ToList();
            });
            if (org2Employees.IsNotEmpty())
            {
                foreach (var item in pushTargetOrganizationsGroup)
                {
                    var employeeInPushUpgradeRule = employeeInPushUpgradeRules.FirstOrDefault(c => c.PushUpgradeRuleId == item.Key);
                    if (employeeInPushUpgradeRule == null)
                    {
                        employeeInPushUpgradeRule = new EmployeeInPushUpgradeRule() { PushUpgradeRuleId = item.Key, EmployeeIds = new List<double>() };
                        employeeInPushUpgradeRules.Add(employeeInPushUpgradeRule);
                    }
                    var belongemployeeGroupIds = item.Select(c => c.TargetId).ToList();
                    var employeeIds = org2Employees.Where(c => belongemployeeGroupIds.Contains(c.OrganizationId)).Select(c => c.EmployeeId).ToList();
                    employeeInPushUpgradeRule.EmployeeIds.AddRange(employeeIds);
                    employeeInPushUpgradeRule.EmployeeIds = employeeInPushUpgradeRule.EmployeeIds.Distinct().ToList();

                }
            }
            return org2Employees.Select(c => c.EmployeeId).ToList();
        }

        private List<double> GetUserIdsFromUserGroups(List<PushTarget> pushTargets, List<UserInPushUpgradeRule> userInPushUpgradeRules)
        {
            var pushTargetUserGroups = pushTargets.Where(c => c.TargetType == PushTargetEnum.UserGroup).Select(c => new { c.TargetId, c.PushUpgradeRuleId });
            var pushTargetUserGroupGroup = pushTargetUserGroups.GroupBy(c => c.PushUpgradeRuleId);
            var userGroupIds = pushTargetUserGroups.Select(c => c.TargetId).Distinct().ToList();

            var userInUserGroupQueryResult = userGroupIds.SplitContains(tempIds =>
            {
                return Query<UserInUserGroup>().Where(x => tempIds.Contains(x.UserGroupId)).ToList();
            });
            if (userInUserGroupQueryResult.IsNotEmpty())
            {

                foreach (var item in pushTargetUserGroupGroup)
                {
                    var userInPushUpgradeRule = userInPushUpgradeRules.FirstOrDefault(c => c.PushUpgradeRuleId == item.Key);
                    if (userInPushUpgradeRule == null)
                    {
                        userInPushUpgradeRule = new UserInPushUpgradeRule() { PushUpgradeRuleId = item.Key, UserIds = new List<double>() };
                        userInPushUpgradeRules.Add(userInPushUpgradeRule);
                    }
                    var belongUserGroupIds = item.Select(c => c.TargetId).ToList();
                    var userIds = userInUserGroupQueryResult.Where(c => belongUserGroupIds.Contains(c.UserGroupId)).Select(c => c.UserId).ToList();
                    userInPushUpgradeRule.UserIds.AddRange(userIds);
                    userInPushUpgradeRule.UserIds = userInPushUpgradeRule.UserIds.Distinct().ToList();

                }
            }
            return userInUserGroupQueryResult.Select(c => c.UserId).ToList();
        }

        private List<double> GetEmployeeIdsDirect(List<PushTarget> pushTargets, List<EmployeeInPushUpgradeRule> employeeInPushUpgradeRules)
        {
            var pushTargetEmployees = pushTargets.Where(c => c.TargetType == PushTargetEnum.Staff).Select(c => new { c.TargetId, c.PushUpgradeRuleId });
            var pushTargetEmployeeGroup = pushTargetEmployees.GroupBy(c => c.PushUpgradeRuleId);
            foreach (var item in pushTargetEmployeeGroup)
            {
                var employeeInPushUpgradeRule = employeeInPushUpgradeRules.FirstOrDefault(c => c.PushUpgradeRuleId == item.Key);
                if (employeeInPushUpgradeRule == null)
                    employeeInPushUpgradeRules.Add(new EmployeeInPushUpgradeRule() { PushUpgradeRuleId = item.Key, EmployeeIds = item.Select(c => c.TargetId).ToList() });
                else
                {
                    employeeInPushUpgradeRule.EmployeeIds.AddRange(item.Select(c => c.TargetId).ToList());
                    employeeInPushUpgradeRule.EmployeeIds = employeeInPushUpgradeRule.EmployeeIds.Distinct().ToList();
                }
            }
            return pushTargetEmployees.Select(c => c.TargetId).ToList();
        }

        private List<double> GetUserIdsFromRoles(List<PushTarget> pushTargets, List<UserInPushUpgradeRule> userInPushUpgradeRules)
        {
            List<double> result;
            var pushTargetRoles = pushTargets.Where(c => c.TargetType == PushTargetEnum.Role).Select(c => new { c.TargetId, c.PushUpgradeRuleId });
            var pushTargetRoleGroup = pushTargetRoles.GroupBy(c => c.PushUpgradeRuleId);
            var roleIds = pushTargetRoles.Select(c => c.TargetId).Distinct().ToList();


            List<KeyItems> roleInPushUpgrades = new List<KeyItems>();
            foreach (var item in pushTargetRoleGroup)
            {
                roleInPushUpgrades.Add(new KeyItems() { Key = item.Key, Items = item.Select(c => c.TargetId).ToList() });
            }
            List<KeyItems> userInRoles = new List<KeyItems>();
            //角色下的第一层用户

            var userInRoleQueryResult = roleIds.SplitContains(tempIds =>
            {
                return Query<UserInRole>().Where(x => tempIds.Contains(x.RoleId)).ToList();
            });
            if (userInRoleQueryResult.IsNotEmpty())
            {
                roleIds.ForEach(roleId =>
                {
                    var items = userInRoleQueryResult.Where(c => c.RoleId == roleId).Select(c => c.UserId).ToList();
                    if (items.IsNotEmpty())
                    {
                        var userInRole = userInRoles.FirstOrDefault(c => c.Key == roleId);
                        if (userInRole == null)
                            userInRoles.Add(new KeyItems() { Key = roleId, Items = items });
                        else
                        {
                            userInRole.Items.AddRange(items);
                            userInRole.Items = userInRole.Items.Distinct().ToList();
                        }
                    }
                });
            }

            //角色下的用户组的用户
            List<KeyItems> userGroupInRoles = new List<KeyItems>();
            var userGroupInRoleQueryResult = roleIds.SplitContains(tempIds =>
            {
                return Query<UserGroupInRole>().Where(x => tempIds.Contains(x.RoleId)).ToList();
            });
            if (userGroupInRoleQueryResult.IsNotEmpty())
            {
                roleIds.ForEach(roleId =>
                {
                    var items = userGroupInRoleQueryResult.Where(c => c.RoleId == roleId).Select(c => c.UserGroupId).ToList();
                    if (items.IsNotEmpty())
                    {
                        userGroupInRoles.Add(new KeyItems() { Key = roleId, Items = items });
                    }
                });
                var allUserGroupIds = userGroupInRoleQueryResult.Select(c => c.UserGroupId).ToList();

                var userInUserGroupQueryResult = allUserGroupIds.SplitContains(tempIds =>
                {
                    return Query<UserInUserGroup>().Where(x => tempIds.Contains(x.UserGroupId)).ToList();
                });
                if (userInUserGroupQueryResult.IsNotEmpty())
                {
                    userGroupInRoles.ForEach(userGroupInRole =>
                    {
                        var userIds = userInUserGroupQueryResult.Where(c => userGroupInRole.Items.Contains(c.UserGroupId)).Select(c => c.UserId).ToList();

                        if (userIds.IsNotEmpty())
                        {
                            var userInRole = userInRoles.FirstOrDefault(c => c.Key == userGroupInRole.Key);
                            if (userInRole == null)
                                userInRoles.Add(new KeyItems() { Key = userGroupInRole.Key, Items = userIds });
                            else
                            {
                                userInRole.Items.AddRange(userIds);
                                userInRole.Items = userInRole.Items.Distinct().ToList();
                            }
                        }

                    });
                }
            }
            roleInPushUpgrades.ForEach(roleInPushUpgrade =>
            {
                var userInPushUpgradeRule = userInPushUpgradeRules.FirstOrDefault(c => c.PushUpgradeRuleId == roleInPushUpgrade.Key);
                if (userInPushUpgradeRule == null)
                {
                    userInPushUpgradeRule = new UserInPushUpgradeRule() { PushUpgradeRuleId = roleInPushUpgrade.Key, UserIds = new List<double>() };
                    userInPushUpgradeRules.Add(userInPushUpgradeRule);
                }

                var userIds = userInRoles.Where(c => roleInPushUpgrade.Items.Contains(c.Key)).SelectMany(c => c.Items);
                userInPushUpgradeRule.UserIds.AddRange(userIds);
            });


            result = userInRoles.SelectMany(c => c.Items).ToList();
            return result;
        }

        #endregion

        #region 5.信息推送(包括保存推送日志)

        /// <summary>
        /// 对象
        /// </summary>
        object g_locker = new object();
        /// <summary>
        /// 推送方式发送(包括保存推送日志)
        /// </summary>
        /// <param name="pushSends"></param>
        private void PushMessages(List<PushSend> pushSends)
        {
            if (pushSends.IsNullOrEmpty()) return;


            var pusherIds = pushSends.SelectMany(c => c.PushSendChildrens).Select(c => c.PushUpgradeRule.PusherId).ToList();
            var pushers = this.GetPushers(pusherIds);
            if (pushers.IsNullOrEmpty()) return;
            var pushPlugIds = pushers.Select(x => x.PushPlugId).Distinct().ToList();
            var pushPlugs = this.GetPushPlugs(pushPlugIds);
            if (pushPlugs.IsNullOrEmpty()) return;

            Dictionary<double, ISender> iSenderDic = new Dictionary<double, ISender>();
            var attachments = (new BodyBuilder()).Attachments;

            var logCount = pushSends.SelectMany(c => c.PushSendChildrens).Count();
            EntityList<AbnormalMonitorSendLog> logs = new EntityList<AbnormalMonitorSendLog>();
            var logIdList = RT.Service.Resolve<CommonController>().GetBatchIdList<AbnormalMonitorSendLog>(logCount);
            var logIdGetter = new EntityIdGetter(logIdList);

            EntityList<AbnormalMonitorTaskLog> taskLogs = new EntityList<AbnormalMonitorTaskLog>();
            var taskLogIdList = RT.Service.Resolve<CommonController>().GetBatchIdList<AbnormalMonitorSendLog>(logCount);
            var taskLogIdGetter = new EntityIdGetter(taskLogIdList);

            //异步推送，以免阻断当前线程
            Threading.AsyncHelper.InvokeSafe(() =>
            {
                foreach (var pushSend in pushSends)
                {
                    ReceiveParam rparam = new ReceiveParam();
                    foreach (var pushSendChildren in pushSend.PushSendChildrens)
                    {
                        try
                        {
                            var pushUpgradeRule = pushSendChildren.PushUpgradeRule;
                            var pusher = pushers.FirstOrDefault(c => c.Id == pushSendChildren.PushUpgradeRule.PusherId);
                            if (pusher == null) continue;
                            if (pushSendChildren.Employees.IsNullOrEmpty())
                            {
                                //没有邮件人，也相当是推送一次，要记录日志
                                //发送日志,
                                logs.Add(GenerateLog(logIdGetter, pushSend, pushSendChildren));
                                //任务日志
                                var contentT = $"升级机制【{pushUpgradeRule.AbnormalNode.ToLabel()}|{pushUpgradeRule.Time}{pushUpgradeRule.UnitType.ToLabel()}】推送通知，推送人员：无".L10N();
                                taskLogs.Add(GenerateTaskLog(taskLogIdGetter, pushSend, pushSendChildren, contentT));
                                if (pushSend.AbnormalMonitorTask.WarnTimes.HasValue)
                                    pushSend.AbnormalMonitorTask.WarnTimes++;
                                else
                                    pushSend.AbnormalMonitorTask.WarnTimes = 1;
                                continue;
                            }
                            rparam.MessageTemplateJson = pusher.MessageTemplate;
                            rparam.Employees.AddRange(pushSendChildren.Employees);
                            rparam.EmailAttachmentCollection = attachments;

                            var pushPlug = pushPlugs.FirstOrDefault(c => c.Id == pusher.PushPlugId);
                            if (pushPlug == null) continue;
                            //获取推送插件
                            ISender sender;
                            if (!iSenderDic.ContainsKey(pushSendChildren.PushUpgradeRule.PusherId))
                            {
                                sender = RT.Service.Resolve<PushPlugController>().GetSender(pushPlug);
                                iSenderDic.Add(pushSendChildren.PushUpgradeRule.PusherId, sender);
                            }
                            else
                                sender = iSenderDic[pushSendChildren.PushUpgradeRule.PusherId];

                            //初始化推送插件参数
                            sender.Config.Initialize(pushPlug.Config);

                            //创建推送参数
                            ISendParam sparam = null;
                            AbnormalMonitorAlertResult result = CreateAlertResult(pushSend, pushSendChildren);
                            lock (g_locker)
                            {                             
                                sparam = sender.CreateSendParam(result, rparam);
                            }

                            //推送信息
                            sender.Send(sparam);

                            //发送日志
                            logs.Add(GenerateLog(logIdGetter, pushSend, pushSendChildren));
                            //任务日志
                            var joinEmployeeNames = String.Join(",", pushSendChildren.Employees.Select(c => c.Name).ToList());
                            var content = $"升级机制【{pushUpgradeRule.AbnormalNode.ToLabel()}|{pushUpgradeRule.Time}{pushUpgradeRule.UnitType.ToLabel()}】推送通知，推送人员：[{joinEmployeeNames}]".L10N();
                            taskLogs.Add(GenerateTaskLog(taskLogIdGetter, pushSend, pushSendChildren, content));
                            if (pushSend.AbnormalMonitorTask.WarnTimes.HasValue)
                                pushSend.AbnormalMonitorTask.WarnTimes++;
                            else
                                pushSend.AbnormalMonitorTask.WarnTimes = 1;

                        }
                        catch (Exception ex)
                        {
                            string content = $"推送通知失败，失败原因：{ex.Message}".L10N();
                            taskLogs.Add(GenerateTaskLog(taskLogIdGetter, pushSend, pushSendChildren, content));
                        }
                    }
                }
                if (logs.IsNotEmpty())
                    RF.Save(logs);
                if (taskLogs.IsNotEmpty())
                    RF.Save(taskLogs);
                var saveTasks = pushSends.Where(c => c.AbnormalMonitorTask.PersistenceStatus == PersistenceStatus.Modified).Select(c => c.AbnormalMonitorTask).AsEntityList();
                if (saveTasks.Any())
                {
                    RF.Save(saveTasks);
                }
            });
        }

        /// <summary>
        /// 生成异常信息推送信息
        /// </summary>
        /// <param name="pushSend"></param>
        /// <param name="pushSendChildren"></param>
        /// <returns></returns>
        private AbnormalMonitorAlertResult CreateAlertResult(PushSend pushSend, PushSendChildren pushSendChildren)
        {
            var task = pushSend.AbnormalMonitorTask;
            var pushUpgradeRule = pushSendChildren.PushUpgradeRule;
            var result = new AbnormalMonitorAlertResult() { AlertInfoList = new List<AbnormalMonitorPusher>() };
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "异常编码".L10N(),
                Value = task.Code
            });
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "发生时间".L10N(),
                Value = task.CreateDate.ToString("yyyy年MM月dd日 HH:mm")
            });
            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "异常描述".L10N(),
                Value = task.ProblemDescription
            });

            result.AlertInfoList.Add(new AbnormalMonitorPusher()
            {
                Name = "推送升级机制".L10N(),
                Value = $"任务【{pushUpgradeRule.AbnormalNode.ToLabel()}】阶段，超过【{pushUpgradeRule.Time}{pushUpgradeRule.UnitType.ToLabel()}】未处理"
            });
            return result;
        }

        private AbnormalMonitorSendLog GenerateLog(EntityIdGetter logIdGetter, PushSend pushSend, PushSendChildren pushSendChildren)
        {
            AbnormalMonitorSendLog log = new AbnormalMonitorSendLog();
            log.Id = logIdGetter.GetNextId();
            log.JoinPushTargetNames = String.Join(",", pushSendChildren.Employees.Select(c => c.Name));
            log.PushContent = Newtonsoft.Json.JsonConvert.SerializeObject(pushSendChildren.SendContent);
            log.PushUpgradeRuleId = pushSendChildren.PushUpgradeRule.Id;
            log.PusherId = pushSendChildren.PushUpgradeRule.PusherId;
            log.AbnormalMonitorTaskId = pushSend.AbnormalMonitorTask.Id;
            log.AbnormalDefineId = pushSend.AbnormalDefine?.Id;
            log.AbnormalWarnDefineId = pushSend.AbnormalWarnDefine.Id;
            return log;
        }

        private AbnormalMonitorTaskLog GenerateTaskLog(EntityIdGetter taskLogIdGetter, PushSend pushSend, PushSendChildren pushSendChildren, string content)
        {
            AbnormalMonitorTaskLog log = new AbnormalMonitorTaskLog();
            log.Id = taskLogIdGetter.GetNextId();
            log.AbnormalMonitorTaskId = pushSend.AbnormalMonitorTask.Id;
            log.HandleNode = pushSendChildren.PushUpgradeRule.AbnormalNode;
            log.TaskHandleAction = TaskHandleAction.Message;
            log.HandlerId = RT.IdentityId;

            log.Content = content;
            return log;
        }

        #endregion

        #region 类
        /// <summary>
        /// 推送类
        /// </summary>
        public class PushSend
        {
            /// <summary>
            /// 升级机制发送集合
            /// </summary>

            public List<PushSendChildren> PushSendChildrens { get; set; }

            /// <summary>
            /// 异常任务
            /// </summary>
            public AbnormalMonitorTask AbnormalMonitorTask { get; set; }

            /// <summary>
            /// 异常定义
            /// </summary>
            public AbnormalDefine AbnormalDefine { get; set; }

            /// <summary>
            /// 异常预警定义
            /// </summary>
            public AbnormalWarnDefine AbnormalWarnDefine { get; set; }
        }

        /// <summary>
        /// 推送类子类
        /// </summary>
        public class PushSendChildren
        {
            /// <summary>
            /// 升级机制
            /// </summary>
            public PushUpgradeRule PushUpgradeRule { get; set; }

            /// <summary>
            /// 推送对象
            /// </summary>
            public List<PushTarget> PushTargets { get; set; }

            /// <summary>
            /// 推送内容
            /// </summary>
            public AlertResultBase SendContent { get; set; }

            /// <summary>
            /// 推送员工集合
            /// </summary>
            public List<SIE.Common.Employees.Employee> Employees { get; set; }
        }

        /// <summary>
        /// 【推送升级机制】用户类
        /// </summary>
        public class UserInPushUpgradeRule
        {
            /// <summary>
            /// 【推送升级机制】Id
            /// </summary>
            public double PushUpgradeRuleId { get; set; }

            /// <summary>
            /// 用户Id集合
            /// </summary>
            public List<double> UserIds { get; set; }
        }

        /// <summary>
        /// 【推送升级机制】员工类
        /// </summary>
        public class EmployeeInPushUpgradeRule
        {
            /// <summary>
            /// 【推送升级机制】Id
            /// </summary>
            public double PushUpgradeRuleId { get; set; }

            /// <summary>
            /// 员工Id集合
            /// </summary>
            public List<double> EmployeeIds { get; set; }
        }

        /// <summary>
        /// keyItem
        /// </summary>
        public class KeyItems
        {
            /// <summary>
            /// Key
            /// </summary>
            public double Key { get; set; }
            /// <summary>
            /// Items
            /// </summary>
            public List<double> Items { get; set; }
        }

        #endregion



        #endregion
    }
}
