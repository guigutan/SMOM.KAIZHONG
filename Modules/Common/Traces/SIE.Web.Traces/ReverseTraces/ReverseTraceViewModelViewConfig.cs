using Newtonsoft.Json;
using SIE.Traces.Common;
using SIE.Traces.ForwardTraces;
using SIE.Traces.ReverseTraces;
using SIE.Web.Traces.Common;
using SIE.Web.Traces.ForwardTraces;

namespace SIE.Web.Traces.ReverseTraces
{
    /// <summary>
    /// 正向追溯视图配置
    /// </summary>
    internal class ReverseTraceViewModelViewConfig : WebViewConfig<ReverseTraceViewModel>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseLayoutSize(0.4, 0.6);
            View.DisableEditing();
            View.ClearCommands();
            View.Property(p => p.ProductCode).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProductName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProductExtPropName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProductSn).ShowInList(width: 200).DisableSort();
            View.Property(p => p.Qty).ShowInList().DisableSort();
            View.Property(p => p.WorkOrderNo).ShowInList(width: 200).DisableSort();
            View.Property(p => p.WorkShopName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.VersionName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProductionDate).ShowInList(width: 200).DisableSort();

            View.AttachChildrenProperty(typeof(MesProcessCollectViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ReverseTraceViewModel parent = JsonConvert.DeserializeObject<ReverseTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ReverseTraceController>().GetMesProcessCollectDatas(parent, args.PagingInfo);
                return result;
            }, MesProcessCollectViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(PackageTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ReverseTraceViewModel parent = JsonConvert.DeserializeObject<ReverseTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetPackageData(parent.ProductId, parent.ProductSn, args.PagingInfo);
                return result;
            }, PackageTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(QmsTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ReverseTraceViewModel parent = JsonConvert.DeserializeObject<ReverseTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductQmsData(parent.ProductSn, parent.ProductId, parent.WorkOrderId, args.PagingInfo);
                return result;
            }, QmsTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(ProductInspectTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ReverseTraceViewModel parent = JsonConvert.DeserializeObject<ReverseTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductInspectData(parent.VersionId, args.PagingInfo);
                return result;
            }, ProductInspectTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(ProductDefectTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ReverseTraceViewModel parent = JsonConvert.DeserializeObject<ReverseTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductDefectData(parent.VersionId, args.PagingInfo);
                return result;
            }, ProductDefectTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(ProductRepairTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ReverseTraceViewModel parent = JsonConvert.DeserializeObject<ReverseTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<TraceCommonController>().GetMesProductRepairData(parent.VersionId, args.PagingInfo);
                return result;
            }, ProductRepairTraceViewModelViewConfig.ListView);
        }
    }
}