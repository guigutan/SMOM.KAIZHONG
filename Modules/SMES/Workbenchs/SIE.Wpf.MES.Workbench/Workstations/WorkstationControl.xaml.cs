using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.MES.Workbench.Helper;
using SIE.Wpf.MES.Workbench.Properties;
using System;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.Workstations
{
    /// <summary>
    /// WorkstationControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class WorkstationControl : ComponentItem
    {
        /// <summary>
        /// 工序工位输出参数
        /// </summary>
        WorkstationOutput _output;

        /// <summary>
        /// 工作站
        /// </summary>
        Workstation _workstation;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkstationControl()
        {
            InitializeComponent();
            _output = UseOutput<WorkstationOutput>();
            UseProperty<WorkstationProperty>();
            _workstation = new Workstation(null);
            this.DataContext = _workstation;
            _workstation.PropertyChanged += Workstation_PropertyChanged;
        }

        protected override void OnRun()
        {
            base.OnRun();
            //LoadWorkstation();
        }

        /// <summary>
        /// 工作站信息属性变更事件
        /// </summary>
        /// <param name="sender">工作站</param>
        /// <param name="e">参数</param>
        private void Workstation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var workstation = sender as Workstation;
            if (workstation == null) return;
            if (e.PropertyName == nameof(Workstation.EmployeeId))
            {
                _output.UserId = workstation.EmployeeId.Value;
                workstation.ResourceId = null;
                workstation.ProcessId = null;
                workstation.StationId = null;
            }
            if (e.PropertyName == nameof(Workstation.ResourceId) && workstation.ResourceId != null)
            {
                _output.ResourceId = workstation.ResourceId.Value;
                workstation.ProcessId = null;
                workstation.StationId = null;
            }
            if (e.PropertyName == nameof(Workstation.ProcessId) && workstation.ProcessId != null)
            {
                _output.ProcessId = workstation.ProcessId.Value;
                workstation.StationId = null;
            }
            if (e.PropertyName == nameof(Workstation.StationId) && workstation.StationId != null)
            {
                _output.StationId = workstation.StationId.Value;
                // SaveWorkstation();
            }
        }
    }

    /// <summary>
    /// 工序工位输出参数
    /// </summary>
    public class WorkstationOutput : ComponentOutput<WorkstationControl>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        [Description("工序工位组件选择用户后输出")]
        public virtual double UserId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        [DisplayName("资源ID")]
        [Description("工序工位组件选择资源后输出")]
        public virtual double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        [DisplayName("工序ID")]
        [Description("工序工位组件选择工序后输出")]
        public virtual double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        [DisplayName("工位ID")]
        [Description("工序工位组件选择工位后输出")]
        public virtual double StationId { get; set; }
    }

    /// <summary>
    /// 工序工位属性
    /// </summary>
    public class WorkstationProperty : ComponentProperty<WorkstationControl>
    {
    }
}