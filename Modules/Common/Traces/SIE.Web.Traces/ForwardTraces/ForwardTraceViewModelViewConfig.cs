using DevExpress.XtraRichEdit.Model;
using Newtonsoft.Json;
using SIE.Traces.Common;
using SIE.Traces.ForwardTraces;
using SIE.Web.Traces.Common;

namespace SIE.Web.Traces.ForwardTraces
{
    /// <summary>
    /// 正向追溯视图配置
    /// </summary>
    internal class ForwardTraceViewModelViewConfig : WebViewConfig<ForwardTraceViewModel>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.Property(p => p.Sn).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ItemLot).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ItemCode).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ItemName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ItemExtPropName).ShowInList(width: 200).DisableSort();
           
           
            View.AttachChildrenProperty(typeof(WmsFwdTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ForwardTraceViewModel parent = JsonConvert.DeserializeObject<ForwardTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ForwardTraceController>().GetWmsTraceDatas(parent, args.PagingInfo);
                return result;
            }, WmsFwdTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(QmsTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ForwardTraceViewModel parent = JsonConvert.DeserializeObject<ForwardTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ForwardTraceController>().GetQmsIqcTraceDatas(parent, args.PagingInfo);
                return result;
            }, QmsTraceViewModelViewConfig.ListView);
            View.AttachChildrenProperty(typeof(MesFwdTraceViewModel), o =>
            {
                var args = o as ChildPagingDataWithParentEntityArgs;
                ForwardTraceViewModel parent = JsonConvert.DeserializeObject<ForwardTraceViewModel>(args.ParentEntity);
                var result = RT.Service.Resolve<ForwardTraceController>().GetTraceInfoForProduct(parent, args.PagingInfo);
                return result;
            }, MesFwdTraceViewModelViewConfig.ListView);
        }
    }
}