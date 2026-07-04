using SIE.Domain;
using System.ComponentModel;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 产品（考虑扩展属性）的第一个bom设置为缺省
    /// </summary>
    [DisplayName("相同产品编码的产品BOM为非默认")]
    [Description("相同产品编码的产品BOM为非默认")]
    public class ProductBomOnSubmitting : OnSubmitting<ProductBom>
    {
        /// <summary>
        /// 表示将处理事件的方法
        /// </summary>
        /// <param name="bom">泛型实体</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void Invoke(ProductBom bom, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert &&
                !RT.Service.Resolve<ProductBomController>().IsExistDefaultProductBomWithExtProp(bom.ProductId,bom.ItemExtProp))
            {
                bom.IsDefault = true;
            }
        }
    }
}