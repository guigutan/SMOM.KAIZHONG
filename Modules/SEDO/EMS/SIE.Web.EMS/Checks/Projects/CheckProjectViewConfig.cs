using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Enums;
using SIE.MetaModel.View;
using SIE.Web.EMS.Checks.Commands;
using SIE.Web.EMS.Checks.Plans.Commands;
using SIE.Core.Common;
using System;
using System.Linq.Expressions;

namespace SIE.Web.EMS.Checks.Projects
{
    /// <summary>
    /// 点检项目视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CheckProjectViewConfig : WebViewConfig<CheckProject>
    {
        /// <summary>
        /// 点检确认所用的点检项目视图
        /// </summary>
        public const string CheckConfirmationListView = "CheckConfirmationListView";

        #region 缺陷描述是否可编辑
        /// <summary>
        /// 缺陷描述是否可编辑
        /// </summary>
        public static readonly Expression<Func<CheckProject, bool>> DefectReadonlyProperty = p => p.CheckResult == null || p.CheckResult == CheckMaintainResult.OK || p.CheckResult == CheckMaintainResult.Unright;
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.DeclareExtendViewGroup(CheckConfirmationListView);
            if (ViewGroup == CheckConfirmationListView)
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
            View.UseCommands(WebCommandNames.Edit);
            View.UseCommand("SIE.Web.EMS.Checks.Plans.Commands.QuickCheckPlanCommand");
            View.UseCommand(typeof(GetProjectRealTimeDataCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.AccountCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.AccountName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ParaCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ParaName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipParamSource).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CycleType).HasLabel("周期类型").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Part).HasLabel("部位").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ProjectConsumable).HasLabel("项目耗材").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Standard).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MinValue).HasLabel("最小值").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MaxValue).HasLabel("最大值").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActualValue).Show(ShowInWhere.All).Readonly(ViewGroup == CheckConfirmationListView);

                if (ViewGroup == CheckConfirmationListView)
                {
                    View.Property(p => p.CheckResult).UseEnumEditor(w => w.ColumnXType = "setCheckResultStyle_comboboxcolumn")
                        .HasLabel("点检结果".L10N()+"*")
                        .Show(ShowInWhere.All)
                        .Readonly();
                }
                else
                {
                    View.Property(p => p.CheckResult).UseEnumEditor(w => w.ColumnXType = "setCheckResultStyle_comboboxcolumn")
                        .HasLabel("点检结果".L10N() + "*")
                        .Show(ShowInWhere.All)
                        .Readonly(p => p.MaxValue != null || p.MinValue != null);
                }

                View.Property(p => p.DefectDesc).Show(ShowInWhere.All).Readonly(DefectReadonlyProperty).Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.Unit).HasLabel("单位").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseTime).HasLabel("用时（分钟）").Show(ShowInWhere.All).Readonly();

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
            View.Property(p => p.EquipCheckProjectId);
            View.Property(p => p.EquipPhysicalUnionId);
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
            View.Property(p => p.EquipCheckProjectId);
            View.Property(p => p.EquipPhysicalUnionId);
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
            View.Property(p => p.EquipCheckProjectId);
            View.Property(p => p.EquipPhysicalUnionId);
            View.Property(p => p.ProjectName);
            View.Property(p => p.MaxValue);
            View.Property(p => p.MinValue);
        }
    }
}
