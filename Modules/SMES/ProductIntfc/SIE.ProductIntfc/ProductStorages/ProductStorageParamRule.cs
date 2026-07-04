using SIE.Domain.Validation;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库参数验证规则
    /// </summary>
    class ProductStorageParamRule
    {

        /// <summary>
        /// 产品编码非重验证
        /// </summary>
        [System.ComponentModel.DisplayName("产品编码非重验证")]
        [System.ComponentModel.Description("产品编码不能重复")]
        public class ProductStorageParamItemNotDuplicateRule : NotDuplicateRule<ProductStorageParam>
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public ProductStorageParamItemNotDuplicateRule()
            {
                Properties.Add(ProductStorageParam.ItemIdProperty);
                MessageBuilder = (e) =>
                {
                    var par = e as ProductStorageParam;
                    return "产品编码[{0}] 不能重复".L10nFormat(par.Item?.Code);
                };
            }
        }
    }
}
