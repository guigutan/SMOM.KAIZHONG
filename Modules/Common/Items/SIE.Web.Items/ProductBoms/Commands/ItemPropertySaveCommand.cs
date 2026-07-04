using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductBoms.Commands.ItemPropertySaveCommand")]
    public class ItemPropertySaveCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ItemPropertySaveCommandViewArgs>();
            var productBomDetail = RF.GetById<ProductBomDetail>(data.BomDetailId);
            if (productBomDetail != null)
            {
                productBomDetail.PropertyValueList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                data.ProductBomDetailPropertyValueList.ForEach(p => p.GenerateId());
                productBomDetail.PropertyValueList.AddRange(data.ProductBomDetailPropertyValueList);
                RF.Save(productBomDetail.PropertyValueList);
            }
            else
            {
                throw new ValidationException("当前产品BOM明细未保存，请先保存再选择物料属性值！".L10N());
            }
            return "保存成功！".L10N();
        }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public class ItemPropertySaveCommandViewArgs
    {
        /// <summary>
        /// 产品BOM ID
        /// </summary>
        public double BomDetailId { get; set; }

        /// <summary>
        /// 属性值列表
        /// </summary>
        public EntityList<ProductBomDetailPropertyValue> ProductBomDetailPropertyValueList { get; set; }
    }
}
