using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Configs;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Items.Commands
{
    /// <summary>
    /// 产品BOM添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    class ProductBomAddCommand : ListAddCommand
    {
        /// <summary>
        /// 重写实体创建方法
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            ProductBom bom = entity as ProductBom;
            var config = ConfigService.GetConfig(new ProductBomVersionConfig(), typeof(ProductBom));
            if (config.Version != null)
            {
                bom.Version = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.Version, 1).FirstOrDefault();
            }
            //bom.IsDefault = true;
        }
    }
}
