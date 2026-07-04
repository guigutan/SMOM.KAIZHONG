using SIE.KZ.Base.InvOrgs;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using SIE.Web.Command;
using SIE.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.Web.KZ.RBAC.InvOrgs.Commands
{
    /// <summary>
    /// 切换组织(跨工厂)
    /// </summary>
    [AllowAnonymous]
    public class SwitchInvOrgCommand : ViewCommand<InvOrgGroup>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="invOrg"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(InvOrgGroup invOrg, string scope)
        {
            //return base.Excute(invOrg, scope);
            var url = string.Empty;
            Check.NotNull(invOrg, "invOrg");
            if (invOrg.Code != RT.InvOrg)
            {
                if (invOrg.WebSite.IsNullOrEmpty())
                {
                    RT.Service.Resolve<IIdentityService>().SetCurInvOrg(invOrg.Code, invOrg.Name);
                }
                else
                {
                    var ticket = string.Empty;
                    Match match = Regex.Match(RT.Identity.Name, @"\[(.*?)\]");
                    if (match.Success)
                    {
                        ticket = match.Groups[1].Value;
                    }
                    else
                    {
                        throw new Exception("当前登录用户解析失败");
                    }

                    url = $"{invOrg.WebSite}/SSO/SmomLogin?invOrg={invOrg.Code}&ticket={ticket}";

                }
            }

            return url;
        }
    }
}
