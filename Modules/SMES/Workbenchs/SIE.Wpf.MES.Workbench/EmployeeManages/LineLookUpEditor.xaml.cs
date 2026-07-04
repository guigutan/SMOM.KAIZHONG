using SIE.Resources.Employees;
using System.Linq;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// LineLookUpEditor.xaml 的交互逻辑
    /// </summary>
    public partial class LineLookUpEditor : UserControl
    {
        double? _employeeId;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LineLookUpEditor()
        {
            InitializeComponent();
            this.Loaded += (o, e) =>
            {
                LoadResoueceList();
            };
            _employeeId = RT.Service.Resolve<EmployeeController>().GetEmployeeByUserId(RT.IdentityId)?.Id;
        }

        /// <summary>
        /// 下拉编辑器下拉事件
        /// </summary>
        /// <param name="sender">下拉编辑器</param>
        /// <param name="e">参数</param>
        private void PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            LoadResoueceList();
        }

        /// <summary>
        /// 加载产线列表
        /// </summary>
        void LoadResoueceList()
        {
            if (!_employeeId.HasValue) return;
            this.groupLookupEdit.ItemsSource = RT.Service.Resolve<EmployeeController>().GetEmployeeResources(_employeeId.Value).Select(p => p.Resource);
        }
    }
}