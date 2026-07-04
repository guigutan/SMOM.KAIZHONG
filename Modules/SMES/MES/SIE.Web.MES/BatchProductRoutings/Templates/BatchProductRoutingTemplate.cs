using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.RoutingSettings;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Web.MES.ProductRoutings;

namespace SIE.Web.MES.BatchProductRoutings
{
    /// <summary>
    /// 批次产品工艺路线模板
    /// </summary>
    class BatchProductRoutingTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义布局块（批次产品工艺路线的各个块）
        /// </summary>
        /// <returns>控件块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            AggtBlocks result = new AggtBlocks
            {
                //批次产品自定义查询
                MainBlock = new Block(typeof(WipBatchExt), BatchExtViewConfig.BatchProductViewGroup)
            };
            result.Surrounders.Add(new AggtBlocks()
            {
                //批次产品关健件
                MainBlock = new CommonSurroundBlock(typeof(BatchWipProductProcessKeyItem), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                //工序BOM
                MainBlock = new CommonSurroundBlock(typeof(ProductBomViewModel), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                //批次产品缺陷记录
                MainBlock = new CommonSurroundBlock(typeof(BatchWipProductDefect), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                //批次产品维修记录
                MainBlock = new CommonSurroundBlock(typeof(BatchWipProductRepaire), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                //批次产品工艺路线事件
                MainBlock = new CommonSurroundBlock(typeof(BatchWipProductRoutingEvent), ViewConfig.ListView)
            });

            result.Surrounders.Add(CreateQueryBlocks());
            result.Layout = new LayoutMeta("SIE.Web.MES.ProductRoutings.BatchRoutingLayout");
            return result;
        }

        /// <summary>
        /// 创建自定义查询块
        /// </summary>
        /// <returns>控件块</returns>
        AggtBlocks CreateQueryBlocks()
        {
            var conditionBlock = new ConditionBlock(typeof(BatchCriteria), BatchCriteriaViewConfig.BatchProductRoutingView);
            var entityMeta = CommonModel.Entities.Get(conditionBlock.EntityType);
            var aggtBlocks = this.DefineAggtBlocks(entityMeta, conditionBlock);
            return aggtBlocks;
        }
    }
}