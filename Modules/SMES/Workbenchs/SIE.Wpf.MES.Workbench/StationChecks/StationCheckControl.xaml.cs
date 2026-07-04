using SIE.MES.Workbench.StationChecks;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.Helper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class StationCheckControl : ComponentItem, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性变更方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 点检 SpotInspectionInfos 
        /// <summary>
        /// 月度排产集合
        /// </summary>        
        private ObservableCollection<StationCheck> _spotInspectionInfos;

        /// <summary>
        /// 月度排产集合
        /// </summary>
        public ObservableCollection<StationCheck> SpotInspectionInfos
        {
            get
            {
                if (_spotInspectionInfos == null)
                {
                    _spotInspectionInfos = new ObservableCollection<StationCheck>();
                }

                return _spotInspectionInfos;
            }

            set
            {
                if (_spotInspectionInfos != value)
                {
                    _spotInspectionInfos = value;
                    OnPropertyChanged("SpotInspectionInfos");
                }
            }
        }

        /// <summary>
        /// 当前选中项
        /// </summary>       
        private StationCheck selectedSpotInspection;

        /// <summary>
        /// 当前选中项
        /// </summary>
        public StationCheck SelectedSpotInspection
        {
            get
            {
                return selectedSpotInspection;
            }

            set
            {
                selectedSpotInspection = value;
            }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public StationCheckControl()
        {
            InitializeComponent();
            UseProperty<StationCheckProperty>();
            var input = UseInput<StationCheckInput>();
            input.PropertyChanged += Input_PropertyChanged;
        }

        protected override void OnRun()
        {
            base.OnRun();
            //LoadSettingWorkstation();
        }

        private void Input_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var input = sender as StationCheckInput;
            if (e.PropertyName == nameof(StationCheckInput.StationId) && input.StationId != 0)
                LoadSpotInspectionInfos(input.StationId);
        }

        /// <summary>
        /// 加载工位点检
        /// </summary>
        private void LoadSpotInspectionInfos(double stationId)
        {
            SpotInspectionInfos.Clear();
            if (stationId == 0) return;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SpotInspectionInfos = RT.Service.Resolve<StationCheckController>().GetStationChecks(stationId);
            }));
        }

        /// <summary>
        /// 点检状态变更方法
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void ChkSelect_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var state = checkBox.IsChecked;
            RT.Service.Resolve<StationCheckController>().SaveCheckItemResult(SelectedSpotInspection, state);
        }
    }

    /// <summary>
    /// 工位点检输入参数
    /// </summary>
    public class StationCheckInput : ComponentInput<StationCheckControl>
    {
        /// <summary>
        /// 工位ID
        /// </summary>
        [Description("工位ID")]
        [DisplayName("工序工位组件选择工位后输入")]
        public virtual double StationId { get; set; }
    }

    /// <summary>
    /// 工位点检属性
    /// </summary>
    public class StationCheckProperty : ComponentProperty<StationCheckControl>
    {
    }
}