using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 备件清单
    /// </summary>
    [RootEntity, Serializable]
    [Label("备件清单")]
    public class SparePartList : DataEntity
    {

        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty =
            P<SparePartList>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Normal);

        /// <summary>
        /// 盘点计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return (double)this.GetRefId(InventoryPlanIdProperty); }
            set { this.SetRefId(InventoryPlanIdProperty, value); }
        }

        /// <summary>
        /// 盘点计划
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty =
            P<SparePartList>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return this.GetRefEntity(InventoryPlanProperty); }
            set { this.SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion

        #region 备件清单 SparePart
        /// <summary>
        /// 备件清单Id
        /// </summary>
        [Label("备件清单")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<SparePartList>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件清单Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件清单
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<SparePartList>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件清单
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartList>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<SparePartList>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 规格型号 SpartPartSpec
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpartPartSpecProperty = P<SparePartList>.RegisterView(e => e.SpartPartSpec, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpartPartSpec
        {
            get { return this.GetProperty(SpartPartSpecProperty); }
        }
        #endregion

        #region 分类层级 ItemCateName
        /// <summary>
        /// 分类层级
        /// </summary>
        [Label("分类层级")]
        public static readonly Property<string> ItemCateNameProperty = P<SparePartList>.RegisterView(e => e.ItemCateName, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 分类层级
        /// </summary>
        public string ItemCateName
        {
            get { return this.GetProperty(ItemCateNameProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<SparePartList>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType.ToLabel());

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 设备类型 TypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> TypeCodeProperty = P<SparePartList>.RegisterView(e => e.TypeCode, p => p.SparePart.SpartEquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string TypeCode
        {
            get { return this.GetProperty(TypeCodeProperty); }
        }
        #endregion

        #region 设备型号 SpartEquipModel
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> SpartEquipModelProperty = P<SparePartList>.RegisterView(e => e.SpartEquipModel, p => p.SparePart.SpartEquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string SpartEquipModel
        {
            get { return this.GetProperty(SpartEquipModelProperty); }
        }
        #endregion

        #region 状态 SparePartState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> SparePartStateProperty = P<SparePartList>.RegisterView(e => e.SparePartState, p => p.SparePart.State.ToLabel());

        /// <summary>
        /// 状态
        /// </summary>
        public State SparePartState
        {
            get { return this.GetProperty(SparePartStateProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartList>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod.ToLabel());

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion


        #endregion

    }

    /// <summary>
    /// 设备清单 实体配置
    /// </summary>
    internal class SparePartListConfig : EntityConfig<SparePartList>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_SPARE_LIST").MapAllProperties();
            //Meta.EnablePhantoms();
        }
    }

}
