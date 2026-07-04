using SIE.Common;
using SIE.Core.Common;
using SIE.Domain;
using SIE.KZ.Base.InvOrgs;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Security;
using SIE.Web.Data;
using SIE.Web.KZ.RBAC.InvOrgs.Datas;

namespace SIE.Web.KZ.RBAC.InvOrgs
{
    /// <summary>
    /// 数据查询器
    /// </summary>
    [AllowAnonymous]
    public class InvOrgDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取所有基地库存组织数据 (前提条件:员工编码与用户编码相同)
        /// </summary>
        /// <returns></returns>
        public EntityList<InvOrgGroup> GetInvOrgGroupsByUser()
        {
            //return RT.Service.Resolve<InvOrgGroupController>().GetInvOrgGroupsByUserId(AppRuntime.Identity.UserId);

            var localInvOrgs = RT.Service.Resolve<InvOrgController>().GetInvOrgsByUserId(RT.Identity.UserId);
            var list = new EntityList<InvOrgGroup>();
            list.AddRange(localInvOrgs.Where(p => p.ExternalId.IsNotEmpty()).Select(p => new InvOrgGroup()
            {
                Code = p.Code,
                Name = p.Name,
                ExternalId = p.ExternalId,
                Remark = p.Remark
            }));

            var user = RF.GetById<User>(RT.Identity.UserId);
            var webSites = RT.Config.Get("SMOM.WebSite", new List<string>() { "http://localhost:61617" });
            var notDuplicate = RT.Config.Get("SMOM.NotDuplicate", true);
            //var urls = webSites.Split(",");
            List<Task> tasks = new List<Task>();
            foreach (var url in webSites)
            {
                if (url.IsNullOrEmpty())
                    continue;
                tasks.Add(Task.Run(() =>
                {
                    var headers = new Dictionary<string, string>();
                    var param = new Dictionary<string, string>();
                    try
                    {
                        var result = HttpClientHelper.GetAsync(url + $"/SSO/GetInvOrgsByUser?userCode={user?.Code}", headers, param).Result;
                        if (result.IsNullOrEmpty())
                            return;
                        var datas = result.ToJsonObject<List<InvOrgData>>();
                        foreach (var p in datas)
                        {
                            if (p.ExternalId.IsNullOrEmpty())
                                continue;
                            if (notDuplicate && list.Any(x => x.Code == p.Code && x.ExternalId == p.ExternalId))
                                continue;
                            list.Add(new InvOrgGroup()
                            {
                                Code = p.Code,
                                Name = p.Name,
                                ExternalId = p.ExternalId,
                                Remark = p.Remark,
                                WebSite = url
                            });
                        }

                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            return list.OrderBy(p => p.ExternalId).AsEntityList();
        }
    }
}
