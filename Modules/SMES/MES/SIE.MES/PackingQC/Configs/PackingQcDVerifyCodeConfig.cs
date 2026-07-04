using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC.Configs
{
    /// <summary>
    /// 包装二维码长度设置配置
    /// </summary>
    [System.ComponentModel.DisplayName("包装二维码长度设置配置")]
    [System.ComponentModel.Description("包装二维码长度设置配置")]
    public class PackingQcDVerifyCodeConfig : ModuleConfig<PackingQcDVerifyCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override PackingQcDVerifyCodeConfigValue DefaultValue { get; } = new PackingQcDVerifyCodeConfigValue { };
    }

    /// <summary>
    /// D包装采集SN长度设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("安灯事件看板自动刷新时间配置的值")]
    public class PackingQcDVerifyCodeConfigValue : ConfigValue
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingQcDVerifyCodeConfigValue()
        {
            this.CodeLength = 22;
        }
        #endregion


        /// <summary>
        /// SN长度
        /// </summary>
        [MinValue(0)]
        [Label("SN长度")]
        public static readonly Property<int> CodeLengthProperty = P<PackingQcDVerifyCodeConfigValue>.Register(e => e.CodeLength);

        /// <summary>
        ///  SN长度
        /// </summary>
        public int CodeLength
        {
            get { return this.GetProperty(CodeLengthProperty); }
            set { this.SetProperty(CodeLengthProperty, value); }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>SN长度</returns>
        public override string Display()
        {
            return CodeLength.ToString();
        }
    }
}
