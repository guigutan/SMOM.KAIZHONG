using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 产品规格件对照表
	/// </summary>
	[QueryEntity, Serializable]
    [Label("产品规格件对照表")]
    public partial class ProductSpecificationCriteria : Criteria
    {
        #region 物料编码 Code
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<ProductSpecificationCriteria>.Register(e => e.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料名称 Name
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> NameProperty = P<ProductSpecificationCriteria>.Register(e => e.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<ProductSpecificationCriteria>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return GetProperty(SpecificationModelProperty); }
            set { SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 基本类型 Type
        /// <summary>
        /// 基本类型
        /// </summary>
        [Label("基本类型")]
        public static readonly Property<ItemType?> TypeProperty = P<ProductSpecificationCriteria>.Register(e => e.Type);

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
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<ProductSpecificationCriteria>.Register(e => e.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType
        {
            get { return GetProperty(ItemSourceTypeProperty); }
            set { SetProperty(ItemSourceTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<ProductSpecificationCriteria>.Register(e => e.State);

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
        public static readonly Property<SourceType?> SourceTypeProperty = P<ProductSpecificationCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType? SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductSpecificationController>().GetProductSpecificationList(this);
        }
    }

}
