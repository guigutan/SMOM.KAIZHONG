using Microsoft.Scripting.Utils;
using MimeKit;
using Newtonsoft.Json;
using SIE.Common.Organizations;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.ISript;
using SIE.MES.QTimes.Datas;
using SIE.MES.QTimes.ViewModels;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Handles
{
    /// <summary>
    /// QT超时报表消息推送处理类
    /// </summary>
    public class QTimeReportMsgSendHandle
    {
        /// <summary>
        /// 构造
        /// </summary>
        public QTimeReportMsgSendHandle()
        {
            PushObjectStandards = new List<PushObjectStandard>();
            UserInUserGroups = new List<UserInUserGroup>();
            Users = new List<User>();
            Employees = new List<Employee>();
            UserInRoles = new List<UserInRole>();
            UserGroupInRoles = new List<UserGroupInRole>();
            Org2Employees = new List<Org2Employee>();
            PushPlugs = new List<PushPlug>();
            AllEmployees = new List<Employee>();
            AllEmployeeIds = new List<double>();
            AllUserGroupIds = new List<double>();
            AllWorkGroupIds = new List<double>();
            AllRoleIds = new List<double>();
            AllDepartmentIds = new List<double>();
        }

        #region 属性
        // 邮件格式正则匹配校验
        const string emailRule = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

        /// <summary>
        /// QT规则下的推送对象
        /// </summary>
        private List<PushObjectStandard> PushObjectStandards { get; set; }

        /// <summary>
        /// 所有员工Ids
        /// </summary>
        private List<double> AllEmployeeIds {  get; set; }

        /// <summary>
        /// 所有用户组Ids(查询数据库用)
        /// </summary>
        private List<double> AllUserGroupIds { get; set; }

        /// <summary>
        /// 所有班组Ids(查询数据库用)
        /// </summary>
        private List<double> AllWorkGroupIds { get; set; }

        /// <summary>
        /// 所有角色Ids(查询数据库用)
        /// </summary>
        private List<double> AllRoleIds { get; set; }

        /// <summary>
        /// 所有部门Ids(查询数据库用)
        /// </summary>
        private List<double> AllDepartmentIds {  get; set; }

        /// <summary>
        /// 用户与用户组
        /// </summary>
        private List<UserInUserGroup> UserInUserGroups { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        private List<User> Users { get; set; }

        /// <summary>
        /// 员工
        /// </summary>
        private List<Employee> Employees { get; set; }

        /// <summary>
        /// 用户与角色
        /// </summary>
        private List<UserInRole> UserInRoles { get; set; }

        /// <summary>
        /// 用户组与角色
        /// </summary>
        private List<UserGroupInRole> UserGroupInRoles { get; set; }

        /// <summary>
        /// 部门人员
        /// </summary>
        private List<Org2Employee> Org2Employees { get; set; }

        /// <summary>
        /// 推送方式(判断是否为邮件)
        /// </summary>
        private List<PushPlug> PushPlugs { get; set; }

        /// <summary>
        /// 所有推送对象员工
        /// </summary>
        private List<Employee> AllEmployees { get; set; }
        #endregion

        #region 方法

        /// <summary>
        /// 返回推送对象Ids
        /// </summary>
        /// <returns></returns>
        public List<PushObjectStandard> OutputPushObjectStandards()
        {
            return PushObjectStandards;
        }

        /// <summary>
        /// 获取QT标准规则下的推送对象
        /// </summary>
        /// <param name="standardIds">标准规则Ids</param>
        public void GetStandardPushObjectToEmployee(List<double> standardIds)
        {
            var pushObjectList = standardIds.SplitContains(tempIds =>
            {
                return DB.Query<QTPushObject>().Where(p => tempIds.Contains(p.QTStandardId)).ToList();
            });
            // 记录推送对象和QT标准规则
            foreach (var standaedId in standardIds)
            {
                PushObjectStandard pushObjectStandard = new PushObjectStandard();
                pushObjectStandard.StandardId = standaedId;

                // 员工
                pushObjectStandard.PushObjectIds.AddRange(pushObjectList.Where(p => p.ObjectType == Enums.QTPushType.Employee && p.QTStandardId == standaedId).Select(p => p.ObjectId).ToList());
                this.AllEmployeeIds.AddRange(pushObjectStandard.PushObjectIds);

                // 用户组
                pushObjectStandard.UserGroupIds.AddRange(pushObjectList.Where(p => p.ObjectType == Enums.QTPushType.UserGroup && p.QTStandardId == standaedId).Select(p => p.ObjectId).ToList());
                this.AllUserGroupIds.AddRange(pushObjectStandard.UserGroupIds);

                // 班组
                pushObjectStandard.WorkGroupIds.AddRange(pushObjectList.Where(p => p.ObjectType == Enums.QTPushType.WorkGroup && p.QTStandardId == standaedId).Select(p => p.ObjectId).ToList());
                this.AllWorkGroupIds.AddRange(pushObjectStandard.WorkGroupIds);

                // 角色
                pushObjectStandard.RoleIds.AddRange(pushObjectList.Where(p => p.ObjectType == Enums.QTPushType.Role && p.QTStandardId == standaedId).Select(p => p.ObjectId).ToList());
                this.AllRoleIds.AddRange(pushObjectStandard.RoleIds);

                // 部门
                pushObjectStandard.DepartmentIds.AddRange(pushObjectList.Where(p => p.ObjectType == Enums.QTPushType.Department && p.QTStandardId == standaedId).Select(p => p.ObjectId).ToList());
                this.AllDepartmentIds.AddRange(pushObjectStandard.DepartmentIds);

                this.PushObjectStandards.Add(pushObjectStandard);
            }
            // 查询数据库获取用户组、班组、角色、部门
            QueryDBPushObject();

            // 把每个标准规则下的非员工对象转化为员工
            PushObjectChangeToEmployee();
        }

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="standard"></param>
        /// <param name="qTimeReportViewModels"></param>
        public async void SendEmail(QTimeStandard standard, List<QTimeReportViewModel> qTimeReportViewModels)
        {
            // 推送方式
            var pushPlug = this.PushPlugs.FirstOrDefault(p => p.Id == standard.PushPlugId);
            if (pushPlug == null || (pushPlug != null && pushPlug.Name != "邮件")) 
            {
                return;
            }
            // QT标准
            var qtPushObjects = PushObjectStandards.FirstOrDefault(p => p.StandardId == standard.Id);
            var pushEmployee = this.AllEmployees.Where(p => qtPushObjects.PushObjectIds.Contains(p.Id)).ToList();


            // 邮件推送
            EmailSendParam emailSendParam = new EmailSendParam();
            foreach (var employee in pushEmployee)
            {
                if (employee.Email.IsNullOrEmpty())
                {
                    continue;
                }
                // 验证邮件格式是否合格
                Regex emailRegex = new Regex(emailRule);
                var matches = emailRegex.IsMatch(employee.Email);
                if (matches)
                {
                    emailSendParam.SendTos.Add(new MailboxAddress(employee.Email));
                }
            }

            string mailContent = "";
            // 无配置消息模板
            if (standard.MessageTemplate.IsNullOrEmpty())
            {
                emailSendParam.Subject = "QTime超时报表推送".L10N();
                mailContent = CreateMailContentByNoModel(standard, qTimeReportViewModels);
            }
            // 有消息模板
            else
            {
                var messageTemplate = JsonConvert.DeserializeObject<MessageTemplate>(standard.MessageTemplate);
                emailSendParam.Subject = messageTemplate.Subject;
                mailContent = await CreateMailContentByModel(standard, qTimeReportViewModels, messageTemplate);
            }
            emailSendParam.Body = mailContent;
            EmailSender senderObj = new EmailSender
            {
                Config = JsonConvert.DeserializeObject<EmailSenderConfig>(pushPlug.Config),
            };
            senderObj.Send(emailSendParam);
        }

        /// <summary>
        /// 一次性查询数据库获取用户组、班组、角色、部门
        /// </summary>
        private void QueryDBPushObject()
        {
            // 用户组
            GetUserGroup(AllUserGroupIds);
            // 班组
            GetWorkGroup(AllWorkGroupIds);
            // 角色
            GetRole(AllRoleIds);
            // 部门
            GetDepartment(AllDepartmentIds);
        }

        /// <summary>
        /// 把每个标准规则下的非员工对象转化为员工
        /// </summary>
        private void PushObjectChangeToEmployee()
        {
            foreach(var push in PushObjectStandards)
            {
                // 将用户组转化为员工
                ChangeUserGroupToEmployee(push, push.UserGroupIds);

                // 将班组转化为员工
                ChangeWorkGroupToEmployee(push, push.WorkGroupIds);

                // 将角色转化为员工
                ChangeRoleToEmployee(push, push.RoleIds);

                // 将部门转化为员工
                ChangeDptRoEmployee(push, push.DepartmentIds);

                // 去重
                push.PushObjectIds = push.PushObjectIds.Distinct().ToList();
            }
            this.AllEmployeeIds = this.AllEmployeeIds.Distinct().ToList();
            GetAllEmployee();
        }

        /// <summary>
        /// 将部门转化为员工
        /// </summary>
        /// <param name="push"></param>
        /// <param name="dptIds">部门Ids</param>
        private void ChangeDptRoEmployee(PushObjectStandard push, List<double> dptIds)
        {
            var org2Employees = this.Org2Employees.Where(p => dptIds.Contains(p.OrganizationId)).ToList();
            push.PushObjectIds.AddRange(org2Employees.Select(p => p.EmployeeId));
            this.AllEmployeeIds.AddRange(org2Employees.Select(p => p.EmployeeId));
        }

        /// <summary>
        /// 将角色转化为员工
        /// </summary>
        /// <param name="push"></param>
        /// <param name="roleIds">角色Ids</param>
        private void ChangeRoleToEmployee(PushObjectStandard push, List<double> roleIds)
        {
            // 角色下的用户
            var userInRoles = this.UserInRoles.Where(p => roleIds.Contains(p.RoleId)).ToList();
            var userIds = userInRoles.Select(p => p.UserId).ToList();
            var users = this.Users.Where(p => userIds.Contains(p.Id)).ToList();
            push.PushObjectIds.AddRange(users.Where(p => p.EmployeeId != null).Select(p => (double)p.EmployeeId));
            this.AllEmployeeIds.AddRange(users.Where(p => p.EmployeeId != null).Select(p => (double)p.EmployeeId));

            // 角色下的用户组
            var userGroupInRoles = this.UserGroupInRoles.Where(p => roleIds.Contains(p.RoleId)).ToList();
            var userGroupIds = userGroupInRoles.Select(p => p.UserGroupId).ToList();
            ChangeUserGroupToEmployee(push, userGroupIds);
        }

        /// <summary>
        /// 将用户组转化为员工
        /// </summary>
        /// <param name="push"></param>
        /// <param name="userGroupIds">用户组Ids</param>
        private void ChangeUserGroupToEmployee(PushObjectStandard push, List<double> userGroupIds)
        {
            var userInGroups = this.UserInUserGroups.Where(p => userGroupIds.Contains(p.UserGroupId)).ToList();
            var userIds = userInGroups.Select(p => p.UserId).ToList();
            var users = this.Users.Where(p => userIds.Contains(p.Id)).ToList();
            push.PushObjectIds.AddRange(users.Where(p => p.EmployeeId != null).Select(p => (double)p.EmployeeId));
            this.AllEmployeeIds.AddRange(users.Where(p => p.EmployeeId != null).Select(p => (double)p.EmployeeId));
        }

        /// <summary>
        /// 将班组转化为员工
        /// </summary>
        /// <param name="push"></param>
        /// <param name="workGroupIds">班组Ids</param>
        private void ChangeWorkGroupToEmployee(PushObjectStandard push, List<double> workGroupIds)
        {
            var employee = this.Employees.Where(p => p.WorkGroupId != null && workGroupIds.Contains((double)p.WorkGroupId)).ToList();
            push.PushObjectIds.AddRange(employee.Select(p => p.Id));
            this.AllEmployeeIds.AddRange(employee.Select(p => p.Id));
        }

        /// <summary>
        /// 数据库查询部门下的员工
        /// </summary>
        /// <param name="departmentIds"></param>
        private void GetDepartment(List<double> departmentIds)
        {
            var employeeInDpt = departmentIds.SplitContains(tempIds =>
            {
                return DB.Query<Org2Employee>().Where(p => tempIds.Contains(p.OrganizationId)).ToList();
            });
            this.Org2Employees.AddRange(employeeInDpt);
        }

        /// <summary>
        /// 数据库查询角色下的员工以及角色下的用户组中的员工
        /// </summary>
        /// <param name="roleIds"></param>
        private void GetRole(List<double> roleIds)
        {
            // 角色下的用户
            var userInRoles = roleIds.SplitContains(tempIds =>
            {
                return DB.Query<UserInRole>().Where(p => tempIds.Contains(p.RoleId)).ToList();
            });
            var userIds = userInRoles.Select(p => p.UserId).ToList();
            this.UserInRoles.AddRange(userInRoles);
            var users = userIds.SplitContains(tempIds =>
            {
                return DB.Query<User>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            this.Users.AddRange(users);

            // 角色下的用户组
            var userGroupInRoles = roleIds.SplitContains(tempIds =>
            {
                return DB.Query<UserGroupInRole>().Where(p => tempIds.Contains(p.RoleId)).ToList();
            });
            this.UserGroupInRoles.AddRange(userGroupInRoles);
            var userGroupIds = userGroupInRoles.Select(p => p.UserGroupId).ToList();
            GetUserGroup(userGroupIds);
        }

        /// <summary>
        /// 数据库查询班组下的员工
        /// </summary>
        /// <param name="workGroupIds">班组Ids</param>
        private void GetWorkGroup(List<double> workGroupIds)
        {
            // 属于班组的员工
            var employees = workGroupIds.SplitContains(tempIds =>
            {
                return DB.Query<Employee>().Where(p => p.WorkGroupId != null && tempIds.Contains((double)p.WorkGroupId)).ToList();
            });
            this.Employees.AddRange(employees);
        }

        /// <summary>
        /// 数据库查询用户组下的员工
        /// </summary>
        /// <param name="userGroupIds">用户组Ids</param>
        private void GetUserGroup(List<double> userGroupIds)
        {
            // 用户组下的用户
            var userInGroupList = userGroupIds.SplitContains(tempIds =>
            {
                return DB.Query<UserInUserGroup>().Where(p => tempIds.Contains(p.UserGroupId)).ToList();
            });
            this.UserInUserGroups.AddRange(userInGroupList);
            // 用户
            var userIds = userInGroupList.Select(p => p.UserId).ToList();
            var users = userIds.SplitContains(tempIds =>
            {
                return DB.Query<User>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            this.Users.AddRange(users);
        }

        /// <summary>
        /// 获取所有推送员工
        /// </summary>
        private void GetAllEmployee()
        {
            var allEmployeeList = this.AllEmployeeIds.SplitContains(tempIds =>
            {
                return DB.Query<Employee>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            this.AllEmployees.AddRange(allEmployeeList);
        }

        /// <summary>
        /// 获取推送方式
        /// </summary>
        /// <param name="pushPlugIds"></param>
        public void GetPushPlugs(List<double> pushPlugIds)
        {
            var pushPlugList = pushPlugIds.SplitContains(tempIds =>
            {
                return DB.Query<PushPlug>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            this.PushPlugs.AddRange(pushPlugList);
        }

        /// <summary>
        /// 根据razor语法创建推送消息
        /// </summary>
        /// <param name="qTimeStandard"></param>
        /// <param name="reportRecord">报表模板</param>
        /// <param name="messageTemplate"></param>
        /// <returns></returns>
        private async Task<string> CreateMailContentByModel(QTimeStandard qTimeStandard, List<QTimeReportViewModel> reportRecord, MessageTemplate messageTemplate)
        {
            var sendMessage = string.Empty;
            var templateContent = messageTemplate.Message;

            //var engine = new RazorLightEngineBuilder()
            //    .UseEmbeddedResourcesProject(typeof(QTimeReportHandle))//必须有一个模板的类型
            //    .SetOperatingAssembly(typeof(QTimeReportHandle).Assembly)
            //    .UseMemoryCachingProvider()
            //    .DisableEncoding()//禁用编码，否则会把中文字符串编码成Unicode
            //    .Build();

            foreach(var report in reportRecord)
            {
                var hasEnd = report.EndProcess.IsNotEmpty();
                var model = new PushRazorModel
                {
                    Sn = report.Barcode,
                    StartProcess = report.StartProcess,
                    StartState = report.StartState.ToLabel(),
                    OverTime = report.Qtime- report.QTStandard,
                    EndProcess = hasEnd ? report.EndProcess : qTimeStandard.EndProcessName,
                    EndState = hasEnd ? report.EndState.ToLabel() : qTimeStandard.EndState.ToLabel(),
                    HasEnd = hasEnd,
                };
                var razor = RT.Service.Resolve<IRazorScript>();
                var razorString = razor.Parse(templateContent, model);

                //await engine.CompileRenderStringAsync("templateKey", templateContent, model);
                sendMessage += razorString + "<br>"; // 邮件客户端是根据html格式来解析的，所有要用br为不是\n
            }
            return sendMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        private string CreateMailContentByNoModel(QTimeStandard qTimeStandard, List<QTimeReportViewModel> reportRecord)
        {
            var sendMessage = string.Empty;
            foreach (var report in reportRecord)
            {
                if (report.EndProcess.IsNotEmpty())
                {
                    sendMessage += "条码{0}在开始工序{1}开始状态{2}结束工序{3}结束状态{4}，采集时间超时{5}(min)，请注意！"
                        .FormatArgs(report.Barcode, report.StartProcess, report.StartState, report.EndProcess,report.EndState, (report.Qtime - report.QTStandard)) + "<br>";
                    // 邮件客户端是根据html格式来解析的，所有要用br为不是\n
                }
                else
                {
                    sendMessage += "条码{0}在开始工序{1}开始状态{2}采集后，超时{3}(min)未采集结束工序{4}结束状态{5}，请注意及时采集！"
                        .FormatArgs(report.Barcode, report.StartProcess, report.StartState, (report.Qtime - report.QTStandard), qTimeStandard.EndProcessName, qTimeStandard.EndState.ToLabel()) + "<br>";
                }
            }
            return sendMessage;
        }
        #endregion
    }

}
