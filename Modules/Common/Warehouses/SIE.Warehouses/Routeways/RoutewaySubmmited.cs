using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 巷道保存后事件
    /// </summary>
    [System.ComponentModel.DisplayName("巷道保存后事件")]
    [System.ComponentModel.Description("巷道保存后事件")]
    public class RoutewaySubmmited : OnSubmitted<Routeway>
    {
        /// <summary>
        /// 保存巷道后执行
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">提交参数</param>
        protected override void Invoke(Routeway entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Delete)
            {
                var locs = RT.Service.Resolve<WarehouseController>().GetStorageLocationByRouteway(entity.Id, null);
                if (locs.Count > 0)
                    throw new ValidationException("巷道[{0}]有库位使用，不能删除".L10nFormat(entity.Name));
            }
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                RT.Service.Resolve<WcsAddressController>().CheckAddr(entity.Code);
            }
        }
    }
}
