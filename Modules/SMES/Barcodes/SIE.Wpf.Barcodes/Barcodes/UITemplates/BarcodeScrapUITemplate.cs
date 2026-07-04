using SIE.Barcodes;
using SIE.MetaModel;
using SIE.MetaModel.View;

namespace SIE.Wpf.Barcodes.UITemplates
{
    /// <summary>
    /// 条码报废UI模板
    /// </summary>
    public class BarcodeScrapUITemplate : ListUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeScrapUITemplate() : base(typeof(Barcode))
        {
            ViewGroup = BarcodeViewConfig.ScrapView;
        }

        /// <summary>
        /// 指定查询界面
        /// </summary>
        /// <param name="entityMeta">实体元数据</param>
        /// <param name="aggtBlocks">聚合块定义</param>
        protected override void DefineQueryBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks)
        {
            if (entityMeta.EntityType == typeof(Barcode))
            {
                var conditionBlock = new ConditionBlock(typeof(BarcodeCriteria), BarcodeViewConfig.ScrapView);
                var em = CommonModel.Entities.Get(conditionBlock.EntityType);
                var result = this.DefineAggtBlocks(em, conditionBlock);
                aggtBlocks.Surrounders.Add(result);
            }
        }
    }
}