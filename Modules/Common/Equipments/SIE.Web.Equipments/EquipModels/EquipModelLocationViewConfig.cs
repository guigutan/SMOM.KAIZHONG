using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.MetaModel.View;

namespace SIE.Web.Equipments.EquipModels
{
    /// <summary>
    /// 设备型号位置视图配置
    /// </summary>
    internal class EquipModelLocationViewConfig : WebViewConfig<EquipModelLocation>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EquipModel));
            View.AssignAuthorize(typeof(EquipAccount));
        }
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.UseImportCommands();
            View.Property(p => p.Subarea);
            View.Property(p => p.BigStance);
            View.Property(p => p.Stance);
            View.Property(p => p.StanceType);
            //隐藏列
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.EquipModel.Code).HasLabel("设备型号编码");
            View.Property(p => p.Subarea);
            View.Property(p => p.BigStance);
            View.Property(p => p.Stance);
            View.Property(p => p.StanceType);
        }
    }
}