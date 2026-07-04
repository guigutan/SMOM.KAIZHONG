using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Skills;
using System;

namespace SIE.Web.Resources.Skills
{
    /// <summary>
    /// 员工技能视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EmployeeSkillViewConfig : WebViewConfig<EmployeeSkill>
    {
        /// <summary>
        /// 与技能认证管理关联的技能 ViewGroup字符串定义
        /// </summary>
        public const string SkillAuthView = "SkillAuthView";

        #region 当前状态 ExpireDateState
        /// <summary>
        /// 当前状态
        /// </summary>
        [Label("当前状态")]
        public static readonly Property<string> ExpireDateStateProperty = P<EmployeeSkill>.RegisterExtensionReadOnly("ExpireDateState", typeof(EmployeeSkillViewConfig),
            GetExpireDateState, EmployeeSkill.ExpireDateProperty);

        /// <summary>
        /// 当前状态
        /// </summary>
        public static string GetExpireDateState(Entity me)
        {
            var item = me as EmployeeSkill;
            //不取数据库时间，否则会导致多次查数据
            var dbtime = DateTime.Now.Date;
            if (!item.ExpireDate.HasValue || dbtime <= item.ExpireDate)
                return "有效".L10N();
            else
                return "失效".L10N();
        }
        #endregion

        ///<summary>
        /// 与员工列表关联的技能 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SkillAuthView);
            if (ViewGroup == SkillAuthView)
            {
                ConfigSkillAuthView();
            }
        }

        /// <summary>
        /// 与技能认证管理关联的技能 配置列表视图
        /// </summary>
        protected void ConfigSkillAuthView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).Readonly().ShowInList();
                View.Property(p => p.EmployeeName).Readonly().ShowInList();
                View.Property(p => p.EmployeeSex).Readonly().ShowInList();
                View.Property(p => p.EmployeeStatus).Readonly().ShowInList();
                View.Property(p => p.TrainingRequired).Readonly().ShowInList();
                View.Property(p => p.ExamRequired).Readonly().ShowInList();
                View.Property(p => p.OperationRequired).Readonly().ShowInList();
                View.Property(p => p.AuthDate).Readonly().ShowInList(150);
                View.Property(p => p.ExpireDate).UseDateEditor().Readonly().ShowInList(150);
                View.Property(ExpireDateStateProperty).Readonly().ShowInList();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.SkillCode).Readonly();
            View.Property(p => p.SkillName).Readonly();
            View.Property(p => p.SkillCategoryName).Readonly();
            View.Property(p => p.SkillRemark).Readonly();
            View.Property(p => p.TrainingRequired).Readonly();
            View.Property(p => p.ExamRequired).Readonly();
            View.Property(p => p.OperationRequired).Readonly();
            View.Property(p => p.AuthDate).ShowInList(150).Readonly();
            View.Property(p => p.ExpireDate).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.Validity).Readonly();
            View.Property(ExpireDateStateProperty).Readonly();
            View.Property(p => p.CreateByName).Readonly();
            View.Property(p => p.CreateDate).ShowInList(150).Readonly();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
