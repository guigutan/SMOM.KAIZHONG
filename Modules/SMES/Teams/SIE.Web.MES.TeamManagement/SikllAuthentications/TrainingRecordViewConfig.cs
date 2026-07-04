using SIE.MES.TeamManagement.SikllAuthentications;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.MES.TeamManagement.SikllAuthentications.Commands;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 培训记录视图配置
    /// </summary> 
    public class TrainingRecordViewConfig : WebViewConfig<TrainingRecord>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            const string helpInfo = "历史数据不可编辑";
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.TrainingRecordEditCommand");
            View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.TrainingRecordDelCommand");
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.MES.TeamManagement.SikllAuthentications.TrainingRecordCopyCommand");
            View.UseCommands(typeof(TrainingRecordImportCommand).FullName);
            View.Property(p => p.EmployeeCode).Readonly().HasLabel("工号");
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
            View.Property(p => p.BeginDate).UseDateTimeEditor().ShowInList(150).Readonly(p => p.IsHistory)
                .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.EndDate).UseDateTimeEditor().ShowInList(150).Readonly(p => p.IsHistory)
                .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.Duration).Readonly();
            View.Property(p => p.Result).UseMultiFilterEnumEditor(p => { p.Filters = new string[] { "Common", "Result" }; p.AllowBlank = false; }).Readonly(p => p.IsHistory)
                .UseListSetting(e => { e.HelpInfo = helpInfo; });
            View.Property(p => p.IsHistory).Show(ShowInWhere.Hide);
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
            View.Property(p => p.BeginDate).HasLabel("培训开始时间*");
            View.Property(p => p.EndDate).HasLabel("培训结束时间*");
            View.Property(p => p.Duration);
            View.Property(p => p.Result).HasLabel("培训结果*");
        }
    }
}