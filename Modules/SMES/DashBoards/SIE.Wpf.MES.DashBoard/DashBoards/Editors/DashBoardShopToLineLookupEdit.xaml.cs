using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Editors
{
    /// <summary>
    /// 车间产线下拉选择（产线多选）
    /// DashBoardShopToLineLookupEdit.xaml 的交互逻辑
    /// </summary>
    public partial class DashBoardShopToLineLookupEdit : UserControl
    {
        /// <summary>
        /// 车间与产线联动对象
        /// </summary>
        public ShopAndLine ShopAndLine
        {
            get { return (ShopAndLine)GetValue(ShopAndLineProperty); }
            set { SetValue(ShopAndLineProperty, value); }
        }

        /// <summary>
        /// 依赖属性
        /// </summary>
        // Using a DependencyProperty as the backing store for ShopAndLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShopAndLineProperty =
            DependencyProperty.Register("ShopAndLine", typeof(ShopAndLine), typeof(DashBoardShopToLineLookupEdit), new PropertyMetadata(new ShopAndLine()));

        /// <summary>
        /// 构造函数
        /// </summary>
        public DashBoardShopToLineLookupEdit()
        {
            InitializeComponent();

            this.Loaded += (o, e) =>
            {
                var path = PropertyEditorBinder.GetPropertyName(this);
                this.SetBinding(ShopAndLineProperty, new Binding(path));
                this.displayGrid.DataContext = this.ShopAndLine;

                this.shopLookupEdit.ItemsSource = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(null, string.Empty);
                if (this.ShopAndLine.Shop == null)
                    this.lineLookupEdit.ItemsSource = null;
                else
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    this.lineLookupEdit.ItemsSource = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, ShopAndLine.Shop.Value, null, null);
                }

                this.ShopAndLine.PropertyChanged += ShopAndLine_PropertyChanged;
            };
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="sender">实体对象</param>
        /// <param name="e">变更属性参数</param>
        private void ShopAndLine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var shopAndLine = sender as ShopAndLine;
            if (e.PropertyName == "Shop")
            {
                shopAndLine.Lines = null;

                if (shopAndLine.Shop == null)
                    this.lineLookupEdit.ItemsSource = null;
                else
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    this.lineLookupEdit.ItemsSource = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, ShopAndLine.Shop.Value, null, null);
                }
            }
        }
    }

    /// <summary>
    /// 车间与产线联动对象
    /// </summary>
    [Serializable]
    public class ShopAndLine : ObservableObject
    {
        /// <summary>
        /// 车间
        /// </summary>
        public double? Shop
        {
            get { return GetProperty<double?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public string Lines
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
