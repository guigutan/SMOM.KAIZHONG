using DevExpress.Xpf.Editors;
using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.WorkBenchCommon.SwitchWorkShop;
using SIE.Wpf.Common.Diagram;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.SwitchWorkShop
{
    /// <summary>
    /// SwitchWorkShopControl.xaml 的交互逻辑
    /// </summary>
    [Category("目标管理")]
    public partial class SwitchWorkShopControl : ComponentItem
    {
        /// <summary>
        /// 当前员工所属车间集合
        /// </summary>
        EntityList<Enterprise> workShops = new EntityList<Enterprise>();

        /// <summary>
        /// 目标车间
        /// </summary>
        double? _targetWorkShopId = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SwitchWorkShopControl()
        {
            InitializeComponent();
            UseProperty<SwitchWorkShopProperty>();
            DataContext = this;
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            LoadWorkShops();
            _targetWorkShopId = RT.Service.Resolve<TargetWorkShopController>().GetTargetWorkShop(RT.IdentityId)?.Id;
            resourceControl.EditValue = _targetWorkShopId == null ? 0 : _targetWorkShopId.Value;
        }

        /// <summary>
        /// 加载车间
        /// </summary>
        void LoadWorkShops()
        {
            workShops.Clear();
            workShops.AddRange(RT.Service.Resolve<EnterpriseController>().GetEnterpriseByEmployee(RT.IdentityId));
            resourceControl.ItemsSource = workShops;
        }

        /// <summary>
        /// 车间下拉事件
        /// </summary>
        /// <param name="sender">下拉控件</param>
        /// <param name="e">测试</param>
        private void PopupOpening(object sender, OpenPopupEventArgs e)
        {
            LoadWorkShops();
        }

        /// <summary>
        /// 车间值变更事件
        /// </summary>
        /// <param name="sender">下拉控件</param>
        /// <param name="e">测试</param>
        private void ResourceControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            double resourceId = 0;
            double.TryParse(e.NewValue?.ToString(), out resourceId);
            if (resourceId == 0)
                return;
            if (_targetWorkShopId != resourceId)
                RT.Service.Resolve<TargetWorkShopController>().SaveTargetWorkShop(RT.IdentityId, resourceId);
            RT.EventBus.Publish(new WorkShopChangedEvent(resourceId));
        }
    }

    /// <summary>
    /// 直通率属性
    /// </summary>
    public class SwitchWorkShopProperty : ComponentProperty<SwitchWorkShopControl>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SwitchWorkShopProperty()
        {
            Priority = 9999;
        }
    }
}