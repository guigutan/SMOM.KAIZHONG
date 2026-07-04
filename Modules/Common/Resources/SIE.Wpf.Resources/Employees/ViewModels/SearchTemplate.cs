using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Common.Users;

namespace SIE.Wpf.Resources.Employees.ViewModels
{
    /// <summary>
    /// 关联账户的弹框界面模板
    /// </summary>
    public class SearchTemplate : ListUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SearchTemplate() : base(typeof(User)) { }

        /// <summary>
        /// 定义查询块
        /// </summary>
        /// <param name="entityMeta">EntityMeta</param>
        /// <param name="aggtBlocks">AggtBlocks</param>
        protected override void DefineQueryBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks)
        {
           //////if (em.EntityType == typeof(User))
           // //{
           // //    ////条件面板块
           // //    var surBlock = new ConditionBlock(typeof(EmployeeLinkUserlCriteria), WPFViewConfig.QueryView);
           // //    ////用通用模板去获取 条件面板块 查询到的结果
           // //    var surEM = CommonModel.Entities.Get(surBlock.EntityType);
           // //    ////定义聚合块
           // //    var surAggt = this.DefineAggtBlocks(surEM, surBlock);
           // //    result.Surrounders.Add(surAggt);
           // //}
           // //else
                base.DefineQueryBlocks(entityMeta, aggtBlocks);
        }

        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>块</returns>
        protected override Block DefineMainBlock()
        {
            var block = base.DefineMainBlock();
            (block.ViewMeta as WPFEntityViewMeta).ClearCommands();
            return block;
        }

        /// <summary>
        /// 清空子块
        /// </summary>
        /// <param name="entityMeta">EntityMeta</param>
        /// <param name="aggtBlocks">AggtBlocks</param>
        /// <param name="mainBlock">Block</param>
        protected override void DefineChildBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks, Block mainBlock)
        {
            ////base.DefineChildBlocks(entityMeta, aggtBlocks, mainBlock);
        }
    }
}
