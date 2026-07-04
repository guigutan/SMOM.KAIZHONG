using SIE.Web.ClientMetaModel;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    /// 货币编辑器
    /// </summary>
    public class CurrencyConfig :  FieldConfig
    {
        /// <summary>
        /// 货币编辑器配置
        /// </summary>
        public CurrencyConfig()
        {
            this.XType = "currencyField";
            this.AllowBlank = true;
        }

    }
}
