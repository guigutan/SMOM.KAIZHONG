using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 子件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("子件")]
    public class ProcessParamCollectComponent : DataEntity
    {
        #region 工序参数数采 ProcessParamCollect
        /// <summary>
        /// 工序参数数采Id
        /// </summary>
        [Label("工序参数数采")]
        public static readonly IRefIdProperty ProcessParamCollectIdProperty =
            P<ProcessParamCollectComponent>.RegisterRefId(e => e.ProcessParamCollectId, ReferenceType.Parent);

        /// <summary>
        /// 工序参数数采Id
        /// </summary>
        public double ProcessParamCollectId
        {
            get { return (double)this.GetRefId(ProcessParamCollectIdProperty); }
            set { this.SetRefId(ProcessParamCollectIdProperty, value); }
        }

        /// <summary>
        /// 工序参数数采
        /// </summary>
        public static readonly RefEntityProperty<ProcessParamCollect> ProcessParamCollectProperty =
            P<ProcessParamCollectComponent>.RegisterRef(e => e.ProcessParamCollect, ProcessParamCollectIdProperty);

        /// <summary>
        /// 工序参数数采
        /// </summary>
        public ProcessParamCollect ProcessParamCollect
        {
            get { return this.GetRefEntity(ProcessParamCollectProperty); }
            set { this.SetRefEntity(ProcessParamCollectProperty, value); }
        }
        #endregion

        #region 子件SN ComponentSN
        /// <summary>
        /// 子件SN
        /// </summary>
        [Label("子件SN")]
        public static readonly Property<string> ComponentSNProperty = P<ProcessParamCollectComponent>.Register(e => e.ComponentSN);

        /// <summary>
        /// 子件SN
        /// </summary>
        public string ComponentSN
        {
            get { return this.GetProperty(ComponentSNProperty); }
            set { this.SetProperty(ComponentSNProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 子件 实体配置
    /// </summary>
    internal class ProcessParamCollectComponentConfig : EntityConfig<ProcessParamCollectComponent>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRO_PAR_COL_SN").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}
