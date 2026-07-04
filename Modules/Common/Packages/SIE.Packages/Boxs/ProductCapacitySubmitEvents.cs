using SIE.Domain;
using System.ComponentModel;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 产品容量提交前事件
    /// </summary>
    [DisplayName("产品容量提交前事件")]
    [Description("新增或修改默认产品容量时，修改")]
    public class ProductCapacitySubmitting : OnSubmitting<ProductCapacity>
    {
        /// <summary>
        /// 保存前执行方法
        /// </summary>
        /// <param name="entity">产品容量</param>
        /// <param name="e">提交参数</param>
        protected override void Invoke(ProductCapacity entity, EntitySubmittingEventArgs e)
        {
            if (entity.PersistenceStatus == PersistenceStatus.Deleted) return;

            var boxController = RT.Service.Resolve<BoxController>();
            if (entity.IsDefault)
            {
                boxController.UpdateDefaultProductCapacitys(entity.ItemId, entity.Id);
                return;
            }
            if (!boxController.HasDefaultProductCapacity(entity))
            {
                entity.IsDefault = true;
            }
        }
    }
}
