using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 供应商被引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("供应商删除验证规则")]
    [System.ComponentModel.Description("供应商被引用不允许删除")]
    public class SupplierNoReferenceIqcBillRule : EntityRule<Supplier>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierNoReferenceIqcBillRule()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var supplier = entity as Supplier;
            if (RT.Service.Resolve<ItemInspCharacteristicsController>().GetInspCharacteristicsList(supplier.Id).Count > 0)
                e.BrokenDescription = "不能删除，供应商被物料检验特性引用".L10N();
        }
    }
}
