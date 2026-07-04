using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.Suppliers.Configs
{
    /// <summary>
    /// 是否自动创建供应商用户
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(IsAutoCreate))]
    [Label("启用规则")]
    public class SupplierIsAutoCreateConfigValue : ConfigValue
    {
        #region 配置规则 IsAutoCreate
        /// <summary>
        /// 是否自动创建供应商用户
        /// </summary>
        [Label("自动创建用户信息")]
        public static readonly Property<bool> IsAutoCreateProperty = P<SupplierIsAutoCreateConfigValue>.Register(e => e.IsAutoCreate);

        /// <summary>
        /// 是否自动创建供应商用户
        /// </summary>
        public bool IsAutoCreate
        {
            get { return this.GetProperty(IsAutoCreateProperty); }
            set { this.SetProperty(IsAutoCreateProperty, value); }
        }

        /// <summary>
        /// 显示接口配置
        /// </summary>
        /// <returns>返回接口配置</returns>
        public override string Display()
        {
            return IsAutoCreate.ToString().L10N();
        }
        #endregion
    }
}
