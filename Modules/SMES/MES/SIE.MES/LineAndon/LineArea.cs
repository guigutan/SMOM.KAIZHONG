using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.MES.Andon;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LineAndon
{
    /// <summary>
    /// 产线区域维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LineAreaCriteria))]
    [Label("产线区域维护")]
    public class LineArea : DataEntity
    {

        #region 产线/机台编码 MachineCode
        /// <summary>
        /// 产线/机台编码
        /// </summary>
        [Required]
        [Label("产线/机台编码")]
        public static readonly Property<string> MachineCodeProperty = P<LineArea>.Register(e => e.MachineCode);

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
        public static readonly Property<string> MachineNameProperty = P<LineArea>.Register(e => e.MachineName);

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
        public static readonly IRefIdProperty EquipmentIdProperty = P<LineArea>.RegisterRefId(e => e.EquipmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipAccount> EquipmentProperty = P<LineArea>.RegisterRef(e => e.Equipment, EquipmentIdProperty);

        /// <summary>
        /// 主设备号
        /// </summary>
        public EquipAccount Equipment
        {
            get { return GetRefEntity(EquipmentProperty); }
            set { SetRefEntity(EquipmentProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<LineArea>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<LineArea>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenter
        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        [Label("工作中心编码")]
        public static readonly IRefIdProperty WorkCenterIdProperty = P<LineArea>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty = P<LineArea>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

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
            P<LineArea>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<LineArea>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 区域描述 AndonUphold
        /// <summary>
        /// 区域描述Id
        /// </summary>
        [Label("区域描述")]
        public static readonly IRefIdProperty AndonUpholdIdProperty =
            P<LineArea>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

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
            P<LineArea>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

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
        public static readonly Property<string> AndonCodeProperty = P<LineArea>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<LineArea>.RegisterView(e => e.EquipmentName, p => p.Equipment.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return GetProperty(EquipmentNameProperty); }
        }
        #endregion

        #region 购置日期 EquipmentDate
        /// <summary>
        /// 购置日期
        /// </summary>
        [Label("购置日期")]
        public static readonly Property<string> EquipmentDateProperty = P<LineArea>.RegisterView(e => e.EquipmentDate, p => p.Equipment.PurchaseDate);

        /// <summary>
        /// 购置日期
        /// </summary>
        public string EquipmentDate
        {
            get { return GetProperty(EquipmentDateProperty); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<LineArea>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }

        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<LineArea>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }

        #endregion

        #region 工作中心编码 WorkCenterCode
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> WorkCenterCodeProperty = P<LineArea>.RegisterView(e => e.WorkCenterCode, p => p.WorkCenter.Code);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
        }

        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<LineArea>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }

        #endregion

        #region 设备编码 EquipmentNo
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentNoProperty = P<LineArea>.RegisterView(e => e.EquipmentNo, p => p.Equipment.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentNo
        {
            get { return this.GetProperty(EquipmentNoProperty); }
        }

        #endregion

        #region 安灯描述 AndonDesc
        /// <summary>
        /// 安灯描述
        /// </summary>
        [Label("安灯描述")]
        public static readonly Property<string> AndonDescProperty = P<LineArea>.RegisterView(e => e.AndonDesc, p => p.AndonUphold.AndonDesc);

        /// <summary>
        /// 安灯描述
        /// </summary>
        public string AndonDesc
        {
            get { return this.GetProperty(AndonDescProperty); }
        }

        #endregion

        #endregion
    }

    internal class LineAreaConfig : EntityConfig<LineArea>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(LineArea.MachineCodeProperty, new NotDuplicateRule());
            rules.AddRule(LineArea.MachineNameProperty, new RequiredRule());
            rules.AddRule(LineArea.MachineCodeProperty, new RequiredRule());
            rules.AddRule(LineArea.FactoryIdProperty, new RequiredRule());
            rules.AddRule(LineArea.WorkShopIdProperty, new RequiredRule());

            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("LINE_AREA").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
