using SIE.Equipments.EquipTypes;
using SIE.Web.Common;
using SIE.Web.Equipments.EquipTypes.Commands;

namespace SIE.Web.EMS.Equipments.EquipTypes
{
    /// <summary>
    /// 设备类型视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipTypeViewConfig : WebViewConfig<SIE.Core.Equipments.EquipType>//此ViewConfig类型所用到的泛型中的字段，如果是继承而来的则无用。
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Equipments.EquipTypes.Behaviors.EquipTypePropertyChangedBehavior");
            View.UseDefaultCommands();
            View.UseCommands(typeof(EquipTypeImportCommand).FullName);
            View.Property(p => p.TypeCode);
            View.Property(p => p.TypeName);
            View.Property(p => p.Num).DefaultValue(0).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.TypeCode);
            View.Property(p => p.TypeName);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
        }

        /// <summary>
		/// 配置查询视图
		/// </summary>
		protected override void ConfigQueryView()
        {
            View.Property(p => p.TypeCode);
            View.Property(p => p.TypeName);
        }
    }
}