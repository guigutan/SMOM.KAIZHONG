using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.Configs
{
    /// <summary>
    /// 工单归属验证配置值
    /// </summary>
    [RootEntity, Serializable]
    public class BelongWoValidationConfigValue : ConfigValue
    {

        #region 工单归属验证模式 ValidationModel
        /// <summary>
        /// 工单归属验证模式
        /// </summary>
        [Label("工单归属验证模式")]
        public static readonly Property<BelongWoValidationModel> ValidationModelProperty = P<BelongWoValidationConfigValue>.Register(e => e.ValidationModel);

        /// <summary>
        /// 工单归属验证模式
        /// </summary>
        public BelongWoValidationModel ValidationModel
        {
            get { return this.GetProperty(ValidationModelProperty); }
            set { this.SetProperty(ValidationModelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "工单归属验证模式：{0}".L10nFormat(ValidationModel.ToLabel().L10N());
        }
    }
}