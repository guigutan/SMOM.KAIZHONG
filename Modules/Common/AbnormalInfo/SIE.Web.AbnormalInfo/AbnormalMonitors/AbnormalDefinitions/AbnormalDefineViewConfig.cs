using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions;
using SIE.Common.Schdules;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Schedule;
using SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.AnomalyMonitors
{
    /// <summary>
    /// 异常定义视图配置
    /// </summary>
    internal class AbnormalDefineViewConfig : WebViewConfig<AbnormalDefine>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands.AddAbnormalDefineCommand");
            View.UseCommands(typeof(JobRunCommand).FullName);
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.AbnormalRuleId).UsePagingLookUpEditor(
                        (m, e) =>
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add(nameof(e.RuleName), nameof(e.AbnormalRule.RuleName));
                            dic.Add(nameof(e.MonitorName), nameof(e.AbnormalRule.MonitorName));
                            m.DicLinkField = dic;
                        });
            View.Property(p => p.RuleName).Readonly();
            View.Property(p => p.MonitorName).Readonly();
            View.Property(p => p.AbnormalWarnDefineId).UsePagingLookUpEditor(
                      (m, e) =>
                      {
                          Dictionary<string, string> dic = new Dictionary<string, string>();
                          dic.Add(nameof(e.AbnormalWarnDefineName), nameof(e.AbnormalWarnDefine.Name));
                          m.DicLinkField = dic;
                      });
            View.Property(p => p.AbnormalWarnDefineName).Readonly();
            View.Property(p => p.JobConfigId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<AbnormalJobConfigController>().GetJobConfigs(keyword, pagingInfo);
            }).UsePagingLookUpEditor(
                        (m, e) =>
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add(nameof(e.JobConfigState), nameof(e.JobConfig.State));
                            m.DicLinkField = dic;
                            m.DisplayField = JobConfig.NameProperty.Name;
                        }).ShowInList(width:200);
            View.Property(p => p.State).Readonly();
            View.AttachChildrenProperty(typeof(JobLog), e =>
            {
                var args = e as ChildPagingDataWithParentEntityArgs;
                AbnormalDefine item = JsonConvert.DeserializeObject<AbnormalDefine>(args.ParentEntity);
                var job = RF.GetById<JobConfig>(item.JobConfigId);
                if (job == null)
                    return new EntityList<JobLog>();
                return RT.Service.Resolve<JobController>().GetLogs(job.Key, args.PagingInfo, args.SortInfo);
            });
            View.ChildrenProperty(p=>p.ResumeList).Show(ChildShowInWhere.Hide);
        }

        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.AbnormalRuleId);
            View.Property(p => p.AbnormalWarnDefineId);
            View.Property(p => p.JobConfigId).ShowInList(width: 200);
            View.Property(p => p.JobConfigState);
        }
    }
}