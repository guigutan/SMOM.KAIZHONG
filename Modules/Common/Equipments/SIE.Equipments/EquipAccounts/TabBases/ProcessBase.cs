using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Equipments.EquipAccounts.TabBases
{
    /// <summary>
	/// 台账工序基类
	/// </summary>
	[RootEntity, Serializable]
    [Label("台账工序基类")]
    public class ProcessBase : DataEntity
    {
        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProcessBase>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ProcessBase>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty
            = P<ProcessBase>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProcessBaseConfig : EntityConfig<ProcessBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_PROCESS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
