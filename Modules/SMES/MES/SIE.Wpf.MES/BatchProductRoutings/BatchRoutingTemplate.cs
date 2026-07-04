using SIE.Barcodes.WipBatchs;
using SIE.MES.BatchWIP;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Wpf.MES.ProductRoutings;

namespace SIE.Wpf.MES.BatchProductRoutings
{
    /// <summary>
    /// 产品工艺路线模板
    /// </summary>
    public class BatchRoutingTemplate : ListUITemplate
    {
        /// <summary>
        /// 初始化实体和视图
        /// </summary>
        public BatchRoutingTemplate() : base(typeof(WipBatch))
        {
            ViewGroup = BatchExtViewConfig.BatchProductViewGroup;
        }

        /// <summary>
        /// 重写生成聚合块的方法
        /// </summary>
        /// <returns>返回新的聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(DockLayout));
            return blocks;
        }

        /// <summary>
        /// 重写定义查询聚合块的方法
        /// </summary>
        /// <param name="em">实体元数据</param>
        /// <param name="result">聚合块</param>
        protected override void DefineQueryBlocks(EntityMeta em, AggtBlocks result)
        {
            if (em.EntityType == typeof(WipBatch)) //指定查询界面
            {
                var conditionBlock = new ConditionBlock(typeof(BatchCriteria), ViewConfig.ListView);
                var entityMeta = CommonModel.Entities.Get(conditionBlock.EntityType);
                var aggtBlocks = this.DefineAggtBlocks(entityMeta, conditionBlock);
                result.Surrounders.Add(aggtBlocks);
            }
        }
    }
}