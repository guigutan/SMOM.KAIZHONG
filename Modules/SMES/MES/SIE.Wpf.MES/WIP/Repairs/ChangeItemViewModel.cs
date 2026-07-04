using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.LoadItems;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 换料视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("换料")]
    public class ChangeItemViewModel : ViewModel
    {
        /// <summary>
        /// 换料装配信息
        /// </summary>
        public ProductAssemblyDetailViewModel ProductAssemblyDetailViewModel { get; set; }

        #region 换料条码 ChangeSn
        /// <summary>
        /// 换料条码
        /// </summary>
        [Label("换料条码")]
        public static readonly Property<string> ChangeSnProperty = P<ChangeItemViewModel>.Register(e => e.ChangeSn);

        /// <summary>
        /// 换料条码
        /// </summary>
        public string ChangeSn
        {
            get { return this.GetProperty(ChangeSnProperty); }
            set { this.SetProperty(ChangeSnProperty, value); }
        }
        #endregion

        #region 换料数量 ChangeQty
        /// <summary>
        /// 换料数量
        /// </summary>
        [Label("换料数量"), MinValue(0)]
        public static readonly Property<decimal> ChangeQtyProperty
            = P<ChangeItemViewModel>.Register(e => e.ChangeQty, new PropertyMetadata<decimal>()
            {
                PropertyChangedCallBack = (s, e) => (s as ChangeItemViewModel).OnChangeQtyChanged(e)
            });

        /// <summary>
        /// 换料数量
        /// </summary>
        public decimal ChangeQty
        {
            get { return this.GetProperty(ChangeQtyProperty); }
            set { this.SetProperty(ChangeQtyProperty, value); }
        }

        /// <summary>
        /// 换料数量变更事件
        /// </summary>
        /// <param name="e">参数</param>
        private void OnChangeQtyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (ProductAssemblyDetailViewModel != null)
            {
                ProductAssemblyDetailViewModel.ComputeTotalChangeQty();
            }
        }

        #endregion

        #region 是否已上料 IsLoadItem
        /// <summary>
        /// 是否已上料
        /// </summary>
        public static readonly Property<bool> IsLoadItemProperty = P<ChangeItemViewModel>.Register(e => e.IsLoadItem);

        /// <summary>
        /// 是否已上料
        /// </summary>
        public bool IsLoadItem
        {
            get { return this.GetProperty(IsLoadItemProperty); }
            set { this.SetProperty(IsLoadItemProperty, value); }
        }
        #endregion

        #region 上料条码信息 LoadItemBarcodeInfo
        /// <summary>
        /// 上料条码信息
        /// </summary>
        [Label("上料条码信息")]
        public static readonly Property<LoadItemBarcodeInfo> LoadItemBarcodeInfoProperty
            = P<ChangeItemViewModel>.Register(e => e.LoadItemBarcodeInfo);

        /// <summary>
        /// 上料条码信息
        /// </summary>
        public LoadItemBarcodeInfo LoadItemBarcodeInfo
        {
            get { return this.GetProperty(LoadItemBarcodeInfoProperty); }
            set { this.SetProperty(LoadItemBarcodeInfoProperty, value); }
        }
        #endregion
    }
}
