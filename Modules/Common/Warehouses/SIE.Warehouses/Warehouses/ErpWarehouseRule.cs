using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Warehouses
{
    [DisplayName("ERP库存组织+子库代码+仓库编码+库区编码+库位编码不能重复验证规则")]
    class ExternalIdNotDuplicate : EntityRule<ErpWarehouse>
    {
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var erpWarehouse = entity as ErpWarehouse;
            if (erpWarehouse != null && erpWarehouse.StorageLocationId.HasValue)
            {
                var ew = RT.Service.Resolve<WarehouseController>().GetErpWarehouse(erpWarehouse);
                if (ew != null && ew.Id != erpWarehouse.Id)
                    e.BrokenDescription = "当前库位编码已经有对应的子库".L10N();
            }
        }
    }
}
