using SIE.MES.Projects;
using SIE.MetaModel.View;
using SIE.Web.MES.Projects.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects
{
    /// <summary>
    /// 工序标准参数明细视图配置
    /// </summary>
    public class ProcessStandardParamDtlViewConfig : WebViewConfig<ProcessStandardParamDtl>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(ProcessStandardDtlAddCommand).FullName, "SIE.Web.MES.Projects.Commands.ProcessStandardDtlEditCommand", "SIE.Web.MES.Projects.Commands.ProcessStandardDtlDeleteCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectParam).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProjectParamController>().GetProjectParams(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProjectParamName), nameof(e.ProjectParam.Name));
                    keyValues.Add(nameof(e.ProjectParamType), nameof(e.ProjectParam.Type));
                    m.DicLinkField = keyValues;
                }).HasLabel("项目参数编码").Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList(width: 120);
                View.Property(p => p.ProjectParamName).Readonly().ShowInList(width: 120);
                View.Property(p => p.ProjectParamType).Readonly().ShowInList();
                View.Property(p => p.ProcessStDtlValueType).Cascade(p => p.SingleValue, null).Cascade(p => p.RangeMaxValue, null).Cascade(p => p.RangeMinValue, null).Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.Unit).Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.SingleValue).Readonly(p => p.ProcessStDtlValueType == SIE.MES.Projects.Enums.ProcessStDtlValueType.Range || p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.RangeMaxValue).Readonly(p => p.ProcessStDtlValueType == SIE.MES.Projects.Enums.ProcessStDtlValueType.Single || p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.RangeMinValue).Readonly(p => p.ProcessStDtlValueType == SIE.MES.Projects.Enums.ProcessStDtlValueType.Single || p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
            }
        }
    }
}
