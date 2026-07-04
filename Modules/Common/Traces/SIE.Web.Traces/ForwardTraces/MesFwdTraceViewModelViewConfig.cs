using Newtonsoft.Json;
using SIE.Traces.Common;
using SIE.Traces.ForwardTraces;
using SIE.Web.Traces.Common;

namespace SIE.Web.Traces.ForwardTraces
{
    /// <summary>
    /// 过程追溯视图配置
    /// </summary>
    internal class MesFwdTraceViewModelViewConfig : WebViewConfig<MesFwdTraceViewModel>
    {

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Traces.Behaviors.MesFwdTraceViewModelBehavior");
            View.DisableEditing();
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.Property(p => p.RelatedProductSn).ShowInList(width: 200).DisableSort();
            View.Property(p => p.RelatedProductCode).ShowInList(width: 200).DisableSort();
            View.Property(p => p.RelatedProductName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.RelatedWorkOrderNo).ShowInList(width: 200).DisableSort();
            View.AttachChildrenProperty(typeof(MesProcessKeyItemFwdViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesFwdTraceViewModel parent = JsonConvert.DeserializeObject<MesFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ForwardTraceController>().GetMesProcssKeyItemData(parent, args.PagingInfo);
                return result;
            }, MesProcessKeyItemFwdViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(ProductInspectTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesFwdTraceViewModel parent = JsonConvert.DeserializeObject<MesFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductInspectData(parent.WipProductVersionId, args.PagingInfo);
                return result;
            }, ProductInspectTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(ProductDefectTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesFwdTraceViewModel parent = JsonConvert.DeserializeObject<MesFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductDefectData(parent.WipProductVersionId, args.PagingInfo);
                return result;
            }, ProductDefectTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(ProductRepairTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesFwdTraceViewModel parent = JsonConvert.DeserializeObject<MesFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductRepairData(parent.WipProductVersionId, args.PagingInfo);
                return result;
            }, ProductRepairTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(PackageTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesFwdTraceViewModel parent = JsonConvert.DeserializeObject<MesFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetPackageData(parent.RelatedProductId, parent.RelatedProductSn, args.PagingInfo);
                return result;
            }, PackageTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(QmsTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                MesFwdTraceViewModel parent = JsonConvert.DeserializeObject<MesFwdTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductQmsData(parent.RelatedProductSn,parent.RelatedProductId,parent.RelatedWorkOrderId, args.PagingInfo);
                return result;
            }, QmsTraceViewModelViewConfig.ListView).HasLabel("产品质量追溯");
        }
    }
}