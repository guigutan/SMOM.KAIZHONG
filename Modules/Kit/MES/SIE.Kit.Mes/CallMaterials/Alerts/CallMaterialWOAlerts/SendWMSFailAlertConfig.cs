using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工单叫料发送WMS失败预警插件配置类
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单叫料发送WMS失败预警插件配置类")]
    public class SendWMSFailAlertConfig : CallMaterialAlertConfig
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">预警插件对象Json字符串</param>
        public override void Initialize(string value)
        {
            base.Initialize(value);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result = base.ToString();
            return result;
        }
    }
}