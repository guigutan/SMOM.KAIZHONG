using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings.ViewModels
{
    /// <summary>
    /// 导入工单 实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("导入保存的数据")]
    public class RoutingImportSaveViewModel : ViewModel
    {
        #region 产品族分类Id Category
        /// <summary>
        /// 产品族分类Id
        /// </summary>
        [Label("产品族分类Id")]
        public static readonly Property<double> CategoryIdProperty = P<RoutingImportSaveViewModel>.Register(e => e.CategoryId);

        /// <summary>
        /// 产品族分类Id
        /// </summary>
        public double CategoryId
        {
            get { return this.GetProperty(CategoryIdProperty); }
            set { this.SetProperty(CategoryIdProperty, value); }
        }
        #endregion

        #region 产品族分类 Category
        /// <summary>
        /// 产品族分类
        /// </summary>
        [Label("产品族分类")]
        public static readonly Property<string> CategoryProperty = P<RoutingImportSaveViewModel>.Register(e => e.Category);

        /// <summary>
        /// 产品族分类
        /// </summary>
        public string Category
        {
            get { return this.GetProperty(CategoryProperty); }
            set { this.SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 工艺路线ID RoutingId
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        [Label("工艺路线ID")]
        public static readonly Property<double?> RoutingIdProperty = P<RoutingImportSaveViewModel>.Register(e => e.RoutingId);

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double? RoutingId
        {
            get { return this.GetProperty(RoutingIdProperty); }
            set { this.SetProperty(RoutingIdProperty, value); }
        }
        #endregion 

        #region 工艺路线名称 RoutingName
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [Label("工艺路线名称")]
        public static readonly Property<string> RoutingNameProperty = P<RoutingImportSaveViewModel>.Register(e => e.RoutingName);

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string RoutingName
        {
            get { return this.GetProperty(RoutingNameProperty); }
            set { this.SetProperty(RoutingNameProperty, value); }
        }
        #endregion

        #region 工艺路线描述 RoutingDesc
        /// <summary>
        /// 工艺路线描述
        /// </summary>
        [Label("工艺路线描述")]
        public static readonly Property<string> RoutingDescProperty = P<RoutingImportSaveViewModel>.Register(e => e.RoutingDesc);

        /// <summary>
        /// 工艺路线描述
        /// </summary>
        public string RoutingDesc
        {
            get { return this.GetProperty(RoutingDescProperty); }
            set { this.SetProperty(RoutingDescProperty, value); }
        }
        #endregion

        #region 行号 RowNum
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> RowNumProperty = P<RoutingImportSaveViewModel>.Register(e => e.RowNum);

        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get { return this.GetProperty(RowNumProperty); }
            set { this.SetProperty(RowNumProperty, value); }
        }
        #endregion

        #region 是否验证通过 IsPass
        /// <summary>
        /// 是否验证通过
        /// </summary>
        [Label("是否验证通过")]
        public static readonly Property<bool> IsPassProperty = P<RoutingImportSaveViewModel>.Register(e => e.IsPass);

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsPass
        {
            get { return this.GetProperty(IsPassProperty); }
            set { this.SetProperty(IsPassProperty, value); }
        }
        #endregion 

        #region 工序集合 ImportDataViewModelList 
        /// <summary>
        /// 工序集合
        /// </summary>
        public static readonly ListProperty<EntityList<ProcessViewModel>> ProcessDetailModelListProperty =
            P<RoutingImportSaveViewModel>.RegisterList(e => e.ProcessDetailModelList, new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
            });

        /// <summary>
        /// 工序集合
        /// </summary>
        public EntityList<ProcessViewModel> ProcessDetailModelList
        {
            get { return this.GetLazyList(ProcessDetailModelListProperty); }
        }
        #endregion
    }
}