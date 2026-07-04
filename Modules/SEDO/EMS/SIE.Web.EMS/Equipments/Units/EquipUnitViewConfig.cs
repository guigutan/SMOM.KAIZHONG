using SIE.EMS.Equipments.Units;
using SIE.MetaModel.View;
using SIE.Web.EMS.Equipments.Units.Commands;

namespace SIE.Web.EMS.Equipments.Units
{
    /// <summary>
	/// 设备单元视图配置
	/// </summary>
	internal class EquipUnitViewConfig : WebViewConfig<EquipUnit>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.DraggableForTree();
            View.UseCommands(
                typeof(AddUnitCommand).FullName,
                typeof(AddChildUnitCommand).FullName,
                WebCommandNames.Edit,
                WebCommandNames.Delete,
                WebCommandNames.Save);
            View.Property(p => p.Code).ShowInList(width: 300);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
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
