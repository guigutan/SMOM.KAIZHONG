using SIE.Common;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Items.Items
{
    /// <summary>
    /// 物料多选查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class MultiQueryItemCriteria : Criteria
    {
        #region 排除Id FilterId
        /// <summary>
        /// 排除Id
        /// </summary>
        public static readonly Property<List<double>> FilterIdProperty = P<MultiQueryItemCriteria>.Register(e => e.FilterId);

        /// <summary>
        /// 排除Id
        /// </summary>
        public List<double> FilterId
        {
            get { return this.GetProperty(FilterIdProperty); }
            set { this.SetProperty(FilterIdProperty, value); }
        }
        #endregion

        #region 编码 Code 
        /// <summary>
        /// 编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<MultiQueryItemCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<MultiQueryItemCriteria>.Register(e => e.Name);

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
        public static readonly Property<string> SpecificationModelProperty = P<MultiQueryItemCriteria>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规则型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 基本类型 Type 
        /// <summary>
        /// 基本类型
        /// </summary>
        [Label("基本类型")]
        public static readonly Property<ItemType?> TypeProperty = P<MultiQueryItemCriteria>.Register(e => e.Type);

        /// <summary>
        /// 基本类型
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
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<MultiQueryItemCriteria>.Register(e => e.ItemSourceType);

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
        public static readonly Property<State?> StateProperty = P<MultiQueryItemCriteria>.Register(e => e.State);

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
        public static readonly Property<SourceType?> SourceTypeProperty = P<MultiQueryItemCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 物料分类 ItemCategorys
        /// <summary>
        /// 物料分类
        /// </summary>
        [Label("物料分类")]
        public static readonly Property<string> ItemCategorysProperty = P<MultiQueryItemCriteria>.Register(e => e.ItemCategorys);

        /// <summary>
        /// 物料分类
        /// </summary>
        public string ItemCategorys
        {
            get { return this.GetProperty(ItemCategorysProperty); }
            set { this.SetProperty(ItemCategorysProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>查询数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItems(this);
        }
    }
}
