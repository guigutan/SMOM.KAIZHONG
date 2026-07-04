using SIE.Domain;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Enterprises
{
    /// <summary>
    /// Ebs企业层级控制器
    /// </summary>
    public class EbsEnterpriseLevelController : DomainController
    {
        /// <summary>
        /// 初始化层级
        /// </summary>
        public virtual EntityList<EnterpriseLevel> InitLevel()
        {
            //EBS层级只有0：集团；1：公司; 2:工厂；3：部门；4：车间）//mom补充一个 5产线
            //只检查通用的组织
            var levels = Query<EnterpriseLevel>().Where(f => f.InvOrgId == 0 || f.InvOrgId == null).ToList();
            var jituan = levels.FirstOrDefault(f => f.Type == EnterpriseType.Group);
            if (jituan != null && levels.Any(f => f.Type == EnterpriseType.Company && f.TreePId == jituan.Id))
            {
                //已手工建立集团公司的层级,则需要手工补充完成
                return levels;
            }
            else
            {//没有加入过数据或者没有集团公司的数据，才进行新增,其他情况默认不需要初始化
                List<EnterpriseType> enterpriseTypes = new List<EnterpriseType>() {
                 EnterpriseType.Group, EnterpriseType.Company,EnterpriseType.Plant, EnterpriseType.Department,EnterpriseType.Shop,EnterpriseType.Line
                };
                EntityList<EnterpriseLevel> les = new EntityList<EnterpriseLevel>();
                for (var i = 0; i < enterpriseTypes.Count; i++)
                {
                    if (i == 0 && jituan != null)
                    {
                        les.Add(jituan);
                    }
                    else
                    {
                        EnterpriseLevel enterpriseLevel = new EnterpriseLevel()
                        {
                            Code = enterpriseTypes[i].ToLabel().L10N(),
                            Name = enterpriseTypes[i].ToLabel().L10N(),
                            InvOrgId = 0,
                            IsResource = true,
                            Type = enterpriseTypes[i]
                        };
                        enterpriseLevel.GenerateId();
                        if (i > 0)
                            enterpriseLevel.TreePId = les[i - 1].Id;

                        les.Add(enterpriseLevel);
                    }
                }
                RF.Save(les);
                return les;
            }             
        }
    }
}
