using SIE.EMS.Enums;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Projects;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.EMS.EquipMaint.Maintains.Projects
{
    /// <summary>
    /// 保养项目视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class MaintainProjectViewConfig : WebViewConfig<MaintainProject>
    {
        /// <summary>
        /// 保养确认所用的保养项目视图
        /// </summary>
        public const string MaintainConfirmationListView = "MaintainConfirmationListView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.DeclareExtendViewGroup(MaintainConfirmationListView);
            if (ViewGroup == MaintainConfirmationListView)
            {
                ConfigListView();
                View.ClearCommands();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));

            View.UseCommands(WebCommandNames.Edit,
                "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.QuickMaintainPlanCommand",
                "SIE.Web.EMS.EquipMaint.Maintains.Projects.Commands.ExportXls");

            using (View.OrderProperties())
            {
                View.Property(p => p.AccountCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipMaintainProjectId).Show(ShowInWhere.Hide);
                View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CycleType).HasLabel("周期类型").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Part).HasLabel("部位").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MinValue).HasLabel("最小值").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MaxValue).HasLabel("最大值").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActualValue).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView);

                if (ViewGroup == MaintainConfirmationListView)
                {
                    View.Property(p => p.MaintainResult).UseEnumEditor(w => w.ColumnXType = "setMaintainResultStyle_comboboxcolumn".L10N())
                        .Show(ShowInWhere.All);
                }
                else
                {
                    View.Property(p => p.MaintainResult).UseEnumEditor(w => w.ColumnXType = "setMaintainResultStyle_comboboxcolumn".L10N())
                        .Show(ShowInWhere.All)
                        .Readonly(p => p.MinValue != null && p.MaxValue != null).HasLabel("保养结果".L10N() + "*");
                }
                View.Property(p => p.Defect).Show(ShowInWhere.All).Readonly(p => p.MaintainResult == CheckMaintainResult.OK || p.MaintainResult == CheckMaintainResult.Unright).Readonly(ViewGroup == MaintainConfirmationListView);
                View.Property(p => p.Unit).HasLabel("单位").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseTime).HasLabel("用时（分钟）").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ProjectConsumable).HasLabel("项目耗材").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Method).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView);
                View.Property(p => p.Standard).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView);

                View.Property(p => p.ExeState).Show(ShowInWhere.Hide);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.EquipAccount);
            View.Property(p => p.EquipMaintainProject);
            View.Property(p => p.ProjectName);
            View.Property(p => p.MaxValue);
            View.Property(p => p.MinValue);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.EquipAccount);
            View.Property(p => p.EquipMaintainProject);
            View.Property(p => p.ProjectName);
            View.Property(p => p.MaxValue);
            View.Property(p => p.MinValue);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipAccount);
            View.Property(p => p.EquipMaintainProject);
            View.Property(p => p.ProjectName);
            View.Property(p => p.MaxValue);
            View.Property(p => p.MinValue);
        }
    }
}
