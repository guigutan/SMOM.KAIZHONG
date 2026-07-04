using SIE.Core.Items;
using SIE.Domain;
using System.Linq;

namespace SIE.Items.Items
{
    /// <summary>
    /// 物料保存前需要保存物料批次规则
    /// </summary>
    [System.ComponentModel.DisplayName("物料保存后需要保存转换单位")]
    [System.ComponentModel.Description("物料保存后需要保存转换单位")]
    public class ItemSecondUnitOnSubmitted : OnSubmitted<Item>
    {
        /// <summary>
        /// 保存物料前执行
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(Item entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                Item item = entity;
                RT.Service.Resolve<ItemUnitController>().SetDefaultUnit(item.Id, item.SecondUnitId);
            }
            if (e.Action == SubmitAction.Delete)
            {
                var unitChages = RT.Service.Resolve<ItemUnitController>().GetUnitChanges(entity.Id);
                unitChages.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                RF.Save(unitChages);
            }
        }
    }
}