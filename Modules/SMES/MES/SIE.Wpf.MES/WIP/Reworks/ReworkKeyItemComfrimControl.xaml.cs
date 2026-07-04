using SIE.LES.LinesideWarehouses;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// ReworkKeyItemComfrimControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReworkKeyItemComfrimControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// 返工关键件提交确认弹窗
        /// </summary>
        public ReworkKeyItemComfrimControl()
        {
            InitializeComponent();
            DataContext = this;
            WarehouseList = new ObservableCollection<LinesideWarehouse>();
        }
        /// <summary>
        /// 对话框结果(0 确定；1 取消) (默认为取消)
        /// </summary>
        public int Result { get; set; } = 1;


        #region INotifyPropertyChanged Members  
        /// <summary>
        /// 属性变更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性变更通知
        /// </summary>
        /// <param name="txt"></param>
        public void OnPropertyChanged(string txt)
        {
            PropertyChangedEventHandler handle = PropertyChanged;
            if (handle != null)
            {
                handle(this, new PropertyChangedEventArgs(txt));
            }
        }
        #endregion

        private bool selectedCancelWay;

        /// <summary>
        /// 选中的作废处理方式
        /// </summary>
        public bool SelectedCancelWay
        {
            get
            {
                return selectedCancelWay;
            }
            set
            {
                if (value != selectedCancelWay)
                {
                    selectedCancelWay = value;
                    OnPropertyChanged("SelectedCancelWay");
                    ShowWarehouseSelection = SelectedGoodBlankingWay || SelectedBlankingWay;
                }
            }
        }
        private bool selectedBlankingWay;

        /// <summary>
        /// 选中的下料处理方式
        /// </summary>
        public bool SelectedBlankingWay
        {
            get
            {
                return selectedBlankingWay;
            }
            set
            {
                if (value != selectedBlankingWay)
                {
                    selectedBlankingWay = value;
                    OnPropertyChanged("SelectedBlankingWay");
                    ShowWarehouseSelection = SelectedGoodBlankingWay || SelectedBlankingWay;
                }
            }
        }

        private bool selectedGoodBlankingWay;

        /// <summary>
        /// 选中的良品下料处理方式
        /// </summary>
        public bool SelectedGoodBlankingWay
        {
            get
            {
                return selectedGoodBlankingWay;
            }
            set
            {
                selectedGoodBlankingWay = value;
                OnPropertyChanged("SelectedGoodBlankingWay");
                ShowWarehouseSelection = SelectedGoodBlankingWay || SelectedBlankingWay;
            }
        }

        private bool _showWarehouseSelection;
        /// <summary>
        /// 显示线边仓
        /// </summary>
        public bool ShowWarehouseSelection
        {
            get { return _showWarehouseSelection; }
            set
            {
                _showWarehouseSelection = value;
                OnPropertyChanged(nameof(ShowWarehouseSelection)); // 触发属性更改通知
            }
        }

        /// <summary>
        /// 产线列表
        /// </summary>
        public ObservableCollection<LinesideWarehouse> WarehouseList { get; }

        private LinesideWarehouse selectedWarehouse;

        /// <summary>
        /// 选中的线边仓
        /// </summary>
        public LinesideWarehouse SelectedWarehouse
        {
            get
            {
                return selectedWarehouse;
            }
            set
            {
                if (value != selectedWarehouse)
                {
                    selectedWarehouse = value;
                    OnPropertyChanged("SelectedWarehouse");
                }
            }
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var window = this.GetVisualParent<Window>();
            if (window != null)
            {
                this.Result = 0;
                window.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var window = this.GetVisualParent<Window>();
            if (window != null)
            {
                this.Result = 1;
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
