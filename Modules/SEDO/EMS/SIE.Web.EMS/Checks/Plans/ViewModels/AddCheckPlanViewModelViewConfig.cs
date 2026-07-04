using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.Equipments.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 点检计划视图配置
    /// </summary>
    public class AddCheckPlanViewModelViewConfig : WebViewConfig<AddCheckPlanViewModel>
    {
        /// <summary>
        /// 批次添加视图
        /// </summary>
        public const string ViewGroup_BatchAddCheckPlan = "BatchAddCheckPlanView";
        private const string YMD_FORMAT = "Y/m/d";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ViewGroup_BatchAddCheckPlan);
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            if (ViewGroup == ViewGroup_BatchAddCheckPlan)
                BatchAddCheckPlanView();
        }

        /// <summary>
        /// 配置添加列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            View.Property(p => p.EquipCheckType).Readonly();
            View.Property(p => p.EquipAccountId).UseDataSource((e, c, r) =>
            {
                EntityList<EquipAccountSelect> list = RT.Service.Resolve<EquipAccountSelectController>()
                    .GetCheckPlanEquipAccounts(r, c);
                list.ForEach(account => { account.TreePId = null; });
                return list;
            }).UsePagingLookUpEditor((m, e) =>
            {
                var keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.MachineNo), nameof(e.EquipAccount.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.MachineNo).Readonly().HasLabel("设备名称");
            View.Property(p => p.CheckCycleType).UseEnumEditor(p => p.AllowBlank = false).Show(ShowInWhere.Hide);

            View.Property(p => p.BeginDate).UseDateEditor(d => d.Format = YMD_FORMAT).HasLabel("计划区间*");
            View.Property(p => p.EndDate).UseDateEditor(d => d.Format = YMD_FORMAT).HasLabel("至*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

            View.Property(p => p.CheckTime).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; });
        }

        /// <summary>
        /// 配置批量添加视图
        /// </summary>
        protected void BatchAddCheckPlanView()
        {
            View.AddBehavior("SIE.Web.EMS.Checks.Plans.Scripts.BatchAddCheckPlanBehavior");
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(3);
                View.Property(p => p.EquipCheckType).Show(ShowInWhere.All).Cascade(p => p.ResourceId, null).Cascade(p => p.ResourceName, null);

                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, null);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ResourceName), nameof(e.Resource.Name));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.EquipCheckType != EquipCheckType.Line).HasLabel("产线编码").Show(ShowInWhere.All);

                View.Property(p => p.ResourceName).Readonly().HasLabel("产线名称").Show(ShowInWhere.All);
                View.Property(p => p.BeginDate).UseDateEditor(d => d.Format = YMD_FORMAT).Show(ShowInWhere.All).HasLabel("计划区间".L10N()+"*");
                View.Property(p => p.EndDate).UseDateEditor(d => d.Format = YMD_FORMAT).Show(ShowInWhere.All).HasLabel("至".L10N()+"*"+"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                View.Property(p => p.CheckTime).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).Show(ShowInWhere.All);
                View.AttachChildrenProperty(typeof(EquipAccountSelect), e => new EntityList<EquipAccountSelect>(), EquipAccountSelectViewConfig.CheckPlanBatchAddList).Show(ChildShowInWhere.All).HasLabel("设备台账").Readonly();
            }
        }
    }
}
