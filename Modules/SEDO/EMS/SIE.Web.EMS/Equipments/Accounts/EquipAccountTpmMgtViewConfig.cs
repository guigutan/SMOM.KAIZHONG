using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Maintains.Controller;
using SIE.Web.EMS.Equipments.Accounts.ViewModels;
using System;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
	/// 设备台账 TPM管理 视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipAccountTpmMgtViewConfig : WebViewConfig<EquipAccountTpmMgt>
    {
        /// <summary>
        /// 表单配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.EquipAccountTabBehavior");
            View.AttachChildrenProperty(typeof(TpmViewModel), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccountTpmMgt>();
                if (parent == null)
                    return new EntityList<TpmViewModel>();

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanTpmViewModel(parent.Id);
                return list;
            }, TpmViewModelViewConfig.CheckPlanViewGroup).HasLabel("点检计划").HasOrderNo(1).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(TpmViewModel), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccountTpmMgt>();
                if (parent == null)
                    return new EntityList<TpmViewModel>();

                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanTpmViewModel(parent.Id);
                return list;
            }, TpmViewModelViewConfig.MaintainPlanViewGroup).HasLabel("保养计划").HasOrderNo(2).Show(ChildShowInWhere.All);           
        }
    }
}
