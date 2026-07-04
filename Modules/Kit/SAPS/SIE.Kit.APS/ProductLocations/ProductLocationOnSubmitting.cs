using SIE.Domain;
using System.ComponentModel;

namespace SIE.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位提交前事件
    /// </summary>
    [DisplayName("产品定位提交前事件")]
    [Description("修改产品定位分类值")]
    public class ProductLocationOnSubmitting : OnSubmitting<ProductLocation>
    {
        /// <summary>
        /// 产品定位提交前事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(ProductLocation entity, EntitySubmittingEventArgs e)
        {
            RT.Service.Resolve<ProductLocationController>().TransformationTypeValue(entity);
        }
    }
}
