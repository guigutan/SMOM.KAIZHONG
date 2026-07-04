using SIE.Domain;
using SIE.MES.TeamManagement.SikllAuthentications;
using SIE.MetaModel.View;
using SIE.Resources.Skills;
using SIE.Web.Resources.Skills;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 员工技能认证管理视图配置
    /// </summary>
    public class SkillAuthenticationViewConfig : WebViewConfig<SkillAuthentication>
    {
        /// <summary>
        /// Common字符串
        /// </summary>
        private readonly string _commonString = "Common";

        /// <summary>
        /// Required字符串
        /// </summary>
        private readonly string _requiredString = "Required";

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.MES.TeamManagement.SikllAuthentications.SkillAuthCopyCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.MES.TeamManagement.SikllAuthentications.SkillAuthEditCommand");
            View.FormEdit();
            View.Property(p => p.SkillCode).Readonly();
            View.Property(p => p.SkillName).Readonly();
            View.Property(p => p.SkillValidity).Readonly();
            View.Property(p => p.SkillCategoryName).Readonly();
            View.Property(p => p.SkillRemark).Readonly();
            View.Property(p => p.TrainingRequired).UseMultiFilterEnumEditor(p => { p.Filters = new string[] { _commonString, _requiredString }; p.AllowBlank = false; });
            View.Property(p => p.ExamRequired).UseMultiFilterEnumEditor(p => { p.Filters = new string[] { _commonString, _requiredString }; p.AllowBlank = false; });
            View.Property(p => p.OperationRequired).UseMultiFilterEnumEditor(p => { p.Filters = new string[] { _commonString, _requiredString }; p.AllowBlank = false; });
            View.ChildrenProperty(p => p.TrainList).HasLabel("培训记录");
            View.ChildrenProperty(p => p.ExamList).HasLabel("考试结果");
            View.ChildrenProperty(p => p.OperationList).HasLabel("实操记录");
            View.AttachChildrenProperty(typeof(EmployeeSkill), w =>
            {
                var args = w as ChildPagingDataArgs;
                var skillAuth = RT.Service.Resolve<SkillAuthController>().GetSkillAuthentication((args.Parent as SkillAuthentication).Id);
                EntityList<EmployeeSkill> employeeSkillList = new EntityList<EmployeeSkill>();
                if (skillAuth != null)
                {
                    var ctl = RT.Service.Resolve<SkillController>();
                    employeeSkillList = ctl.GetEmployeeSkillListBySkill(skillAuth.SkillId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                }

                return employeeSkillList;
            }, EmployeeSkillViewConfig.SkillAuthView).Show(ChildShowInWhere.All).HasLabel("技能授予记录").OrderNo = 99;
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.HasDetailColumnsCount(4);
            View.Property(p => p.SkillCategory).UsePagingLookUpEditor().Cascade(p => p.Skill, null)
                .Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInDetail(columnSpan: 2)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑,更改技能分类清空技能清单"; });
            View.Property(p => p.Skill).UseDataSource((e, p, s) =>
            {
                var auth = e as SkillAuthentication;
                if (auth == null || auth.SkillCategory == null)
                    return new EntityList<Skill>();
                return RT.Service.Resolve<SkillController>().GetSkills(auth.SkillCategoryId);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SkillName), nameof(Skill.Name));
                keyValues.Add(nameof(e.SkillValidity), nameof(Skill.Validity));
                keyValues.Add(nameof(e.SkillRemark), nameof(Skill.Remark));
                m.DicLinkField = keyValues;
                m.DisplayField = Skill.CodeProperty.Name;
            }).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInDetail(columnSpan: 2).HasLabel("技能编码")
            .UseListSetting(e => { e.HelpInfo = "显示当前技能分类的技能清单,新增状态可编辑"; });
            View.Property(p => p.SkillName).ShowInDetail(columnSpan: 2).Readonly().HasLabel("技能名称");
            View.Property(p => p.SkillRemark).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.TrainingRequired).ShowInDetail(columnSpan: 2).UseMultiFilterEnumEditor(p => p.Filters = new string[] { _commonString, _requiredString });
            View.Property(p => p.ExamRequired).ShowInDetail(columnSpan: 2).UseMultiFilterEnumEditor(p => p.Filters = new string[] { _commonString, _requiredString });
            View.Property(p => p.OperationRequired).ShowInDetail(columnSpan: 2).UseMultiFilterEnumEditor(p => p.Filters = new string[] { _commonString, _requiredString });
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Skill);
            View.Property(p => p.TrainingRequired);
            View.Property(p => p.ExamRequired);
            View.Property(p => p.OperationRequired);
        }
    }
}