using SIE.EMS.RunStandards;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.RunStandards.Commands;

namespace SIE.Web.EMS.RunStandards
{
    /// <summary>
    /// 设备明细
    /// </summary>
    public class RunStandardEquipmentViewConfig : WebViewConfig<RunStandardEquipment>
    {
        /// <summary>
		/// 编辑视图
		/// </summary>
		public const string EditView = "EditView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                DetailListView();
            }
        }

        /// <summary>
        /// 明细页面
        /// </summary>
        public void DetailListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(SelEquipsCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountId).HasLabel("设备编码").ShowInList(120);
                View.Property(p => p.EquipAccountName).ShowInList(120);
                View.Property(p => p.Specifications).HasLabel("规格型号").ShowInList(80);
                View.Property(p => p.EquipTypeCategory).ShowInList(80);
                View.Property(p => p.UseDepartmentName).ShowInList(80);
                View.Property(p => p.Manufacturers).ShowInList(80);
                View.Property(p => p.CardDate).ShowInList(80);
                View.Property(p => p.UseState).ShowInList(80);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountId).HasLabel("设备编码").ShowInList(120);
                View.Property(p => p.EquipAccountName).ShowInList(120); 
                View.Property(p => p.Specifications).HasLabel("规格型号").ShowInList(80); 
                View.Property(p => p.EquipTypeCategory).ShowInList(80); 
                View.Property(p => p.UseDepartmentName).ShowInList(80); 
                View.Property(p => p.Manufacturers).ShowInList(80);
                View.Property(p => p.CardDate).ShowInList(80);
                View.Property(p => p.UseState).ShowInList(80);
            }
        }
    }
}
