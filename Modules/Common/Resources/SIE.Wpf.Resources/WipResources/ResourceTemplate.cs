using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;

namespace SIE.Wpf.Resources.WipResources
{
    /// <summary>
    /// 生产资源列表模板
    /// </summary>
    public class ResourceTemplate : ListUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResourceTemplate() : base(typeof(WipResource), ViewConfig.ListView)
        {
        }

        /// <summary>
        /// 定义查询快
        /// </summary>
        /// <param name="entityMeta">实体元数据</param>
        /// <param name="aggtBlocks">聚合快</param>
        protected override void DefineQueryBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks)
        {
            base.DefineQueryBlocks(entityMeta, aggtBlocks);
            //if (entityMeta.EntityType == typeof(WipResource))
            //{
            //    var conditionBlock = new ConditionBlock(typeof(WipResourceCriteria), ViewConfig.QueryView);
            //    var tmpEm = CommonModel.Entities.Get(conditionBlock.EntityType);
            //    var tmpAggBlk = this.DefineAggtBlocks(tmpEm, conditionBlock);
            //    aggtBlocks.Surrounders.Add(tmpAggBlk);
            //}
        }

        /// <summary>
        /// 定义快，设置布局
        /// </summary>
        /// <returns>聚合快</returns>
        protected override AggtBlocks DefineBlocks()
        {
            //return base.DefineBlocks();
            var block = base.DefineBlocks();
            block.Layout = new LayoutMeta(typeof(ResourceDockLayout));
            return block;
        }
    }
}
