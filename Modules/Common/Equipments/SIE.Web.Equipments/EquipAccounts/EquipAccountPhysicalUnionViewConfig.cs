using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
	/// 设备台账物联参数视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipAccountPhysicalUnionViewConfig : WebViewConfig<EquipAccountPhysicalUnion>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelAccountPhysicalUnionCommand).FullName,
                WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.PhysicalUnionId).Readonly();
            View.Property(p => p.ParaName);
            View.Property(p => p.EquipPara);
            View.Property(p => p.MaxValue);
            View.Property(p => p.MinValue);
            View.Property(p => p.Unit);

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
