using SIE.Domain;
using SIE.MES.Workbench.EmployeeManages;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// ShiftLookUpEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ShiftLookUpEditor : UserControl
    {
        /// <summary>
        /// 班次列表
        /// </summary>
        EntityList<Shift> Shift;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftLookUpEditor()
        {
            InitializeComponent();
            Shift = new EntityList<Shift>();
            this.groupLookupEdit.ItemsSource = Shift;
            this.Loaded += (o, e) =>
            {
                var manage = this.DataContext as EmployeeManage;
                manage.PropertyChanged += Manage_PropertyChanged;
                //需要设置默认值，先添加所有数据源
                var shiftList = RF.GetAll<Shift>();
                if (shiftList.Count > 0)
                    Shift.AddRange(shiftList);
            };
        }

        /// <summary>
        /// 员工管理属性变更事件
        /// </summary>
        /// <param name="sender">员工管理</param>
        /// <param name="e">参数</param>
        private void Manage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var manage = sender as EmployeeManage;
            if (e.PropertyName == nameof(EmployeeManage.ResourceId))
                manage.ShiftId = 0;
        }

        /// <summary>
        /// 编辑器下拉事件
        /// </summary>
        /// <param name="sender">下拉编辑器</param>
        /// <param name="e">菜单</param>
        private void PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            LoadShiftList();
        }

        /// <summary>
        /// 加载班次列表
        /// </summary>
        void LoadShiftList()
        {
            Shift.Clear();
            var manage = this.groupLookupEdit.DataContext as EmployeeManage;
            if (manage == null || manage.ResourceId == 0) return;
            var date = RF.Find<Shift>().GetDbTime();
            var calender = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(manage.ResourceId, date);
            if (calender == null || calender.ShiftType == null || calender.ShiftType.ShiftList.Count == 0) return;
            Shift.AddRange(calender.ShiftType.ShiftList);
        }
    }
}