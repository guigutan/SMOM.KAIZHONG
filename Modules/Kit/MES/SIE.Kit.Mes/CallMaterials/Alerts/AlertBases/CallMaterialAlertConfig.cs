using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 物料呼叫预警插件配置
    /// </summary>
    [Serializable]
    [Label("物料呼叫预警插件配置")]
    public class CallMaterialAlertConfig : AlertConfig
    {
        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty LineIdProperty = P<CallMaterialAlertConfig>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double LineId
        {
            get { return (double)this.GetRefId(LineIdProperty); }
            set { this.SetRefId(LineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> LineProperty = P<CallMaterialAlertConfig>.RegisterRef(e => e.Line, LineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Line
        {
            get { return this.GetRefEntity(LineProperty); }
            set { this.SetRefEntity(LineProperty, value); }
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">预警插件对象Json字符串</param>
        public override void Initialize(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return;
            var config = JsonConvert.DeserializeObject<CallMaterialAlertConfig>(value);
            this.LineId = config.LineId;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            var obj = new
            {
                LineId = this.LineId
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}