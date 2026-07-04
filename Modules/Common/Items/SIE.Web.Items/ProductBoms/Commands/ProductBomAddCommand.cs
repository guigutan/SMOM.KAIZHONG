using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Items;
using SIE.Items.Configs;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductBoms.Commands.ProductBomAddCommand")]
    public class ProductBomAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            ProductBom bom = args.Data.ToJsonObject<ProductBom>();
            var config = ConfigService.GetConfig(new ProductBomVersionConfig(), typeof(ProductBom));
            if (config.Version != null)
            {
                bom.Version = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.Version, 1).FirstOrDefault();
            }
            return bom.Version;
        }
    }
}
