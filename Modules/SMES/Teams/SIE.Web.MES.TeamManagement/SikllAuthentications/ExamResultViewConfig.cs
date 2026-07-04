using SIE.MES.TeamManagement.SikllAuthentications;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.MES.TeamManagement.SikllAuthentications.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
	/// 考试结果视图配置
	/// </summary>
	public class ExamResultViewConfig : WebViewConfig<ExamResult>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
           string helpInfo = "历史数据不可编辑".L10N();
            View.InlineEdit();
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.ExamResultEditCommand");
            View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.ExamResultDelCommand");
            View.UseCommands(typeof(ExamResultImportCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.MES.TeamManagement.SikllAuthentications.RecordCopyCommand");
            View.Property(p => p.EmployeeCode).HasLabel("工号").Readonly();
            View.Property(p => p.Employee).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EmployeeCode), nameof(Employee.Code));
                keyValues.Add(nameof(e.EmployeeSex), nameof(Employee.Sex));
                keyValues.Add(nameof(e.EmployeeStatus), nameof(Employee.EmployeeStatus));
                m.DicLinkField = keyValues;
            }).HasLabel("姓名").Readonly(p => p.IsHistory)
            .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.EmployeeSex).Readonly();
            View.Property(p => p.EmployeeStatus).Readonly();
            View.Property(p => p.Score).UseSpinEditor(p => { p.MinValue = 0; p.Value = 0; }).Readonly(p => p.IsHistory)
            .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.ExamTime).UseDateTimeEditor(p => { p.DefaultValue = DateTime.Now; }).ShowInList(150).Readonly(p => p.IsHistory)
            .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.Result).UseMultiFilterEnumEditor(p => { p.Filters = new string[] { "Common", "Result" }; p.AllowBlank = false; }).Readonly(p => p.IsHistory)
            .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.EmployeeCode).HasLabel("工号*");
            View.Property(p => p.EmployeeName);
            View.Property(p => p.EmployeeSex);
            View.Property(p => p.EmployeeStatus);
            View.Property(p => p.Score).HasLabel("考试得分(分)*").Show();
            View.Property(p => p.ExamTime).HasLabel("考试时间*").Show();
            View.Property(p => p.Result).HasLabel("考试结果*").Show();
        }
    }
}