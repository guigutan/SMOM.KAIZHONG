using SIE.Domain;
using System.Linq;

namespace SIE.Warehouses
{
    /// <summary>
    /// 逻辑分区保存后事件
    /// </summary>
    [System.ComponentModel.DisplayName("逻辑分区保存后事件")]
    [System.ComponentModel.Description("逻辑分区保存后事件")]
    public class LogicAreaSubmmited : OnSubmitted<LogicArea>
    {
        /// <summary>
        /// 保存巷道后执行
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">提交参数</param>
        protected override void Invoke(LogicArea entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Delete)
            {
                var locs = RT.Service.Resolve<WarehouseController>().GetLogicAreaLocations(entity.Id, null);
                locs.ForEach(f => f.PersistenceStatus = PersistenceStatus.Deleted);
                RF.Save(locs);
            }
        }
    }
}
