using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Download.Employees;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Security;
using SIE.Security.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 员工下载控制器
    /// </summary>
    public class DownloadEmployeeController : DomainController
    {
        /// <summary>
        /// 从API下载员工到业务表
        /// </summary>
        /// <param name="employeeDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadEmployeeToBusiness(List<EmployeeData> employeeDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<EmployeeData>(
                employeeDatas,
                p => this.SaveEmployee(p.OrderByLastUpdateDate()),
                JobType.Employee,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载员工到业务表
        /// <param name="isCreateAccount">是否创建账号</param>
        /// <param name="isManual">是否手动</param>
        /// </summary>
        public virtual ProcessResult DownloadEmployeeInfToBusiness(bool isCreateAccount, bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<EmployeeInf>(
                () => ctl.GetUnprocessedDatas<EmployeeInf>(),          //员工中间表数据
                p =>
                {
                    var paras = this.GenerateEmployeePara(p, isCreateAccount);
                    return this.SaveEmployee(paras.OrderByLastUpdateDate());
                },
                JobType.Employee, isManual);
        }

        /// <summary>
        /// 生成员工实体
        /// </summary>
        /// <param name="employeeInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<EmployeeData> GenerateEmployeePara(IEnumerable<EmployeeInf> employeeInfs, bool isCreateAccount)
        {
            var paras = new List<EmployeeData>();

            employeeInfs.ForEach(p =>
            {
                var data = new EmployeeData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.HireDate = p.HireDate;
                data.Phone = p.Phone;
                data.Email = p.Email;
                data.Remark = p.Remark;
                data.Gender = p.Sex;
                data.AccountCode = p.AccountCode;
                data.IsCreateAccount = isCreateAccount;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapEmployeeController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadEmployeeInfToBusiness(true);           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;
        }


        /// <summary>
        /// 保存数据到员工
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveEmployee(List<EmployeeData> datas)
        {
            var errors = new List<ErpErrorData>();

            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeList(codeList);
            var employeeDic = employees.ToDictionary(p => p.Code);

            var userCodeList = datas.Select(p => p.AccountCode).Distinct().ToList();
            var users = RT.Service.Resolve<EmployeeController>().GetUserList(userCodeList);
            var userDic = users.ToDictionary(p => p.Code);

            var invOrg = Query<Rbac.InvOrgs.InvOrg>().Where(p => p.Code == RT.InvOrg.Value).FirstOrDefault();
            if (invOrg == null)
                throw new ValidationException("库存组织[{0}]不存在".L10nFormat(RT.InvOrg.Value));

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    using (var tran = DB.TransactionScope(Resources.ResourcesEntityDataProvider.ConnectionStringName))
                    {
                        Employee employee;
                        if (!employeeDic.TryGetValue(p.Code, out employee))
                        {
                            employee = new Employee();
                            employee.GenerateId();
                            employeeDic.Add(p.Code, employee);
                        }

                        if (p.IsDelete)
                        {
                            if (employee.Id != 0)
                            {
                                //删除之前，接触用户绑定
                                if (employee.User?.Employee != null)
                                {
                                    employee.User.Employee = null;
                                    RF.Save(employee.User);
                                }

                                employee.PersistenceStatus = PersistenceStatus.Deleted;
                                employeeDic.Remove(p.Code);
                                employee.User = null;
                                RF.Save(employee);

                                tran.Complete();
                            }
                            else
                            {
                                error.ErrMsg = "员工{0}不存在，不能执行删除".L10nFormat(p.Code);
                                errors.Add(error);
                            }
                            continue;
                        }

                        employee.Code = p.Code;
                        employee.Name = p.Name;
                        employee.Sex = (Sex)p.Gender;
                        employee.HireDate = p.HireDate;
                        employee.EmployeeStatus = (EmployeeStatus)p.EmployeeStatus;
                        employee.Phone = p.Phone;
                        employee.Email = p.Email;
                        employee.Remark = p.Remark;

                        //创建账号
                        if (p.IsCreateAccount && (employee.UserId == null || employee.UserId <= 0))
                        {
                            User user;
                            if (!userDic.TryGetValue(p.AccountCode, out user))
                            {
                                user = new User();
                                user.AuthenticateType = AuthenticateManager.Current.DefautlUniqueIdentifier;
                                user.Code = p.AccountCode;
                                user.EmployeeId = employee.Id;
                                user.State = State.Enable;
                                RF.Save(user);

                                //保存用户和组织关系
                                var orgUser = new UserInInvOrg { InvOrg = invOrg, UserId = user.Id, IsInternal = true };
                                RF.Save(orgUser);
                                //设置默认组织
                                RT.Service.Resolve<IUserService>().SetCurInvOrg(user.Id, RT.InvOrg.Value);

                                userDic.Add(p.AccountCode, user);
                                employee.UserId = user.Id;
                            }
                            else if (user.EmployeeId == null || user.EmployeeId <= 0)
                            {
                                user.EmployeeId = employee.Id;
                                RF.Save(user);

                                employee.UserId = user.Id;
                            }
                        }

                        RF.Save(employee);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }
    }
}
