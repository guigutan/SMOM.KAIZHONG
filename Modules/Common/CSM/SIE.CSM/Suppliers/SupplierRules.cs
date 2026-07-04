using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using System;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商已启用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("供应商删除验证规则")]
    [System.ComponentModel.Description("供应商已启用不能删除")]
    public class UndeleteSupplierEnableRules : EntityRule<Supplier>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteSupplierEnableRules()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var supplier = entity as Supplier;
            if (supplier.State == State.Enable && e != null)
            {
                e.BrokenDescription = "供应商已启用不能删除".L10N();
            }
        }
    }

    /// <summary>
    /// 物料被供应商物料引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料被供应商物料引用不允许删除")]
    [System.ComponentModel.Description("物料被供应商物料引用不允许删除")]
    public class UndeleteInvolveSupplierItem : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteInvolveSupplierItem()
        {
            Properties.Add(SupplierItem.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "供应商物料".L10N());
            };
        }
    }

    /// <summary>
    /// 供应商与用户关系非重验证
    /// </summary>
    [System.ComponentModel.DisplayName("供应商与用户关系非重验证")]
    [System.ComponentModel.Description("供应商与用户关系不能重复")]
    public class SupplierUserNotDuplicateRule : NotDuplicateRule<SupplierUser>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierUserNotDuplicateRule()
        {
            Properties.Add(SupplierUser.SupplierIdProperty);
            Properties.Add(SupplierUser.UserIdProperty);
            MessageBuilder = (e) => { return "供应商与用户关系不能重复。".L10N(); };
        }
    }
}