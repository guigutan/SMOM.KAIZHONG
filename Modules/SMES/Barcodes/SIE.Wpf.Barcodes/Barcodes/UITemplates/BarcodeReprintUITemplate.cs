using SIE.Barcodes;
using SIE.MetaModel;
using SIE.MetaModel.View;

namespace SIE.Wpf.Barcodes.UITemplates
{
    /// <summary>
    /// 条码补打UI模板
    /// </summary>
    public class BarcodeReprintUITemplate : ListUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeReprintUITemplate() : base(typeof(Barcode))
        {
            ViewGroup = BarcodeViewConfig.ReprintView;
        }

        /// <summary>
        /// 指定查询界面
        /// </summary>
        /// <param name="entityMeta">实体元数据</param>
        /// <param name="aggtBlocks">聚合块定义</param>
        protected override void DefineQueryBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks)
        {
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