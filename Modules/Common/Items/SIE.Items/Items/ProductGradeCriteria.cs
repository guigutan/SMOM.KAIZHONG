using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items
{
    /// <summary>
    /// 产品等级查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产品等级查询实体")]
    public partial class ProductGradeCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ProductGradeCriteria>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ProductGradeCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Describe
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescribeProperty = P<ProductGradeCriteria>.Register(e => e.Describe);

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe
        {
            get { return GetProperty(DescribeProperty); }
            set { SetProperty(DescribeProperty, value); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<ProductGradeCriteria>.Register(e => e.ItemId);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetProductGrades(this);
        }
    }
}
