using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.ExtensionViewConfig
{
    /// <summary>
    /// 仪器参数视图配置
    /// </summary>
    internal class MeteringEquipParamViewConfig : WebViewConfig<MeteringEquipParam>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(MeteringEquipParam.NameProperty);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(MeteringEquipmentAccount));

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