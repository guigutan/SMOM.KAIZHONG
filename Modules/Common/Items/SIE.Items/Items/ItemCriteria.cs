using SIE.Common;
using SIE.Domain;
using SIE.Items.Items;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料查询实体")]
    public class ItemCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemCriteria()
        {
            UpdateDate = new DateRange();
            UpdateDate.DateTimePart = DateTimePart.Date;
            UpdateDate.DateRangeType = DateRangeType.All;
        }

        #region 编码 Code 
        /// <summary>
        /// 编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<ItemCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name 
        /// <summary>
        /// 名称
        /// </summary> 
        [Label("物料名称")]
        public static readonly Property<string> NameProperty = P<ItemCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 规则型号 SpecificationModel
        /// <summary>
        /// 规则型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<ItemCriteria>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规则型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 基本分类 Type 
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType?> TypeProperty = P<ItemCriteria>.Register(e => e.Type);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 来源类型 ItemSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<ItemCriteria>.Register(e => e.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType
        {
            get { return this.GetProperty(ItemSourceTypeProperty); }
            set { this.SetProperty(ItemSourceTypeProperty, value); }
        }
        #endregion

        #region 状态 State 
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<ItemCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType?> SourceTypeProperty = P<ItemCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 更新时间 UpdateDate
        /// <summary>
        /// 更新时间
        /// </summary>
        [Label("修改时间")]
        public static readonly Property<DateRange> UpdateDateProperty = P<ItemCriteria>.Register(e => e.UpdateDate);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateRange UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 修改人 UpdateBy
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly Property<string> UpdateByProperty = P<ItemCriteria>.Register(e => e.UpdateBy);

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy
        {
            get { return this.GetProperty(UpdateByProperty); }
            set { this.SetProperty(UpdateByProperty, value); }
        }
        #endregion

        #region 采购员 PurchasingAgent
        /// <summary>
        /// 采购员Id
        /// </summary>
        [Label("采购员")]
        public static readonly IRefIdProperty PurchasingAgentIdProperty =
            P<ItemCriteria>.RegisterRefId(e => e.PurchasingAgentId, ReferenceType.Normal);

        /// <summary>
        /// 采购员Id
        /// </summary>
        public double? PurchasingAgentId
        {
            get { return (double?)this.GetRefNullableId(PurchasingAgentIdProperty); }
            set { this.SetRefNullableId(PurchasingAgentIdProperty, value); }
        }

        /// <summary>
        /// 采购员
        /// </summary>
        public static readonly RefEntityProperty<Employee> PurchasingAgentProperty =
            P<ItemCriteria>.RegisterRef(e => e.PurchasingAgent, PurchasingAgentIdProperty);

        /// <summary>
        /// 采购员
        /// </summary>
        public Employee PurchasingAgent
        {
            get { return this.GetRefEntity(PurchasingAgentProperty); }
            set { this.SetRefEntity(PurchasingAgentProperty, value); }
        }
        #endregion

        #region 分类类型 CategoryType
        /// <summary>
        /// 分类类型
        /// </summary>
        [Label("分类类型")]
        public static readonly Property<CategoryType?> CategoryTypeProperty = P<ItemCriteria>.Register(e => e.CategoryType);

        /// <summary>
        /// 分类类型
        /// </summary>
        public CategoryType? CategoryType
        {
            get { return this.GetProperty(CategoryTypeProperty); }
            set { this.SetProperty(CategoryTypeProperty, value); }
        }
        #endregion

        #region 物料分类 ItemCategory
        /// <summary>
        /// 物料分类Id
        /// </summary>
        [Label("物料分类")]
        public static readonly IRefIdProperty ItemCategoryIdProperty =
            P<ItemCriteria>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 物料分类Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)this.GetRefNullableId(ItemCategoryIdProperty); }
            set { this.SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 物料分类
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty =
            P<ItemCriteria>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 物料分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return this.GetRefEntity(ItemCategoryProperty); }
            set { this.SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 物料分类（多选联动） ItemCategorys
        /// <summary>
        /// 物料分类（多选联动）
        /// </summary>
        [Label("物料分类")]
        public static readonly Property<string> ItemCategorysProperty = P<ItemCriteria>.Register(e => e.ItemCategorys);

        /// <summary>
        /// 物料分类（多选联动）
        /// </summary>
        public string ItemCategorys
        {
            get { return this.GetProperty(ItemCategorysProperty); }
            set { this.SetProperty(ItemCategorysProperty, value); }
        }
        #endregion

        #region 基本类型 SelType  用于选择时排除某个类型  CAD数据维护功能
        /// <summary>
        /// 基本类型
        /// </summary>
        [Label("基本类型")]
        public static readonly Property<ItemType?> SelTypeProperty = P<ItemCriteria>.Register(e => e.SelType);

        /// <summary>
        /// 基本类型
        /// </summary>
        public ItemType? SelType
        {
            get { return GetProperty(SelTypeProperty); }
            set { SetProperty(SelTypeProperty, value); }
        }
        #endregion

        #region 过滤Id NotItemIds
        /// <summary>
        /// 过滤Id
        /// </summary>
        [Label("过滤Id")]
        public static readonly Property<string> NotItemIdsProperty = P<ItemCriteria>.Register(e => e.NotItemIds);

        /// <summary>
        /// 过滤Id
        /// </summary>
        public string NotItemIds
        {
            get { return this.GetProperty(NotItemIdsProperty); }
            set { this.SetProperty(NotItemIdsProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ItemCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 父级旧料号 Bismt
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> BismtProperty = P<ItemCriteria>.Register(e => e.Bismt);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string Bismt
        {
            get { return this.GetProperty(BismtProperty); }
            set { this.SetProperty(BismtProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询实体 获取查询结果
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItems(this);
        }
    }
}