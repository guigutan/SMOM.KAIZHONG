using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.OnOffDuty;
using SIE.MES.OnOffDutyA;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OrgLevels
{

    /// <summary>
    /// 
    /// </summary>  
    public partial class OrgLevelController : DomainController
    {
        /// <summary>
        /// 设置人员组织架构
        /// </summary>
        /// <param name="orgLevelList"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("设置人员组织架构")]
        public virtual List<SetOrgLevelInfoRes> SetOrgLevel(List<OrgLevelInfo> orgLevelList)
        {
            List<SetOrgLevelInfoRes> res = new List<SetOrgLevelInfoRes>();
            foreach (OrgLevelInfo orgLevelInfo in orgLevelList)
            {
                if (orgLevelInfo.OrgCode.IsNullOrEmpty() || orgLevelInfo.OrgName.IsNullOrEmpty())
                {
                    res.Add(new SetOrgLevelInfoRes() { ExceptionMsg = "组织ID/组织名称不能为空" });
                    continue;
                }

                var entityData = Query<OrgLevel>().Where(p => p.OrgCode == orgLevelInfo.OrgCode).FirstOrDefault();
                if (entityData == null) { entityData = new OrgLevel(); entityData.OrgCode = orgLevelInfo.OrgCode; }
                entityData.OrgName = orgLevelInfo.OrgName;
                entityData.ParentLevel = orgLevelInfo.ParentLevel;
                entityData.TheLevel = orgLevelInfo.TheLevel;

                RF.Save(entityData);

                var resultEntity = RF.GetById<OrgLevel>(entityData.Id, new EagerLoadOptions().LoadWithViewProperty());
                var aa = new SetOrgLevelInfoRes()
                {
                    OrgCode = resultEntity.OrgCode,
                    OrgName = resultEntity.OrgName,
                    ParentLevel = resultEntity.ParentLevel,
                    TheLevel = resultEntity.TheLevel
                };
                res.Add(aa);
            }
            return res;
        }

    }
}
