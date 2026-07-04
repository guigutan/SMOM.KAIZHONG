using SIE.EMS.Faults;
using SIE.MetaModel.View;
using SIE.Web.EMS.Faults.Commands;

namespace SIE.Web.EMS.Faults
{
    /// <summary>
    /// 设备故障大类视图配置
    /// </summary>
    internal class EquipLargeFaultViewConfig : WebViewConfig<EquipLargeFault>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().ReplaceCommands(WebCommandNames.Add, typeof(AddEquipLargeFaultCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);
            View.UseCommands(typeof(ImportEquipFaultCommand).FullName);
            View.Property(p => p.Code)
                //.Readonly()
                ;
            View.Property(p => p.Name);
            View.ChildrenProperty(p => p.MiddleFaultList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
