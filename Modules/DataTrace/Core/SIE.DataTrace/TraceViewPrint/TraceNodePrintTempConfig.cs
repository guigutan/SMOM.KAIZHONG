using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据
    /// </summary>
    [CriteriaQuery]
    [Label("追溯主数据模板设置")]
    [DisplayMember(nameof(NodeName))]
    [RootEntity, Serializable]
    public class TraceNodePrintTempCfg: DataEntity
    {
        #region 追溯节点 NodeName
        /// <summary>
        /// 追溯节点
        /// </summary>
        [Label("追溯节点")]
        [Required]
        public static readonly Property<string> NodeNameProperty = P<TraceNodePrintTempCfg>.Register(e => e.NodeName);

        /// <summary>
        /// 追溯节点
        /// </summary>
        public string NodeName
        {
            get { return this.GetProperty(NodeNameProperty); }
            set { this.SetProperty(NodeNameProperty, value); }
        }
        #endregion

        #region 实体类型 EntityType
        /// <summary>
        /// 实体类型
        /// </summary>
        [Label("实体类型")]
        public static readonly Property<string> EntityTypeProperty = P<TraceNodePrintTempCfg>.Register(e => e.EntityType);

        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType
        {
            get { return this.GetProperty(EntityTypeProperty); }
            set { this.SetProperty(EntityTypeProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary> 
        public static readonly IRefIdProperty TemplateIdProperty =
            P<TraceNodePrintTempCfg>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);
        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double)this.GetRefId(TemplateIdProperty); }
            set { this.SetRefId(TemplateIdProperty, value); }
        }
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty =
            P<TraceNodePrintTempCfg>.RegisterRef(e => e.Template, TemplateIdProperty);
        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 追溯主数据 实体配置
    /// </summary>
    public class TraceNodeNamedePrintTempCfgConfig : EntityConfig<TraceNodePrintTempCfg>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TRACE_NODE_PRINTTEMP_CFG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
