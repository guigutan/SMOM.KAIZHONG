using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品机型查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产品机型查询实体")]
    public partial class ProductModelCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编号")]
        public static readonly Property<string> CodeProperty = P<ProductModelCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ProductModelCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetProductModelList(this);
        }
    }
}
