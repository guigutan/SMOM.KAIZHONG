using DevExpress.Xpf.Editors;
using SIE.Wpf.MES.WIP.Inspects;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 编辑器
    /// </summary>
    public class IsNgBoolCheckEditor : BoolCheckEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public new const string EditorName = "IsNgBoolCheckEditor";

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Control_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var inspectionItemViewModel = Source as InspectionItemViewModel;
            if (inspectionItemViewModel != null)
                inspectionItemViewModel.IsNg = (bool)e.NewValue;
        }
    }
}
