using SIE.DataTrace.TraceMainDatas;
using SIE.Domain;
using SIE.Web.DataTrace.TraceMainDatas.Commands;
using SIE.WorkFlow.Base.Common.Models;
using SIE.WorkFlow.Base.FlowInstances;

namespace SIE.Web.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据视图配置
    /// </summary>
    public class TraceMainDataViewConfig : WebViewConfig<TraceMainData>
    {
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkFlowViews.WorkFlowDetailsView, WorkFlowViews.WorkFlowReadonlyView);

            switch (ViewGroup)
            {
                case WorkFlowViews.WorkFlowDetailsView:
                    ConfigWorkFlowDetailsView();
                    break;
                case WorkFlowViews.WorkFlowReadonlyView:
                    ConfigWorkFlowReadonlyView();
                    break;
            }
        }

        private void ConfigWorkFlowReadonlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.Property(p => p.No).Show();
        }

        private void ConfigWorkFlowDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.No).Show();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(ViewMutiDocumenttCommand).FullName,typeof(UploadAttachmentCommand).FullName);
            View.Property(p => p.No);
            View.Property(p => p.WorkFlowType);
            View.Property(p => p.FlowInstanceId);
            View.Property(p => p.WorkFlowStatus);
            View.Property(p => p.Context);
        }

 
    }
}
