using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems.ViewModels
{
    /// <summary>
    /// 一键下料视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("一键下料视图模型")]
    public class UnloadAllItemViewModel : ViewModel
    {
        #region 上料 LoadItem
        /// <summary>
        /// 上料
        /// </summary>
        [Label("上料")]
        public static readonly Property<LoadItem> LoadItemProperty = P<UnloadAllItemViewModel>.Register(e => e.LoadItem);

        /// <summary>
        /// 上料
        /// </summary>
        public LoadItem LoadItem
        {
            get { return this.GetProperty(LoadItemProperty); }
            set { this.SetProperty(LoadItemProperty, value); }
        }
        #endregion

        #region 是否下料 IsLoadItem
        /// <summary>
        /// 是否下料
        /// </summary>
        [Label("是否下料")]
        public static readonly Property<bool> IsLoadItemProperty = P<UnloadAllItemViewModel>.Register(e => e.IsLoadItem);

        /// <summary>
        /// 是否下料
        /// </summary>
        public bool IsLoadItem
        {
            get { return this.GetProperty(IsLoadItemProperty); }
            set { this.SetProperty(IsLoadItemProperty, value); }
        }
        #endregion
    }
}