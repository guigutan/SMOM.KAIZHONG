using SIE.EMS.Faults;
using SIE.MetaModel.View;
using SIE.Web.EMS.Faults.Commands;

namespace SIE.Web.EMS.Faults
{
    /// <summary>
    /// 设备故障中类视图配置
    /// </summary>
    internal class EquipMiddleFaultViewConfig : WebViewConfig<EquipMiddleFault>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save, WebCommandNames.Copy).ReplaceCommands(WebCommandNames.Add, typeof(AddEquipMiddleFaultCommand).FullName).UseChildrenAsHorizontal();
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Name);
            View.ChildrenProperty(p => p.SmallFaultList).Show(ChildShowInWhere.Hide);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
