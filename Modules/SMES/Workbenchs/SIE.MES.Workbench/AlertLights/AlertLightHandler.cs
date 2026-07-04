using SIE.Domain;
using SIE.MES.Workbench.AlertLights;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.WorkBench.AlertLights
{
    /// <summary>
    /// 安灯处理人员
    /// </summary>
    [ChildEntity, Serializable]
    ////[CriteriaQuery]
    [Label("安灯处理人员")]
    public partial class AlertLightHandler : DataEntity
    {
        #region 接收人员名称 HandlerName
        /// <summary>
        /// 处理人员名称
        /// </summary>
        [Required]
        [Label("接收人员名称")]
        public static readonly Property<string> HandlerNameProperty = P<AlertLightHandler>.Register(e => e.HandlerName);

        /// <summary>
        /// 接收人员名称
        /// </summary>
        public string HandlerName
        {
            get { return GetProperty(HandlerNameProperty); }
            set { SetProperty(HandlerNameProperty, value); }
        }
        #endregion

        #region 安灯接收人员 Handler
        /// <summary>
        /// 安灯处理人员Id
        /// </summary>
        [Label("安灯接收人员")]
        public static readonly IRefIdProperty HandlerIdProperty = P<AlertLightHandler>.RegisterRefId(e => e.HandlerId, ReferenceType.Normal);

        /// <summary>
        /// 安灯接收人员Id
        /// </summary>
        public double HandlerId
        {
            get { return (double)GetRefId(HandlerIdProperty); }
            set { SetRefId(HandlerIdProperty, value); }
        }

        /// <summary>
        /// 安灯接收人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandlerProperty = P<AlertLightHandler>.RegisterRef(e => e.Handler, HandlerIdProperty);

        /// <summary>
        /// 安灯接收人员
        /// </summary>
        public Employee Handler
        {
            get { return GetRefEntity(HandlerProperty); }
            set { SetRefEntity(HandlerProperty, value); }
        }
        #endregion

        #region 安灯 AlertLight
        /// <summary>
        /// 安灯Id
        /// </summary>
        [Label("安灯")]
        public static readonly IRefIdProperty AlertLightIdProperty = P<AlertLightHandler>.RegisterRefId(e => e.AlertLightId, ReferenceType.Parent);

        /// <summary>
        /// 安灯Id
        /// </summary>
        public double AlertLightId
        {
            get { return (double)GetRefId(AlertLightIdProperty); }
            set { SetRefId(AlertLightIdProperty, value); }
        }

        /// <summary>
        /// 安灯
        /// </summary>
        public static readonly RefEntityProperty<AlertLight> AlertLightProperty = P<AlertLightHandler>.RegisterRef(e => e.AlertLight, AlertLightIdProperty);

        /// <summary>
        /// 安灯
        /// </summary>
        public AlertLight AlertLight
        {
            get { return GetRefEntity(AlertLightProperty); }
            set { SetRefEntity(AlertLightProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 安灯处理人员 实体配置
    /// </summary>
    internal class AlertLightHandlersConfig : EntityConfig<AlertLightHandler>
    {
        /// <summary>
        /// 安灯处理人员映射DB配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("Alert_Light_Handler").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}