using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 仪器参数视图配置
    /// </summary>
    internal class EquipParamViewConfig : WebViewConfig<EquipParam>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(EquipParam.NameProperty);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipAccount));

            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete,
                WebCommandNames.Save);

            View.Property(p => p.Name);
            View.Property(p => p.UnitId);
            View.Property(p => p.Max);
            View.Property(p => p.Min);
            View.Property(p => p.Remark);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.UnitId);
            View.Property(p => p.Max);
            View.Property(p => p.Min);
        }
    }
}