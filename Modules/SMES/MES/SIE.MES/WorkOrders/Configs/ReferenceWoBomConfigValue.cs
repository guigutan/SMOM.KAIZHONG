using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工序 BOM 参考工单 BOM 配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序 BOM 参考工单 BOM 配置值")]
    public class ReferenceWoBomConfigValue : ConfigValue
    {
        #region 工序 BOM 参考工单 BOM ReferenceWoBom
        /// <summary>
        /// 工序 BOM 参考工单 BOM
        /// </summary>
        [Label("参考工单 BOM")]
        public static readonly Property<bool> ReferenceWoBomProperty 
            = P<ReferenceWoBomConfigValue>.Register(e => e.ReferenceWoBom);

        /// <summary>
        /// 工序 BOM 参考工单 BOM
        /// </summary>
        public bool ReferenceWoBom
        {
            get { return this.GetProperty(ReferenceWoBomProperty); }
            set { this.SetProperty(ReferenceWoBomProperty, value); }
        }
        #endregion

    }
}