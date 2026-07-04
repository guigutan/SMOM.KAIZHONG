using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Logs
{
    /// <summary>
    /// 接口日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("接口日志")]
    public partial class InterfaceLog : DataEntity
    {
        #region 接口名称 Name
        /// <summary>
        /// 接口名称
        /// </summary>
        [Label("接口名称")]
        [MaxLength(200)]
        public static readonly Property<string> NameProperty = P<InterfaceLog>.Register(e => e.Name);

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 接口方法 Method
        /// <summary>
        /// 接口方法
        /// </summary>
        [Label("接口方法")]
        [MaxLength(200)]
        public static readonly Property<string> MethodProperty = P<InterfaceLog>.Register(e => e.Method);

        /// <summary>
        /// 接口方法
        /// </summary>
        public string Method
        {
            get { return GetProperty(MethodProperty); }
            set { SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 控制器名称 ControllerName
        /// <summary>
        /// 控制器名称
        /// </summary>
        [Label("控制器名称")]
        [MaxLength(200)]
        public static readonly Property<string> ControllerNameProperty = P<InterfaceLog>.Register(e => e.ControllerName);

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName
        {
            get { return GetProperty(ControllerNameProperty); }
            set { SetProperty(ControllerNameProperty, value); }
        }
        #endregion

        #region 输入值 InputValue
        /// <summary>
        /// 输入值
        /// </summary>
        [Label("输入值")]
        public static readonly Property<string> InputValueProperty = P<InterfaceLog>.Register(e => e.InputValue);

        /// <summary>
        /// 输入值
        /// </summary>
        public string InputValue
        {
            get { return this.GetProperty(InputValueProperty); }
            set { this.SetProperty(InputValueProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 接口日志 实体配置
    /// </summary>
    internal class InterfaceLogEntityConfig : EntityConfig<InterfaceLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INTERFACE_LOG").MapAllProperties();
            Meta.Property(InterfaceLog.InputValueProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InterfaceLog.NameProperty).ColumnMeta.HasLength(200);
            Meta.Property(InterfaceLog.MethodProperty).ColumnMeta.HasLength(200);
            Meta.Property(InterfaceLog.ControllerNameProperty).ColumnMeta.HasLength(200);
            Meta.DisableDataSync();
            Meta.DisablePhantoms();
        }
    }
}
