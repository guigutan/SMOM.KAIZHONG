using SIE.Core.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工序参数数采
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProcessParamCollectCriteria))]
    [Label("工序参数数采")]
    public class ProcessParamCollect : DataEntity
    {
        #region SN
        /// <summary>
        /// SN
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> SNProperty = P<ProcessParamCollect>.Register(e => e.SN);

        /// <summary>
        /// SN
        /// </summary>
        public string SN
        {
            get { return this.GetProperty(SNProperty); }
            set { this.SetProperty(SNProperty, value); }
        }
        #endregion

        #region 时间 Time
        /// <summary>
        /// 时间
        /// </summary>
        [Label("时间")]
        public static readonly Property<string> TimeProperty = P<ProcessParamCollect>.Register(e => e.Time);

        /// <summary>
        /// 时间
        /// </summary>
        public string Time
        {
            get { return this.GetProperty(TimeProperty); }
            set { this.SetProperty(TimeProperty, value); }
        }
        #endregion

        #region 工序流水码 ProcessFlowCode
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> ProcessFlowCodeProperty = P<ProcessParamCollect>.Register(e => e.ProcessFlowCode);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string ProcessFlowCode
        {
            get { return this.GetProperty(ProcessFlowCodeProperty); }
            set { this.SetProperty(ProcessFlowCodeProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProcessParamCollect>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessParamCollect>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<ProcessParamCollect>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<ProcessParamCollect>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return this.GetProperty(EquipmentCodeProperty); }
            set { this.SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<ProcessParamCollect>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return this.GetProperty(EquipmentNameProperty); }
            set { this.SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<InspResult> QualityStatusProperty = P<ProcessParamCollect>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public InspResult QualityStatus
        {
            get { return this.GetProperty(QualityStatusProperty); }
            set { this.SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 工艺参数 ProcessParamCollectParamList
        /// <summary>
        /// 工艺参数
        /// </summary>
        [Label("工艺参数")]
        public static readonly ListProperty<EntityList<ProcessParamCollectParam>> ProcessParamCollectParamListProperty =
            P<ProcessParamCollect>.RegisterList(e => e.ProcessParamCollectParamList);

        /// <summary>
        /// 工艺参数
        /// </summary>
        public EntityList<ProcessParamCollectParam> ProcessParamCollectParamList
        {
            get { return this.GetLazyList(ProcessParamCollectParamListProperty); }
        }
        #endregion

        #region 子件 ProcessParamCollectComponentList
        /// <summary>
        /// 子件
        /// </summary>
        [Label("子件")]
        public static readonly ListProperty<EntityList<ProcessParamCollectComponent>> ProcessParamCollectComponentListProperty =
            P<ProcessParamCollect>.RegisterList(e => e.ProcessParamCollectComponentList);

        /// <summary>
        /// 子件
        /// </summary>
        public EntityList<ProcessParamCollectComponent> ProcessParamCollectComponentList
        {
            get { return this.GetLazyList(ProcessParamCollectComponentListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 工序参数数采 实体配置
    /// </summary>
    public class ProcessParamCollectConfig : EntityConfig<ProcessParamCollect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRO_PAR_COL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}
