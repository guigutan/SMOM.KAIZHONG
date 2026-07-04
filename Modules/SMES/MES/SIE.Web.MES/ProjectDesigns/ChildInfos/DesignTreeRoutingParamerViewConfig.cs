using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MES.Projects;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线工序参数视图配置
    /// </summary>
    public class DesignTreeRoutingParamerViewConfig : WebViewConfig<DesignTreeRoutingParamer>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectDesign));
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(TreeRoutingPraAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete);
            using(View.OrderProperties())
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
                }).HasLabel("项目参数编码").ShowInList(width: 120);
                View.Property(p => p.ProjectParamName).Readonly().ShowInList(width: 120);
                View.Property(p => p.ProjectParamType).Readonly().ShowInList();
                View.Property(p => p.ProcessStDtlValueType).Cascade(p => p.SingleValue, null).Cascade(p => p.RangeMaxValue, null).Cascade(p => p.RangeMinValue, null).ShowInList();
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).ShowInList(width: 120);
                View.Property(p => p.Unit).ShowInList();
                View.Property(p => p.SingleValue).Readonly(p => p.ProcessStDtlValueType == SIE.MES.Projects.Enums.ProcessStDtlValueType.Range).ShowInList();
                View.Property(p => p.RangeMaxValue).Readonly(p => p.ProcessStDtlValueType == SIE.MES.Projects.Enums.ProcessStDtlValueType.Single).ShowInList();
                View.Property(p => p.RangeMinValue).Readonly(p => p.ProcessStDtlValueType == SIE.MES.Projects.Enums.ProcessStDtlValueType.Single).ShowInList();
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectParam).HasLabel("项目参数编码").ShowInList(width: 120);
                View.Property(p => p.ProjectParamName).Readonly().ShowInList(width: 120);
                View.Property(p => p.ProjectParamType).Readonly().ShowInList();
                View.Property(p => p.ProcessStDtlValueType).ShowInList();
                View.Property(p => p.Unit).ShowInList();
                View.Property(p => p.SingleValue).Readonly().ShowInList();
                View.Property(p => p.RangeMaxValue).Readonly().ShowInList();
                View.Property(p => p.RangeMinValue).Readonly().ShowInList();
            }
        }
    }
}
