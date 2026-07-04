using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 考勤机管理
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("考勤机管理")]
    public partial class ClockInMachine : DataEntity
    {
        #region 考勤机名称 Name
        /// <summary>
        /// 考勤机名称
        /// </summary>
        [Label("考勤机名称")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NameProperty = P<ClockInMachine>.Register(e => e.Name);

        /// <summary>
        /// 考勤机名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region IP地址 IPAddress
        /// <summary>
        /// IP地址
        /// </summary>
        [Label("IP地址")]
        [Required]
        public static readonly Property<string> IpAddressProperty = P<ClockInMachine>.Register(e => e.IpAddress);

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress
        {
            get { return GetProperty(IpAddressProperty); }
            set { SetProperty(IpAddressProperty, value); }
        }
        #endregion

        #region 端口 Port
        /// <summary>
        /// 端口
        /// </summary>
        [Label("端口")]
        [Required]
        [MinValue(1)]
        public static readonly Property<int> PortProperty = P<ClockInMachine>.Register(e => e.Port);

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return GetProperty(PortProperty); }
            set { SetProperty(PortProperty, value); }
        }
        #endregion

        #region 型号 Model
        /// <summary>
        /// 型号
        /// </summary>
        [Label("型号")]
        public static readonly Property<string> ModelProperty = P<ClockInMachine>.Register(e => e.Model);

        /// <summary>
        /// 型号
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 序列号 SN
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SNProperty = P<ClockInMachine>.Register(e => e.SN);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SN
        {
            get { return GetProperty(SNProperty); }
            set { SetProperty(SNProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 考勤机管理 实体配置
    /// </summary>
    internal class ClockInMachineConfig : EntityConfig<ClockInMachine>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMP_CLOCK_IN_MACHINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 机器唯一验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("机器唯一验证规则")]
    [System.ComponentModel.Description("IP+端口唯一")]
    public class ClockInMachineIPAndPortNotDuplicateRule : NotDuplicateRule<ClockInMachine>
    {
        /// <summary>
        /// 构造函数，添加验证属性
        /// </summary>
        public ClockInMachineIPAndPortNotDuplicateRule()
        {
            Properties.Add(ClockInMachine.IpAddressProperty);
            Properties.Add(ClockInMachine.PortProperty);
            MessageBuilder = (e) =>
            {
                return "该IP与端口组合已经被使用".L10N();
            };
        }
    }

    /// <summary>
    /// IP地址格式验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("IP地址格式验证规则")]
    [System.ComponentModel.Description("IP地址格式验证")]
    public class ClockInMachineIPRegularRule : RegularRule<ClockInMachine>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClockInMachineIPRegularRule()
        {
            Pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            MessageBuilder = (e) =>
            {
                return "IP地址格式有误，请输入正确的IP地址".L10N();
            };
        }
        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return ClockInMachine.IpAddressProperty;
            }
        }

    }
}