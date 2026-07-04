using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ldap.Connection;
using SIE.Rbac.Config;
using System;

namespace SIE.ERPInterface.Ldap
{
    /// <summary>
    /// 企业模型下载控制器
    /// </summary>
    public class LdapDownloadEnterpriseController : DownloadInfBaseController
    {
        /// <summary>
        /// 从ERP下载员工到中间表
        /// </summary>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            //加载LDAP配置参数
            var ldapAddress = RT.Config.Get<string>(RbacConfigKeys.LdapAddress);
            var ldapAdUser = RT.Config.Get<string>(RbacConfigKeys.LdapAdUser);
            var ldapAdPwd = RT.Config.Get<string>(RbacConfigKeys.LdapAdPwd);

            //加载构建任务下载时间
            var jobTime = this.GetDownloadJobTime(JobType.Employee);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();
            jobTimeDetail.RequestDate = DateTime.Now;
            jobTimeDetail.RequestStr = "ldapAddress:{0};ldapAdUser:{1};ldapAdPwd:{2};"
                .FormatArgs(ldapAddress + System.Environment.NewLine, ldapAdUser + System.Environment.NewLine, ldapAdPwd + System.Environment.NewLine);

            //链接IDM
            var propertiesToLoad = new string[]
                { "uid" , "employeeNumber", "mail", "smart-status" ,"smart-gender","cn","mobile","smart-type"};
            var resultCollection = LDAPHelper.GetDatas(ldapAddress, ldapAdUser, ldapAdPwd, propertiesToLoad);
            jobTimeDetail.ResponseDate = DateTime.Now;

            //执行保存中间表
            return this.SaveInfData<EmployeeInf>(resultCollection, p =>
            {
                var propertyCollection = p.Properties;

                var propertyUid = propertyCollection["uid"];
                var propertyCn = propertyCollection["cn"];
                var propertyGender = propertyCollection["smart-gender"];
                var propertyMail = propertyCollection["mail"];
                var propertyEmployeeNumber = propertyCollection["employeeNumber"];
                ////var propertyType = propertyCollection["smart-type"];

                if (propertyUid == null)
                    throw new ValidationException("[{0}]不存在uid".L10nFormat(p.Path));
                if (propertyCn == null)
                    throw new ValidationException("[{0}]不存在cn".L10nFormat(p.Path));

                var employeeInf = new EmployeeInf();
                employeeInf.AccountCode = propertyUid[0].ToString();
                employeeInf.Name = propertyCn[0].ToString();
                employeeInf.Email = propertyMail == null || propertyMail.Count <= 0 ? string.Empty : propertyMail[0]?.ToString();
                employeeInf.Sex = propertyGender == null || propertyGender.Count <= 0 ? 0 : int.Parse(propertyGender[0]?.ToString()) - 1;
                employeeInf.LastUpdateDate = DateTime.Now;
                employeeInf.IsManual = false;

                if (ldapAdUser.Contains(employeeInf.AccountCode))
                    //绑定账号没有工号，员工编码取cn
                    employeeInf.Code = employeeInf.Name;
                else
                    //O类员工在IDM没有工号，员工编码取uid
                    employeeInf.Code = propertyEmployeeNumber == null || propertyEmployeeNumber.Count <= 0 ? employeeInf.AccountCode : propertyEmployeeNumber[0]?.ToString();

                employeeInf.ErpKey = employeeInf.Code;
                return employeeInf;

            }, JobType.Employee, jobTime, jobTimeDetail, DateTime.Now, false);
        }
    }
}
