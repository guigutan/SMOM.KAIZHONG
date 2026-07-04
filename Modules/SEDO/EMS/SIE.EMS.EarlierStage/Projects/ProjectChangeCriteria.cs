using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("项目变更查询实体")]
    public partial class ProjectChangeCriteria : ProjectCriteria
    {
        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProjectChangeController>().CriteriaProjectChanges(this);
        }
    }
}
