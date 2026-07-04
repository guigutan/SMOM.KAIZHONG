using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Controls
{
    /// <summary>
    /// WorkCellSelectorControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorkCellSelectorControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkCellSelectorControl()
        {
            InitializeComponent();

            WipResourceList = new ObservableCollection<WipResource>();
            ProcessList = new ObservableCollection<Process>();
            StationList = new ObservableCollection<Station>();

            this.DataContext = this;
        }

        /// <summary>
        /// 对话框结果(0 确定；1 取消) (默认为取消)
        /// </summary>
        public int Result { get; set; } = 1;

        /// <summary>
        /// 产线列表
        /// </summary>
        public ObservableCollection<WipResource> WipResourceList { get; }

        private WipResource selectedWipResource;

        /// <summary>
        /// 选中的产线
        /// </summary>
        public WipResource SelectedWipResource
        {
            get
            {
                return selectedWipResource;
            }
            set
            {
                if (value != selectedWipResource)
                {
                    selectedWipResource = value;

                    GetStations();

                    OnPropertyChanged("SelectedWipResource");
                }
            }
        }

        /// <summary>
        /// 工序列表
        /// </summary>
        public ObservableCollection<Process> ProcessList { get; }

        private Process selectedProcess;

        /// <summary>
        /// 选中的工序
        /// </summary>
        public Process SelectedProcess
        {
            get
            {
                return selectedProcess;
            }
            set
            {
                if (value != selectedProcess)
                {
                    selectedProcess = value;
                    GetStations();

                    OnPropertyChanged("SelectedProcess");
                }
            }
        }

        /// <summary>
        /// 工位列表
        /// </summary>
        public ObservableCollection<Station> StationList { get; }

        private Station selectedStation;

        /// <summary>
        /// 选中的工位
        /// </summary>
        public Station SelectedStation
        {
            get
            {
                return selectedStation;
            }
            set
            {
                if (value != selectedStation)
                {
                    selectedStation = value;
                    OnPropertyChanged("SelectedStation");
                }
            }
        }

        /// <summary>
        /// 重新加载工位列表
        /// </summary>
        private void GetStations()
        {
            StationList.Clear();
            if (selectedProcess != null && selectedWipResource != null)
            {
                StationList.AddRange(RT.Service.Resolve<StationController>()
                    .GetStationsByResourceId(SelectedWipResource.Id, SelectedProcess.Id));
            }
        }

        #region INotifyPropertyChanged Members  
        /// <summary>
        /// 属性变更
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
