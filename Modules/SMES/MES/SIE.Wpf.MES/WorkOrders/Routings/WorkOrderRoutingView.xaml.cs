using SIE.Tech.Routings.Technologys;
using SIE.Wpf.MES.WorkOrders.ViewModels;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WorkOrders.Routings
{
    /// <summary>
    /// WorkOrderRoutingView.xaml 的交互逻辑
    /// </summary>
    public partial class WorkOrderRoutingView : UserControl
    {
        /// <summary>
        /// 工单工艺路线工序属性
        /// </summary>
        WorkOrderRoutingProcessProperty property;

        /// <summary>
        /// 构造方法
        /// </summary>
        public WorkOrderRoutingView()
        {
            InitializeComponent();
            property = new WorkOrderRoutingProcessProperty();
            var template = new DetailsUITemplate<WorkOrderRoutingProcessProperty>();
            var ui = template.CreateUI();
            ui.MainView.Data = property;
            propertyControl.Content = ui.Control;
            container.ModelChanged += Container_ModelChanged;
        }

        /// <summary>
        /// 布局元素变更事件
        /// </summary>
        /// <param name="obj">元素</param>
        private void Container_ModelChanged(IContainer obj)
        {
            if (obj != null)
            {
                obj.SelectedElementChanged += Model_SelectedElementChanged;
            }
        }

        /// <summary>
        /// 元素选中变更事件
        /// </summary>
        /// <param name="obj">元素</param>
        private void Model_SelectedElementChanged(IElement obj)
        {
            container.svContainer.Focus();
            var model = obj as ActivityModel;
            if (model != null && model.ProcessId != 0)
            {
                property.Name = model.Text;
                property.CreateSku = model.CreateSku;
                property.IsOptional = model.IsOptional;
                property.IsRepeat = model.IsRepeat;
                property.IsGenerateTask = model.IsGenerateTask;
                property.IsBuckleMaterial = model.IsBuckleMaterial;
                property.IsPassRate = model.IsPassRate;
                property.IsBinding = model.IsBinding;
                property.IsUnBinding = model.IsUnBinding;
                property.Overtime = model.Overtime;
                property.StartProcess = model.StartProcess;
                property.NormalVictoryId = model.NormalVictory;
                property.RepairVictoryId = model.RepairVictory;
                property.IsStricter = model.IsStricter;
            }
            else
            {
                property.Name = string.Empty;
                property.CreateSku = false;
                property.IsOptional = false;
                property.IsRepeat = false;
                property.IsGenerateTask = false;
                property.IsBuckleMaterial = false;
                property.IsPassRate = false;
                property.IsBinding = false;
                property.IsUnBinding = false;
                property.StartProcess = null;
                property.NormalVictoryId = null;
                property.RepairVictoryId = null;
                property.IsStricter = false;
                property.Overtime = null;
            }
        }
    }
}