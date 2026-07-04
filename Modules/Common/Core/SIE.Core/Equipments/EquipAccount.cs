using SIE.Core.Enums;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Equipments
{
    /// <summary>
    /// 设备台账
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Code))]
    [Label("设备台账")]
    public partial class EquipAccount : DataEntity
    {
        /// <summary>
        /// ABC分类
        /// </summary>
        public const string EquipAccountUseLevel = "USE_LEVEL";

        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("设备编码")]
        public static readonly Property<string> CodeProperty = P<EquipAccount>.Register(e => e.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>        
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<EquipAccount>.Register(e => e.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 仪器状态 QualityState
        /// <summary>
        /// 仪器状态
        /// </summary>
        [Label("仪器状态")]
        public static readonly Property<QualityState?> QualityStateProperty = P<EquipAccount>.Register(e => e.QualityState);

        /// <summary>
        /// 仪器状态
        /// </summary>
        public QualityState? QualityState
        {
            get { return this.GetProperty(QualityStateProperty); }
            set { this.SetProperty(QualityStateProperty, value); }
        }
        #endregion

        #region 设备状态 State
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState> StateProperty = P<EquipAccount>.Register(e => e.State);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 管理状态 UseState 
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState> UseStateProperty = P<EquipAccount>.Register(e => e.UseState);

        /// <summary>
        /// 管理状态（原使用状态）
        /// </summary>
        public AccountUseState UseState
        {
            get { return GetProperty(UseStateProperty); }
            set { SetProperty(UseStateProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipAccount>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipAccount>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region ABC分类 UseLevel
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<string> UseLevelProperty = P<EquipAccount>.Register(e => e.UseLevel);

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel
        {
            get { return this.GetProperty(UseLevelProperty); }
            set { this.SetProperty(UseLevelProperty, value); }
        }
        #endregion

        #region 视图属性 
        #region 设备型号编码 ModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<EquipAccount>.RegisterView(e => e.ModelCode, p => p.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion 

        #region 设备型号名称 ModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> ModelNameProperty = P<EquipAccount>.RegisterView(e => e.ModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 设备类型id EquipTypeViewId
        /// <summary>
        /// 设备类型id
        /// </summary>
        [Label("设备类型id")]
        public static readonly Property<double> EquipTypeViewIdProperty = P<EquipAccount>.RegisterView(e => e.EquipTypeViewId, p => p.EquipModel.EquipTypeId);

        /// <summary>
        /// 设备类型id
        /// </summary>
        public double EquipTypeViewId
        {
            get { return this.GetProperty(EquipTypeViewIdProperty); }
        }
        #endregion

        #region 设备类型编码 EquipTypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> EquipTypeCodeProperty = P<EquipAccount>.RegisterView(e => e.EquipTypeCode, p => p.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion 

        #region 设备类型名称 EquipTypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<EquipAccount>.RegisterView(e => e.EquipTypeName, p => p.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion 

        #region 设备类别 EquipTypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> EquipTypeCategoryProperty = P<EquipAccount>.RegisterView(e => e.EquipTypeCategory, p => p.EquipModel.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipTypeCategory
        {
            get { return this.GetProperty(EquipTypeCategoryProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备台账 实体配置
    /// </summary>
    internal class EquipAccountConfig : EntityConfig<EquipAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.Property(EquipAccount.CodeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}