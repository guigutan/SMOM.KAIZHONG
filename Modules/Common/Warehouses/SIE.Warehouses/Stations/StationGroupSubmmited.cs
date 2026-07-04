using SIE.Domain;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台组保存后事件
    /// </summary>
    [System.ComponentModel.DisplayName("站台组保存后事件")]
    [System.ComponentModel.Description("站台组保存后事件")]
    public class StationGroupSubmmited : OnSubmitted<StationGroup>
    {
        /// <summary>
        /// 保存巷道后执行
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">提交参数</param>
        protected override void Invoke(StationGroup entity, EntitySubmittedEventArgs e)
        {             
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                RT.Service.Resolve<WcsAddressController>().CheckAddr(entity.Code);
            }
        }
    }
}
