using SIE.Barcodes;
using SIE.MetaModel;
using SIE.MetaModel.View;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 补打模板
    /// </summary>
    public class BarcodeReprintTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义查询块
        /// </summary>
        /// <param name="entityMeta">实体元数据</param>
        /// <param name="aggtBlocks">聚合块</param>
        protected override void DefineQueryBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks)
        {
            base.DefineQueryBlocks(entityMeta, aggtBlocks);
            if (entityMeta.EntityType == typeof(BarcodeReprint))
            {
                var conditionBlock = new ConditionBlock(typeof(BarcodeCriteria), BarcodeViewConfig.ReprintView);
                var em = CommonModel.Entities.Get(conditionBlock.EntityType);
                var result = this.DefineAggtBlocks(em, conditionBlock);
                aggtBlocks.Surrounders.Add(result);
            }
        }
    }
}