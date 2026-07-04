using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工艺参数
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺参数")]
    public class ProcessParamCollectParam : DataEntity
    {
        #region 工序参数数采 ProcessParamCollect
        /// <summary>
        /// 工序参数数采Id
        /// </summary>
        [Label("工序参数数采")]
        public static readonly IRefIdProperty ProcessParamCollectIdProperty =
            P<ProcessParamCollectParam>.RegisterRefId(e => e.ProcessParamCollectId, ReferenceType.Parent);

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
            P<ProcessParamCollectParam>.RegisterRef(e => e.ProcessParamCollect, ProcessParamCollectIdProperty);

        /// <summary>
        /// 工序参数数采
        /// </summary>
        public ProcessParamCollect ProcessParamCollect
        {
            get { return this.GetRefEntity(ProcessParamCollectProperty); }
            set { this.SetRefEntity(ProcessParamCollectProperty, value); }
        }
        #endregion

        #region 参数名 ParamName
        /// <summary>
        /// 参数名
        /// </summary>
        [Label("参数名")]
        public static readonly Property<string> ParamNameProperty = P<ProcessParamCollectParam>.Register(e => e.ParamName);

        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName
        {
            get { return this.GetProperty(ParamNameProperty); }
            set { this.SetProperty(ParamNameProperty, value); }
        }
        #endregion

        #region 参数值 ParamValue
        /// <summary>
        /// 参数值
        /// </summary>
        [Label("参数值")]
        public static readonly Property<string> ParamValueProperty = P<ProcessParamCollectParam>.Register(e => e.ParamValue);

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue
        {
            get { return this.GetProperty(ParamValueProperty); }
            set { this.SetProperty(ParamValueProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<ProcessParamCollectParam>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工艺参数 实体配置
    /// </summary>
    internal class ProcessParamCollectParamConfig : EntityConfig<ProcessParamCollectParam>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRO_PAR_COL_PV").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}
