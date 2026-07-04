using SIE.Domain;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 分配方案明细为空时，同时删除分配方案
    /// </summary>
    [System.ComponentModel.DisplayName("删除分配方案")]
    [System.ComponentModel.Description("分配方案明细为空时,同时删除分配方案")]
    public class BranchFactoryProgrammeOnSubmitted : OnSubmitted<BranchFactoryProgramme>
    {
        /// <summary>
        /// 分配方案明细为空时，同时删除分配方案
        /// </summary>
        /// <param name="entity">分配方案</param>
        /// <param name="e">提交事件参数</param>
        protected override void Invoke(BranchFactoryProgramme entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                var batchRule = RT.Service.Resolve<BranchFactoryProgrammeController>().CountBranchFactoryProgrammeDetail(entity.Id);
                if (batchRule <= 0)
                {
                    BranchFactoryProgramme branchFactoryProgramme = entity;
                    branchFactoryProgramme.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(branchFactoryProgramme);
                }
            }
        }
    }
}
