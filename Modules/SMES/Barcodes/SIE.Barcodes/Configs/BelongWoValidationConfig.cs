using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.Barcodes.Configs
{
    /// <summary>
    /// 归属工单验证配置
    /// </summary>
    [DisplayName("归属工单验证配置")]
    [Description("归属工单验证配置")]
    public class BelongWoValidationConfig :GlobalConfig<BelongWoValidationConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override BelongWoValidationConfigValue DefaultValue { get; } = new BelongWoValidationConfigValue
        {
            ValidationModel = BelongWoValidationModel.BelongBom
        };

    }
}
