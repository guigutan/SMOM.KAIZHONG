using SIE.CSM.Suppliers;
using SIE.Domain.Validation;
using SIE.Kit.MES.SingleLabels;
using System;

namespace SIE.Packages.SingleLables
{

    /// <summary>
    /// 供应商被单体条码引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("供应商删除单体条码引用验证规则")]
    [System.ComponentModel.Description("供应商被单体条码引用引用不允许删除")]
    public class SupplierNoReferenceSingleLabelRule : NoReferencedRule<Supplier>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierNoReferenceSingleLabelRule()
        {
            Properties.Add(SingleLabel.SupplierIdProperty);
            MessageBuilder = (o, e) =>
            {
                var supplier = o as Supplier;
                return "供应商[{0}]已经被[{1}]引用，不能删除".L10nFormat(supplier.Code, "单体条码");
            };
        }
    }
}
