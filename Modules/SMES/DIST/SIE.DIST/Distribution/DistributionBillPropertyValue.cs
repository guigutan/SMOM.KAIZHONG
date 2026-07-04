using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 配送单属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("配送单属性值")]
    [DisplayMember(nameof(Value))]
    public partial class DistributionBillPropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<DistributionBillPropertyValue>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Required]
        [Label("物料属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<DistributionBillPropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double? DefinitionId
        {
            get { return (double?)this.GetRefNullableId(DefinitionIdProperty); }
            set { this.SetRefNullableId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<DistributionBillPropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 属性列表 Bill
        /// <summary>
        /// 属性列表Id
        /// </summary>
        [Required]
        public static readonly IRefIdProperty BillIdProperty = P<DistributionBillPropertyValue>.RegisterRefId(e => e.BillId, ReferenceType.Parent);

        /// <summary>
        /// 属性列表Id
        /// </summary>
        public double BillId
        {
            get { return (double)GetRefId(BillIdProperty); }
            set { SetRefId(BillIdProperty, value); }
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public static readonly RefEntityProperty<DistributionBill> BillProperty = P<DistributionBillPropertyValue>.RegisterRef(e => e.Bill, BillIdProperty);

        /// <summary>
        /// 属性列表
        /// </summary>
        public DistributionBill Bill
        {
            get { return GetRefEntity(BillProperty); }
            set { SetRefEntity(BillProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 属性名称 DefinitionName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> DefinitionNameProperty = P<DistributionBillPropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class DistributionBillPropertyValueConfig : EntityConfig<DistributionBillPropertyValue>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DIST_BILL_PROP_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}