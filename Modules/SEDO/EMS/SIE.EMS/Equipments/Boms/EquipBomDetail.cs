using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM明细
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(SparePartCode))]
    [Label("设备BOM明细")]
    public partial class EquipBomDetail : DataEntity
    {
        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<EquipBomDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<EquipBomDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 部位 SparePartSite
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> SparePartSiteProperty = P<EquipBomDetail>.Register(e => e.SparePartSite);

        /// <summary>
        /// 部位
        /// </summary>
        public string SparePartSite
        {
            get { return this.GetProperty(SparePartSiteProperty); }
            set { this.SetProperty(SparePartSiteProperty, value); }
        }
        #endregion

        #region 数量 SparePartQty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> SparePartQtyProperty = P<EquipBomDetail>.Register(e => e.SparePartQty);

        /// <summary>
        /// 数量
        /// </summary>
        public int SparePartQty
        {
            get { return GetProperty(SparePartQtyProperty); }
            set { SetProperty(SparePartQtyProperty, value); }
        }
        #endregion

        #region 库存数量 StockQty
        /// <summary>
        /// 库存数量
        /// </summary>
        [Label("库存数量")]
        public static readonly Property<int?> StockQtyProperty = P<EquipBomDetail>.Register(e => e.StockQty);

        /// <summary>
        /// 库存数量
        /// </summary>
        public int? StockQty
        {
            get { return this.GetProperty(StockQtyProperty); }
            set { this.SetProperty(StockQtyProperty, value); }
        }
        #endregion

        #region 设备BOM EquipBom
        /// <summary>
        /// 设备BOMId
        /// </summary>
        public static readonly IRefIdProperty EquipBomIdProperty = P<EquipBomDetail>.RegisterRefId(e => e.EquipBomId, ReferenceType.Parent);

        /// <summary>
        /// 设备BOMId
        /// </summary>
        public double EquipBomId
        {
            get { return (double)GetRefId(EquipBomIdProperty); }
            set { SetRefId(EquipBomIdProperty, value); }
        }

        /// <summary>
        /// 设备BOM
        /// </summary>
        public static readonly RefEntityProperty<EquipBom> EquipBomProperty = P<EquipBomDetail>.RegisterRef(e => e.EquipBom, EquipBomIdProperty);

        /// <summary>
        /// 设备BOM
        /// </summary>
        public EquipBom EquipBom
        {
            get { return GetRefEntity(EquipBomProperty); }
            set { SetRefEntity(EquipBomProperty, value); }
        }
        #endregion

        #region 设备BOM明细（选择视图） EquipBomDetailSelect
        /// <summary>
        /// 设备BOM明细Id
        /// </summary>
        public static readonly IRefIdProperty EquipBomDetailSelectIdProperty =
            P<EquipBomDetail>.RegisterRefId(e => e.EquipBomDetailSelectId, ReferenceType.Normal);

        /// <summary>
        /// 设备BOM明细Id
        /// </summary>
        public double? EquipBomDetailSelectId
        {
            get { return (double?)this.GetRefId(EquipBomDetailSelectIdProperty); }
            set { this.SetRefId(EquipBomDetailSelectIdProperty, value); }
        }

        /// <summary>
        /// 设备BOM明细
        /// </summary>
        public static readonly RefEntityProperty<EquipBomDetail> EquipBomDetailSelectProperty =
            P<EquipBomDetail>.RegisterRef(e => e.EquipBomDetailSelect, EquipBomDetailSelectIdProperty);

        /// <summary>
        /// 设备BOM明细
        /// </summary>
        public EquipBomDetail EquipBomDetailSelect
        {
            get { return this.GetRefEntity(EquipBomDetailSelectProperty); }
            set { this.SetRefEntity(EquipBomDetailSelectProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<EquipBomDetail>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<EquipBomDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<EquipBomDetail>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 备件类型 SparePartType
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("备件类型")]
        public static readonly Property<string> SparePartTypeProperty = P<EquipBomDetail>.RegisterView(e => e.SparePartType, p => p.SparePart.ItemCategory.Code);

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartType
        {
            get { return this.GetProperty(SparePartTypeProperty); }
        }
        #endregion

        #region 更换周期(天) LifeTime
        /// <summary>
        /// 更换周期(天)
        /// </summary>
        [Label("更换周期(天)")]
        public static readonly Property<int?> LifeTimeProperty = P<EquipBomDetail>.RegisterView(e => e.LifeTime, p => p.SparePart.LifeTime);

        /// <summary>
        /// 更换周期(天)
        /// </summary>
        public int? LifeTime
        {
            get { return this.GetProperty(LifeTimeProperty); }
        }
        #endregion

        #region 可用时间(小时) UseTime
        /// <summary>
        /// 可用时间(小时)
        /// </summary>
        [Label("可用时间(小时)")]
        public static readonly Property<int?> UseTimeProperty = P<EquipBomDetail>.RegisterView(e => e.UseTime, p => p.SparePart.UseTime);

        /// <summary>
        /// 可用时间(小时)
        /// </summary>
        public int? UseTime
        {
            get { return this.GetProperty(UseTimeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<EquipBomDetail>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 备件类型名称 SparePartTypeName
        /// <summary>
        /// 备件类型名称
        /// </summary>
        [Label("备件类型名称")]
        public static readonly Property<string> SparePartTypeNameProperty
            = P<EquipBomDetail>.RegisterView(e => e.SparePartTypeName, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName
        {
            get { return this.GetProperty(SparePartTypeNameProperty); }
        }
        #endregion

        #region 设备型号 EquipModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelCodeProperty
            = P<EquipBomDetail>.RegisterView(e => e.EquipModelCode, p => p.SparePart.SpartEquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelNameProperty
            = P<EquipBomDetail>.RegisterView(e => e.EquipModelName, p => p.SparePart.SpartEquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<EquipBomDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #endregion

        #region 复制Id CopyFromId
        /// <summary>
        /// 复制Id
        /// </summary>
        [Label("复制Id")]
        public static readonly Property<double?> CopyFromIdProperty = P<EquipBomDetail>.Register(e => e.CopyFromId);

        /// <summary>
        /// 复制Id
        /// </summary>
        public double? CopyFromId
        {
            get { return this.GetProperty(CopyFromIdProperty); }
            set { this.SetProperty(CopyFromIdProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备BOM明细 实体配置
    /// </summary>
    internal class EquipBomDetailConfig : EntityConfig<EquipBomDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_BOM_DET").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.SupportTree();
            Meta.Property(EquipBomDetail.EquipBomDetailSelectIdProperty).DontMapColumn();
            Meta.Property(EquipBomDetail.EquipBomDetailSelectProperty).DontMapColumn();
            Meta.Property(EquipBomDetail.CopyFromIdProperty).DontMapColumn();
        }

        /// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(EquipBomDetail.SparePartQtyProperty, new PositiveNumberRule());

            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<EquipBomDetail>();
                    if (RT.Service.Resolve<EquipBomController>().VerifySparePartIsRepeat(para))
                    {
                        e.BrokenDescription = "同一层级下，【备件编码】不能重复，请确认！".L10N();
                    }
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}