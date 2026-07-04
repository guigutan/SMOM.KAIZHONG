using crypto;
using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources;
using SIE.Resources.Employees;
using SIE.Resources.WorkCenters;
using SIE.Security.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee = SIE.Resources.Employees.Employee;
using EmployeeStatus = SIE.Resources.Employees.EmployeeStatus;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadEmployeeController : DomainController
    {

        /// <summary>
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="erpDataInfLog">日志</param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveEmployees(List<EmployeeData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<EmployeeData> list = new List<EmployeeData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.Employee, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    var codes = datas.Select(p => p.PERNR).Distinct().ToList();
                    var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeList(codes);
                    var users = RT.Service.Resolve<EmployeeController>().GetUserList(codes);
                    var invOrg = Query<Rbac.InvOrgs.InvOrg>().Where(p => p.Code == RT.InvOrg.Value).FirstOrDefault();
                    if (invOrg == null)
                        throw new ValidationException("库存组织[{0}]不存在".L10nFormat(RT.InvOrg.Value));

                    foreach (var item in datas)
                    {
                        try
                        {
                            var employee = employees.FirstOrDefault(p => p.Code == item.PERNR);
                            var user = users.FirstOrDefault(p => p.Code == item.PERNR);
                            employee = CreateEmployee(employee, item, user, invOrg);
                            if (employees.All(p => p.Id != employee.Id))
                                employees.Add(employee);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);

                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"编码{item.PERNR}:" + ex.GetBaseException()?.Message);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<EmployeeData>(erpDataInfLog, list, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<EmployeeData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveEmployees(List<EmployeeData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<EmployeeData> list = new List<EmployeeData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            var codes = datas.Select(p => p.PERNR).Distinct().ToList();
            var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeList(codes);
            var users = RT.Service.Resolve<EmployeeController>().GetUserList(codes);
            var invOrg = Query<Rbac.InvOrgs.InvOrg>().Where(p => p.Code == RT.InvOrg.Value).FirstOrDefault();
            if (invOrg == null)
                throw new ValidationException("库存组织[{0}]不存在".L10nFormat(RT.InvOrg.Value));
            try
            {
                if (datas != null || datas.Count > 0)
                {

                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {

                                var employee = employees.FirstOrDefault(p => p.Code == item.PERNR);
                                var user = users.FirstOrDefault(p => p.Code == item.PERNR);
                                employee = CreateEmployee(employee, item, user, invOrg);

                                if (employees.All(p => p.Id != employee.Id))
                                    employees.Add(employee);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"编码{item.PERNR}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<EmployeeData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;

        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private Employee CreateEmployee(Employee employee, EmployeeData item, User user, Rbac.InvOrgs.InvOrg invOrg)
        {
            if (employee == null)
            {
                employee = new Employee();
                employee.Code = item.PERNR;
                employee.Name = item.ENAME;
                employee.EmployeeStatus = EmployeeStatus.Job;
                employee.GenerateId();
                employee.PersistenceStatus = PersistenceStatus.New;
                RF.Save(employee);
            }

            if (item.GESCH == "男")
                employee.Sex = Sex.Man;
            else
                employee.Sex = Sex.Madam;

            if (item.STAT2 == "3")
                employee.EmployeeStatus = EmployeeStatus.Job;
            else
                employee.EmployeeStatus = EmployeeStatus.UnJob;
            employee.OrgLevel1 = item.ZYJZXID;
            employee.OrgLevel2 = item.ZEJBMID;
            employee.OrgLevel3 = item.ZSJBZID;
            employee.OrgLevel4 = item.ZSJXBID;
            employee.Phone = item.TELNR;
            employee.Email = item.EMAIL;
            employee.Name = item.ENAME;
            if (employee.UserId == null || employee.UserId <= 0)
            {
                if (user == null)
                {
                    user = new User();
                    user.AuthenticateType = AuthenticateManager.Current.DefautlUniqueIdentifier;
                    user.Code = employee.Code;
                    user.EmployeeId = employee.Id;
                    user.State = State.Enable;
                    //员工离职状态时，同步用户应该调整为“禁用”状态。
                    if (employee.EmployeeStatus == EmployeeStatus.UnJob)
                        user.State = State.Disable;
                    RF.Save(user);
                    //保存用户和组织关系
                    //var orgUser = new UserInInvOrg { InvOrg = invOrg, UserId = user.Id, IsInternal = false };
                    //RF.Save(orgUser);

                    //初始用户密码与员工号保持一致
                    var userSecurity = user.GetProperty(UserExtProperty.UserSecurityProperty);
                    if (userSecurity != null)
                    {
                        //取消强制安全信息
                        //userSecurity.ChangePwdNextTime = false;
                        //userSecurity.EnforcePwdExpiration = false;
                        //userSecurity.EnforcePwdPolicy = false;
                        //RT.Service.Resolve<UserController>().SaveRawPwd(userSecurity, user.Code);
                        DB.Update<UserSecurity>()
                            .Set(p => p.ChangePwdNextTime, false)
                            .Set(p => p.EnforcePwdExpiration, false)
                            .Set(p => p.EnforcePwdPolicy, false).Where(p => p.Id == userSecurity.Id).Execute();
                    }
                    else
                    {
                        userSecurity = RT.Service.Resolve<UserController>().GetUserSecurityByUserId(user.Id);
                        if (userSecurity == null)
                        {
                            userSecurity = new UserSecurity();
                            userSecurity.GenerateId();
                            userSecurity.UserId = user.Id;
                            userSecurity.Password = SIE.Security.CryptographyHelper.MD5("123456");
                            userSecurity.ChangePwdNextTime = false;
                            userSecurity.EnforcePwdExpiration = false;
                            userSecurity.EnforcePwdPolicy = false;
                            RF.Save(userSecurity);
                        }
                        DB.Update<UserSecurity>().Set(p => p.ChangePwdNextTime, false).Set(p => p.EnforcePwdExpiration, false).Set(p => p.EnforcePwdPolicy, false).Where(p => p.Id == userSecurity.Id).Execute();
                    }
                }
                employee.UserId = user.Id;
            }
            else
            {
                if (employee.EmployeeStatus == EmployeeStatus.Job && employee.User.State == State.Disable)
                {
                    user.State = State.Enable;
                    RF.Save(user);
                }
                if (employee.EmployeeStatus == EmployeeStatus.UnJob && employee.User.State == State.Enable)
                {
                    user.State = State.Disable;
                    RF.Save(user);
                }
            }
            RF.Save(employee);
            return employee;
        }


    }
}
