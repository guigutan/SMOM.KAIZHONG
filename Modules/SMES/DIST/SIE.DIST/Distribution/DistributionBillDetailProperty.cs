using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 配送单明细属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("配送单明细属性值")]
    [DisplayMember(nameof(Value))]
    public partial class DistributionBillDetailProperty : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<DistributionBillDetailProperty>.Register(e => e.Value);

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
            P<DistributionBillDetailProperty>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

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
            P<DistributionBillDetailProperty>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 配送单明细 Bill
        /// <summary>
        /// 配送单明细Id
        /// </summary>
        public static readonly IRefIdProperty BillDetailIdProperty = P<DistributionBillDetailProperty>.RegisterRefId(e => e.BillId, ReferenceType.Parent);

        /// <summary>
        /// 配送单明细Id
        /// </summary>
        public double BillId
        {
            get { return (double)GetRefId(BillDetailIdProperty); }
            set { SetRefId(BillDetailIdProperty, value); }
        }

        /// <summary>
        /// 配送单明细
        /// </summary>
        public static readonly RefEntityProperty<DistributionBillDetail> BillProperty = P<DistributionBillDetailProperty>.RegisterRef(e => e.BillDetail, BillDetailIdProperty);

        /// <summary>
        /// 配送单明细
        /// </summary>
        public DistributionBillDetail BillDetail
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
        public static readonly Property<string> DefinitionNameProperty = P<DistributionBillDetailProperty>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

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
    internal class DistributionBillDetailPropertyConfig : EntityConfig<DistributionBillDetailProperty>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DIST_BILL_DTL_PROP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}