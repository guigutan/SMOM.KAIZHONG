using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 工位物料ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位物料")]
    public class StationMateriaViewModel : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<StationMateriaViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<StationMateriaViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<StationMateriaViewModel>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<StationMateriaViewModel>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 工位容量 Capacity
        /// <summary>
        /// 工位容量
        /// </summary>
        [Label("工位容量")]
        public static readonly Property<decimal> CapacityProperty = P<StationMateriaViewModel>.Register(e => e.Capacity);

        /// <summary>
        /// 工位容量
        /// </summary>
        public decimal Capacity
        {
            get { return this.GetProperty(CapacityProperty); }
            set { this.SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 预警值 AlterValue
        /// <summary>
        /// 预警值
        /// </summary>
        [Label("预警值")]
        public static readonly Property<decimal> AlterValueProperty = P<StationMateriaViewModel>.Register(e => e.AlterValue);

        /// <summary>
        /// 预警值
        /// </summary>
        public decimal AlterValue
        {
            get { return this.GetProperty(AlterValueProperty); }
            set { this.SetProperty(AlterValueProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<StationMateriaViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 叫料数量 CallQty
        /// <summary>
        /// 叫料数量
        /// </summary>
        [Label("叫料数量")]
        public static readonly Property<decimal> CallQtyProperty = P<StationMateriaViewModel>.Register(e => e.CallQty);

        /// <summary>
        /// 叫料数量
        /// </summary>
        public decimal CallQty
        {
            get { return this.GetProperty(CallQtyProperty); }
            set { this.SetProperty(CallQtyProperty, value); }
        }
        #endregion

        #region 工位库存（物料接收未上料） StockQty
        /// <summary>
        /// 工位库存
        /// </summary>
        [Label("工位库存")]
        public static readonly Property<decimal> StockQtyProperty = P<StationMateriaViewModel>.Register(e => e.StockQty);

        /// <summary>
        /// 工位库存
        /// </summary>
        public decimal StockQty
        {
            get { return this.GetProperty(StockQtyProperty); }
            set { this.SetProperty(StockQtyProperty, value); }
        }
        #endregion

        #region 在途数量 SendingQty
        /// <summary>
        /// 在途数量
        /// </summary>
        [Label("在途数量")]
        public static readonly Property<decimal> SendingQtyProperty = P<StationMateriaViewModel>.Register(e => e.SendingQty);

        /// <summary>
        /// 在途数量
        /// </summary>
        public decimal SendingQty
        {
            get { return this.GetProperty(SendingQtyProperty); }
            set { this.SetProperty(SendingQtyProperty, value); }
        }
        #endregion

        #region 物料需求诊断 RequirementDiagnosis
        /// <summary>
        /// 物料需求诊断
        /// </summary>
        [Label("物料需求诊断")]
        public static readonly Property<string> RequirementDiagnosisProperty = P<StationMateriaViewModel>.Register(e => e.RequirementDiagnosis);

        /// <summary>
        /// 物料需求诊断
        /// </summary>
        public string RequirementDiagnosis
        {
            get { return this.GetProperty(RequirementDiagnosisProperty); }
            set { this.SetProperty(RequirementDiagnosisProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工位物料视图模型配置
    /// </summary>
    class StationMateriaEntityConfig : EntityConfig<StationMateriaViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);

            rules.AddRule(StationMateriaViewModel.CallQtyProperty, new HandlerRule
            {
                Handler = (o, e) =>
               {
                   var entity = o as StationMateriaViewModel;
                   if (entity.CallQty > entity.Capacity)
                       e.BrokenDescription = "叫料数量不能够超过工位物料容量".L10N();
                   if (entity.CallQty < 0)
                       e.BrokenDescription = "叫料数量不能小于0".L10N();
               }
            });
        }
    }
}
