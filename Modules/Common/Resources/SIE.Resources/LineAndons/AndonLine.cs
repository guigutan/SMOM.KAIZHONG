using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.LineAndons
{
    /// <summary>
    /// 产线与安灯区域
    /// </summary>
    [RootEntity, Serializable]
    public class AndonLine : DataEntity
    {
        #region 产线/机台编码 MachineCode
        /// <summary>
        /// 产线/机台编码
        /// </summary>
        [Required]
        [Label("产线/机台编码")]
        public static readonly Property<string> MachineCodeProperty = P<AndonLine>.Register(e => e.MachineCode);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string MachineCode
        {
            get { return this.GetProperty(MachineCodeProperty); }
            set { this.SetProperty(MachineCodeProperty, value); }
        }
        #endregion

        #region 产线/机台名称 MachineName
        /// <summary>
        /// 产线/机台名称
        /// </summary>
        [Required]
        [Label("产线/机台名称")]
        public static readonly Property<string> MachineNameProperty = P<AndonLine>.Register(e => e.MachineName);

        /// <summary>
        /// 产线/机台名称
        /// </summary>
        public string MachineName
        {
            get { return this.GetProperty(MachineNameProperty); }
            set { this.SetProperty(MachineNameProperty, value); }
        }
        #endregion

        #region 主设备号 Equipment
        /// <summary>
        /// 主设备号Id
        /// </summary>
        [Label("主设备号")]
        public static readonly IRefIdProperty EquipmentIdProperty = P<AndonLine>.RegisterRefId(e => e.EquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 主设备号Id
        /// </summary>
        public double? EquipmentId
        {
            get { return (double?)GetRefNullableId(EquipmentIdProperty); }
            set { SetRefNullableId(EquipmentIdProperty, value); }
        }

        /// <summary>
        /// 主设备号
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipmentProperty = P<AndonLine>.RegisterRef(e => e.Equipment, EquipmentIdProperty);

        /// <summary>
        /// 主设备号
        /// </summary>
        public EquipAccount Equipment
        {
            get { return GetRefEntity(EquipmentProperty); }
            set { SetRefEntity(EquipmentProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenter
        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        [Label("工作中心编码")]
        public static readonly IRefIdProperty WorkCenterIdProperty = P<AndonLine>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        public double WorkCenterId
        {
            get { return (double)GetRefNullableId(WorkCenterIdProperty); }
            set { SetRefNullableId(WorkCenterIdProperty, value); }
        }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty = P<AndonLine>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public WorkCenter WorkCenter
        {
            get { return GetRefEntity(WorkCenterProperty); }
            set { SetRefEntity(WorkCenterProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<AndonLine>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AndonLine>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AndonLine>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AndonLine>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 区域描述 AndonUphold
        /// <summary>
        /// 区域描述Id
        /// </summary>
        [Label("区域描述")]
        public static readonly IRefIdProperty AndonUpholdIdProperty =
            P<AndonLine>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 区域描述Id
        /// </summary>
        public double? AndonUpholdId
        {
            get { return (double?)this.GetRefNullableId(AndonUpholdIdProperty); }
            set { this.SetRefNullableId(AndonUpholdIdProperty, value); }
        }

        /// <summary>
        /// 区域描述
        /// </summary>
        public static readonly RefEntityProperty<AndonUphold> AndonUpholdProperty =
            P<AndonLine>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

        /// <summary>
        /// 区域描述
        /// </summary>
        public AndonUphold AndonUphold
        {
            get { return this.GetRefEntity(AndonUpholdProperty); }
            set { this.SetRefEntity(AndonUpholdProperty, value); }
        }
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<AndonLine>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 打印机IP PrinterIp
        /// <summary>
        /// 打印机IP
        /// </summary>
        [Label("打印机IP")]
        public static readonly Property<string> PrinterIpProperty = P<AndonLine>.Register(e => e.PrinterIp);

        /// <summary>
        /// 打印机IP
        /// </summary>
        public string PrinterIp
        {
            get { return this.GetProperty(PrinterIpProperty); }
            set { this.SetProperty(PrinterIpProperty, value); }
        }
        #endregion

        #region IOT指令 AndonOrder
        /// <summary>
        /// IOT指令
        /// </summary>
        [Label("IOT指令")]
        public static readonly Property<string> AndonOrderProperty = P<AndonLine>.Register(e => e.AndonOrder);

        /// <summary>
        /// IOT指令
        /// </summary>
        public string AndonOrder
        {
            get { return this.GetProperty(AndonOrderProperty); }
            set { this.SetProperty(AndonOrderProperty, value); }
        }
        #endregion

        #region IOT实体 AndonEntity
        /// <summary>
        /// IOT实体
        /// </summary>
        [Label("IOT实体")]
        public static readonly Property<string> AndonEntityProperty = P<AndonLine>.Register(e => e.AndonEntity);

        /// <summary>
        /// IOT实体
        /// </summary>
        public string AndonEntity
        {
            get { return this.GetProperty(AndonEntityProperty); }
            set { this.SetProperty(AndonEntityProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<AndonLine>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }
        #endregion

        #region 主设备编码 EquipmentCode
        /// <summary>
        /// 主设备编码
        /// </summary>
        [Label("主设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<AndonLine>.RegisterView(e => e.EquipmentCode, p => p.Equipment.Code);

        /// <summary>
        /// 主设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return this.GetProperty(EquipmentCodeProperty); }
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 产线与安灯区域 实体配置
    /// </summary>
    internal class AndonLineConfig : EntityConfig<AndonLine>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_LINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
