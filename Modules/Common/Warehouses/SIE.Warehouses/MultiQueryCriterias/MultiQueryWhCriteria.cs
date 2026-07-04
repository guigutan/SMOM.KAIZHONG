using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库多选查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class MultiQueryWhCriteria : Criteria
    {
        #region 排除Id FilterId
        /// <summary>
        /// 排除Id
        /// </summary>
        public static readonly Property<List<double>> FilterIdProperty = P<MultiQueryWhCriteria>.Register(e => e.FilterId);

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
        [Label("编码")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<MultiQueryWhCriteria>.Register(e => e.Code);

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
        [MaxLength(80)]
        public static readonly Property<string> NameProperty = P<MultiQueryWhCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType?> LibraryTypeProperty = P<MultiQueryWhCriteria>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType? LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<MultiQueryWhCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>仓库</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WarehouseController>().GetMultiWarehouses(this);
        }
    }
}
