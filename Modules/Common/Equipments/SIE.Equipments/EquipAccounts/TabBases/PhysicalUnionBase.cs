using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipAccounts.TabBases
{
    /// <summary>
    /// 台账物联参数基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("台账物联参数基类")]
    public class PhysicalUnionBase : DataEntity
    {
        #region 物联参数 PhysicalUnion
        /// <summary>
        /// 物联参数Id
        /// </summary>
        [Label("物联参数")]
        public static readonly IRefIdProperty PhysicalUnionIdProperty =
            P<PhysicalUnionBase>.RegisterRefId(e => e.PhysicalUnionId, ReferenceType.Normal);

        /// <summary>
        /// 物联参数Id
        /// </summary>
        public double PhysicalUnionId
        {
            get { return (double)this.GetRefId(PhysicalUnionIdProperty); }
            set { this.SetRefId(PhysicalUnionIdProperty, value); }
        }

        /// <summary>
        /// 物联参数
        /// </summary>
        public static readonly RefEntityProperty<PhysicalUnion> PhysicalUnionProperty =
            P<PhysicalUnionBase>.RegisterRef(e => e.PhysicalUnion, PhysicalUnionIdProperty);

        /// <summary>
        /// 物联参数
        /// </summary>
        public PhysicalUnion PhysicalUnion
        {
            get { return this.GetRefEntity(PhysicalUnionProperty); }
            set { this.SetRefEntity(PhysicalUnionProperty, value); }
        }
        #endregion

        #region 设备参数 EquipPara
        /// <summary>
        /// 设备参数
        /// </summary>
        [Label("设备参数")]
        public static readonly Property<EquipPara> EquipParaProperty = P<PhysicalUnionBase>.Register(e => e.EquipPara);

        /// <summary>
        /// 设备参数
        /// </summary>
        public EquipPara EquipPara
        {
            get { return this.GetProperty(EquipParaProperty); }
            set { this.SetProperty(EquipParaProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 参数编码 PararCode
        /// <summary>
        /// 参数编码
        /// </summary>
        [Label("参数编码")]
        public static readonly Property<string> PararCodeProperty = P<PhysicalUnionBase>.RegisterView(e => e.PararCode, p => p.PhysicalUnion.PararCode);

        /// <summary>
        /// 参数编码
        /// </summary>
        public string PararCode
        {
            get { return this.GetProperty(PararCodeProperty); }
        }
        #endregion

        #region 参数名称 ParaName
        /// <summary>
        /// 参数编码
        /// </summary>
        [Label("参数名称")]
        public static readonly Property<string> ParaNameProperty = P<PhysicalUnionBase>.RegisterView(e => e.ParaName, p => p.PhysicalUnion.ParaName);

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParaName
        {
            get { return this.GetProperty(ParaNameProperty); }
        }
        #endregion

        #region MDC编码 MDCVariableCode
        /// <summary>
        /// MDC编码
        /// </summary>
        [Label("MDC编码")]
        public static readonly Property<string> MDCVariableCodeProperty = P<PhysicalUnionBase>.RegisterView(e => e.MDCVariableCode, p => p.PhysicalUnion.MDCVariableCode);

        /// <summary>
        /// 参数编码
        /// </summary>
        public string MDCVariableCode
        {
            get { return this.GetProperty(MDCVariableCodeProperty); }
        }
        #endregion

        #region MDC变量名 MDCVariableName
        /// <summary>
        /// MDC变量名
        /// </summary>
        [Label("MDC变量名")]
        public static readonly Property<string> MDCVariableNameProperty = P<PhysicalUnionBase>.RegisterView(e => e.MDCVariableName, p => p.PhysicalUnion.MDCVariableName);

        /// <summary>
        /// MDC变量名
        /// </summary>
        public string MDCVariableName
        {
            get { return this.GetProperty(MDCVariableNameProperty); }
        }
        #endregion

        #region 最大值 MaxValue
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<double> MaxValueProperty = P<PhysicalUnionBase>.RegisterView(e => e.MaxValue, p => p.PhysicalUnion.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get { return this.GetProperty(MaxValueProperty); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<double> MinValueProperty = P<PhysicalUnionBase>.RegisterView(e => e.MinValue, p => p.PhysicalUnion.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get { return this.GetProperty(MinValueProperty); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<PhysicalUnionBase>.RegisterView(e => e.Unit, p => p.PhysicalUnion.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class PhysicalUnionBaseConfig : EntityConfig<PhysicalUnionBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_PHYSICAL_UNION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
