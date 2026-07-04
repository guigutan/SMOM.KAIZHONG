using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯经验库查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯经验库查询实体")]
    public class AndonExperienceCriterial : AndonManageCriterial
    {
        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonManageController>().QueryAndonExperience(this);
        }
    }
}
