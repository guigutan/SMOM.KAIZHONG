using Microsoft.Scripting.Utils;
using MimeKit;
using Newtonsoft.Json;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ForWinform.ApiModels;
using SIE.Andon.Andons.ViewModels;
using SIE.Common.Organizations;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ISript;
using SIE.MES.Andon.MessageSendJob;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Senders;
using SIE.Senders.FeiShus;
using SIE.Tech.Processs.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Community.CsharpSqlite.Sqlite3;

namespace SIE.Andon.MessageSendJob
{
    /// <summary>
    /// 安灯消息推送控制器
    /// </summary>
    public partial class MessageSendController : DomainController
    {
        /// <summary>
        /// 获取状态不是取消和已完成的安灯事件
        /// </summary>
        /// <returns></returns>
        private EntityList<AndonManage> GetAndonManages()
        {
            var query = Query<AndonManage>();
            query.Where(x => x.State != AndonManageState.Cancel && x.State != AndonManageState.Closed);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 发送即时消息
        /// </summary>
        /// <param name="andonManage">安灯事件</param>
        /// <param name="newAndonManageState">新的安灯状态</param>
        /// <param name="isReject">是否驳回</param>
        /// <param name="isReassign">是否转派</param>
        public virtual void SendInstantMessage(AndonManage andonManage, AndonManageState newAndonManageState,
            bool isReject, bool isReassign = false)
        {
            if (andonManage == null)
            {
                return;
            }
            // 懒加载获取安灯信息
            andonManage = RF.GetById<AndonManage>(andonManage.Id, new EagerLoadOptions().LoadWithViewProperty());
            var andonMessageSends = Query<AndonMessageSend>()
                .Where(x => andonManage.AndonId == x.AndonId && x.Minute == 0)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (!andonMessageSends.Any())
            {
                return;
            }

            //消息推送对象
            EntityList<AndonPushObject> andonPushObjects = GetAndonPushObjects(andonMessageSends);

            List<double> userIds = new List<double>();
            List<double> employeeIds = new List<double>();

            //推送对象列表中的角色
            EntityList<Role> roles = GetRoles(andonPushObjects);

            var roleIds = roles.Select(x => x.Id).Distinct().ToList();

            //角色下的员工
            EntityList<UserInRole> userInRoles = GetUserInRoles(roleIds);

            userIds.AddRange(userInRoles.Select(x => x.UserId).Distinct());

            //角色下的用户组
            EntityList<UserGroupInRole> userGroupInRoles = GetUserGroupInRoles(roleIds);

            var userGroupIdsOfRole = userGroupInRoles.Select(x => x.UserGroupId);

            //推送对象列表中的用户组
            EntityList<UserGroup> userGroups = GetUserGroups(andonPushObjects);

            //用户组下的用户
            var userGroupIds = userGroups.Select(x => x.Id).Distinct().ToList();

            //合并角色下的用户组
            userGroupIds = userGroupIds.Union(userGroupIdsOfRole).ToList();

            //用户组下的用户
            EntityList<UserInUserGroup> userInUserGroups = GetUserInGroups(userGroupIds);
            userIds.AddRange(userInUserGroups.Select(x => x.UserId).Distinct());

            //推送对象列表中的部门
            EntityList<Organization> organizations = GetOrganizations(andonPushObjects);

            //部门下的员工
            EntityList<Org2Employee> org2Employees = GetOrg2Employees(organizations);
            employeeIds.AddRange(org2Employees.Select(x => x.EmployeeId).Distinct());

            //安灯事件列表中的班组
            var workGroups = Query<WorkGroup>().Where(x => x.Name == andonManage.WorkGroup)
                    .ToList();

            //班组长
            EntityList<Employee> employeesOfWorkGroup = GetEmployeesOfWorkGroup(workGroups);

            //用户
            EntityList<User> users = GetUsers(ref userIds);

            employeeIds.AddRange(users.Where(x => x.EmployeeId != null)
                .Select(x => x.EmployeeId.Value).Distinct());

            //推送对象中按员工发送的员工
            EntityList<Employee> employeesOfStaff = GetEmployeesOfStaff(andonPushObjects, employeesOfWorkGroup);

            //员工
            List<double> allEmployeeIds = new List<double>();

            allEmployeeIds.AddRange(employeeIds);

            //触发人
            var trigger = andonPushObjects.Where(p => p.Type == PushObjectType.Trigger);
            if (trigger.Any())
            {
                employeeIds.Add(andonManage.TriggerId);
            }

            //处理人
            var handler = andonPushObjects.Where(p => p.Type == PushObjectType.Handler);
            if (handler.Any() && andonManage.HandlerId != null)
            {
                employeeIds.Add(andonManage.HandlerId.Value);
            }

            //负责人
            var charger = andonPushObjects.Where(p => p.Type == PushObjectType.AndonCharger);
            if (charger.Any())
            {
                var andon = RF.GetById<SIE.Andon.Andons.Andon>(andonManage.AndonId);
                var chargerId = andon.ChargerId;
                employeeIds.Add((double)chargerId);
            }

            employeeIds = employeeIds.Distinct().ToList();

            var employeeIdsOfWorkGroup = employeesOfWorkGroup.Select(x => x.Id);

            employeeIds = employeeIds.Except(employeeIdsOfWorkGroup).ToList();
            var employees = employeeIds.SplitContains(tempIds =>
            {
                return Query<Employee>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            //employees.AddRange(employeesOfWorkGroup);
            //employees.AddRange(employeesOfStaff);
            employeesOfWorkGroup.ForEach(item =>
            {
                if (!employeeIds.Contains(item.Id))
                {
                    employees.Add(item);
                }
            });
            employeesOfStaff.ForEach(item =>
            {
                if (!employeeIds.Contains(item.Id))
                {
                    employees.Add(item);
                }
            });

            //安灯管理消息推送子表
            EntityList<AndonManageMessageSend> andonManageMessageSends
                = GetAndonManageMessageSends(new List<double>() { andonManage.Id });

            SendInstantMessage(andonManage, newAndonManageState, isReject, isReassign, andonMessageSends, andonPushObjects, roles, userInRoles, userGroupInRoles, userGroups, userInUserGroups, organizations, org2Employees, workGroups, employeesOfWorkGroup, users, employees, andonManageMessageSends);

        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="andonManage">安灯事件</param>
        /// <param name="newAndonManageState">安灯状态</param>
        /// <param name="isReject">是否驳回</param>
        /// <param name="isReassign">是否转派</param>
        /// <param name="andonMessageSends">安灯消息推送子表</param>
        /// <param name="andonPushObjects">安灯推送对象孙表</param>
        /// <param name="roles">推送对象列表中的角色</param>
        /// <param name="userInRoles">角色下的员工</param>
        /// <param name="userGroupInRoles">角色下的用户组</param>
        /// <param name="userGroups">推送对象列表中的用户组</param>
        /// <param name="userInUserGroups">用户组下的用户</param>
        /// <param name="organizations">推送对象列表中的部门</param>
        /// <param name="org2Employees">部门下的员工</param>
        /// <param name="workGroups">安灯事件列表中的班组</param>
        /// <param name="employeesOfWorkGroup">班组长</param>
        /// <param name="users">用户</param>
        /// <param name="employees"></param>
        /// <param name="andonManageMessageSends"></param>
        private void SendInstantMessage(AndonManage andonManage, AndonManageState newAndonManageState, bool isReject, bool isReassign, EntityList<AndonMessageSend> andonMessageSends, EntityList<AndonPushObject> andonPushObjects, EntityList<Role> roles, EntityList<UserInRole> userInRoles, EntityList<UserGroupInRole> userGroupInRoles, EntityList<UserGroup> userGroups, EntityList<UserInUserGroup> userInUserGroups, EntityList<Organization> organizations, EntityList<Org2Employee> org2Employees, EntityList<WorkGroup> workGroups, EntityList<Employee> employeesOfWorkGroup, EntityList<User> users, EntityList<Employee> employees, EntityList<AndonManageMessageSend> andonManageMessageSends)
        {
            //第一步.根据安灯事件的状态获取节点符合的消息推送机制，获取不到时结束
            AndonTypeMessageSendState messageSendState = AndonTypeMessageSendState.Standby;

            switch (newAndonManageState)
            {
                case AndonManageState.Standby:
                    messageSendState = AndonTypeMessageSendState.Standby;
                    break;
                case AndonManageState.Processing:
                    {
                        if (isReject)
                        {
                            messageSendState = AndonTypeMessageSendState.Reject;
                        }
                        else
                        {
                            messageSendState = AndonTypeMessageSendState.Processing;
                        }
                    }
                    break;
                case AndonManageState.ToAccepted:
                    messageSendState = AndonTypeMessageSendState.ToAccepted;
                    break;
                case AndonManageState.Closed:
                    messageSendState = AndonTypeMessageSendState.Closed;
                    break;
                case AndonManageState.Cancel:
                    messageSendState = AndonTypeMessageSendState.Cancel;
                    break;
                default:
                    break;
            }

            if (!andonMessageSends.Any(x => x.Node == messageSendState))
            {
                return;
            }

            //时间为0的消息推送
            var andonMessageSend = andonMessageSends.FirstOrDefault(x => x.Node == messageSendState);

            if (andonMessageSend == null)
            {
                return;
            }

            //第四步.判断是否也推送过：第二步获取那条数据的节点和时间（分钟）字段到该安灯时间的推送记录获取数据，能获取到说明已经推送过来，不再推送，结束。获取不到时继续执行
            //且判断是不是转派，如果是转派即使被推送了也再推送一次
            if (andonManageMessageSends.Any(x => x.AndonManageId == andonManage.Id
                && x.AndonMessageSendId == andonMessageSend.Id) && !isReassign)
            {
                return;
            }

            //第五步.获取第三步获取的数据的推送对象并转化为员工（比如推送对应是角色，则获取角色对应的员工），去重，
            var andonPushObjectsOfCurrent = andonPushObjects.Where(x => x.MessageSendId == andonMessageSend.Id).ToList();

            //第六步.再获取这些员工的邮箱
            List<Employee> employeeToSendMail = new List<Employee>();
            GetEmployeesToSend(roles, userInRoles, userGroupInRoles, userGroups, userInUserGroups, organizations,
                org2Employees, workGroups, employeesOfWorkGroup, users, employees, andonManage, andonPushObjectsOfCurrent, employeeToSendMail);

            //第七步.获取消息模板：取第三步获取的数据的消息模板，为空时取安灯编码主表的消息模板，为空时取安灯类型的消息模板
            double? pushPlugId = 0;
            pushPlugId = GetPushPlugUp(andonMessageSend);

            //第八步.推送成功时生成安灯事件的【消息推送明细】数据。生成安灯事件的推送记录：
            var dbDateTime = RF.Find<AndonManage>().GetDbTime();
            //只实现邮件推送
            if (pushPlugId != 0)
            {
                var pushPlug = RF.GetById<PushPlug>(pushPlugId);
                if (pushPlug.Name == "邮件")
                {

                    SendMail(employeeToSendMail, dbDateTime, andonManage, andonMessageSend);
                }
                else if (pushPlug.Name == "飞书群组")
                {
                    SendFeiShu(employeeToSendMail, dbDateTime, andonManage, andonMessageSend);
                }
            }

        }

        /// <summary>
        /// 定时发送安灯消息
        /// </summary>
        public virtual void SendMessage()
        {
            var dbDateTime = RF.Find<AndonManage>().GetDbTime();

            //获取状态不是取消和已完成的安灯事件，逐个执行
            var andonManages = GetAndonManages();

            var andonIds = andonManages.Select(x => x.AndonId).Distinct().ToList();
            var andonMessageSends = andonIds.SplitContains(tempIds =>
            {
                return Query<AndonMessageSend>()
                    .Where(x => tempIds.Contains(x.AndonId) && x.Minute > 0)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            SendAndonMessage(dbDateTime, andonManages, andonMessageSends);
        }

        /// <summary>
        /// 发送安灯信息的内部方法
        /// </summary>
        /// <param name="dbDateTime"></param>
        /// <param name="andonManages"></param>
        /// <param name="andonMessageSends"></param>
        private void SendAndonMessage(System.DateTime dbDateTime, EntityList<AndonManage> andonManages,
            EntityList<AndonMessageSend> andonMessageSends)
        {
            //消息推送对象
            EntityList<AndonPushObject> andonPushObjects = GetAndonPushObjects(andonMessageSends);

            List<double> userIds = new List<double>();
            List<double> employeeIds = new List<double>();

            //推送对象列表中的角色
            EntityList<Role> roles = GetRoles(andonPushObjects);

            var roleIds = roles.Select(x => x.Id).Distinct().ToList();

            //角色下的员工
            EntityList<UserInRole> userInRoles = GetUserInRoles(roleIds);

            userIds.AddRange(userInRoles.Select(x => x.UserId).Distinct());

            //角色下的用户组
            EntityList<UserGroupInRole> userGroupInRoles = GetUserGroupInRoles(roleIds);

            var userGroupIdsOfRole = userGroupInRoles.Select(x => x.UserGroupId);

            //推送对象列表中的用户组
            EntityList<UserGroup> userGroups = GetUserGroups(andonPushObjects);

            //用户组下的用户
            var userGroupIds = userGroups.Select(x => x.Id).Distinct().ToList();

            //合并角色下的用户组
            userGroupIds = userGroupIds.Union(userGroupIdsOfRole).ToList();

            //用户组下的用户
            EntityList<UserInUserGroup> userInUserGroups = GetUserInGroups(userGroupIds);
            userIds.AddRange(userInUserGroups.Select(x => x.UserId).Distinct());

            //推送对象列表中的部门
            EntityList<Organization> organizations = GetOrganizations(andonPushObjects);

            //部门下的员工
            EntityList<Org2Employee> org2Employees = GetOrg2Employees(organizations);
            employeeIds.AddRange(org2Employees.Select(x => x.EmployeeId).Distinct());

            //安灯事件列表中的班组
            EntityList<WorkGroup> workGroups = GetWorkGroups(andonManages);

            //班组长
            EntityList<Employee> employeesOfWorkGroup = GetEmployeesOfWorkGroup(workGroups);

            //用户
            EntityList<User> users = GetUsers(ref userIds);

            employeeIds.AddRange(users.Where(x => x.EmployeeId != null)
                .Select(x => x.EmployeeId.Value).Distinct());

            //推送对象中按员工发送的员工
            EntityList<Employee> employeesOfStaff = GetEmployeesOfStaff(andonPushObjects, employeesOfWorkGroup);

            //员工
            EntityList<Employee> employees = GetEmployees(andonManages, employeeIds, employeesOfWorkGroup, employeesOfStaff);

            //安灯操作记录
            var andonManageIds = andonManages.Select(x => x.Id).Distinct().ToList();
            EntityList<AndonManageOperateLog> andonManageOperateLogs = GetAndonManageOperateLogs(andonManageIds);

            //安灯管理消息推送子表
            EntityList<AndonManageMessageSend> andonManageMessageSends = GetAndonManageMessageSends(andonManageIds);

            foreach (var andonManage in andonManages)
            {
                var andonManageOperateLog = andonManageOperateLogs
                    .Where(x => x.AndonManageId == andonManage.Id)
                    .OrderByDescending(x => x.OperateTime)
                    .FirstOrDefault();

                //第一步.根据安灯事件的状态获取节点符合的消息推送机制，获取不到时结束
                AndonTypeMessageSendState messageSendState = ComputeAndonTypeMessageSendState(andonManage, andonManageOperateLog);

                if (!andonMessageSends.Any(x => x.Node == messageSendState))
                {
                    continue;
                }

                //第二步.再获取安灯事件更新为当前状态到当前时间的时长，换算成分钟，既为T
                double totalMinutes = 0;
                if (andonManageOperateLog != null)
                {
                    totalMinutes = (dbDateTime - andonManageOperateLog.OperateTime).TotalMinutes;
                }
                else
                {
                    totalMinutes = (dbDateTime - andonManage.UpdateDate).TotalMinutes;
                }

                //第三步.第一步获取的的数据中，取时间（分钟）字段【小于等于T且最大】的数据，获取不到时结束
                var andonMessageSend = andonMessageSends.Where(x => x.Node == messageSendState
                    && x.Minute <= totalMinutes).OrderByDescending(x => x.Minute).FirstOrDefault();

                if (andonMessageSend == null)
                {
                    continue;
                }

                //第四步.判断是否也推送过：第二步获取那条数据的节点和时间（分钟）字段到该安灯时间的推送记录获取数据，能获取到说明已经推送过来，不再推送，结束。获取不到时继续执行
                if (andonManageMessageSends
                    .Any(x => x.AndonManageId == andonManage.Id && x.AndonMessageSendId == andonMessageSend.Id))
                {
                    continue;
                }

                //第五步.获取第三步获取的数据的推送对象并转化为员工（比如推送对应是角色，则获取角色对应的员工），去重，
                var andonPushObjectsOfCurrent = andonPushObjects.Where(x => x.MessageSendId == andonMessageSend.Id).ToList();

                //第六步.再获取这些员工的邮箱
                List<Employee> employeeToSendMail = new List<Employee>();
                GetEmployeesToSend(roles, userInRoles, userGroupInRoles, userGroups, userInUserGroups, organizations,
                    org2Employees, workGroups, employeesOfWorkGroup, users, employees, andonManage, andonPushObjectsOfCurrent, employeeToSendMail);

                //第七步.获取消息模板：取第三步获取的数据的消息模板，为空时取安灯编码主表的消息模板，为空时取安灯类型的消息模板
                //第七步改.只实现邮箱推送
                double? pushPlugId = 0;
                pushPlugId = GetPushPlugUp(andonMessageSend);

                //只实现邮件推送
                if (pushPlugId != 0)
                {
                    var pushPlug = RF.GetById<PushPlug>(pushPlugId);
                    if (pushPlug.Name == "邮件")
                    {
                        //第八步.推送成功时生成安灯事件的【消息推送明细】数据。生成安灯事件的推送记录：
                        SendMail(employeeToSendMail, dbDateTime, andonManage, andonMessageSend);
                    }
                    else if (pushPlug.Name == "飞书群组")
                    {
                        SendFeiShu(employeeToSendMail, dbDateTime, andonManage, andonMessageSend);
                    }
                    else if (pushPlug.Name == "微信")
                    {
                        //SendMarkdownMessageAsync(
                        //         userIds: "00101074",
                        //         content: "这是一条测试消息");
                    }
                }


            }
        }

        /// <summary>
        /// 获取消息模板(为空则向上获取)
        /// </summary>
        /// <param name="andonMessageSend"></param>
        /// <returns></returns>
        private double? GetPushPlugUp(AndonMessageSend andonMessageSend)
        {
            var andonMessageSendPushPlugId = andonMessageSend.PushPlugId;
            //if (andonMessageSendPushPlugId == null || andonMessageSendPushPlugId == 0)
            //{
            //    //找安灯维护主表的推送模板
            //    var andonPushPlugId = GetAndonPushPlug(andonMessageSend.AndonId);
            //    if (andonPushPlugId == null || andonPushPlugId == 0)
            //    {
            //        //找安灯维护主表的安灯类型维护的推送模板
            //        var andonTypePushPlugId = GetAndonTypePushPlug(andonMessageSend.AndonId);
            //        if (andonTypePushPlugId == null || andonTypePushPlugId == 0)
            //        {
            //            //没模板
            //            return 0;
            //        }
            //        else
            //        {
            //            return andonTypePushPlugId;
            //        }
            //    }
            //    else
            //    {
            //        return andonPushPlugId;
            //    }
            //}
            //else
            //{
            return andonMessageSendPushPlugId;
            //}
        }

        /// <summary>
        /// 推送飞书
        /// </summary>
        /// <param name="employeeToSendFei"></param>
        /// <param name="dbDateTime"></param>
        /// <param name="andonManage"></param>
        /// <param name="andonMessageSend"></param>
        private void SendFeiShu(List<Employee> employeeToSendFei, System.DateTime dbDateTime, AndonManage andonManage, AndonMessageSend andonMessageSend)
        {
            FeiShuGroupSenderParam feiShuGroupSenderParam = new FeiShuGroupSenderParam();
            foreach (var employee in employeeToSendFei)
            {
                if (employee.FeiId.IsNullOrEmpty())
                {
                    continue;
                }
                feiShuGroupSenderParam.userIds.Add(employee.FeiId);
            }
            string content = string.Empty;
            if (andonMessageSend == null)
            {
                throw new ValidationException("获取安灯维护消息推送失败，请检查".L10N());
            }
            if (andonMessageSend.MessageTemplate.IsNotEmpty())
            {
                content = JsonConvert.DeserializeObject<MessageTemplate>(andonMessageSend.MessageTemplate).Message;
            }
            else
            {
                content = "安灯推送".L10N();
            }
            feiShuGroupSenderParam.Message = content;
            feiShuGroupSenderParam.title = "安灯推送".L10N();
            try
            {
                var config = GetFeiSenderConfig();
                FeiShuGroupSender feiShuGroupSender = new FeiShuGroupSender
                {
                    Config = config,
                };
                feiShuGroupSenderParam.url = config.FeiShuGroupUrl;
                feiShuGroupSender.Send(feiShuGroupSenderParam);

                //生成消息推送记录
                using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
                {
                    EntityList<AndonManageMessageSend> andonManageMessageSends = new EntityList<AndonManageMessageSend>();
                    foreach (var employee in employeeToSendFei)
                    {
                        if (employee.FeiId.IsNullOrEmpty())
                        {
                            continue;
                        }
                        AndonManageMessageSend andonManageMessageSend = new AndonManageMessageSend()
                        {
                            AndonManageId = andonManage.Id,
                            AndonMessageSendId = andonMessageSend.Id,
                            MessageSendTime = dbDateTime,
                            MessageSendPersonId = employee.Id,
                            MessageSendAddress = employee.FeiId,
                        };
                        andonManageMessageSends.Add(andonManageMessageSend);

                    }
                    RF.Save(andonManageMessageSends);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="employeeToSendMail"></param>
        /// <param name="dbDateTime"></param>
        /// <param name="andonManage"></param>
        /// <param name="andonMessageSend"></param>
        /// 
        private async void SendMail(List<Employee> employeeToSendMail, System.DateTime dbDateTime, AndonManage andonManage, AndonMessageSend andonMessageSend)
        {
            EmailSendParam emailSendParam = new EmailSendParam();
            foreach (var employee in employeeToSendMail)
            {
                if (employee.Email.IsNullOrEmpty())
                {
                    continue;
                }
                const string emailRule = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
                Regex emailRegex = new Regex(emailRule);
                var matches = emailRegex.IsMatch(employee.Email);
                if (matches)
                {
                    emailSendParam.SendTos.Add(new MailboxAddress(employee.Email));
                }
            }
            if (andonMessageSend == null)
            {
                throw new ValidationException("获取安灯维护消息推送失败，请检查".L10N());
            }
            string mailContent = "";
            if (andonMessageSend.MessageTemplate.IsNullOrEmpty())
            {
                emailSendParam.Subject = "安灯管理消息推送".L10N();
                mailContent = CreateMailContent(andonManage);
            }
            else
            {
                var messageTemplate = JsonConvert.DeserializeObject<MessageTemplate>(andonMessageSend.MessageTemplate);
                emailSendParam.Subject = messageTemplate.Subject;
                mailContent = await CreateMailContentByModel(andonManage, messageTemplate);
            }

            emailSendParam.Body = mailContent;
            try
            {
                EmailSender senderObj = new EmailSender
                {
                    Config = GetEmailSenderConfig()
                };
                senderObj.Send(emailSendParam);
                //生成消息推送记录
                using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
                {
                    EntityList<AndonManageMessageSend> andonManageMessageSends = new EntityList<AndonManageMessageSend>();
                    foreach (var employee in employeeToSendMail)
                    {
                        if (employee.Email.IsNullOrEmpty())
                        {
                            continue;
                        }
                        AndonManageMessageSend andonManageMessageSend = new AndonManageMessageSend()
                        {
                            AndonManageId = andonManage.Id,
                            AndonMessageSendId = andonMessageSend.Id,
                            MessageSendTime = dbDateTime,
                            MessageSendPersonId = employee.Id,
                            MessageSendAddress = employee.Email,
                        };
                        andonManageMessageSends.Add(andonManageMessageSend);

                    }
                    RF.Save(andonManageMessageSends);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                //doNothing
            }
        }

        /// <summary>
        /// 通过模板渲染生成推送内容
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="messageTemplate"></param>
        /// <returns></returns>
        private async Task<string> CreateMailContentByModel(AndonManage andonManage, MessageTemplate messageTemplate)
        {// 创建模板引擎实例
            var templateContent = messageTemplate.Message;
            //var engine = new RazorLightEngineBuilder().UseMemoryCachingProvider().Build();

            //var engine = new RazorLightEngineBuilder()
            //    .UseEmbeddedResourcesProject(typeof(MessageSendController))//必须有一个模板的类型
            //    .SetOperatingAssembly(typeof(MessageSendController).Assembly)
            //    .UseMemoryCachingProvider()
            //    .DisableEncoding()//禁用编码，否则会把中文字符串编码成Unicode
            //    .Build();


            const string format = "yyyy-MM-dd HH:mm:ss";

            var model = new AndonManageModel
            {
                AndonManageCode = andonManage.AndonManageCode,
                AndonManageClass = andonManage.AndonManageClass.ToLabel(),
                AndonTypeName = andonManage.AndonTypeName,
                AndonName = andonManage.AndonName,
                Solution = andonManage.Solution,
                ProblemDesc = andonManage.ProblemDesc,
                Priority = andonManage.Priority,
                Defect = andonManage.Defect,
                Department = andonManage.Department,
                State = andonManage.State.ToLabel(),
                FaultTime = andonManage.FaultTime.ToString(),
                TriggerName = andonManage.TriggerByName,
                TriggerTime = andonManage.TriggerTime.ToString(format),
                HandlerName = andonManage.HandlerName,
                CloseTime = andonManage.CloseTime.HasValue ? andonManage.CloseTime.Value.ToString(format) : "",
                LastTime = andonManage.LastTime.HasValue ? andonManage.LastTime.Value.ToString(format) : "",
                ActualTime = andonManage.ActualTime.HasValue ? andonManage.ActualTime.Value.ToString(format) : "",
                FactoryName = andonManage.FactoryName,
                WorkShopName = andonManage.WipResourceName,
                WipResourceName = andonManage.WipResourceName,
                StationName = andonManage.StationName,
                EquipAccountCode = andonManage.EquipAccountCode,
                EquipAccountName = andonManage.EquipAccountName,
                WorkGroup = andonManage.WorkGroup,
                WoNo = andonManage.WoNo,
                ProductCode = andonManage.ProductCode,
                ProductName = andonManage.ProductName,
                ProcessName = andonManage.ProcessName,
                BarCode = andonManage.BarCode,
                LineStop = andonManage.LineStop ? "是".L10N() : "否".L10N(),
                AskMaterial = andonManage.AskMaterial ? "是".L10N() : "否".L10N()
            };
            var razor = RT.Service.Resolve<IRazorScript>();

            var result = razor.Parse(templateContent, model);

            return result;
        }

        /// <summary>
        /// 邮件推送内容
        /// </summary>
        /// <param name="andonManage"></param>
        /// <returns></returns>
        private string CreateMailContent(AndonManage andonManage)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><body><table>");
            sb.AppendLine(String.Format(@"
                            <tr>
                            <td>安灯事件编码</td>
                            <td>{0}</td>
                            </tr><tr>
                            <td>安灯大类</td>
                            <td>{1}</td>
                            </tr><tr>
                            <td>安灯类型</td>
                            <td>{2}</td>
                            </tr><tr>
                            <td>安灯名称</td>
                            <td>{3}</td>
                            </tr><tr>
                            <td>解决方案</td>
                            <td>{4}</td>
                            </tr><tr>
                            <td>问题描述</td>
                            <td>{5}</td>
                            </tr><tr>
                            <td>优先级</td>
                            <td>{6}</td>
                            </tr><tr>
                            <td>缺陷代码</td>
                            <td>{7}</td>
                            </tr><tr>
                            <td>负责部门</td>
                            <td>{8}</td>
                            </tr><tr>
                            <td>状态</td>
                            <td>{9}</td>
                            </tr><tr>
                            <td>故障发生时间</td>
                            <td>{10}</td>
                            </tr><tr>
                            <td>触发人</td>
                            <td>{11}</td>
                            </tr><tr>
                            <td>触发时间</td>
                            <td>{12}</td>
                            </tr><tr>
                            <td>处理人</td>
                            <td>{13}</td>
                            </tr><tr>
                            <td>关闭时间</td>
                            <td>{14}</td>
                            </tr><tr>
                            <td>持续时间</td>
                            <td>{15}</td>
                            </tr><tr>
                            <td>实际影响时间</td>
                            <td>{16}</td>
                            </tr><tr>
                            <td>工厂</td>
                            <td>{17}</td>
                            </tr><tr>
                            <td>车间</td>
                            <td>{18}</td>
                            </tr><tr>
                            <td>产线</td>
                            <td>{19}</td>
                            </tr><tr>
                            <td>工位</td>
                            <td>{20}</td>
                            </tr><tr>
                            <td>设备编码</td>
                            <td>{21}</td>
                            </tr><tr>
                            <td>设备名称</td>
                            <td>{22}</td>
                            </tr><tr>
                            <td>班组</td>
                            <td>{23}</td>
                            </tr><tr>
                            <td>工单</td>
                            <td>{24}</td>
                            </tr><tr>
                            <td>产品编码</td>
                            <td>{25}</td>
                            </tr><tr>
                            <td>产品名称</td>
                            <td>{26}</td>
                            </tr><tr>
                            <td>工序</td>
                            <td>{27}</td>
                            </tr><tr>
                            <td>条码号</td>
                            <td>{28}</td>
                            </tr><tr>
                            <td>是否停线</td>
                            <td>{29}</td>
                            </tr><tr>
                            <td>是否叫料</td>
                            <td>{30}</td>
                            </tr>", andonManage.AndonManageCode, andonManage.AndonManageClass.ToLabel(), andonManage.AndonTypeName, andonManage.AndonName, andonManage.Solution, andonManage.ProblemDesc, andonManage.Priority, andonManage.Defect, andonManage.Department, andonManage.State.ToLabel(), andonManage.FaultTime, andonManage.TriggerByName, andonManage.TriggerTime, andonManage.Handler != null ? andonManage.HandlerName : string.Empty, andonManage.CloseTime, andonManage.LastTime, andonManage.ActualTime, andonManage.FactoryName, andonManage.WorkShopName, andonManage.WipResourceName, andonManage.StationName, andonManage.EquipAccountCode, andonManage.EquipAccountName, andonManage.WorkGroup, andonManage.WoNo, andonManage.ProductCode, andonManage.ProductName, andonManage.ProcessName, andonManage.BarCode, andonManage.LineStop ? "是".L10N() : "否".L10N(), andonManage.AskMaterial ? "是".L10N() : "否".L10N()));
            sb.AppendLine("</table></body></html>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取邮件配置
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private EmailSenderConfig GetEmailSenderConfig()
        {
            var plug = GetPushPlug();
            if (plug == null)
                throw new ValidationException("推送管理模块没有类型为[{0}]的邮件推送方式！".L10nFormat(typeof(EmailSender).ToString()));
            var config = JsonConvert.DeserializeObject<EmailSenderConfig>(plug.Config);
            return config;
        }

        private FeiShuGroupSenderConfig GetFeiSenderConfig()
        {
            var fei = GetFeiShu();
            if (fei == null)
            {
                throw new ValidationException("推送管理模块没有类型为[{0}]的飞书推送方式！".L10nFormat(typeof(FeiShuGroupSender).ToString()));
            }
            var config = JsonConvert.DeserializeObject<FeiShuGroupSenderConfig>(fei.Config);
            return config;
        }
        /// <summary>
        /// 获取邮箱配置
        /// </summary>
        /// <returns></returns>
        private PushPlug GetPushPlug()
        {
            return Query<PushPlug>().Where(p => p.PushClass == typeof(EmailSender).ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 获取飞书配置url
        /// </summary>
        /// <returns></returns>
        private PushPlug GetFeiShu()
        {
            return Query<PushPlug>().Where(p => p.PushClass == typeof(FeiShuGroupSender).ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 获取安灯类型的推送模块
        /// </summary>
        /// <param name="andonId"></param>
        /// <returns></returns>
        //private double? GetAndonTypePushPlug(double andonId)
        //{
        //    var andon = RF.GetById<SIE.Andon.Andons.Andon>(andonId);
        //    var andonType = RF.GetById<AndonType>(andon.AndonTypeId);
        //    return andonType.PushPlugId;
        //}
        /// <summary>
        /// 获取安灯推送模块
        /// </summary>
        /// <param name="andonId"></param>
        /// <returns></returns>
        //private double? GetAndonPushPlug(double andonId)
        //{
        //    var andon = RF.GetById<SIE.Andon.Andons.Andon>(andonId);
        //    return andon.PushPlugId;
        //}

        /// <summary>
        /// 安灯消息推送记录
        /// </summary>
        /// <param name="andonManageIds"></param>
        /// <returns></returns>
        private EntityList<AndonManageMessageSend> GetAndonManageMessageSends(List<double> andonManageIds)
        {
            return andonManageIds.SplitContains(tempIds =>
            {
                return Query<AndonManageMessageSend>()
                    .Where(x => tempIds.Contains(x.AndonManageId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取安灯操作记录
        /// </summary>
        /// <param name="andonManageIds"></param>
        /// <returns></returns>
        private EntityList<AndonManageOperateLog> GetAndonManageOperateLogs(List<double> andonManageIds)
        {
            return andonManageIds.SplitContains(tempIds =>
            {
                return Query<AndonManageOperateLog>()
                    .Where(x => tempIds.Contains(x.AndonManageId))
                    .ToList();
            });
        }

        /// <summary>
        /// 消息推送节点
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="andonManageOperateLog"></param>
        /// <returns></returns>
        private AndonTypeMessageSendState ComputeAndonTypeMessageSendState(AndonManage andonManage, AndonManageOperateLog andonManageOperateLog)
        {
            AndonTypeMessageSendState messageSendState = AndonTypeMessageSendState.Standby;

            switch (andonManage.State)
            {
                case AndonManageState.Standby:
                    messageSendState = AndonTypeMessageSendState.Standby;
                    break;
                case AndonManageState.Processing:
                    {
                        if (andonManageOperateLog != null
                            && andonManageOperateLog.OperateType == AndonManageOperateType.Reject)
                        {
                            messageSendState = AndonTypeMessageSendState.Reject;
                        }
                        else
                        {
                            messageSendState = AndonTypeMessageSendState.Processing;
                        }
                    }
                    break;
                case AndonManageState.ToAccepted:
                    messageSendState = AndonTypeMessageSendState.ToAccepted;
                    break;
                case AndonManageState.Closed:
                    messageSendState = AndonTypeMessageSendState.Closed;
                    break;
                case AndonManageState.Cancel:
                    messageSendState = AndonTypeMessageSendState.Cancel;
                    break;
                default:
                    break;
            }

            return messageSendState;
        }

        /// <summary>
        /// 所有员工
        /// </summary>
        /// <param name="andonManages"></param>
        /// <param name="employeeIds"></param>
        /// <param name="employeesOfWorkGroup"></param>
        /// <param name="employeesOfStaff"></param>
        /// <returns></returns>
        private EntityList<Employee> GetEmployees(EntityList<AndonManage> andonManages, List<double> employeeIds,
            EntityList<Employee> employeesOfWorkGroup, EntityList<Employee> employeesOfStaff)
        {
            List<double> allEmployeeIds = new List<double>();

            allEmployeeIds.AddRange(employeeIds);

            //触发人
            employeeIds.AddRange(andonManages.Select(x => x.TriggerId));

            //处理人
            employeeIds.AddRange(andonManages
                .Where(x => x.HandlerId != null)
                .Select(x => x.HandlerId.Value));

            employeeIds = employeeIds.Distinct().ToList();

            var employeeIdsOfWorkGroup = employeesOfWorkGroup.Select(x => x.Id);

            employeeIds = employeeIds.Except(employeeIdsOfWorkGroup).ToList();
            var employees = employeeIds.SplitContains(tempIds =>
            {
                return Query<Employee>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });

            employees.AddRange(employeesOfWorkGroup);
            employees.AddRange(employeesOfStaff);
            return employees;
        }

        /// <summary>
        /// 推送对象为员工的员工资料
        /// </summary>
        /// <param name="andonPushObjects"></param>
        /// <param name="employeesOfWorkGroup"></param>
        /// <returns></returns>
        private EntityList<Employee> GetEmployeesOfStaff(EntityList<AndonPushObject> andonPushObjects, EntityList<Employee> employeesOfWorkGroup)
        {
            var employeeCodes = andonPushObjects
                            .Where(x => x.Type == PushObjectType.Staff)
                            .Select(x => x.Code).Distinct().ToList();

            var employeeCodesOfWorkGroup = employeesOfWorkGroup.Select(x => x.Code).Distinct().ToList();
            employeeCodes = employeeCodes.Except(employeeCodesOfWorkGroup).ToList();
            var employeesOfStaff = employeeCodes.SplitContains(tempCodes =>
            {
                return Query<Employee>()
                    .Where(x => tempCodes.Contains(x.Code))
                    .ToList();
            });
            return employeesOfStaff;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private EntityList<User> GetUsers(ref List<double> userIds)
        {
            userIds = userIds.Distinct().ToList();
            var users = userIds.SplitContains(tempIds =>
            {
                return Query<User>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });
            return users;
        }

        /// <summary>
        /// 班组的班长、组长、班组长
        /// </summary>
        /// <param name="workGroups"></param>
        /// <returns></returns>
        private EntityList<Employee> GetEmployeesOfWorkGroup(EntityList<WorkGroup> workGroups)
        {
            var workGroupIds = workGroups.Select(x => (double?)x.Id).Distinct().ToList();
            var employeesOfWorkGroup = workGroupIds.SplitContains(tempIds =>
            {
                //员工类型不为空，则是班组长、班长、组长
                return Query<Employee>()
                    .Where(x => tempIds.Contains(x.WorkGroupId))
                    .Where(x => x.EmployeeType != null)
                    .ToList();
            });
            return employeesOfWorkGroup;
        }

        /// <summary>
        /// 班组
        /// </summary>
        /// <param name="andonManages"></param>
        /// <returns></returns>
        private EntityList<WorkGroup> GetWorkGroups(EntityList<AndonManage> andonManages)
        {
            var workGroupNames = andonManages.Select(x => x.WorkGroup).Distinct().ToList();
            var workGroups = workGroupNames.SplitContains(tempWorkGroupNames =>
            {
                return Query<WorkGroup>().Where(x => tempWorkGroupNames.Contains(x.Name))
                    .ToList();
            });
            return workGroups;
        }

        /// <summary>
        /// 部门下的员工
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        private EntityList<Org2Employee> GetOrg2Employees(EntityList<Organization> organizations)
        {
            var organizationIds = organizations.Select(x => x.Id).Distinct().ToList();
            var org2Employees = organizationIds.SplitContains(tempIds =>
            {
                return Query<Org2Employee>().Where(x => tempIds.Contains(x.OrganizationId))
                    .ToList();
            });
            return org2Employees;
        }

        /// <summary>
        /// 推送对象为部门
        /// </summary>
        /// <param name="andonPushObjects"></param>
        /// <returns></returns>
        private EntityList<Organization> GetOrganizations(EntityList<AndonPushObject> andonPushObjects)
        {
            var departmentCodes = andonPushObjects
                .Where(x => x.Type == PushObjectType.Department)
                .Select(x => x.Code).Distinct().ToList();
            var organizations = departmentCodes.SplitContains(tempCodes =>
            {
                return Query<Organization>().Where(x => tempCodes.Contains(x.Code))
                    .ToList();
            });
            return organizations;
        }

        /// <summary>
        /// 用户组下的用户
        /// </summary>
        /// <param name="userGroupIds"></param>
        /// <returns></returns>
        private EntityList<UserInUserGroup> GetUserInGroups(List<double> userGroupIds)
        {
            return userGroupIds.SplitContains(tempIds =>
            {
                return Query<UserInUserGroup>()
                    .Where(x => tempIds.Contains(x.UserGroupId))
                    .ToList();
            });
        }

        /// <summary>
        /// 推送对象为用户组
        /// </summary>
        /// <param name="andonPushObjects"></param>
        /// <returns></returns>
        private EntityList<UserGroup> GetUserGroups(EntityList<AndonPushObject> andonPushObjects)
        {
            var userGroupCodes = andonPushObjects.Where(x => x.Type == PushObjectType.UserGroup)
                            .Select(x => x.Code).Distinct().ToList();

            var userGroups = userGroupCodes.SplitContains(tempCodes =>
            {
                return Query<UserGroup>()
                    .Where(x => tempCodes.Contains(x.Code))
                    .ToList();
            });
            return userGroups;
        }

        /// <summary>
        /// 角色下的用户组
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        private EntityList<UserGroupInRole> GetUserGroupInRoles(List<double> roleIds)
        {
            return roleIds.SplitContains(tempIds =>
            {
                return Query<UserGroupInRole>().Where(x => tempIds.Contains(x.RoleId))
                    .ToList();
            });
        }

        /// <summary>
        /// 角色下的用户
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        private EntityList<UserInRole> GetUserInRoles(List<double> roleIds)
        {
            return roleIds.SplitContains(tempIds =>
            {
                return Query<UserInRole>().Where(x => tempIds.Contains(x.RoleId))
                    .ToList();
            });
        }

        /// <summary>
        /// 推送对象为角色
        /// </summary>
        /// <param name="andonPushObjects"></param>
        /// <returns></returns>
        private EntityList<Role> GetRoles(EntityList<AndonPushObject> andonPushObjects)
        {
            var roleCodes = andonPushObjects.Where(x => x.Type == PushObjectType.Role)
                            .Select(x => x.Code).Distinct().ToList();

            var roles = roleCodes.SplitContains(tempCodes =>
            {
                return Query<Role>().Where(x => tempCodes.Contains(x.Code))
                    .ToList();
            });
            return roles;
        }

        /// <summary>
        /// 推送对象
        /// </summary>
        /// <param name="andonMessageSends"></param>
        /// <returns></returns>
        private EntityList<AndonPushObject> GetAndonPushObjects(EntityList<AndonMessageSend> andonMessageSends)
        {
            var andonMessageSendIds = andonMessageSends.Select(x => x.Id).Distinct().ToList();
            var andonPushObjects = andonMessageSendIds.SplitContains(tempIds =>
            {
                return Query<AndonPushObject>()
                    .Where(x => tempIds.Contains(x.MessageSendId))
                    .ToList();
            });
            return andonPushObjects;
        }

        /// <summary>
        /// 计算要发送邮件员工
        /// </summary>
        /// <param name="roles">推送对象列表中的角色</param>
        /// <param name="userInRoles">角色下的员工</param>
        /// <param name="userGroupInRoles">角色下的用户组</param>
        /// <param name="userGroups">推送对象列表中的用户组</param>
        /// <param name="userInUserGroups">用户组下的用户</param>
        /// <param name="organizations">推送对象列表中的部门</param>
        /// <param name="org2Employees">部门下的员工</param>
        /// <param name="workGroups">安灯事件列表中的班组</param>
        /// <param name="employeesOfWorkGroup">班组长</param>
        /// <param name="users">用户</param>
        /// <param name="employees"></param>
        /// <param name="andonManage"></param>
        /// <param name="andonPushObjectsOfCurrent"></param>
        /// <param name="employeeToSendMail"></param>
        private void GetEmployeesToSend(EntityList<Role> roles, EntityList<UserInRole> userInRoles, EntityList<UserGroupInRole> userGroupInRoles, EntityList<UserGroup> userGroups, EntityList<UserInUserGroup> userInUserGroups, EntityList<Organization> organizations, EntityList<Org2Employee> org2Employees, EntityList<WorkGroup> workGroups, EntityList<Employee> employeesOfWorkGroup, EntityList<User> users, EntityList<Employee> employees, AndonManage andonManage, List<AndonPushObject> andonPushObjectsOfCurrent, List<Employee> employeeToSendMail)
        {
            List<double> userIdsOfCurrent = new List<double>();
            List<double> employeeIdsOfCurrent = new List<double>();

            foreach (var andonPushObject in andonPushObjectsOfCurrent)
            {
                ComputeEmployeesAndUsers(roles, userInRoles, userGroupInRoles, userGroups, userInUserGroups,
                    organizations, org2Employees, workGroups, employeesOfWorkGroup, employees, andonManage,
                    employeeToSendMail, userIdsOfCurrent, employeeIdsOfCurrent, andonPushObject);
            }

            var usersOfCurrent = users.Where(x => userIdsOfCurrent.Contains(x.Id)).ToList();
            employeeIdsOfCurrent.AddRange(usersOfCurrent
                .Where(x => x.EmployeeId != null).Select(x => x.EmployeeId.Value));

            employeeIdsOfCurrent = employeeIdsOfCurrent.Except(employeeToSendMail.Select(x => x.Id))
               .ToList();

            employeeToSendMail.AddRange(employees
                                   .Where(x => employeeIdsOfCurrent.Contains(x.Id)));
        }

        /// <summary>
        /// 计算要推送消息的用户或员工
        /// </summary>
        /// <param name="roles">推送对象列表中的角色</param>
        /// <param name="userInRoles">角色下的员工</param>
        /// <param name="userGroupInRoles">角色下的用户组</param>
        /// <param name="userGroups">推送对象列表中的用户组</param>
        /// <param name="userInUserGroups">用户组下的用户</param>
        /// <param name="organizations">推送对象列表中的部门</param>
        /// <param name="org2Employees">部门下的员工</param>
        /// <param name="workGroups">安灯事件列表中的班组</param>
        /// <param name="employeesOfWorkGroup">班组长</param>
        /// <param name="employees"></param>
        /// <param name="andonManage"></param>
        /// <param name="employeeToSendMail"></param>
        /// <param name="userIdsOfCurrent"></param>
        /// <param name="employeeIdsOfCurrent"></param>
        /// <param name="andonPushObject"></param>
        private void ComputeEmployeesAndUsers(EntityList<Role> roles, EntityList<UserInRole> userInRoles,
            EntityList<UserGroupInRole> userGroupInRoles, EntityList<UserGroup> userGroups,
            EntityList<UserInUserGroup> userInUserGroups, EntityList<Organization> organizations,
            EntityList<Org2Employee> org2Employees, EntityList<WorkGroup> workGroups, EntityList<Employee> employeesOfWorkGroup,
            EntityList<Employee> employees, AndonManage andonManage, List<Employee> employeeToSendMail,
            List<double> userIdsOfCurrent, List<double> employeeIdsOfCurrent, AndonPushObject andonPushObject)
        {
            switch (andonPushObject.Type)
            {
                case Andons.Enum.PushObjectType.Staff:
                    var employee = employees.FirstOrDefault(x => x.Code == andonPushObject.Code);
                    if (employee != null)
                    {
                        employeeToSendMail.Add(employee);
                    }
                    break;
                case Andons.Enum.PushObjectType.Role:
                    GetUserIdsInRole(roles, userInRoles, userGroupInRoles, userInUserGroups, userIdsOfCurrent, andonPushObject);
                    break;
                case Andons.Enum.PushObjectType.UserGroup:
                    var userGroupsOfCurrent = userGroups.FirstOrDefault(x => x.Code == andonPushObject.Code);
                    if (userGroupsOfCurrent != null)
                    {
                        var userInUserGroupsOfCurrent = userInUserGroups
                               .Where(x => x.UserGroupId == userGroupsOfCurrent.Id).ToList();
                        userIdsOfCurrent.AddRange(userInUserGroupsOfCurrent.Select(x => x.UserId));
                    }

                    break;
                case Andons.Enum.PushObjectType.Department:
                    var organizationsOfCurrent = organizations.FirstOrDefault(x => x.Code == andonPushObject.Code);
                    if (organizationsOfCurrent != null)
                    {
                        var employeesOfOrganization = org2Employees
                               .Where(x => x.OrganizationId == organizationsOfCurrent.Id).ToList();
                        employeeIdsOfCurrent.AddRange(employeesOfOrganization.Select(x => x.EmployeeId));
                    }
                    break;
                case Andons.Enum.PushObjectType.Trigger:
                    employeeIdsOfCurrent.Add(andonManage.TriggerId);
                    break;
                case Andons.Enum.PushObjectType.Handler:
                    if (andonManage.HandlerId.HasValue)
                    {
                        employeeIdsOfCurrent.Add(andonManage.HandlerId.Value);
                    }
                    break;
                case Andons.Enum.PushObjectType.AndonCharger:
                    var andon = RF.GetById<SIE.Andon.Andons.Andon>(andonManage.AndonId);
                    employeeIdsOfCurrent.Add((double)andon.ChargerId);
                    break;
                case Andons.Enum.PushObjectType.WorkGroupCharge:
                    var workGroupOfCurrent = workGroups.FirstOrDefault(x => x.Name == andonManage.WorkGroup);
                    if (workGroupOfCurrent != null)
                    {
                        employeeToSendMail.AddRange(employeesOfWorkGroup
                               .Where(x => x.WorkGroupId == workGroupOfCurrent.Id));
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 计算角色下的用户
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="userInRoles"></param>
        /// <param name="userGroupInRoles"></param>
        /// <param name="userInUserGroups"></param>
        /// <param name="userIdsOfCurrent"></param>
        /// <param name="andonPushObject"></param>
        private void GetUserIdsInRole(EntityList<Role> roles, EntityList<UserInRole> userInRoles, EntityList<UserGroupInRole> userGroupInRoles, EntityList<UserInUserGroup> userInUserGroups, List<double> userIdsOfCurrent, AndonPushObject andonPushObject)
        {
            var role = roles.FirstOrDefault(x => x.Code == andonPushObject.Code);
            if (role != null)
            {
                var userInRolesOfCurrent = userInRoles.Where(x => x.RoleId == role.Id).ToList();
                if (userInRolesOfCurrent.Any())
                {
                    userIdsOfCurrent.AddRange(userInRolesOfCurrent.Select(x => x.UserId));
                }

                var userGroupInRolesOfCurrent = userGroupInRoles
                    .Where(x => x.RoleId == role.Id).ToList();
                if (userGroupInRolesOfCurrent.Any())
                {
                    var userGroupIdsOfRole = userGroupInRolesOfCurrent.Select(x => x.UserGroupId)
                        .Distinct().ToList();

                    var userInUserGroupsOfRole = userInUserGroups
                        .Where(x => userGroupIdsOfRole.Contains(x.UserGroupId)).ToList();

                    userIdsOfCurrent.AddRange(userInUserGroupsOfRole.Select(x => x.UserId));
                }
            }
        }

        #region 企业微信推送

        /// <summary>
        /// 安灯消息推送
        /// </summary>
        public virtual void AndonMessagePushAsync()
        {
            var senders = new WeComMessageSender(
                            corpId: "wx78ac1de6dc983cb0",
                            corpSecret: "yusMO36JshZVnA9bmHuS-ceCMKVio6_Ue0OjknV_JmA",
                            agentId: 1000053);

            try
            {
                var dbDateTime = RF.Find<AndonManage>().GetDbTime();
                var andonManages = GetAndonManagePs();
                var andonIds = andonManages.Select(x => x.AndonId).Distinct().ToList();
                var andonMessageSends = andonIds.SplitContains(tempIds =>
                {
                    return Query<AndonMessageSend>()
                        .Where(x => tempIds.Contains(x.AndonId) && x.Minute > 0)
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
                //SendAndonMessage(dbDateTime, andonManages, andonMessageSends);



                //安灯操作记录
                var andonManageIds = andonManages.Select(x => x.Id).Distinct().ToList();
                EntityList<AndonManageOperateLog> andonManageOperateLogs = GetAndonManageOperateLogs(andonManageIds);

                //安灯管理消息推送子表
                EntityList<AndonManageMessageSend> andonManageMessageSends = GetAndonManageMessageSends(andonManageIds);
                //消息推送对象
                EntityList<AndonPushObject> andonPushObjects = GetAndonPushObjects(andonMessageSends);

                //第一步 获取所有安灯管理异常信息
                foreach (var andonManage in andonManages)
                {
                    var factoryName = RF.GetById<Enterprise>(andonManage.FactoryId,
             new EagerLoadOptions().LoadWithViewProperty());
                    //List<double> employeeId = new List<double>();
                    var andonManageOperateLog = andonManageOperateLogs
                   .Where(x => x.AndonManageId == andonManage.Id)
                   .OrderByDescending(x => x.OperateTime)
                   .FirstOrDefault();
                    //第二步.根据安灯事件的状态获取节点符合的消息推送机制，获取不到时结束
                    AndonTypeMessageSendState messageSendState = ComputeAndonTypeMessageSendState(andonManage, andonManageOperateLog);

                    if (!andonMessageSends.Any(x => x.Node == messageSendState))
                    {
                        continue;
                    }

                    //第三步.再获取安灯事件更新为当前状态到当前时间的时长，换算成分钟，既为T
                    double totalMinutes = 0;
                    if (andonManageOperateLog != null)
                    {
                        totalMinutes = (dbDateTime - andonManageOperateLog.OperateTime).TotalMinutes;
                    }
                    else
                    {
                        totalMinutes = (dbDateTime - andonManage.UpdateDate).TotalMinutes;
                    }

                    //第四步.第一步获取的的数据中，取时间（分钟）字段【小于等于T且最大】的数据，获取不到时结束
                    List<AndonMessageSend> messageSends = andonMessageSends.Where(x => x.Node == messageSendState
                        && x.Minute <= totalMinutes && x.AndonId == andonManage.AndonId).OrderByDescending(x => x.Minute).ToList();
                    var andonMessageSend = messageSends.FirstOrDefault();

                    if (andonMessageSend == null)
                    {
                        continue;
                    }

                    //第五步.判断是否也推送过：第二步获取那条数据的节点和时间（分钟）字段到该安灯时间的推送记录获取数据，能获取到说明已经推送过来，不再推送，结束。获取不到时继续执行
                    AndonManageMessageSend messageSend = andonManageMessageSends
                        .Where(x => x.AndonManageId == andonManage.Id && x.AndonMessageSendId == andonMessageSend.Id).FirstOrDefault();
                    if (messageSend != null)
                    {
                        continue;
                    }
                    //第六步，获取级别
                    var andonPushObjectData = andonPushObjects.Where(p => p.MessageSendId == andonMessageSend.Id).FirstOrDefault();

                    //第七步，根据产线获取区域
                    var wipResources = GetWipResource(andonManage.WipResourceId);
                    //EntityList<AndonSesp> andonSesps = GetAndonSeep(andonManage.AndonId, andonPushObjectData.AndonLevel, wipResources.AndonUpholdId);
                    //第八步，获取人员
                    //employeeId.AddRange(andonSesps.Select(x => x.EmployeeId).Distinct());
                    //string userIds = GetEmpCodes(employeeId);
                    //if (userIds == "")
                    //{
                    //    continue;
                    //}
                    //单独获取 1级责任人

                    //根据安灯明细获取A1的人员
                    //var andonSespsA1 = Query<AndonSesp>().Where(p => p.AndonId == andonManage.AndonId && p.AndonUpholdId == wipResources.AndonUpholdId).OrderBy(p => p.AndonLevel).ToList();
                    List<double> employeeIdA1 = new List<double>();

                    //找出上一级的人
                    var lastMinutemessageSend = messageSends.Where(p => p.Minute < andonMessageSend.Minute).OrderByDescending(p => p.Minute).FirstOrDefault();
                    //默认是最早的，当存在上一级时间的时候，就不是最早的了
                    //string lastAndonLevel = andonSespsA1.FirstOrDefault().AndonLevel;
                    var lastAndonLevel = andonManage.Andon.AndonResponseDetailList.Where(p => p.AndonUpholdId == wipResources.AndonUpholdId).OrderBy(p => p.AndonseepLevel).FirstOrDefault()?.AndonseepLevel;
                    if (lastMinutemessageSend != null)
                    {
                        //第六步，获取级别
                        lastAndonLevel = andonPushObjects.Where(p => p.MessageSendId == lastMinutemessageSend.Id).FirstOrDefault()?.AndonLevel;
                    }
                    //EntityList<AndonSesp> lastAndonSesps = GetAndonSeep(andonManage.AndonId, lastAndonLevel, wipResources.AndonUpholdId);
                    //employeeIdA1 = lastAndonSesps.Select(p => p.EmployeeId).Distinct().ToList();
                    //string userNames = GetEmpNames(employeeIdA1);


                    //List<double> employeeIdA1 = new List<double>();
                    //string andonLevel = andonSespsA1.FirstOrDefault().AndonLevel;
                    //andonSespsA1 = Query<AndonSesp>().Where(p => p.AndonId == andonManage.AndonId && p.AndonUpholdId == wipResources.AndonUpholdId && p.AndonLevel == andonLevel).ToList();
                    //employeeIdA1.AddRange(andonSespsA1.Select(x => x.EmployeeId).Distinct());
                    //string userNames = GetEmpNames(employeeIdA1);

                    string message = "";

                    TimeSpan timeDifference = System.DateTime.Now - andonManage.FaultTime;
                    var waitingTime = Math.Round((double)timeDifference.TotalMinutes, 1);

                    var andonResponseDtls = andonManage.Andon.AndonResponseDetailList.Where(p => p.AndonseepLevel == lastAndonLevel && p.AndonUpholdId == wipResources.AndonUpholdId).ToList();
                    if (andonResponseDtls == null || andonResponseDtls.Count <= 0)
                    {
                        continue;
                        //throw new ValidationException("没有维护安灯级别为【{0}】的安灯责任组".L10nFormat(lastAndonLevel));
                    }
                    employeeIdA1 = andonResponseDtls.Select(p => p.AndonGroup).SelectMany(p => p.AndonGroupDetailList).Where(p => p.User.State == State.Enable && p.IsResponser == true).Select(p => p.User.EmployeeId ?? 0).Where(p => p != 0).Distinct().ToList();
                    if (employeeIdA1 == null || employeeIdA1.Count < 1)
                    {
                        continue;
                        //throw new ValidationException("维护安灯级别为【{0}】的安灯责任组，未维护对应的发送人员".L10nFormat(lastAndonLevel));
                    }
                    string userNames = GetEmpNames(employeeIdA1);
                    //推送给当前级别的人
                    var curLevelDtls = andonManage.Andon.AndonResponseDetailList.Where(p => p.AndonseepLevel == andonPushObjectData.AndonLevel && p.AndonUpholdId == wipResources.AndonUpholdId).ToList();
                    var employeeId = new List<double>();
                    foreach (var curLevelDtl in curLevelDtls)
                    {
                        if (curLevelDtl.AndonGroup != null)
                            employeeId.AddRange(curLevelDtl.AndonGroup.AndonGroupDetailList.Where(p => p.User.State == State.Enable).Select(p => p.User.EmployeeId ?? 0).Where(p => p != 0).Distinct().ToList());
                    }
                    string userIds = GetEmpCodes(employeeId);
                    if (userIds == "")
                    {
                        continue;
                    }
                    if (messageSendState == AndonTypeMessageSendState.Standby)
                    {
                        //第九步，组合消息
                        if (andonPushObjectData.AndonLevel == "A1")
                            message = message = "您有一个**" + andonManage.AndonName + "**安灯待响应\n" +
                            "**安灯事件编号**：" + andonManage.AndonManageCode + "\n" +
                            "**工**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**厂**：" + factoryName.Name + "\n" +
                            "**产**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**线**：" + andonManage.WipResourceName + "\n" +
                            "**所属区域**：" + wipResources.AndonUphold?.AndonDesc + "\n" +
                            "**设备编码**：" + andonManage?.EquipAccount?.Code + "\n" +
                            "**问题描述**：" + andonManage.ProblemDesc + "\n" +
                            "**异常发生时间**：" + andonManage.FaultTime + "\n" +
                            "**等待时长**：" + waitingTime + "分钟\n" +
                            "**升级等级**：" + andonPushObjectData.AndonLevel + "\n" +
                            "**责**\u00A0\u00A0**任**\u00A0\u00A0**人**：" + userNames + "\n" +
                            "请及时处理!";
                        else
                            message = "**" + andonManage.AndonName + "**安灯超时待响应\n" +
                                "**安灯事件编号**：" + andonManage.AndonManageCode + "\n" +
                                "**工**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**厂**：" + factoryName.Name + "\n" +
                                "**产**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**线**：" + andonManage.WipResourceName + "\n" +
                                "**所属区域**：" + wipResources.AndonUphold?.AndonDesc + "\n" +
                                "**设备编码**：" + andonManage?.EquipAccountCode + "\n" +
                                "**问题描述**：" + andonManage.ProblemDesc + "\n" +
                                "**异常发生时间**：" + andonManage.FaultTime + "\n" +
                                "**等待时长**：" + waitingTime + "分钟\n" +
                                "**升级等级**：" + andonPushObjectData.AndonLevel + "\n"+
                                "**责**\u00A0\u00A0**任**\u00A0\u00A0**人**：" + userNames + "\n" +
                                "请关注!";
                    }
                    else
                    {
                        //第九步，组合消息
                        if (andonPushObjectData.AndonLevel == "A1")
                            message = message = "您有一个**" + andonManage.AndonName + "**安灯待处理\n" +
                            "**安灯事件编号**：" + andonManage.AndonManageCode + "\n" +
                            "**工**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**厂**：" + factoryName.Name + "\n" +
                            "**产**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**线**：" + andonManage.WipResourceName + "\n" +
                            "**所属区域**：" + wipResources.AndonUphold?.AndonDesc + "\n" +
                            "**设备编码**：" + andonManage?.EquipAccountCode + "\n" +
                            "**问题描述**：" + andonManage.ProblemDesc + "\n" +
                            "**异常发生时间**：" + andonManage.FaultTime + "\n" +
                            "**等待时长**：" + waitingTime + "分钟\n" +
                            "**升级等级**：" + andonPushObjectData.AndonLevel + "\n" +
                            "**责**\u00A0\u00A0**任**\u00A0\u00A0**人**：" + userNames + "\n" +
                            "请及时处理!";
                        else
                            message = "**" + andonManage.AndonName + "**安灯超时待处理\n" +
                                "**安灯事件编号**：" + andonManage.AndonManageCode + "\n" +
                                "**工**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**厂**：" + factoryName.Name + "\n" +
                                "**产**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**线**：" + andonManage.WipResourceName + "\n" +
                                "**所属区域**：" + wipResources.AndonUphold?.AndonDesc + "\n" +
                                "**设备编码**：" + andonManage?.EquipAccountCode + "\n" +
                                "**问题描述**：" + andonManage.ProblemDesc + "\n" +
                                "**异常发生时间**：" + andonManage.FaultTime + "\n" +
                                "**等待时长**：" + waitingTime + "分钟\n" +
                                "**升级等级**：" + andonPushObjectData.AndonLevel + "\n" +
                                "**责**\u00A0\u00A0**任**\u00A0\u00A0**人**：" + userNames + "\n" +
                                "请关注!";
                    }
                    //第十步 发送信息
                    senders.SendMarkdownMessageAsync
                        (userIds: userIds,//"00101074"
                         content: message);

                    //第十一生成消息推送记录
                    using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
                    {
                        AndonManageMessageSend andonManageMessageSend = new AndonManageMessageSend()
                        {
                            AndonManageId = andonManage.Id,
                            AndonMessageSendId = andonMessageSend.Id,
                            MessageSendTime = dbDateTime,
                            MessageSendTemplate = message,
                            AbnormalTime = andonManage.FaultTime,
                            WaitinglTime = waitingTime,
                        };
                        andonManageMessageSends.Add(andonManageMessageSend);

                        RF.Save(andonManageMessageSends);
                        tran.Complete();
                    }
                }

                Console.WriteLine("消息发送成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送失败: {ex.Message}");
                throw new ValidationException("发送失败：{0}".L10nFormat(ex.GetBaseException()?.Message));            
            }
        }

        /// <summary>
        /// 根据id获取名称
        /// </summary>
        /// <param name="empids"></param>
        /// <returns></returns>
        private string GetEmpNames(List<double> empids)
        {
            string empName = "";
            foreach (var item in empids)
            {
                var EmployeeData = Query<Employee>().Where(p => p.Id == item).ToList().FirstOrDefault();
                empName += EmployeeData.Name + ",";
            }
            if (empName != "")
            {
                empName = empName.Substring(0, empName.Length - 1);
            }
            return empName;
        }

        /// <summary>
        /// 根据id获取名称
        /// </summary>
        /// <param name="empids"></param>
        /// <returns></returns>
        private string GetEmpCodes(List<double> empids)
        {
            string empCode = "";
            foreach (var item in empids)
            {
                var EmployeeData = Query<Employee>().Where(p => p.Id == item).ToList().FirstOrDefault();
                empCode += EmployeeData.Code + "|";
            }
            if (empCode != "")
            {
                empCode = empCode.Substring(0, empCode.Length - 1);
            }
            return empCode;
        }

        /// <summary>
        /// 获取产线的区域
        /// </summary>
        /// <param name="wipResourceId"></param>
        /// <returns></returns>
        private WipResource GetWipResource(double wipResourceId)
        {
            return Query<WipResource>().Where(p => p.Id == wipResourceId).FirstOrDefault();
        }

        /// <summary>
        /// 获取状态是未响应,未处理的安灯事件
        /// </summary>
        /// <returns></returns>
        private EntityList<AndonManage> GetAndonManagePs()
        {
            var query = Query<AndonManage>();
            query.Where(x => x.State == AndonManageState.Processing || x.State == AndonManageState.Standby);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前区域当前级别下的人员
        /// </summary>
        /// <param name="andonId">安灯ID</param>
        /// <param name="andonLevel">级别</param>
        /// <param name="andonUpholdId">区域</param>
        /// <returns></returns>
        private EntityList<AndonSesp> GetAndonSeep(double andonId, string andonLevel, double? andonUpholdId)
        {
            var query = Query<AndonSesp>();
            query.Where(x => x.AndonId == andonId && x.AndonLevel == andonLevel && x.AndonUpholdId == andonUpholdId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion
    }
}
