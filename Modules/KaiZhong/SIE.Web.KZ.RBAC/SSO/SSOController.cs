using DevExpress.DataProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Web.KZ.RBAC.InvOrgs.Datas;
using SIE.Web.Security;

namespace SIE.Web.KZ.RBAC.SSO
{
    /// <summary>
    /// SSO控制器
    /// </summary>
    [Authorize]
    public class SSOController : Controller
    {
        /// <summary>
        /// SMOM登录
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult SmomLogin()
        {
            var mode = this.Request.Query["mode"].FirstOrDefault();
            var ticket = this.Request.Query["ticket"].FirstOrDefault();
            var code = this.Request.Query["code"].FirstOrDefault();
            var culture = this.Request.Query["culture"].FirstOrDefault();
            var url = this.Request.Query["url"].FirstOrDefault();
            var invOrgId = this.Request.Query["invOrg"].FirstOrDefault();

            if (ticket.IsNullOrEmpty())
                return this.RedirectToAction("Login", "Account");

            if (mode.IsNullOrEmpty())
                mode = "KZSmomNative";

            try
            {
                var login = RT.Service.Resolve<HttpAuthenticateService>().SingInWithTicket(mode, ticket, culture ?? "zh-CN");
                var invOrgs = RF.GetAll<SIE.Rbac.InvOrgs.InvOrg>();
                var invOrg = invOrgs.FirstOrDefault(p => p.ExternalId == invOrgId || p.Code == double.Parse(invOrgId ?? "0"));
                if (invOrg == null)
                    throw new ValidationException("库存组织[{0}]不存在".FormatArgs(invOrgId));
                //RT.Service.Resolve<IIdentityService>().SetCurInvOrg(invOrg.Code, invOrg.Name);
            }
            catch (Exception ex)
            {
                //throw;
                return Content("<script>alert('登录认证失败: " + ex.Message + "');window.location.href='/';</script>", "text/html");
            }

            url = url.IsNotEmpty() ? url : "/";
            return this.Redirect(url);
        }

        /// <summary>
        /// 通过员工编码获取员工所有的库存组织数据
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public JsonResult? GetInvOrgsByUser(string userCode)
        {
            var orgDatas = new List<InvOrgData>();
            var user = RT.Service.Resolve<UserController>().GetUserByCode(userCode);
            if (user == null)
                return Json(orgDatas);
            var datas = RT.Service.Resolve<InvOrgController>().GetInvOrgsByUserId(user?.Id ?? 0);
            orgDatas.AddRange(datas.Select(p => new InvOrgData()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ExternalId = p.ExternalId,
                Remark = p.Remark,
            }));
            return Json(orgDatas);
        }
    }

}
