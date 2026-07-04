using NPOI.SS.Formula.Functions;
using SIE.Common.Employees;
using SIE.Common.Users;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// 接口登录控制器
    /// </summary>
    public class KzLoginController : DomainController
    {
        public static double? EmployeeId;
        public static double? UserId;

        #region 接口账号登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="InvOrgId"></param>
        public virtual void Login(string InvOrgId)
        {
            //赋值操作人员
            Login();
            //再指定库存组织
            SetInvOrgByExternalId(InvOrgId);
        }

        /// <summary>
        /// 指定库存组织
        /// </summary>
        /// <param name="InvOrgId"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void SetInvOrgByExternalId(string InvOrgId)
        {
            var invOrg = Query<InvOrg>().Where(p => p.ExternalId == InvOrgId).FirstOrDefault();
            if (invOrg == null)
                throw new ValidationException("{0}库存组织不存在".L10nFormat(InvOrgId));
            RT.InvOrg = (int)invOrg.Code;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public virtual void Login()
        {

            //if (!EmployeeId.HasValue || !UserId.HasValue)
            //{
            //    var code = AppRuntime.Config.Get("InfLoginUser");
            //    EmployeeId = DB.Query<Employee>().Where(p => p.Code == code).FirstOrDefault()?.Id;
            //    UserId = DB.Query<User>().Where(p => p.EmployeeId == EmployeeId).FirstOrDefault()?.Id;
            //}
            //if (!EmployeeId.HasValue || !UserId.HasValue)
            //{
            //    var code = AppRuntime.Config.Get("InfLoginUser");
            //    throw new ValidationException("账号[{0}]在SMOM不存在".L10nFormat(code));
            //}

            //RT.Principal = new DataPortal.DataPortalPrincipal(EmployeeId.Value, UserId.Value, "");
            var user = AppRuntime.Config.Get("InfLoginUser");
            var password = AppRuntime.Config.Get("InfLoginUserPw");
            RT.Service.Resolve<AuthenticationController>().Login(user, password);
        }
        #endregion

    }
}
