using SIE.Domain.Validation;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 同一仓库中巷道号非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("同一仓库中巷道号非重验证规则")]
    [System.ComponentModel.Description("同一仓库中巷道号非重验证规则")]
    public class NotDuplicatRoutewayRule : NotDuplicateRule<Routeway>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicatRoutewayRule()
        {
            Properties.Add(Routeway.WarehouseIdProperty);
            Properties.Add(Routeway.RoutewayNumberProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as Routeway;
                return "巷道[{0}]所在仓库已经存在巷道号[{1}]".L10nFormat(entity.Code, entity.RoutewayNumber);
            };
        }
    }

    /// <summary>
    /// 仓库删除规则
    /// </summary>
    [System.ComponentModel.DisplayName("巷道引用删除规则")]
    [System.ComponentModel.Description("巷道被库位引用，不能删除")]
    public class RoutewayReferencedRule : NoReferencedRule<Routeway>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutewayReferencedRule()
        {
            Properties.Add(StorageLocation.RoutewayIdProperty);
            MessageBuilder = (o, e) =>
            {
                var loc = o as StorageLocation;
                return "巷道[{0}]已经被[{1}]引用，不能删除".L10nFormat(loc.Code, "库位".L10N());
            };
        }
    }
}
