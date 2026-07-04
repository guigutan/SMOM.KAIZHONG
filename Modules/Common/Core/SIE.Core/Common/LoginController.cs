using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Common
{
    public class LoginController : DomainController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="InvOrgId"></param>
        public virtual void Login(string InvOrgId)
        {
            //赋值操作人员
            Login();
            //再指定库存组织
            var invOrg = Query<InvOrg>().Where(p => p.ExternalId == InvOrgId).FirstOrDefault();
            if (invOrg == null)
                throw new ValidationException("");
            RT.InvOrg = (int)invOrg.Code;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public virtual void Login()
        {
            var user = AppRuntime.Config.Get("InfLoginUser");
            var password = AppRuntime.Config.Get("InfLoginUserPw");
            RT.Service.Resolve<AuthenticationController>().Login(user, password);
        }
    }
}
