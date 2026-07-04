using SIE.MetaModel;
using SIE.MetaModel.View;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码报废模板
    /// </summary>
    public class BarcodeScrapTemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 定义查询块
        /// </summary>
        /// <param name="entityMeta">实体元数据</param>
        /// <param name="aggtBlocks">聚合块</param>
        protected override void DefineQueryBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks)
        {
            base.DefineQueryBlocks(entityMeta, aggtBlocks);
        }
    }
}