using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 子库对照保存后需要前验证
    /// </summary>
    [System.ComponentModel.DisplayName("子库对照保存后需要前验证")]
    [System.ComponentModel.Description("子库对照保存后需要前验证")]
    class ErpWarehouseDetailSubmitting : OnSubmitting<ErpWarehouseDetail>
    {
        protected override void Invoke(ErpWarehouseDetail entity, EntitySubmittingEventArgs e)
        {
            if (e.Action != SubmitAction.Delete && entity.StorageLocationId.HasValue)
            {
                if (RT.Service.Resolve<WarehouseController>().ChecLocHasErpWarehouse(entity.ErpWarehouseId, entity.StorageLocationId.Value))
                    throw new ValidationException("当前库位[{0}]已经配置了对应的ERP子库".L10nFormat(entity.StorageLocation.Code));
            }
        }
    }
}
