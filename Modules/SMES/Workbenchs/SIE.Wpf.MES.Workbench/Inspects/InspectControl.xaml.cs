using DevExpress.Xpf.Editors;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.Helper;
using System.ComponentModel;
using System.Windows.Input;

namespace SIE.Wpf.MES.Workbench.Inspects
{
    /// <summary>
    /// 检验采集 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class InspectControl : ComponentItem
    {
        /// <summary>
        /// 工作台检验采集视图模型
        /// </summary>
        WorkBenchInspectViewModel InspectViewModel;

        /// <summary>
        /// 检验采集输出参数
        /// </summary>
        InspectOutput _output;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InspectControl()
        {
            InitializeComponent();
            _output = UseOutput<InspectOutput>();
            InspectViewModel = new WorkBenchInspectViewModel();
            ctlInspect.DataContext = InspectViewModel;
            InspectViewModel.PropertyChanged += InspectViewModel_PropertyChanged;
            UseProperty<InspectProperty>();
            var input = UseInput<InspectInput>();
            input.PropertyChanged += Input_PropertyChanged;
        }

        protected override void OnRun()
        {
            base.OnRun();
            //LoadSettingWorkstation();
        }

        /// <summary>
        /// 输入参数变更事件
        /// </summary>
        /// <param name="sender">输入参数</param>
        /// <param name="e">参数</param>
        private void Input_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var input = sender as InspectInput;
            var workstation = InspectViewModel.Workstation;
            if (e.PropertyName == nameof(InspectInput.UserId))
            {
                workstation.EmployeeId = input.UserId;
                workstation.ResourceId = null;
                workstation.ProcessId = null;
                workstation.StationId = null;
            }
            if (e.PropertyName == nameof(InspectInput.ResourceId))
            {
                workstation.ResourceId = input.ResourceId;
                workstation.StationId = null;
            }
            if (e.PropertyName == nameof(InspectInput.ProcessId))
            {
                workstation.ProcessId = input.ProcessId;
                workstation.StationId = null;
                InspectViewModel.ResetDefects(workstation.ProcessId);
            }
            if (e.PropertyName == nameof(InspectInput.StationId))
                workstation.StationId = input.StationId;
            InspectViewModel.ValidateWorkstaion();
        }

        /// <summary>
        /// 工作台检验采集视图模型属性变更
        /// </summary>
        /// <param name="sender">工作台检验采集视图模型</param>
        /// <param name="e">参数</param>
        private void InspectViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = sender as WorkBenchInspectViewModel;
            if (e.PropertyName == nameof(WorkBenchInspectViewModel.ProductId))
                _output.ProductId = vm.ProductId;
            else if (e.PropertyName == nameof(WorkBenchInspectViewModel.WorkOrder))
            {
                _output.WorkOrderId = vm.WorkOrder.Id;
                _output.ProductId = vm.WorkOrder.ProductId;
            }
        }

        /// <summary>
        /// 在焦点位于此元素上并且用户按下键时发生
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void Barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textbox = sender as TextEdit;
                var binding = textbox.GetBindingExpression(TextEdit.TextProperty);
                if (binding != null)
                    binding.UpdateSource();
                e.Handled = true;  ////解决条码输入框按下Enter键后无法获取焦点问题
            }
        }
    }

    /// <summary>
    /// 检验采集输出参数
    /// </summary>
    public class InspectOutput : ComponentOutput<InspectControl>
    {
        /// <summary>
        /// 产品ID 
        /// </summary>
        [DisplayName("产品ID")]
        [Description("检验采集产品切换时输出")]
        public virtual double ProductId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        [DisplayName("工单ID")]
        [Description("检验采集产品切换时输出")]
        public virtual double WorkOrderId { get; set; }
    }

    /// <summary>
    /// 检验采集输入参数
    /// </summary>
    public class InspectInput : ComponentInput<InspectControl>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        [Description("工序工位组件选择用户后输入")]
        public virtual double UserId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        [DisplayName("资源ID")]
        [Description("工序工位组件选择资源后输入")]
        public virtual double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        [DisplayName("工序ID")]
        [Description("工序工位组件选择工序后输入")]
        public virtual double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        [DisplayName("工位ID")]
        [Description("工序工位组件选择工位后输入")]
        public virtual double StationId { get; set; }
    }

    /// <summary>
    /// 检验采集属性
    /// </summary>
    public class InspectProperty : ComponentProperty<InspectControl>
    {
    }
}