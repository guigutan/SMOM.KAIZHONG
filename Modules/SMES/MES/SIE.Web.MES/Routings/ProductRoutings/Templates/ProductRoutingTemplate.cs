using SIE.Barcodes;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.MetaModel.View;

namespace SIE.Web.MES.ProductRoutings
{
    class ProductRoutingTemplate : CodeBlocksTemplate
    {
        protected override AggtBlocks DefineBlocks()
        {
            AggtBlocks result = new AggtBlocks
            {
                MainBlock = new Block(typeof(WipProductBarcode), WipProductBarcodeViewConfig.BarcodeViewGroup)
            };
            result.Surrounders.Add(new AggtBlocks()
            {
                MainBlock = new CommonSurroundBlock(typeof(WipProductProcessKeyItem), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                MainBlock = new CommonSurroundBlock(typeof(WipProductTestResult), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                MainBlock = new CommonSurroundBlock(typeof(ProductBomViewModel), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                MainBlock = new CommonSurroundBlock(typeof(WipProductDefect), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                MainBlock = new CommonSurroundBlock(typeof(WipProductRepair), ViewConfig.ListView)
            });
            result.Surrounders.Add(new AggtBlocks()
            {
                MainBlock = new CommonSurroundBlock(typeof(WipProductRoutingEvent), ViewConfig.ListView)
            });

            result.Surrounders.Add(createQueryBlocks());
            result.Layout = new LayoutMeta("SIE.Web.MES.ProductRoutings.ProductRoutingLayout");
            return result;
        }

        AggtBlocks createQueryBlocks()
        {
            var conditionBlock = new ConditionBlock(typeof(WipProductBarcodeCriteria), WipProductBarcodeCriteriaViewConfig.ProductRoutingView);
            var entityMeta = CommonModel.Entities.Get(conditionBlock.EntityType);
            var aggtBlocks = this.DefineAggtBlocks(entityMeta, conditionBlock);
            return aggtBlocks;
        }
    }
}