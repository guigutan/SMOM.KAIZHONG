using NPOI.SS.UserModel;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WorkOrders.Configs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemEquipAccount.Configs
{
    /// <summary>
    /// 模具与产品关系配置项s
    /// </summary>
    [System.ComponentModel.DisplayName("模具与产品关系配置项")]
    [System.ComponentModel.Description("模具与产品关系配置项")]
    public class EquipAccountItemConfig: ModuleConfig<EquipAccountItemConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipAccountItemConfigValue DefaultValue { get; } = new EquipAccountItemConfigValue() { IsValidAcupoint = false };

    }

    /// <summary>
    /// 打印设置配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("模具与产品关系配置项值")]
    public class EquipAccountItemConfigValue : ConfigValue
    {
        #region 是否开机准备校验穴位 IsValidAcupoint
        /// <summary>
        /// 是否开机准备校验穴位
        /// </summary>
        [Label("是否开机准备校验穴位")]
        public static readonly Property<bool?> IsValidAcupointProperty = P<EquipAccountItemConfigValue>.Register(e => e.IsValidAcupoint);

        /// <summary>
        /// 是否开机准备校验穴位
        /// </summary>
        public bool? IsValidAcupoint
        {
            get { return this.GetProperty(IsValidAcupointProperty); }
            set { this.SetProperty(IsValidAcupointProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>打印设置</returns>
        public override string Display()
        {
            return "是否开机准备校验穴位:{0}".L10nFormat(IsValidAcupoint == true ? "是" : "否");
        }
    }
}
